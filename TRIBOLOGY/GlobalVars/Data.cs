using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    public static class Data
    {
        private static double motoSpeed = 0;
        public static double Pressure1 { set; get; } //压力1
        public static double Pressure2 { set; get; } //压力2
        public static double Humidity { set; get; } //相对湿度
        public static double Temperature { set; get; } //温度
        public static double PlatSpeed { set; get; } //平台转速
        public static double MotoSpeed
        {
            set
            {
                if (value == 65535) motoSpeed = 0;
                else motoSpeed = value;
            }
            get
            {
                return motoSpeed;
            }
        } //电机转速
        public static double MotoTorque { set; get; } //电机扭矩
        public static double FricSpeed { set; get; } //摩擦速度
        public static double FricForce { set; get; } //摩擦力
    }
}
