using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infrastructure.CrossCutting.IoC.Ninject;
using Infrastructure.Crosscutting.IoC;
using MyTools.TaoBao.Impl;
using MyTools.TaoBao.Interface;

namespace MyTools
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartUp();
            Application.Run(new MainForm());
            //test abc
            
        }

        private static void StartUp()
        {
            InstanceLocator.SetLocator(
               new NinjectContainer().WireDependenciesInAssemblies(typeof(ItemCats).Assembly.FullName).Locator);

        }
    }
}
