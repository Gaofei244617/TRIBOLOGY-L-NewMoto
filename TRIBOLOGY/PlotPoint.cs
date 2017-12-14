using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace TRIBOLOGY
{
    //绘图坐标点
    class PlotPoint
    {
        public double xLabel { get; set; }//x值
        public double yValue { get; set; }//y值
        public PlotPoint(double x, double y)
        {
            this.xLabel = x;
            this.yValue = y;
        }
    }    
}
