using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    public static class MotoData
    {
        public static int N { set; get; }            // 电机运行总时间(秒)
        public static int MotoTime { set; get; }     // 电机当前运行时间点
        public static int[] SpeedData { set; get; }  // 电机运行数据点
        public static bool StartMoto { set; get; }        
        public static bool CurveOK = false;          // 电机曲线确认
    }
}