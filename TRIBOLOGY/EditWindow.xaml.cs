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

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : ModernWindow
    {
        internal event EditPlot OnEditPlot;
        public EditWindow(string s)
        {
            InitializeComponent();
            this.Title = s;
        }

        //确定
        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
            if(OnEditPlot != null)
                OnEditPlot(this,new PlotEventArgs(this.Title + ":" + TBox.Text));
            this.Close();
        }
        //取消
        private void ModernButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //TextBox中按下回车键
        private void EnterDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ModernButton_Click(this,new RoutedEventArgs());
            }
        }
    }
}
