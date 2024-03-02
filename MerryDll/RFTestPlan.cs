using SWDevelopKit.USBDevice;
using System;
using System.Threading;

namespace SWDevelopKit.Testitem
{
    /// <summary>
    /// 包含一些RF测试相关方法
    /// </summary>
    public static class RFTestPlan
    {

        /// <summary>
        /// RF测试前置动作，开启RSSIRF测试通道 
        /// </summary>
        /// <param name="sender">发送方</param>
        /// <param name="receiver">接收方</param>
        /// <returns></returns>
        public static bool GetReady(ComPort sender, ComPort receiver)
        {
            if (sender is null)
            {
                throw new Exception("[exception] sender == null");
            }

            if (receiver is null)
            {
                throw new Exception("[exception] receiver == null");
            }

            //dongle准备动作
            var value = sender.Send("01 A4 FC 01 01");    //04 0E 04 01 A4 FC 00  Set Bridge Mode
            if (value == null)
            {
                return false;
            }

            value = sender.Send("01 03 0C 00"); //04 0E 04 01 03 0C 00  DTM Reset
            if (value != "04 0E 04 01 03 0C 00")
            {
                return false;
            }

            value = sender.Send("01 88 FC 01 01");//04 0E 04 01 88 FC 00  Set PHY Mode 2M
            if (value != "04 0E 04 01 88 FC 00")
            {
                return false;
            }

            value = sender.Send("01 8A FC 01 07");//04 0E 04 01 8A FC 00  Set Output Power = 4 dBm
            if (value != "04 0E 04 01 8A FC 00")
            {
                return false;
            }

            //headset准备动作
            value = receiver.Send("01 A4 FC 01 01");//04 0E 04 01 A4 FC 00  Set Bridge Mode
            if (value == null)
            {
                return false;
            }

            value = receiver.Send("01 03 0C 00");//04 0E 04 01 03 0C 00  DTM Reset
            if (value != "04 0E 04 01 03 0C 00")
            {
                return false;
            }

            value = receiver.Send("01 88 FC 01 01");//04 0E 04 01 88 FC 00  Set PHY Mode 2M
            if (value != "04 0E 04 01 88 FC 00")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">发送方</param>
        /// <param name="receiver">接收方</param>
        /// <param name="offset">补偿值</param>
        /// <returns></returns>
        public static string RF2402(ComPort sender, ComPort receiver, double offset)
        {
            isNull(sender, "sender == null");
            isNull(sender, "receiver == null");
            var value = "";
            //打开Rx 2402 RSSI资源
            // GetReady(Dport, Hport);
            value = sender.Send("01 89 FC 01 80"); // 04 0E 04 01 89 FC 00  Enable CW Tx CH0
            value = receiver.Send("01 8B FC 01 80"); // 04 0E 04 01 89 FC 00  Enable RSSI Measure CH0

            value = receiver.Send("01 8C FC 00");// 04 0E 06 01 8C FC 00 E7 FF  Get RSSI  04 0E 06 01 8C FC 00 CB FF
                                                 // 关闭Rx 2402 RSSI资源
            receiver.Send("01 8B FC 01 00");//04 0E 04 01 8B FC 00  Get RSSI
            sender.Send("01 89 FC 01 00");//04 0E 04 01 89 FC 00  Get RSSI
            if (value == null || value == "04 0E 06 01 8C FC 01") return "False";
            value = value.Split(' ')[7];
            var values = 256 - Convert.ToInt32(value, 16);
            return (offset - values).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">发送方</param>
        /// <param name="receiver">接收方</param>
        /// <param name="offset">补偿值</param>
        /// <returns></returns>
        public static string RF2440(ComPort sender, ComPort receiver, double offset)
        {
            var value = "";
            //打开Rx 2402 RSSI资源
            // GetReady(Dport, Hport);
            value = sender.Send("01 89 FC 01 93");//04 0E 04 01 89 FC 00  Enable CW Tx CH0
            value = receiver.Send("01 8B FC 01 93");//04 0E 04 01 89 FC 00  Enable RSSI Measure CH0

            value = receiver.Send("01 8C FC 00");//04 0E 06 01 8C FC 00 E7 FF  Get RSSI  04 0E 06 01 8C FC 00 CB FF
                                                 //关闭Rx 2402 RSSI资源
            receiver.Send("01 8B FC 01 13");//04 0E 04 01 8B FC 00  Get RSSI
            sender.Send("01 89 FC 01 13");//04 0E 04 01 89 FC 00  Get RSSI
            if (value == null || value == "04 0E 06 01 8C FC 01") return "False";
            value = value.Split(' ')[7];
            var values = 256 - Convert.ToInt32(value, 16);
            return (offset - values).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">发送方</param>
        /// <param name="receiver">接收方</param>
        /// <param name="offset">补偿值</param>
        /// <returns></returns>
        public static string RF2480(ComPort sender, ComPort receiver, double offset)
        {
            var value = "";
            //打开Rx 2402 RSSI资源
            // GetReady(Dport, Hport);
            value = sender.Send("01 89 FC 01 A7");//04 0E 04 01 89 FC 00  Enable CW Tx CH0
            value = receiver.Send("01 8B FC 01 A7");//04 0E 04 01 89 FC 00  Enable RSSI Measure CH0

            value = receiver.Send("01 8C FC 00");//04 0E 06 01 8C FC 00 E7 FF  Get RSSI  04 0E 06 01 8C FC 00 CB FF
                                                 //关闭Rx 2402 RSSI资源
            receiver.Send("01 8B FC 01 27");//04 0E 04 01 8B FC 00  Get RSSI
            sender.Send("01 89 FC 01 27");//04 0E 04 01 89 FC 00  Get RSSI
            if (value == null || value == "04 0E 06 01 8C FC 01") return "False";
            value = value.Split(' ')[7];
            var values = 256 - Convert.ToInt32(value, 16);
            return (offset - values).ToString();
        }

        /// <summary>
        /// PER测试
        /// </summary>
        /// <param name="sender">发送方</param>
        /// <param name="receiver">接收方</param>
        /// <returns></returns>
        public static string PER2440(ComPort sender, ComPort receiver)
        {

            //Headset PER Setup
            receiver.Send("01 A4 FC 01 01");
            receiver.Send("01 03 0C 00");
            receiver.Send("01 88 FC 01 01");
            //Dongle PER Setup
            sender.Send("01 A4 FC 01 01");
            sender.Send("01 03 0C 00");
            sender.Send("01 88 FC 01 01");
            sender.Send("01 8A FC 01 07");
            //Rx Start
            receiver.Send("01 1D 20 01 13");
            //Tx DTM
            sender.Send("01 1E 20 03 13 25 02");
            Thread.Sleep(500);
            //TX End Test
            string value = sender.Send("01 1F 20 00"); // 发射包数04 0E 06 01 1F 20 00 xx xx
            Thread.Sleep(500);
            //RX End Test
            var value1 = receiver.Send("01 1F 20 00"); //接受包数
            string[] arr = value.Split(' ');
            string[] arr1 = value1.Split(' ');

            Thread.Sleep(1000);

            //发射包或者接受包为0
            if ((arr[7] == "00" && arr[8] == "00") || (arr1[7] == "00" && arr1[8] == "00"))
            {
                return false.ToString();
            }
            var num1 = Convert.ToInt32((arr[8] + arr[7]), 16);
            var num2 = Convert.ToInt32((arr1[8] + arr1[7]), 16);
            double PER = (1 - (double)(num1 - num2) / num1) * 100;
            return PER.ToString("f1");
        }


        static bool isNull(object obj, string errorMeg)
        {
            if (obj is null)
            {
                throw new ArgumentNullException($"{errorMeg}");
            }
            return false;
        }
    }
}
