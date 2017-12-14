using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//引用命名空间
using System.ComponentModel;

namespace TRIBOLOGY
{
    public class UIDataPlot : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //绘图选项卡坐标轴和标题
        private string header = "Plot Title";
        private string xAxisLabel = "X-Value";
        private string yAxisLabel = "Y-Value";
        private double avrValue = 0;//平均值

        public string Header
        {
            get { return header; }
            set { header = value; Notify("Header"); }
        }
        public string XAxisLabel
        {
            get { return xAxisLabel; }
            set { xAxisLabel = value; Notify("XAxisLabel"); }
        }
        public string YAxisLabel
        {
            get { return yAxisLabel; }
            set { yAxisLabel = value; Notify("YAxisLabel"); }
        }
        public double AvrValue
        {
            get { return avrValue; }
            set { avrValue = value; Notify("AvrValue"); }
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
