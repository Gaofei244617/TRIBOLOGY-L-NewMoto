using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace TRIBOLOGY
{
    class FileOperator
    {
        SaveFileDialog dlg = new SaveFileDialog();
        OpenFileDialog ofd = new OpenFileDialog();

        //获取保存文件的绝对路径
        public string getSavePath()
        {
            Nullable<bool> result = dlg.ShowDialog();//true:保存； false:取消
            string s = null;
            if (result == true)
            {
                s = dlg.FileName;
                string[] str = s.Split('.');
                if (!str[str.Length - 1].Equals("txt"))
                    s = s + ".txt";
            }
            return s;
        }

        //获取打开文件的绝对路径
        public string getOpenPath()
        {
            string s = null;
            ofd.Filter = "文本文件(.txt)|*.txt";
            ofd.FileName = null;
            Nullable<bool> result = ofd.ShowDialog();//true:保存； false:取消
            if (result == true)
            {
                s = ofd.FileName;
            }
            return s;
        }
    }
}
