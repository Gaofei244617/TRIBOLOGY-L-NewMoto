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
//添加命名空间
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using FirstFloor.ModernUI.Windows.Controls;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for Plot.xaml
    /// </summary>
    public partial class Plot : UserControl
    {
        public static UIDataPlot uiDataPlot = new UIDataPlot();
        //所要添加的平均值曲线
        LineGraph avrGraphLine = new LineGraph();
        //文件数据集合
        ObservableCollection<FileDataItem> fileData = new ObservableCollection<FileDataItem>();
        PlotPointCollection filePtCollection = new PlotPointCollection(1000000);
        PlotPointCollection avrPtCollection = new PlotPointCollection(2);
        //构造函数
        public Plot()
        {
            InitializeComponent();
            this.DataContext = uiDataPlot;
            var plotLine = new EnumerableDataSource<PlotPoint>(filePtCollection);
            plotLine.SetXMapping(x => x.xLabel); 
            plotLine.SetYMapping(y => y.yValue);
            filePlot.AddLineGraph(plotLine, Colors.Green, 2);
            filePlot.Children.Remove(filePlot.Legend);
        }
        //编辑坐标轴和标题
        void EditPlotXYT(object sender,PlotEventArgs e)
        {
            string[] str;
            str = e.Msg.Split(':');
            switch(str[0])
            {
                case "编辑标题：":
                    uiDataPlot.Header = str[1];
                    break;
                case "编辑横坐标：":
                    uiDataPlot.XAxisLabel = str[1];
                    break;
                case "编辑纵坐标：":
                    uiDataPlot.YAxisLabel = str[1];
                    break;
            }

        }
        //打开文件按钮（绘图）
        private void opFileBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOperator fOptor = new FileOperator();
            string s = null; //文件路径
            string line = null; //一条记录
            string[] strs = null;
            string[] titleItem = null;

            int i = 0;
            FileDataItem fileDataItem;
            s = fOptor.getOpenPath(); //打开曲线文件路径
            avrDataLabel.Visibility = Visibility.Hidden;

            if (s != null)
            {
                filePathLabel.Content = "文件：" + s;
                StreamReader sr = new StreamReader(s);
                filePtCollection.Clear();
                
                line = sr.ReadLine();//文件头(包含各项说明)
                titleItem = line.Split('\t');
                for (i = 3; i < titleItem.Length; i++)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = titleItem[i];
                    curTypeCombox.Items.Add(item);
                }

                fileData.Clear();//清空数据以便存入新的文件数据
                while ((line = sr.ReadLine()) != null)
                {
                    //解析数据记录
                    strs = line.Split('\t');
                    fileDataItem = new FileDataItem();

                    try
                    {
                        fileDataItem.Time = double.Parse(strs[0]);
                        for (i = 3; i < strs.Length; i++)
                        {
                            switch (titleItem[i])
                            {
                                case "压力":
                                    fileDataItem.Pressure = double.Parse(strs[i]); break;
                                case "温度":
                                    fileDataItem.Temperature = double.Parse(strs[i]); break;
                                case "相对湿度":
                                    fileDataItem.Humidity = double.Parse(strs[i]); break;
                                case "平台转速":
                                    fileDataItem.PlatSpeed = double.Parse(strs[i]); break;
                                case "电机转速":
                                    fileDataItem.MotoSpeed = double.Parse(strs[i]); break;
                                case "电机扭矩":
                                    fileDataItem.MotoTorque = double.Parse(strs[i]); break;
                                case "摩擦力":
                                    fileDataItem.FricForce = double.Parse(strs[i]); break;
                                case "摩擦速度":
                                    fileDataItem.FricSpeed = double.Parse(strs[i]); break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK);
                        break;
                    }

                    fileData.Add(fileDataItem); //添加一条数据记录
                }
                sr.Close();
            }
        }
        //选择绘图项目
        private void slecChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = fileData.Count;//n条数据记录
            int i = 0;
            if (n > 0)
            {
                filePtCollection.Clear();//清空当前曲线坐标点集合
                uiDataPlot.Header = curTypeCombox.SelectedItem.ToString().Split(':')[1] + "曲线";

                for (i = 0; i < n; i++)
                {
                    switch (curTypeCombox.SelectedItem.ToString().Split(':')[1])
                    {
                        case " 压力":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].Pressure)); break;
                        case " 温度":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].Temperature)); break;
                        case " 相对湿度":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].Humidity)); break;
                        case " 平台转速":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].PlatSpeed)); break;
                        case " 电机转速":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].MotoSpeed)); break;
                        case " 电机扭矩":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].MotoTorque)); break;
                        case " 摩擦力":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].FricForce)); break;
                        case " 摩擦速度":
                            filePtCollection.Add(new PlotPoint(fileData[i].Time, fileData[i].FricSpeed)); break;
                    }
                }
                
                //计算数据平均值
                uiDataPlot.AvrValue = 0;
                for (i = 0; i < n; i++)
                {
                    uiDataPlot.AvrValue = uiDataPlot.AvrValue + filePtCollection[i].yValue;
                }
                uiDataPlot.AvrValue = uiDataPlot.AvrValue / n;

                avrDataLabel.Content = "平均值：" + uiDataPlot.AvrValue.ToString();
                if (avrCheckBox.IsChecked == true)
                {
                    avrDataLabel.Visibility = Visibility.Visible;
                    avrPtCollection.Clear();
                    for (i = 0; i < n; i++)//平均值数据点集合
                    {
                        avrPtCollection.Add(new PlotPoint(filePtCollection[i].xLabel, uiDataPlot.AvrValue));
                    }
                }
                else
                    avrDataLabel.Visibility = Visibility.Hidden;

                //设置坐标轴标签
                switch (curTypeCombox.SelectedItem.ToString().Split(':')[1])
                {
                    case " 压力": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "压力[N]"; break;
                    case " 温度": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "温度[℃]"; break;
                    case " 相对湿度": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "相对湿度[RH%]"; break;
                    case " 平台转速": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "转速[r/min]"; break;
                    case " 电机转速": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "转速[r/min]"; break;
                    case " 电机扭矩": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "相对扭矩[%]"; break;
                    case " 摩擦力": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "摩擦力[N]"; break;
                    case " 摩擦速度": uiDataPlot.XAxisLabel = "Time(S)"; uiDataPlot.YAxisLabel = "摩擦速度[mm/s]"; break;
                }
            }
        }
        //平均值单选框
        private void avrValueChekBox_Checked(object sender, RoutedEventArgs e) //选中
        {
            var avrLine = new EnumerableDataSource<PlotPoint>(avrPtCollection);
            avrLine.SetXMapping(x => x.xLabel); avrLine.SetYMapping(y => y.yValue);
            
            avrDataLabel.Visibility = Visibility.Visible;

            //平均值数据点集合
            avrPtCollection.Add(new PlotPoint(filePtCollection[0].xLabel, uiDataPlot.AvrValue));
            avrPtCollection.Add(new PlotPoint(filePtCollection[filePtCollection.Count-1].xLabel, uiDataPlot.AvrValue));
           
            //显示平均值
            avrDataLabel.Content = "平均值：" + uiDataPlot.AvrValue.ToString();
            //添加平均值曲线
            avrGraphLine = filePlot.AddLineGraph(avrLine, Colors.Gray, 1, "平均值");
            filePlot.Children.Add(filePlot.Legend);

        }
        private void avrValueChekBox_Unchecked(object sender, RoutedEventArgs e) //未选中
        {
            filePlot.Children.Remove(avrGraphLine); //移除平均值曲线
            filePlot.Children.Remove(filePlot.Legend); //移除图例
            avrPtCollection.Clear(); //清空平均值数据
            avrDataLabel.Visibility = Visibility.Hidden;
        }
        //导出绘图数据按钮
        private void expPlotDataBtn_Click(object sender, RoutedEventArgs e)
        {
            int i = 0, n = 0;
            FileOperator fOptor = new FileOperator();
            string s = null;
            n = filePtCollection.Count;
            if (n > 0)
            {
                s = fOptor.getSavePath(); //获取曲线保存路径
                if (s != null)
                {
                    StreamWriter sw = new StreamWriter(s, false);
                    sw.WriteLine("Time(s)\t" + curTypeCombox.Text);
                    for (i = 0; i < n; i++)
                    {
                        sw.WriteLine(filePtCollection[i].xLabel.ToString() + '\t' + filePtCollection[i].yValue.ToString());
                    }
                    sw.Close();
                }
            }
            else
            {
                ModernDialog.ShowMessage("当前不存在绘图数据……", "Message:", MessageBoxButton.OK);
            }
        }
        //编辑标题
        private void EditTitle(object sender, RoutedEventArgs e)
        {
            EditWindow w = new EditWindow("编辑标题：");
            w.OnEditPlot += new EditPlot(this.EditPlotXYT);
            w.Show();
        }
        //编辑横坐标
        private void EditXLabel(object sender, RoutedEventArgs e)
        {
            EditWindow w = new EditWindow("编辑横坐标：");
            w.OnEditPlot += new EditPlot(this.EditPlotXYT);
            w.Show();
        }
        //编辑纵坐标
        private void EditYLabel(object sender, RoutedEventArgs e)
        {
            EditWindow w = new EditWindow("编辑纵坐标：");
            w.OnEditPlot += new EditPlot(this.EditPlotXYT);
            w.Show();
        }

    }
}
