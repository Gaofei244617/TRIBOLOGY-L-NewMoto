using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//添加引用
using System.ComponentModel;


namespace TRIBOLOGY.Pages.Settings
{
    public class UIDataSP : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int sPortNo = 0;
        private int sPBR = 0;
        private int sPortParity = 0;
        private int sPStopBit = 0;

        public int SPortNo
        {
            get { return sPortNo; }
            set { sPortNo = value; Notify("SPortNo"); }
        }
        public int SPBR
        {
            get { return sPBR; }
            set { sPBR = value; Notify("SPBR"); }
        }
        public int SPortParity
        {
            get { return sPortParity; }
            set { sPortParity = value; Notify("SPortParity"); }
        }
        public int SPStopBit
        {
            get { return sPStopBit; }
            set { sPStopBit = value; Notify("SPStopBit"); }
        }

        //触发 PropertyChanged 事件的通用方法
        void Notify(string str)//形参为值发生改变的属性名称
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

    }
}
