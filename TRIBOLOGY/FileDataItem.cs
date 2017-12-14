using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    class FileDataItem
    {
        public double Time { set; get; }//时间
        public double Pressure { set; get; }//压力
        public double Temperature { set; get; }//温度
        public double Humidity { set; get; }//湿度
        public double PlatSpeed { set; get; }//平台转速
        public double MotoSpeed { set; get; }//电机转速
        public double MotoTorque { set; get; }//电机扭矩
        public double FricForce { set; get; }//摩擦力
        public double FricSpeed { set; get; }//摩擦速度
    }
}
