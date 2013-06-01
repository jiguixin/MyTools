/*
 *名称：TextBoxWriter
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-06-01 03:10:54
 *修改时间：
 *备注：
 */

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MyTools.Utility
{
    /// <summary>
    /// 捕获日志记录输出
    /// </summary>
    public class TextBoxWriter : TextWriter
    {
        public Action<string> ChangeTextBoxValue;

        #region Members

        #endregion
         
        #region Constructor
         
        public TextBoxWriter()
        {
        }

        #endregion
         
        #region Public Methods
         
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
            if (ChangeTextBoxValue != null)
                ChangeTextBoxValue(value + Environment.NewLine);
        }

        public override void Write(string value)
        {
            base.Write(value);
            if (ChangeTextBoxValue != null)
                ChangeTextBoxValue(value + Environment.NewLine);
        }

        #endregion


        #region Private Methods

        #endregion

         
    }
}