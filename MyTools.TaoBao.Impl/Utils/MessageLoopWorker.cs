/*
 *名称：MessageLoopWorker
 *功能：
 *创建人：吉桂昕
 *创建时间：2014-01-22 11:41:03
 *修改时间：
 *备注：
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTools.TaoBao.Impl.Utils
{ 
    // a helper class to start the message loop and execute an asynchronous task
    /// <summary>
    /// 用于多线程下，调用COM组件，或者是创建FORM控件
    /// </summary>
    public static class MessageLoopWorker
    {
        public static async Task<object> Run(Func<object[], Task<object>> worker, params object[] args)
        {
            var tcs = new TaskCompletionSource<object>();

            var thread = new Thread(() =>
            {
                EventHandler idleHandler = null;

                idleHandler = async (s, e) =>
                {
                    // handle Application.Idle just once
                    Application.Idle -= idleHandler;

                    // return to the message loop
                    await Task.Yield();

                    // and continue asynchronously
                    // propogate the result or exception
                    try
                    {
                        var result = await worker(args);
                        tcs.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                    // signal to exit the message loop
                    // Application.Run will exit at this point
                    Application.ExitThread();
                };

                // handle Application.Idle just once
                // to make sure we're inside the message loop
                // and SynchronizationContext has been correctly installed
                Application.Idle += idleHandler;
                Application.Run();
            });

            // set STA model for the new thread
            thread.SetApartmentState(ApartmentState.STA);

            // start the thread and await for the task
            thread.Start();
            try
            {
                return await tcs.Task;
            }
            finally
            {
                thread.Join();
            }
        }
    }

}