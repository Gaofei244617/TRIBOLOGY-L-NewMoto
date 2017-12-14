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
using FirstFloor.ModernUI.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        #region 自定义变量
        //停机事件
        internal static event StopMoto OnStopMoto;
        //保存文件
        internal static event SaveFiles OnSaveFile;
        //定时器
        public static DispatcherTimer timer = new DispatcherTimer();
        //串口对象
        public static SerialPort serPort = new SerialPort();
        //发送数据对象
        public static SendData sendData = new SendData();
        //串口收到的数据
        List<byte> dataRec = new List<byte>();

        //计时器标志变量
        int flag = 0;

        //********************************************************************
        //public static StreamWriter RecInfoFile = new StreamWriter("D:\\RecInfo.txt", true);
        //********************************************************************

        #endregion

        //构造函数
        public MainWindow()
        {
            InitializeComponent();
            IniSystem();
        }

        #region 自定义函数
        //初始化函数
        void IniSystem()
        {
            //***************************************************************************
            //RecInfoFile.WriteLine("********************************************");
            //RecInfoFile.Flush();
            //***************************************************************************

            //创建数据文件目录
            if (!Directory.Exists("D:\\Program Files\\TRIBOLOGY"))
            {
                Directory.CreateDirectory("D:\\Program Files\\TRIBOLOGY");
            }

            //读取力臂比值
            if (File.Exists("D:\\Program Files\\TRIBOLOGY\\SysParam.txt"))
            {
                StreamReader paramReader = new StreamReader("D:\\Program Files\\TRIBOLOGY\\SysParam.txt");
                string line = null;
                string[] strs = null;
                while ((line = paramReader.ReadLine()) != null)
                {
                    try
                    {
                        //解析数据
                        strs = line.Split('\t');
                        switch (strs[0])
                        {
                            case "串口号：":
                                SysParam.PortName = int.Parse(strs[1]);
                                break;
                            case "波特率：":
                                SysParam.BaudRate = int.Parse(strs[1]);
                                break;
                            case "校验位：":
                                SysParam.Parity = strs[1];
                                break;
                            case "停止位：":
                                SysParam.StopBit = strs[1];
                                break;
                            case "力臂-1：":
                                if (strs[1] != "")
                                    SysParam.Arm_1 = double.Parse(strs[1]);
                                break;
                            case "力臂-2：":
                                if (strs[1] != "")
                                    SysParam.Arm_2 = double.Parse(strs[1]);
                                break;
                            case "力臂比值：":
                                SysParam.ArmRatio = double.Parse(strs[1]);
                                break;
                            case "压力-1零点附加值：":
                                SysParam.ExtraPress_1 = double.Parse(strs[1]);
                                break;
                            case "压力-2零点附加值：":
                                SysParam.ExtraPress_2 = double.Parse(strs[1]);
                                break;
                        }
                    }
                    catch (Exception e) { MessageBox.Show("解析数据出现问题\n"+e.ToString()); return; }
                }
                paramReader.Close();
            }
            else
            {
                StreamWriter paramFile = new StreamWriter("D:\\Program Files\\TRIBOLOGY\\SysParam.txt");
                paramFile.Close();
                ModernDialog.ShowMessage("首次运行，未找到系统配置文件，将使用默认参数，请及时设置……","Message:",MessageBoxButton.OK);
            }
            //使用事件方式处理串口接收到的数据
            serPort.DataReceived += new SerialDataReceivedEventHandler(seriReceive);

            //定时器，计时周期250ms
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(Timer_Tick);

        }
        //更新UI界面数据曲线
        void updateDataCurve(string s, PlotPoint Pt)
            {

                switch (s)
                {
                    case "PlateSpeed":
                        PlotterData.platSpePtCollection.Add(Pt); //平台转速
                        break;
                    case "Pressure":
                        PlotterData.presPtCollection.Add(Pt); //压力
                        break;
                    case "FricForce":
                        PlotterData.fricForPtCollection.Add(Pt); //摩擦力
                        break;
                    case "Temperature":
                        PlotterData.tempPtCollection.Add(Pt); //温度
                        break;
                    case "Humidity":
                        PlotterData.humPtCollection.Add(Pt); //湿度
                        break;
                    case "MotoSpeed":
                        PlotterData.motoSpeedPtCollection.Add(Pt); //湿度
                        break;
                    case "MotoTor":
                        PlotterData.motoTorPtCollection.Add(Pt); //湿度
                        break;
                    case "FricSpeed":
                        PlotterData.fricSpeedPtCollection.Add(Pt); //湿度
                        break;
                }
            }

        //定时器事件响应（轮询查询）
        void Timer_Tick(object sender, EventArgs e)
        {
            //SendData.testFile.WriteLine("AAAA");
            //SendData.testFile.Flush();

            //查询功能标志
            flag++;
            if (flag > 4)
            {
                flag = 1;
            }
            Flag.SysTime += 0.25;
            
            //以轮询方式查询系统参数
            switch (flag)
            {
                case 1:
                    if (MotoData.StartMoto == true)           //电机是否启动
                    {
                        MotoData.MotoTime++;
                        if (MotoData.MotoTime <= MotoData.N)  //未到达运行总时间
                        {
                            Flag.nFunc = 1;                   //功能标志
                            Flag.nFlag = 8;                   //接收字节数标志
                            dataRec.Clear();
                            //***********************待验证**************************************
                            serPort.DiscardInBuffer();
                            //********************************************************************
                            sendData.setMotoSpeed(MotoData.SpeedData[MotoData.MotoTime], serPort);
                            //进度条
                            RTDatas.uiDataRT.LeftTime = "剩余时间：" + (MotoData.N - MotoData.MotoTime).ToString() + "s";
                            RTDatas.uiDataRT.ProBar = (double)(MotoData.MotoTime) / MotoData.N;
                            RTDatas.uiDataRT.ProBarTex = ((int)(RTDatas.uiDataRT.ProBar * 1000) / 10.0).ToString() + "%";
                            RTDatas.uiDataRT.ProBarVisible = Visibility.Visible;
                        }
                        else
                        {
                            // 发布事件
                            if (OnStopMoto != null)
                                OnStopMoto(this, new EventArgs());

                            if (Flag.RecData == true)
                            {
                                if (OnSaveFile != null)
                                {
                                    OnSaveFile(this, new EventArgs());
                                }
                            }

                            Flag.nFunc = 1; //功能标志
                            Flag.nFlag = 8; //接收字节数标志
                            sendData.setMotoSpeed(0, serPort);

                            ModernDialog.ShowMessage("电机运行结束！", "Message:", MessageBoxButton.OK);
                            MotoData.MotoTime = 0;
                            RTDatas.uiDataRT.ProBarVisible = Visibility.Hidden;
                        }
                    }
                    //SendData.testFile.WriteLine("BBBB");
                    //SendData.testFile.Flush();

                    break;

                //查询电机转速
                case 2:
                    Flag.nFunc = 2; //功能标志
                    Flag.nFlag = 7; //接收字节数标志
                    dataRec.Clear();
                    //***********************待验证**************************************
                    serPort.DiscardInBuffer();
                    //********************************************************************
                    sendData.qurMotoSpeed(serPort);
                    //SendData.testFile.WriteLine("CCCC");
                    //SendData.testFile.Flush();
                    break;

                //查询电机扭矩
                case 3:
                    Flag.nFunc = 3;
                    Flag.nFlag = 7;
                    dataRec.Clear();
                    //***********************待验证**************************************
                    serPort.DiscardInBuffer();
                    //********************************************************************
                    sendData.qurMotoTorque(serPort);
                    //SendData.testFile.WriteLine("DDDD");
                    //SendData.testFile.Flush();
                    break;

                //查询温湿度及压力
                case 4:
                    Flag.nFunc = 4;
                    Flag.nFlag = 27;
                    dataRec.Clear();
                    //***********************待验证**************************************
                    serPort.DiscardInBuffer();
                    //********************************************************************
                    sendData.qurHumTemPre(serPort);
                    //SendData.testFile.WriteLine("EEEE");
                    //SendData.testFile.Flush();
                    break;
            }
        }

        //串口事件响应（在辅助线程中执行，无法直接跨线程更新UI界面）
        private void seriReceive(object sender, SerialDataReceivedEventArgs e)
        {
            //********************************************************************
            //RecInfoFile.WriteLine("Serial port respond：Begin");
            //RecInfoFile.Flush();
            //********************************************************************

            int n = serPort.BytesToRead;//获取接收缓冲区中数据的字节数
            byte[] temp = new byte[n];

            serPort.Read(temp, 0, n);//读取串口数据
            dataRec.AddRange(temp);//数据拼接

            //********************************************************************
            //RecInfoFile.WriteLine("Flag.nFlag：" + Flag.nFlag.ToString()+"\nNum of serialport-data bytes：" + dataRec.Count.ToString());
            //RecInfoFile.Flush();
            //********************************************************************

            if (dataRec.Count >= Flag.nFlag)//数据完整性校验
            {
                //清空接收缓冲区
                serPort.DiscardInBuffer();
                byte[] receiveData = dataRec.ToArray();
                dataRec.Clear();

                //使用新线程处理串口接收到的数据，避免阻塞串口
                
                //********************************************************************
                //RecInfoFile.WriteLine("New thread：Begin");
                //RecInfoFile.Flush();
                //********************************************************************

                Thread dataHandlerThread = new Thread(new ParameterizedThreadStart(DataHandlerThread));
                dataHandlerThread.Start(receiveData);

                //********************************************************************
                //RecInfoFile.WriteLine("New thread：End");
                //RecInfoFile.Flush();
                //********************************************************************

            }
            //********************************************************************
            //RecInfoFile.WriteLine("Serial port respond：End");
            //RecInfoFile.Flush();
            //********************************************************************

        }

        //串口数据处理
        private void DataHandlerThread(object data)
        {
            //形参data为串口接收的数据
            int minusLocation = -1;
            byte[] receiveData = data as byte[];
            //用于将byte[]数组解析成字符串
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            //********************************************************************
            //RecInfoFile.WriteLine("Enter the DataHandlerThread subroutine.");
            //RecInfoFile.Flush();
            //********************************************************************

            //更新数据
            switch (Flag.nFunc)
            {
                case 1: //设置电机转速
                    //********************************************************************
                    //RecInfoFile.WriteLine("Rec:1");
                    //RecInfoFile.Flush();
                    //********************************************************************
                    break;

                case 2: //电机转速
                    //********************************************************************
                    //RecInfoFile.WriteLine("Rec:2");
                    //RecInfoFile.Flush();
                    //********************************************************************
                    Data.MotoSpeed = receiveData[3] * 256 + receiveData[4];
                    Data.PlatSpeed = (int)(Data.MotoSpeed /3.0 * 10) / 10.0;
                    Data.FricSpeed = (int)(Data.PlatSpeed / 60 * SysParam.FricRadius * 100) / 100.0;
                    RTDatas.uiDataRT.MotoSpeed = Data.MotoSpeed.ToString();
                    RTDatas.uiDataRT.PlatSpeed = Data.PlatSpeed.ToString();
                    RTDatas.uiDataRT.FricSpeed = Data.FricSpeed.ToString();
                    
                    //更新界面
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "PlateSpeed", new PlotPoint(Flag.SysTime, Data.PlatSpeed));
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "MotoSpeed", new PlotPoint(Flag.SysTime, Data.MotoSpeed));
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "FricSpeed", new PlotPoint(Flag.SysTime, Data.FricSpeed));
                    break;

                case 3: //电机扭矩
                    //********************************************************************
                    //RecInfoFile.WriteLine("Rec:3");
                    //RecInfoFile.Flush();
                    //********************************************************************
                    Data.MotoTorque = receiveData[3] * 256 + receiveData[4];
                    RTDatas.uiDataRT.MotoTorque = Data.MotoTorque.ToString();
                    //更新界面
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "MotoTor", new PlotPoint(Flag.SysTime, Data.MotoTorque));
                    break;

                case 4: //查询温湿度及压力
                    //********************************************************************
                    //RecInfoFile.WriteLine("Rec:4");
                    //RecInfoFile.Flush();
                    ////**************************************************************************************************

                    ////**************************************************************************************************
                    //RecInfoFile.WriteLine("Expect Bytes：" + Flag.nFlag.ToString());
                    //RecInfoFile.WriteLine("Actual Byte：" + receiveData.Length.ToString());
                    //RecInfoFile.Flush();

                    string s = asciiEncoding.GetString(receiveData);
                    s = s.Substring(0, s.Length - 2);
                    //判断是否出现负号
                    minusLocation = s.IndexOf("-");
                    if (minusLocation >= 0)
                        s = s.Insert(minusLocation, "+");

                    string[] strs = s.Split('+');
                  
                    ////**************************************************************************************************
                    //RecInfoFile.WriteLine("string[] strs：" + s);
                    //RecInfoFile.Flush();

                    try
                    {
                        Data.Pressure1 = double.Parse(strs[1]) + SysParam.ExtraPress_1;
                        Data.Pressure2 = double.Parse(strs[2]) + SysParam.ExtraPress_2;
                        Data.Temperature = double.Parse(strs[3]);
                        Data.Humidity = double.Parse(strs[4]);
                    }
                    catch (Exception e)
                    {
                        ModernDialog.ShowMessage("预计:" + Flag.nFlag.ToString() + "\n实际：" + receiveData.GetLength(0).ToString() + "\n解析：" + strs.Length.ToString() + "\n" + e.ToString(), "Message:", MessageBoxButton.OK);
                    }

                    ////**************************************************************************************************
                    //Data.Pressure1 = 3.14;
                    //Data.Pressure2 = 2.71;
                    //Data.Temperature = 1.234;
                    //Data.Humidity = 10;
                    ////**************************************************************************************************

                    RTDatas.uiDataRT.Pressure = Data.Pressure1.ToString();
                    RTDatas.uiDataRT.FricForce = (Data.Pressure2 * SysParam.ArmRatio).ToString();
                    RTDatas.uiDataRT.Temperature = Data.Temperature.ToString();
                    RTDatas.uiDataRT.Humidity = Data.Humidity.ToString();

                    //将系统参数写出到文件
                    if (MotoData.StartMoto == true)
                    {
                        string str = MotoData.MotoTime.ToString() + "\t" + DateTime.Now.ToString("yyyy-MM-dd") + "\t" + DateTime.Now.ToString("HH:mm:ss") + "\t";
                        str = str + RTDatas.uiDataRT.Pressure + "\t" + RTDatas.uiDataRT.Temperature + "\t" + RTDatas.uiDataRT.Humidity + "\t";
                        str = str + RTDatas.uiDataRT.PlatSpeed + "\t" + RTDatas.uiDataRT.MotoSpeed + "\t" + RTDatas.uiDataRT.MotoTorque + "\t";
                        str = str + RTDatas.uiDataRT.FricForce + "\t" + RTDatas.uiDataRT.FricSpeed;
                        SetParam.dataFile.WriteLine(str);
                        //写出数据
                        if (Flag.RecData == true)
                            RTDatas.mydataFile.WriteLine(str);
                    }
                    //更新界面
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "Pressure", new PlotPoint(Flag.SysTime, Data.Pressure1));
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "Temperature", new PlotPoint(Flag.SysTime, Data.Temperature));
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "Humidity", new PlotPoint(Flag.SysTime, Data.Humidity));
                    Dispatcher.Invoke(new InterfaceUpdate(updateDataCurve), "FricForce", new PlotPoint(Flag.SysTime, Data.Pressure2 * SysParam.ArmRatio));

                    break;
            }
        }

        #endregion
        //关闭窗口
        private void ClosingWin(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = new MessageBoxResult();
            result = ModernDialog.ShowMessage("关闭窗口并退出程序？", "请确认", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                timer.Stop();//停止计时
                if (MotoData.StartMoto == true)
                    SetParam.dataFile.Close();
                if (serPort.IsOpen == true)
                {
                    sendData.setMotoSpeed(0, serPort);
                    serPort.Close();//关闭串口
                }
                if (Flag.RecData == true)
                {
                    //关闭文件
                    try
                    {
                        RTDatas.mydataFile.Close();
                    }
                    catch (Exception ex)
                    { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }

                    FileOperator fOptor = new FileOperator();
                    //获取文件保存路径
                    string s = fOptor.getSavePath();
                    File.Move("D:\\Program Files\\TRIBOLOGY\\temp.txt", s);
                }
                //写出系统状态参数
                try
                {
                    StreamWriter paramFile = new StreamWriter("D:\\Program Files\\TRIBOLOGY\\SysParam.txt");
                    paramFile.WriteLine("串口号：\t" + SysParam.PortName.ToString());
                    paramFile.WriteLine("波特率：\t" + SysParam.BaudRate.ToString());
                    paramFile.WriteLine("校验位：\t" + SysParam.Parity);
                    paramFile.WriteLine("停止位：\t" + SysParam.StopBit);
                    paramFile.WriteLine("力臂-1：\t" + SysParam.Arm_1.ToString());
                    paramFile.WriteLine("力臂-2：\t" + SysParam.Arm_2.ToString());
                    paramFile.WriteLine("力臂比值：\t" + SysParam.ArmRatio.ToString());
                    paramFile.WriteLine("压力-1零点附加值：\t" + SysParam.ExtraPress_1.ToString());
                    paramFile.WriteLine("压力-2零点附加值：\t" + SysParam.ExtraPress_2.ToString());
                    paramFile.Close();
                }
                catch (Exception ex)
                {
                    ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}