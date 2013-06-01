/*
 *名称：TextBoxTraceListener
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-06-01 03:13:09
 *修改时间：
 *备注：
 */

using System;
using System.Diagnostics;
using System.Text;

namespace MyTools.Utility
{
    public class TextBoxTraceListener: TextWriterTraceListener
    { 
        public Action<string> ChangeTextBoxValue;

        #region Members

        #endregion


        #region Constructor

        public TextBoxTraceListener()
        {

        }

        public TextBoxTraceListener(TextBoxWriter tw)
            : base(tw)
        {
            this.Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Information); 
        }

        #endregion


        #region Public Methods
         

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
            if (ChangeTextBoxValue != null)
                ChangeTextBoxValue(value + Environment.NewLine);
        }
  
        #endregion


        #region Private Methods

        #endregion

         
    }
}