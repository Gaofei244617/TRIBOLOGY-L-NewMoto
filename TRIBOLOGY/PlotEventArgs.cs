using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    internal class PlotEventArgs : EventArgs
    {
         private string msg;
         public PlotEventArgs(string s)
             : base()
        {
            this.msg = s;
        }
        public string Msg
        {
            get { return msg; }
        }
    }
}