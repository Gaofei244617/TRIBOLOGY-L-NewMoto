using FirstFloor.ModernUI.Windows.Controls;
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
    /// Interaction logic for PlotWin.xaml
    /// </summary>
    public partial class PlotWin : ModernWindow
    {
        //数据绑定
        Binding binding = new Binding();
        internal PlotWin(EnumerableDataSource<PlotPoint> temp, string s)
        {
            InitializeComponent();
            binding.Source = RTDatas.uiDataRT;
            DataPlot.Children.Remove(DataPlot.Legend);
            switch (s)
            {
                case "Temperature":
                    SetPlot(temp, "温度曲线", "温度[℃]", "Temperature");
                    break;
                case "PlatSpeed":
                    SetPlot(temp, "平台转速曲线", "平台转速[r/min]", "PlatSpeed");
                    break;
                case "Pressure":
                    SetPlot(temp, "压力曲线", "压力[N]", "Pressure");
                    break;
                case "Humidity":
                    SetPlot(temp, "相对湿度", "相对湿度[RH%]", "Humidity");
                    break;
                case "FricForce":
                    SetPlot(temp, "摩擦力曲线", "摩擦力[N]", "FricForce");
                    break;
                case "MotorSpeed":
                    SetPlot(temp, "电机转速", "电机转速[r/min]", "MotoSpeed");
                    break;
                case "MotorTorque":
                    SetPlot(temp, "电机相对扭矩", "相对扭矩[%]", "MotoTorque");
                    break;
                case "FricSpeed":
                    SetPlot(temp, "摩擦速度", "摩擦速度[mm/s]", "FricSpeed");
                    break;
            }
        }
        //绘图设置
        void SetPlot(EnumerableDataSource<PlotPoint> temp, string title, string yLabel, string path)
        {
            DataPlot.AddLineGraph(temp, Colors.Green, 2);
            PlotTitle.Content = title;
            PlotYLabel.Content = yLabel;
            DataLabel.Content = yLabel + ":";
            binding.Path = new PropertyPath(path);//path为形参
            this.ValueLabel.SetBinding(Label.ContentProperty, binding);
        }
    }
}
