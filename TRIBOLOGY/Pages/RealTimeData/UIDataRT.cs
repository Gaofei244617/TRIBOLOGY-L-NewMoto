using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//添加引用
using System.ComponentModel;
using System.Windows;

namespace TRIBOLOGY
{
    public class UIDataRT : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string pressure = "";//压力传感器
        private string humidity = "";//相对湿度
        private string temperature = "";//温度
        private string platSpeed = "";//平台转速
        private string motoSpeed = "";//电机转速
        private string motoTorque = "";//电机扭矩
        private string fricForce = "";//摩擦力
        private string fricSpeed = "";
        //进度条
        private double proBar = 0;
        private string proBarTex = "0%";
        private Visibility proBarVisible = Visibility.Hidden;
        private string leftTime = "剩余时间:";

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
        public string FricSpeed
        {
            get { return fricSpeed; }
            set { fricSpeed = value; Notify("FricSpeed"); }
        }
        public double ProBar
        {
            get { return proBar; }
            set { proBar = value; Notify("ProBar"); }
        }
        public string ProBarTex
        {
            get { return proBarTex; }
            set { proBarTex = value; Notify("ProBarTex"); }
        }
        public Visibility ProBarVisible
        {
            get { return proBarVisible; }
            set { proBarVisible = value; Notify("ProBarVisible"); }
        }
        public string LeftTime
        {
            get { return leftTime; }
            set { leftTime = value; Notify("LeftTime"); }
        }
        void Notify(string str)//形参为值发生改变的属性名称
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
    }
}
