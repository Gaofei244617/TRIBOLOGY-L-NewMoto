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
//添加引用
using System.IO;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class RTDatas : UserControl
    {
        internal static event StopMoto OnStopMoto; //停机事件
        public static UIDataRT uiDataRT = new UIDataRT();
        public static StreamWriter mydataFile;
        PlotterData plotterData = new PlotterData();

        //构造函数
        public RTDatas()
        {
            InitializeComponent();
            //设定数据上下文
            this.DataContext = uiDataRT;
            MainWindow.OnSaveFile += new SaveFiles(this.SaveFile);
        }
        //停止记录后保存文件
        void SaveFile(object sender, EventArgs e)
        {
            Flag.RecData = false;
            //关闭文件
            try
            {
                mydataFile.Close();
            }
            catch (Exception ex)
            { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }

            FileOperator fOptor = new FileOperator();
            //获取文件保存路径
            string s = fOptor.getSavePath();
            if (s != null)
            {
                File.Move("D:\\Program Files\\TRIBOLOGY\\temp.txt", s);
            }

            recDataBtn.Content = "记录数据";
            recDataBtn.IsEnabled = true;
        }
        //电机停转按钮
        private void stopMotoBtn_Click(object sender, RoutedEventArgs e)
        {
            // 触发停机事件
            OnStopMoto(this, new EventArgs());
        }
        //记录数据
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mydataFile = new StreamWriter("D:\\Program Files\\TRIBOLOGY\\temp.txt", false);
                mydataFile.WriteLine("No.\t日期\t时间\t压力\t温度\t相对湿度\t平台转速\t电机转速\t电机扭矩\t摩擦力");
                Flag.RecData = true;
            }
            catch (Exception ex)
            { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }
            Flag.RecData = true;

            recDataBtn.Content = "正在记录…";
            recDataBtn.IsEnabled = false;
        }
        //停止记录
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFile(this, new EventArgs());
        }

        #region Show New Window
        //显示温度窗口
        private void ShowTempWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.temperature,"Temperature");
            pw.Show();
        }
        private void ShowPressWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.pressure, "Pressure");
            pw.Show();
        }
        private void ShowPltspeedWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.platSpeed, "PlatSpeed");
            pw.Show();
        }
        private void ShowFricWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.fricForce, "FricForce");
            pw.Show();
        }
        private void ShowHumWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.humidity, "Humidity");
            pw.Show();
        }
        private void ShowTorWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.motoTor, "MotorTorque");
            pw.Show();
        }
        private void ShowMotospeedWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.motoSpeed, "MotorSpeed");
            pw.Show();
        }
        private void ShowFricSpeedWin(object sender, MouseButtonEventArgs e)
        {
            PlotWin pw = new PlotWin(plotterData.fricSpeed, "FricSpeed");
            pw.Show();
        }

        #endregion
    }
}
