using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    //计算CRC16校验码
    class CRC
    {
        byte lowByte = 0;//低字节位
        byte highByte = 0;//高字节位
        public byte LowByte
        {
            get { return lowByte; }
        }
        public byte HighByte
        {
            get { return highByte; }
        }
        //校验码计算，形参分别为待校验数据和数据个数
        public ushort crcCheck(byte[] dataArray, int n)
        {
            ushort crc = 0xFFFF;//校验码，初始值为0xFFFF
            ushort temp;//缓存变量
            int i, j;

            for (i = 0; i < n; i++)
            {
                crc = (ushort)(crc ^ dataArray[i]);
                for (j = 8; j > 0; j--)
                {
                    temp = (ushort)(crc & 0x0001);
                    if (temp != 0)
                    {
                        crc >>= 1;
                        crc = (ushort)(crc ^ 0xA001);
                    }
                    else
                        crc >>= 1;
                }
            }
            temp = crc;
            lowByte = (byte)temp;//截取低字节位
            highByte = (byte)(temp >>= 8);//截取高字节位
            return crc;
        }
    }
}
