using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//引用命名空间
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for TempHumCur.xaml
    /// </summary>
    public partial class TempHumCur : UserControl
    {
        PlotterData plotterData = new PlotterData();

        public TempHumCur()
        {
            InitializeComponent();
            this.DataContext = RTDatas.uiDataRT;
            IniSystem();
        }

        //初始化函数
        void IniSystem()
        {
            //添加数据曲线(数据源,曲线颜色,线宽,图例)
            humPlot.AddLineGraph(plotterData.humidity, Colors.Blue, 2, "平台转速");
            tempPlot.AddLineGraph(plotterData.temperature, Colors.Brown, 2, "压力");
        }
    }
}
