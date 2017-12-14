using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    class MotoDataItem
    {
        public int Number { get; set; }
        public double Minute { get; set; }
        public double Second { get; set; }
        public double StaSpeed { get; set; }
        public double EndSpeed { get; set; }

        //构造函数
        public MotoDataItem(int num, double min, double sec, double speed1, double speed2)
        {
            Number = num;
            Minute = min;
            Second = sec;
            StaSpeed = speed1;
            EndSpeed = speed2;
        }
        public MotoDataItem() { } //构造函数重载      
    }
}
