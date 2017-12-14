using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    public static class Flag
    {
        public static double SysTime { set; get; }
        public static bool RecData { set; get; }
        
        //功能标志和接收字节数标志
        //功能标志nFunc： 1设定转速，2查询转速，3查询扭矩，4查询温湿度及压力
        //接收字节数标记nFlag
        public static int nFunc = 0;
        public static int nFlag = 0;

    }
}
