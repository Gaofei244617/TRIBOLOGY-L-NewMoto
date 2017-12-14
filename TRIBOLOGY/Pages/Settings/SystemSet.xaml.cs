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
    /// Interaction logic for SystemSet.xaml
    /// </summary>
    public partial class SystemSet : UserControl
    {
        internal static event SysParaUpdate OnSysParaUpdate;
        //构造函数
        public SystemSet()
        {
            InitializeComponent();
            UpdateUI();
        }

        //更新页面数据
        void UpdateUI()
        {
            arm_1.Text = SysParam.Arm_1.ToString();
            arm_2.Text = SysParam.Arm_2.ToString();
            ratio.Text = SysParam.ArmRatio.ToString();
            TextBox_1.Text = SysParam.ExtraPress_1.ToString();
            TextBox_2.Text = SysParam.ExtraPress_2.ToString();
            if (SysParam.FricRadius < 0)
                fricRadius.Text = "";
            else
                fricRadius.Text = SysParam.FricRadius.ToString();
        }
        //力臂比值单选框
        private void Checked(object sender, RoutedEventArgs e) //选中
        {
            arm_1.IsEnabled = false;
            arm_2.IsEnabled = false;
            arm_1.Text = "";
            arm_2.Text = "";
            ratio.IsEnabled = true;
        }
        private void Unchecked(object sender, RoutedEventArgs e) //未选中
        {
            arm_1.IsEnabled = true;
            arm_2.IsEnabled = true;
            ratio.IsEnabled = false;
        }
        //力臂确定按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double temp = 0;
            if (armRatCheckBox.IsChecked == true)
            {
                try { temp = double.Parse(ratio.Text); }
                catch (Exception ex) { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); }
            }
            else
            {
                try
                {
                    temp = double.Parse(arm_1.Text) / double.Parse(arm_2.Text);
                    ratio.Text = ((int)(temp * 1000) / 1000.0).ToString();
                }
                catch (Exception ex)
                { ModernDialog.ShowMessage(ex.ToString(), "", MessageBoxButton.OK); return; }
            }

            MessageBoxResult result = ModernDialog.ShowMessage("力臂比值：" + temp.ToString(), "确认", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                if (arm_1.Text != "" && arm_2.Text != "")
                {
                    try
                    {
                        SysParam.Arm_1 = double.Parse(arm_1.Text);
                        SysParam.Arm_2 = double.Parse(arm_2.Text);
                    }
                    catch (Exception ex) { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }
                }
                SysParam.ArmRatio = temp;
            }
            UpdateUI();
            if (OnSysParaUpdate != null)
                OnSysParaUpdate(this, new EventArgs());
        }
        //取消按钮
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }
        
        //零点附加值确定按钮
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SysParam.ExtraPress_1 = double.Parse(TextBox_1.Text);
                SysParam.ExtraPress_2 = double.Parse(TextBox_2.Text);
            }
            catch (Exception ex) { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }
            UpdateUI();
            if (OnSysParaUpdate != null)
                OnSysParaUpdate(this, new EventArgs());
        }
        //取消按钮
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }
        
        //摩擦速度确定按钮
        private void FROK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SysParam.FricRadius = double.Parse(fricRadius.Text);
            }
            catch (Exception ex) { ModernDialog.ShowMessage(ex.ToString(), "Message:", MessageBoxButton.OK); return; }
            UpdateUI();
            if (OnSysParaUpdate != null)
                OnSysParaUpdate(this, new EventArgs());
        }
        //摩擦速度取消按钮
        private void FRCancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

    }
}
