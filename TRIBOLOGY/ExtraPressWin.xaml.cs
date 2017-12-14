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
using System.Windows.Shapes;

namespace TRIBOLOGY
{
    /// <summary>
    /// ExtraPressWin.xaml 的交互逻辑
    /// </summary>
    public partial class ExtraPressWin : Window
    {
        string temp_1 = MainWindow.sysParam.ExtraPress_1;
        string temp_2 = MainWindow.sysParam.ExtraPress_2;
        public ExtraPressWin()
        {
            InitializeComponent();
            this.DataContext = MainWindow.sysParam;
        }

        //确定按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.sysParam.ExtraPress_1 = TextBox_1.Text;
            MainWindow.sysParam.ExtraPress_2 = TextBox_2.Text;
            this.Close();
        }
        //取消按钮
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.sysParam.ExtraPress_1 = temp_1;
            MainWindow.sysParam.ExtraPress_2 = temp_2;
            this.Close();
        }

    }
}
