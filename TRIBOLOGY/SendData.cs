using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Threading;
using System.Linq;
using System.IO.Ports;
using System.Windows;
using System.IO;

namespace TRIBOLOGY
{
    public class SendData
    {
        const byte motoAddress = 0x02;   //电机驱动器地址        
        CRC dataCheck = new CRC();       //数据校验对象

        byte[] enableSpeed  = { motoAddress, 0x06, 0x08, 0x00, 0x01, 0x01, 0, 0 };  // 速度使能
        byte[] speedData    = { motoAddress, 0x06, 0x08, 0x02, 0x00, 0x00, 0, 0 };  // 给定速度指令
        byte[] speedFwd     = { motoAddress, 0x06, 0x08, 0x00, 0x01, 0x11, 0, 0 };  // 给定速度正方向
        byte[] speedRev     = { motoAddress, 0x06, 0x08, 0x00, 0x01, 0x21, 0, 0 };  // 给定速度反方向
        byte[] disEnableSpd = { motoAddress, 0x06, 0x08, 0x00, 0x00, 0x01, 0, 0 };  // 速度模式去掉使能
        byte[] pulseH       = { motoAddress, 0x06, 0x08, 0x04, 0x00, 0x00, 0, 0 };  // 位置脉冲高位
        byte[] pulseL       = { motoAddress, 0x06, 0x08, 0x05, 0x00, 0x00, 0, 0 };  // 位置脉冲低位
        byte[] disEnableLoc = { motoAddress, 0x06, 0x08, 0x00, 0x00, 0x00, 0, 0 };  // 位置模式去掉使能
        byte[] resetLoc     = { motoAddress, 0x06, 0x08, 0x00, 0x04, 0x00, 0, 0 };  // 位置复位
        byte[] enableLoc    = { motoAddress, 0x06, 0x08, 0x00, 0x01, 0x00, 0, 0 };  // 位置使能

        //*******************************************************************************
        public SendData()
        {
        }
        //*******************************************************************************

        // 速度使能(交流电机)
        public void start(SerialPort sp)
        {
            // 速度使能
            byte[] data = { motoAddress,0x06,0x08,0x00,0x01,0x01,0x4B,0xFB};
            dataCheck.crcCheck(data, 6);
            data[6] = dataCheck.LowByte;
            data[7] = dataCheck.HighByte;

            // 给定速度正方向
            byte[] data2 = { motoAddress, 0x06, 0x08, 0x00, 0x01, 0x11, 0x4A, 0x36 };
            dataCheck.crcCheck(data2, 6);
            data2[6] = dataCheck.LowByte;
            data2[7] = dataCheck.HighByte;

            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                // 发出串口使能命令
                sp.Write(data, 0, data.GetLength(0));
                Thread.Sleep(100);
                sp.Write(data2, 0, data2.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#2", "Message:", MessageBoxButton.OK);
            }
        }

        // 位置使能
        public void startPosition(SerialPort sp)
        {
            // 去掉位置使能
            dataCheck.crcCheck(disEnableLoc, 6);
            disEnableLoc[6] = dataCheck.LowByte;
            disEnableLoc[7] = dataCheck.HighByte;

            // 位置复位
            dataCheck.crcCheck(resetLoc, 6);
            resetLoc[6] = dataCheck.LowByte;
            resetLoc[7] = dataCheck.HighByte;

            // 位置使能
            dataCheck.crcCheck(enableLoc, 6);
            enableLoc[6] = dataCheck.LowByte;
            enableLoc[7] = dataCheck.HighByte;

            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                // 发出串口使能命令
                sp.Write(disEnableLoc, 0, disEnableLoc.GetLength(0));
                Thread.Sleep(100);
                sp.Write(resetLoc, 0, resetLoc.GetLength(0));
                Thread.Sleep(100);
                sp.Write(enableLoc, 0, enableLoc.GetLength(0));
                Thread.Sleep(100);
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#2.1", "Message:", MessageBoxButton.OK);
            }
        }

        // 位置复位
        public void resetPosition(SerialPort sp)
        {
            // 去掉位置使能
            dataCheck.crcCheck(disEnableLoc, 6);
            disEnableLoc[6] = dataCheck.LowByte;
            disEnableLoc[7] = dataCheck.HighByte;

            // 位置复位
            dataCheck.crcCheck(resetLoc, 6);
            resetLoc[6] = dataCheck.LowByte;
            resetLoc[7] = dataCheck.HighByte;

            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                sp.Write(disEnableLoc, 0, disEnableLoc.GetLength(0));
                Thread.Sleep(100);
                sp.Write(resetLoc, 0, resetLoc.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#2.2", "Message:", MessageBoxButton.OK);
            }
        }
        
        // 查询电机转速
        public void qurMotoSpeed(SerialPort sp)
        {
            byte[] data = {motoAddress, 0x03, 0x08, 0x12, 0x00, 0x01, 0x26, 0x6F};
            dataCheck.crcCheck(data, 6);
            data[6] = dataCheck.LowByte;
            data[7] = dataCheck.HighByte;
            // 如果串口存在且已经打开
            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen )
            {
                sp.Write(data, 0, data.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#3", "Message:", MessageBoxButton.OK);
            }
        }

        // 查询电机扭矩%
        public void qurMotoTorque(SerialPort sp)
        {
            byte[] data = { motoAddress, 0x03, 0x08, 0x14, 0, 0x01, 0xC6, 0x5D };
            dataCheck.crcCheck(data, 6);
            data[6] = dataCheck.LowByte;
            data[7] = dataCheck.HighByte;

            // 如果串口存在且已经打开
            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                sp.Write(data, 0, data.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#4", "Message:", MessageBoxButton.OK);
            }
        }

        // 查询温湿度及压力值命令
        public void qurHumTemPre(SerialPort sp)
        {
            byte[] data = { 0x23, 0x30, 0x31, 0x0D };
            // 如果串口存在且已经打开
            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                sp.Write(data, 0, data.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#5", "Message:", MessageBoxButton.OK);
            }
        }
        
        // 设置电机转速
        public void setMotoSpeed(int speed, SerialPort sp)
        {
            // 写入寄存器的数值*0.3 = 电机实际转速
            speed = (int)(speed / 0.3);
            speedData[4] = (byte)(speed / 256);
            speedData[5] = (byte)(speed % 256);
            dataCheck.crcCheck(speedData, 6);
            speedData[6] = dataCheck.LowByte;
            speedData[7] = dataCheck.HighByte;

            // 如果串口存在且已经打开
            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                sp.Write(speedData, 0, speedData.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#1", "Message:", MessageBoxButton.OK);
            }
        }
        
        // 速度模式停机
        public void stopMoto(SerialPort sp)
        {
            // 去掉速度使能
            dataCheck.crcCheck(disEnableSpd, 6);
            disEnableSpd[6] = dataCheck.LowByte;
            disEnableSpd[7] = dataCheck.HighByte;

            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                sp.Write(disEnableSpd, 0, disEnableSpd.GetLength(0));
                Thread.Sleep(100);
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#2.4", "Message:", MessageBoxButton.OK);
            }

        }

        // 设置电机旋转角度
        public void setAngle(double ang, double speed, SerialPort sp)
        {
            bool p = true;
            if (ang < 0)
            {
                p = false;
                ang = ang * -1;
            }

            // 需要的脉冲数(10000脉冲转一圈)
            UInt32 puls = (UInt32)(ang / 360.0 * 10000);

            if (p == false)
            {
                //负数用补码表示，等于原码按位取反再加1
                puls = ~puls + 1;
            }

            // 解析脉冲在命令语句中的Bytes
            pulseH[4] = (byte)((puls & 0xFF000000) >> 24);
            pulseH[5] = (byte)((puls & 0xFF0000) >> 16);
            dataCheck.crcCheck(pulseH, 6);
            pulseH[6] = dataCheck.LowByte;
            pulseH[7] = dataCheck.HighByte;

            pulseL[4] = (byte)((puls & 0xFF00) >> 8);
            pulseL[5] = (byte)(puls & 0xFF);
            dataCheck.crcCheck(pulseL, 6);
            pulseL[6] = dataCheck.LowByte;
            pulseL[7] = dataCheck.HighByte;

            // 写入寄存器的数值*0.3 = 电机实际转速
            speed = (int)(speed / 0.25);
            speedData[4] = (byte)(speed / 256);
            speedData[5] = (byte)(speed % 256);
            dataCheck.crcCheck(speedData, 6);
            speedData[6] = dataCheck.LowByte;
            speedData[7] = dataCheck.HighByte;

            // 如果串口存在且已经打开
            if (SerialPort.GetPortNames().Contains<string>(sp.PortName) && sp.IsOpen)
            {
                // 发送脉冲数命令
                sp.Write(pulseH, 0, pulseH.GetLength(0));
                Thread.Sleep(100);
                sp.Write(pulseL, 0, pulseL.GetLength(0));
                Thread.Sleep(100);
                sp.Write(speedData, 0, speedData.GetLength(0));
            }
            else
            {
                ModernDialog.ShowMessage("串口不存在或未打开，请选择正确串口并打开！#1", "Message:", MessageBoxButton.OK);
            }

        }
    }
}
