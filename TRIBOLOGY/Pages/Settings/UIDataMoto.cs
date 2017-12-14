using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//添加引用
using System.ComponentModel;

namespace TRIBOLOGY
{
    public class UIDataMoto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool btnEnable = true; //启动电机按钮
        string btnContetn = "启动电机";
        bool spBoxEnable = true; //恒转速文本框
        bool dataGridEnable = true; 
        bool checkBox = false; //恒转速单选框

        public bool BtnEnable
        {
            get { return btnEnable; }
            set { btnEnable = value; Notify("BtnEnable"); }
        }

        public string BtnContetn
        {
            get { return btnContetn; }
            set { btnContetn = value; Notify("BtnContetn"); }
        }

        public bool SpBoxEnable
        {
            get { return spBoxEnable; }
            set { spBoxEnable = value; Notify("SpBoxEnable"); }
        }
        public bool DataGridEnable
        {
            get { return dataGridEnable; }
            set { dataGridEnable = value; Notify("DataGridEnable"); }
        }
        public bool CheckBox
        {
            get { return checkBox; }
            set { checkBox = value; Notify("CheckBox"); }
        }

        //触发 PropertyChanged 事件的通用方法
        void Notify(string str)//形参为值发生改变的属性名称
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
    }
}
