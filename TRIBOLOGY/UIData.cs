using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//引用命名空间
using System.ComponentModel;

namespace TRIBOLOGY
{
    //UI界面绑定的属性
    public class UIData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string pressure = "000";//压力传感器
        private string humidity = "000";//相对湿度
        private string temperature = "000";//温度
        private string platSpeed = "000";//平台转速
        private string motoSpeed = "000";//电机转速
        private string motoTorque = "000";//电机扭矩
        private string fricForce = "000";//摩擦力
        //绘图选项卡坐标轴和标题
        private string header = "Plot Title";
        private string xAxisLabel = "X-Value";
        private string yAxisLabel = "Y-Value";
        private double avrValue = 0;//平均值

        public string Pressure
        {
            get { return pressure; }
            set { pressure = value; Notify("Pressure"); }
        }
        public string Humidity
        {
            get { return humidity; }
            set { humidity = value; Notify("Humidity"); }
        }
        public string Temperature
        {
            get { return temperature; }
            set { temperature = value; Notify("Temperature"); }
        }
        public string PlatSpeed
        {
            get { return platSpeed; }
            set { platSpeed = value; Notify("PlatSpeed"); }
        }
        public string MotoSpeed
        {
            get { return motoSpeed; }
            set { motoSpeed = value; Notify("MotoSpeed"); }
        }
        public string MotoTorque
        {
            get { return motoTorque; }
            set { motoTorque = value; Notify("MotoTorque"); }
        }
        public string FricForce
        {
            get { return fricForce; }
            set { fricForce = value; Notify("FricForce"); }
        }
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
