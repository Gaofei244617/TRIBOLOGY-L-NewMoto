using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//添加对命名空间的引用
using Microsoft.Research.DynamicDataDisplay.Common;

namespace TRIBOLOGY
{
    class PlotPointCollection : RingArray<PlotPoint>
    {
        public PlotPointCollection(int num)
            : base(num)
        { 
        }
    }
}
