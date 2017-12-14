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
using FirstFloor.ModernUI.Windows.Controls;

namespace TRIBOLOGY
{
    /// <summary>
    /// Interaction logic for ExtraPress.xaml
    /// </summary>
    public partial class ExtraPress : UserControl
    {
        internal static event SysParaUpdate OnSysParaUpdate;

        public ExtraPress()
        {
            InitializeComponent();
            UpdateUI();
        }
        void UpdateUI()
        {
            TextBox_1.Text = SysParam.ExtraPress_1.ToString();
            TextBox_2.Text = SysParam.ExtraPress_2.ToString();
        }

        //确定按钮
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try {
                SysParam.ExtraPress_1 = double.Parse(TextBox_1.Text);
                SysParam.ExtraPress_2 = double.Parse(TextBox_2.Text);
            }
            catch (Exception ex) { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }
            UpdateUI();
            OnSysParaUpdate(this,new EventArgs());
        }
        //取消按钮
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

    }
}
