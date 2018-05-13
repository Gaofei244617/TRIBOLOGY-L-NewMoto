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
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//添加引用
using FirstFloor.ModernUI.Windows.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>

    public partial class SetParam : UserControl
    {
        //电机数据曲线集合(数据表)
        ObservableCollection<MotoDataItem> speedCurve = new ObservableCollection<MotoDataItem>();
        //电机运行坐标点集合
        PlotPointCollection spCurCollection = new PlotPointCollection(200);//电机转速曲线集合
        EnumerableDataSource<PlotPoint> motoSpCur;

        //数据文件
        public static StreamWriter dataFile;
        internal static event SysParaUpdate OnSysParaUpdate;
        string fileName = null;

        //构造函数
        public SetParam()
        {
            InitializeComponent();
            IniSystem();
        }

        #region 自定义函数
        //初始化函数
        void IniSystem()
        {
            dataGrid.DataContext = speedCurve;
            //订阅事件
            RTDatas.OnStopMoto += new StopMoto(this.stopMotor);            
            MainWindow.OnStopMoto += new StopMoto(this.stopMotor);

            motoSpCur = new EnumerableDataSource<PlotPoint>(spCurCollection);
            motoSpCur.SetXMapping(x => x.xLabel); motoSpCur.SetYMapping(y => y.yValue);
            curvePlot.AddLineGraph(motoSpCur, Colors.Green, 2);

            //取消鼠标缩放,移除图例
            curvePlot.Children.Remove(curvePlot.MouseNavigation);
            curvePlot.Children.Remove(curvePlot.Legend);
            //串口预选
            SPortNoCombox.SelectedIndex = SysParam.PortName - 1;
            SPortBRCombox.SelectedIndex = (int)Math.Log(SysParam.BaudRate / 1200, 2);
            switch (SysParam.Parity)
            {
                case "None":
                    SPortParityCombox.SelectedIndex = 0;
                    break;
                case "Odd":
                    SPortParityCombox.SelectedIndex = 1;
                    break;
                case "Even":
                    SPortParityCombox.SelectedIndex = 2;
                    break;
                case "Mark":
                    SPortParityCombox.SelectedIndex = 3;
                    break;
                case "Space":
                    SPortParityCombox.SelectedIndex = 4;
                    break;
            }
            switch (SysParam.StopBit)
            {
                case "None":
                    SPortStopbitCombox.SelectedIndex = 0;
                    break;
                case "One":
                    SPortStopbitCombox.SelectedIndex = 1;
                    break;
                case "Two":
                    SPortStopbitCombox.SelectedIndex = 2;
                    break;
                case "1.5":
                    SPortStopbitCombox.SelectedIndex = 3;
                    break;
            }
        }
        
        //电机停转(速度模式)
        void stopMotor(object sender, EventArgs e)
        {
            SendData sendData = new SendData();
            MotoData.StartMoto = false;
            MotoData.CurveOK = false;
            Flag.nFunc = 1; //功能标志
            Flag.nFlag = 8; //接收字节数标志
            sendData.setMotoSpeed(0, MainWindow.serPort);
            //Data.MotoSpeed = 0;
            MotoData.MotoTime = 0;

            if (dataFile != null)
            {
                try
                {
                    dataFile.Flush(); // 刷新输出
                }
                catch (Exception ex)
                {
                    ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK);
                    return;
                }
            }

            //FileOperator fOptor = new FileOperator();
            ////获取文件保存路径
            //string s = fOptor.getSavePath();
            //if (s != null)
            //{
            //    File.Move(fileName, s);
            //}

            //控件使能
            startMotoBtn.IsEnabled = true;
            startMotoBtn.Content = "启动电机";
            btnOpenPort.IsEnabled = true;
            spBox.IsEnabled = true;
            dataGrid.IsEnabled = true;
            constSpChecBox.IsEnabled = true;
            impCurBtn.IsEnabled = true;
            clearBtn.IsEnabled = true;
            setSpeedBtn.IsEnabled = true;
            MotoData.MotoTime = MotoData.N;
            RTDatas.uiDataRT.ProBarVisible = Visibility.Hidden;
        }

        #endregion

        //打开串口按钮
        private void btnOpenPort_Click(object sender, RoutedEventArgs e)
        {

            if (btnOpenPort.Content.ToString().Equals("打开串口"))
            {
                MainWindow.serPort.PortName = SPortNoCombox.Text;//设置串口号
                MainWindow.serPort.BaudRate = int.Parse(SPortBRCombox.Text);//设置波特率

                SysParam.PortName = SPortNoCombox.SelectedIndex + 1;
                SysParam.BaudRate = MainWindow.serPort.BaudRate;

                //设置校验位
                switch (SPortParityCombox.SelectedIndex)
                {
                    case 0:
                        MainWindow.serPort.Parity = Parity.None;
                        break;
                    case 1:
                        MainWindow.serPort.Parity = Parity.Odd;
                        break;
                    case 2:
                        MainWindow.serPort.Parity = Parity.Even;
                        break;
                    case 3:
                        MainWindow.serPort.Parity = Parity.Mark;
                        break;
                    case 4:
                        MainWindow.serPort.Parity = Parity.Space;
                        break;
                }

                SysParam.Parity = MainWindow.serPort.Parity.ToString();
               
                //设置停止位
                switch (SPortStopbitCombox.SelectedIndex)
                {
                    case 0:
                        MainWindow.serPort.StopBits = StopBits.None;
                        break;
                    case 1:
                        MainWindow.serPort.StopBits = StopBits.One;
                        break;
                    case 2:
                        MainWindow.serPort.StopBits = StopBits.Two;
                        break;
                    case 3:
                        MainWindow.serPort.StopBits = StopBits.OnePointFive;
                        break;
                }
                SysParam.StopBit = MainWindow.serPort.StopBits.ToString();

                //打开串口
                try
                {
                    MainWindow.serPort.Open();

                    btnOpenPort.Content = "关闭串口";
                    light.Fill = new SolidColorBrush(Color.FromRgb(18, 239, 46));
                    SPortNoCombox.IsEnabled = false;
                    SPortBRCombox.IsEnabled = false;
                    SPortParityCombox.IsEnabled = false;
                    SPortStopbitCombox.IsEnabled = false;

                    ModernDialog.ShowMessage(MainWindow.serPort.PortName + " 已打开！", "", MessageBoxButton.OK);

                    // Commented by Gaofei
                    // MainWindow.timer.Start();

                    //发布系统参数更改事件
                    if (OnSysParaUpdate != null)
                    {
                        OnSysParaUpdate(this, new EventArgs());
                    }

                }
                catch (Exception ex)
                {
                    ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                MainWindow.timer.Stop();//停止计时器
                //关闭串口
                try
                {
                    MainWindow.serPort.Close();
                    btnOpenPort.Content = "打开串口";
                    light.Fill = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    SPortNoCombox.IsEnabled = true;
                    SPortBRCombox.IsEnabled = true;
                    SPortParityCombox.IsEnabled = true;
                    SPortStopbitCombox.IsEnabled = true;

                }
                catch (Exception ex)
                {
                    ModernDialog.ShowMessage(ex.Message, "Message:", MessageBoxButton.OK);
                    return;
                }
            }
        }

        //恒定转速单选框
        private void constSpChecBox_Checked(object sender, RoutedEventArgs e) //选中
        {
            dataGrid.IsEnabled = false;
            spBox.IsEnabled = true;
        }
        private void constSpChecBox_Unchecked(object sender, RoutedEventArgs e) //未选中
        {
            dataGrid.IsEnabled = true;
            spBox.IsEnabled = false;
        }

        //确认电机转速曲线按钮
        private void setSpeedBtn_Click(object sender, RoutedEventArgs e)
        {
            int i = 0, j = 1, n = 0;
            double temp = 0; //缓存变量
            double k = 0; //曲线斜率
            int t = 0; //累加时长
            int interval = 0;//区间时长
            n = speedCurve.Count; //区间数量 
            spCurCollection.Clear();
            MotoData.MotoTime = 0;

            //绘制转速曲线图
            if (constSpChecBox.IsChecked == false) //表格方式控制曲线
            {
                //添加绘图曲线点
                for (i = 0; i < n; i++)
                {
                    spCurCollection.Add(new PlotPoint(temp, speedCurve[i].StaSpeed));
                    temp = temp + speedCurve[i].Minute * 60 + speedCurve[i].Second;
                    spCurCollection.Add(new PlotPoint(temp, speedCurve[i].EndSpeed));
                }

                //电机运行总时间
                MotoData.N = (int)temp;
                MotoData.SpeedData = new int[MotoData.N + 1];
                MotoData.SpeedData[0] = 0;

                //设置电机速度控制点
                for (i = 0; i < n; i++)
                {
                    interval = (int)(speedCurve[i].Minute * 60 + speedCurve[i].Second);
                    k = (speedCurve[i].EndSpeed - speedCurve[i].StaSpeed) / interval;
                    MotoData.SpeedData[j + t - 1] = (int)speedCurve[i].StaSpeed;
                    for (j = 1; j <= interval; j++)
                    {
                        MotoData.SpeedData[j + t] = MotoData.SpeedData[j + t - 1] + (int)k;
                    }
                    t = t + interval;
                    j = 1;
                }
            }
            else //恒转速控制方式
            {
                spCurCollection.Add(new PlotPoint(0, double.Parse(spBox.Text)));
                spCurCollection.Add(new PlotPoint(100000, double.Parse(spBox.Text)));
                MotoData.N = 100000;
                MotoData.SpeedData = new int[MotoData.N];

                try { int.Parse(spBox.Text); }
                catch (Exception ex) { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }               
                //设置电机速度控制点
                for (i = 0; i < MotoData.N; i++)
                {
                    MotoData.SpeedData[i] = int.Parse(spBox.Text);
                }
            }
            //确认曲线变量
            MotoData.CurveOK = true;
        }

        //曲线另存为按钮
        private void svCurBtn_Click(object sender, RoutedEventArgs e)
        {
            int i = 0, n = 0;
            FileOperator fOptor = new FileOperator();
            string s = null;
            n = speedCurve.Count;

            if (n > 0)
            {
                s = fOptor.getSavePath(); //获取曲线保存路径
                if (s != null)
                {
                    StreamWriter sw = new StreamWriter(s, false);
                    sw.WriteLine("电机转速曲线\t" + DateTime.Now.ToString());
                    sw.WriteLine("编号\t分钟\t秒\t起始转速\t末端转速");

                    for (i = 0; i < n; i++)
                    {
                        sw.WriteLine(speedCurve[i].Number.ToString() + "\t" +
                                     speedCurve[i].Minute.ToString() + "\t" +
                                     speedCurve[i].Second.ToString() + "\t" +
                                     speedCurve[i].StaSpeed.ToString() + "\t" +
                                     speedCurve[i].EndSpeed.ToString());
                    }
                    sw.Close();
                }
            }
            else
            {
                ModernDialog.ShowMessage("未设置曲线，无法保存！", "Message:", MessageBoxButton.OK);
            }

        }
        //导入曲线按钮
        private void impCurBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOperator fOptor = new FileOperator();
            string s = null;
            string line = null;
            string[] strs = null;
            MotoDataItem motoDataItem;

            //打开曲线文件路径
            s = fOptor.getOpenPath();
            if (s != null)
            {
                StreamReader sr = new StreamReader(s);
                //清空原有数据曲线，准备添加新的数据曲线
                speedCurve.Clear();

                line = sr.ReadLine();//读取文件头
                line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    //解析数据
                    strs = line.Split('\t');
                    motoDataItem = new MotoDataItem();
                    try
                    {
                        motoDataItem.Number = int.Parse(strs[0]);//编号
                        motoDataItem.Minute = double.Parse(strs[1]);//分钟
                        motoDataItem.Second = double.Parse(strs[2]);//秒
                        motoDataItem.StaSpeed = double.Parse(strs[3]);//起始转速
                        motoDataItem.EndSpeed = double.Parse(strs[4]);//末端转速
                    }
                    catch (Exception ex)
                    {
                        ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK);
                        break;
                    }
                    speedCurve.Add(motoDataItem);//添加数据曲线
                }
                sr.Close();
            }
        }
        //清空曲线
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            speedCurve.Clear();//清空DataGrid
            spCurCollection.Clear();//清空转速曲线
            spCurCollection.Add(new PlotPoint(0, 0));
            spCurCollection.Clear();//清空转速曲线
        }

        //启动电机Button
        private void startMotoBtn_Click(object sender, RoutedEventArgs e)
        {
            // 检查串口是否开启
            if (MainWindow.serPort.IsOpen == false)
            {
                ModernDialog.ShowMessage("串口未打开，请先打开串口！", "Message:", MessageBoxButton.OK);
                return;
            }
            
            // 检查速度曲线是否确认
            if (MotoData.CurveOK == true)
            {
                //创建文件用于记录实验数据
                try
                {
                    fileName = "D:\\Program Files\\TRIBOLOGY\\" + DateTime.Now.Year.ToString();
                    fileName = fileName + "-" + DateTime.Now.Month.ToString();
                    fileName = fileName + "-" + DateTime.Now.Day.ToString();
                    fileName = fileName + "-" + DateTime.Now.Hour.ToString();
                    fileName = fileName + "-" + DateTime.Now.Minute.ToString();
                    fileName = fileName + "-" + DateTime.Now.Second.ToString();
                    dataFile = new StreamWriter(fileName + ".txt", false);
                    dataFile.WriteLine("No.\t日期\t时间\t压力\t温度\t相对湿度\t平台转速\t电机转速\t电机扭矩\t摩擦力\t摩擦速度");
                    dataFile.Flush();
                }
                catch (Exception ex)
                {
                    ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK);
                    return;
                }

                // 清空绘图曲线
                Flag.SysTime = 0;
                PlotterData.platSpePtCollection.Clear();
                PlotterData.presPtCollection.Clear();
                PlotterData.fricForPtCollection.Clear();
                PlotterData.tempPtCollection.Clear();
                PlotterData.humPtCollection.Clear();
                PlotterData.motoSpeedPtCollection.Clear();
                PlotterData.fricSpeedPtCollection.Clear();
                PlotterData.motoTorPtCollection.Clear();

                spBox.IsEnabled = false;            //恒转速文本框
                dataGrid.IsEnabled = false;         //数据表格
                constSpChecBox.IsEnabled = false;   //恒转速单选框
                startMotoBtn.Content = "正在运行…";
                startMotoBtn.IsEnabled = false;
                impCurBtn.IsEnabled = false;
                clearBtn.IsEnabled = false;
                setSpeedBtn.IsEnabled = false;
                MotoData.StartMoto = true;          //标志变量，true表示电机已启动
                btnOpenPort.IsEnabled = false;

                MainWindow.sendData.start(MainWindow.serPort); //速度使能

                //开启定时器 
                if (MainWindow.timer.IsEnabled == false)
                {
                    MainWindow.timer.Start();
                }
            }
            else
            {
                ModernDialog.ShowMessage("请确认电机转速！", "Message:", MessageBoxButton.OK);
            }
        }

        //电机停转Button(旋转速度)
        private void stopMotoBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.timer.Stop(); // 停止计时器
            stopMotor(this,new EventArgs()); // 速度置0
            Thread.Sleep(100);
            MainWindow.sendData.stopMoto(MainWindow.serPort); // 去掉速度使能
        }

        //启动电机Button(旋转角度)
        private void startMotoBtn2_Click(object sender, RoutedEventArgs e)
        {
            double ang = 0;    // 旋转角度
            double speed = 0;  // 转速

            // 检查串口是否开启
            if (MainWindow.serPort.IsOpen == false)
            {
                ModernDialog.ShowMessage("串口未打开，请先打开串口！", "Message:", MessageBoxButton.OK);
                return;
            }

            // 解析输入的旋转角度和速度
            try
            {
                ang = Convert.ToDouble(angBox.Text);
                speed = Convert.ToDouble(angSpBox.Text);
                if (directCombox.SelectedIndex == 1)
                {
                    ang = ang * -1;
                }
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("数据输入有误，请重新输入！", "Message:", MessageBoxButton.OK);
                return;
            }

            // 位置使能
            MainWindow.sendData.startPosition(MainWindow.serPort);
            // 发送角度命令
            MainWindow.sendData.setAngle(ang, speed, MainWindow.serPort);
            
            // 参数输入控件失效
            angBox.IsEnabled = false;
            angSpBox.IsEnabled = false;
            directCombox.IsEnabled = false;
            startMotoBtn2.IsEnabled = false;
            SpdTab.IsEnabled = false;
        }

        //停机Button(旋转角度)
        private void stopMotoBtn2_Click(object sender, RoutedEventArgs e)
        {
            // 位置复位
            MainWindow.sendData.resetPosition(MainWindow.serPort);

            // 参数输入控件使能
            angBox.IsEnabled = true;
            angSpBox.IsEnabled = true;
            directCombox.IsEnabled = true;
            startMotoBtn2.IsEnabled = true;
            SpdTab.IsEnabled = true;

        }
    }
}
