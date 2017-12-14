using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//引用命名空间
using System.Collections.ObjectModel;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Windows.Threading;
//using Microsoft.Win32;
using System.IO;
using System.IO.Ports;
//using System.Threading;


namespace TRIBOLOGY
{
    
    class GlobalVars
    {
        #region 全局变量

        //定时器
        public static DispatcherTimer timer = new DispatcherTimer();
        //串口对象
        public static SerialPort serPort = new SerialPort();
        //发送数据对象
        public static SendData sendData = new SendData();
        //系统参数对象
        public static SysParam sysParam = new SysParam();

        //系统当前参数
        public static Data data = new Data();
        //UI界面数据
        public static UIData uiData = new UIData();
        //实验时间
        public static double time = 0;
        //计时器标志变量
        public static int flag = 0;
        //是否自主选定记录数据
        public static bool recData = false;
        //是否启动电机,用于判断定时器是否发送设置转速命令,及关闭窗口时是否关闭文件
        public static bool startMotoFlag = false;
        //电机曲线确认
        public static bool curveOK = false;
        //电机运行总时间(秒)
        public static int N = 0;
        //电机当前运行时间点(秒)
        public static int motoTime = 0;
        //电机速度曲线数据点
        public static int[] speedData;
        //串口收到的数据
        public static List<byte> dataRec = new List<byte>();
        //用于将byte[]数组解析成字符串
        public static ASCIIEncoding asciiEncoding = new ASCIIEncoding();

        //功能标志和接收字节数标志
        //功能： 1设定转速，2查询转速，3查询扭矩，4查询温湿度及压力
        //接收字节： 1（8字节），2（7字节），3（7字节），4（27字节）
        public static int nFunc = 0;
        public static int nFlag = 0;

        //委托，用于更新UI界面数据曲线
        delegate void interfaceUpdate(string str, PlotPoint Pt);

        //电机数据曲线集合(数据表)
        public static ObservableCollection<MotoDataItem> speedCurve = new ObservableCollection<MotoDataItem>();

        //文件数据集合
        public static ObservableCollection<FileDataItem> fileData = new ObservableCollection<FileDataItem>();

        //所要添加的平均值曲线
        public static LineGraph avrGraphLine = new LineGraph();

        //坐标点数据集合
        public static DataPointCollection presPtCollection = new DataPointCollection(); //压力坐标点集合
        public static DataPointCollection humPtCollection = new DataPointCollection(); //相对湿度坐标点集合
        public static DataPointCollection tempPtCollection = new DataPointCollection(); //温度坐标点集合
        public static DataPointCollection platSpePtCollection = new DataPointCollection(); //平台转速坐标点集合
        public static DataPointCollection fricForPtCollection = new DataPointCollection(); //摩擦力坐标点集合
        public static DataPointCollection motoTorPtCollection = new DataPointCollection();//电机扭矩曲线集合
        public static DataPointCollection motoSpeedPtCollection = new DataPointCollection();//电机转速曲线集合
        public static DataPointCollection spCurCollection = new DataPointCollection(); //电机转速曲线集合
        public static FDataPointCollection filePtCollection = new FDataPointCollection(); //文件数据坐标点集合
        public static FDataPointCollection avrFilePt = new FDataPointCollection(); //平均值曲线数据点集合(绘图)

        //以集合对象为实参，实例化曲线数据源,集合元素为<>内部类型的对象
        public static EnumerableDataSource<PlotPoint> pressure = new EnumerableDataSource<PlotPoint>(presPtCollection);
        public static EnumerableDataSource<PlotPoint> humidity = new EnumerableDataSource<PlotPoint>(humPtCollection);
        public static EnumerableDataSource<PlotPoint> temperature = new EnumerableDataSource<PlotPoint>(tempPtCollection);
        public static EnumerableDataSource<PlotPoint> platSpeed = new EnumerableDataSource<PlotPoint>(platSpePtCollection);
        public static EnumerableDataSource<PlotPoint> fricForce = new EnumerableDataSource<PlotPoint>(fricForPtCollection);
        public static EnumerableDataSource<PlotPoint> motoTor = new EnumerableDataSource<PlotPoint>(motoTorPtCollection);
        public static EnumerableDataSource<PlotPoint> motoSpeed = new EnumerableDataSource<PlotPoint>(motoSpeedPtCollection);
        public static EnumerableDataSource<PlotPoint> motoSpCur = new EnumerableDataSource<PlotPoint>(spCurCollection);
        public static EnumerableDataSource<PlotPoint> plotLine = new EnumerableDataSource<PlotPoint>(filePtCollection);

        //新窗口
        public static PlotWin tempWin;
        public static PlotWin humWin;
        public static PlotWin speedWin;
        public static PlotWin platWin;
        public static PlotWin pressWin;
        public static PlotWin fricWin;
        public static PlotWin torWin;

        //数据文件
        public static StreamWriter dataFile;
        public static StreamWriter mydataFile;

        #endregion

    }
}
