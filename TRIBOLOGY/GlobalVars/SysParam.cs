using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//引用命名空间
using System.ComponentModel;

namespace TRIBOLOGY
{
    public static class SysParam 
    {
        public static int PortName { set; get; }
        public static int BaudRate { set; get; }
        public static string Parity { set; get; }
        public static string StopBit { set; get; }
        public static double Arm_1 { set; get; }
        public static double Arm_2 { set; get; }
        public static double ArmRatio { set; get; }
        public static double ExtraPress_1 { set; get; }
        public static double ExtraPress_2 { set; get; }
        public static double FricRadius { set; get; }

        //构造函数
        public static void Initial()
        {
            PortName = 1;
            BaudRate = 9600;
            Parity = "None";
            StopBit = "One";
            Arm_1 = 1.0;
            Arm_2 = 1.0;
            ArmRatio = 1.0;
            ExtraPress_1 = 0;
            ExtraPress_2 = 0;
            FricRadius = -1;
        }
        static SysParam()
        {
            Initial();
        }
    }
}
