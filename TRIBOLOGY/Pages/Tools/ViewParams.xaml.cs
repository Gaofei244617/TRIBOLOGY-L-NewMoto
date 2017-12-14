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
using System.IO;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for ViewParams.xaml
    /// </summary>
    public partial class ViewParams : UserControl
    {
        string s = null;
        public ViewParams()
        {
            InitializeComponent();
            //订阅事件
            SetParam.OnSysParaUpdate += new SysParaUpdate(this.ViewSysParam);
            SystemSet.OnSysParaUpdate += new SysParaUpdate(this.ViewSysParam);
            ViewSysParam(this,new EventArgs());
        }
        //查看系统参数
        void ViewSysParam(object sender, EventArgs e)
        {
            s = "[b]串口号：[/b]\t\t" + SysParam.PortName.ToString() + "\n\n";
            s += "[b]波特率：[/b]\t\t" + SysParam.BaudRate.ToString() + "\n\n";
            s += "[b]校验位：[/b]\t\t" + SysParam.Parity + "\n\n";
            s += "[b]停止位：[/b]\t\t" + SysParam.StopBit + "\n\n";
            s += "[b]力臂-1长度：[/b]\t\t" + SysParam.Arm_1.ToString() + " mm" + "\n\n";
            s += "[b]力臂-2长度：[/b]\t\t" + SysParam.Arm_2.ToString() + " mm" + "\n\n";
            s += "[b]力臂比值：[/b]\t\t" + SysParam.ArmRatio.ToString() + "\n\n";
            s += "[b]压力1附加值：[/b]\t\t" + SysParam.ExtraPress_1.ToString() + " N" + "\n\n";
            s += "[b]压力2附加值：[/b]\t\t" + SysParam.ExtraPress_2.ToString() + " N";

            this.TextBlock.BBCode = s;
        }
        //另存为按钮
        private void SaveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOperator fOptor = new FileOperator();
            string str = fOptor.getSavePath(); //获取曲线保存路径
            if (str != null)
            {
                StreamWriter sw = new StreamWriter(s, false);
                sw.Write(s);
                sw.Close();
            }
        }
    }
}
