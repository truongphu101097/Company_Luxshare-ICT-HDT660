using MerryTestFramework.testitem.Computer;
using MerryTestFramework.testitem.Headset;
using MESDLL;
using Newtonsoft.Json;
using SWDevelopKit.Config;
using SWDevelopKit.Core;
using SWDevelopKit.Testitem;
using SWDevelopKit.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using static SWDevelopKit.USBDevice.HidManager;
using ComPort = SWDevelopKit.USBDevice.ComPort;
using System.IO;
using SwATE_Net;
using System.Windows.Forms;
using static MerryTest.SingleTestAPI.SingleTestAddTool.INIOperationClass;

namespace MerryDllFramework {
    public class MerryDll : BaseTestPlan {
        readonly MESBDA Mes = new MESBDA();
        private SwATE mesVN = new SwATE();
        readonly ControlTestPlan ControlTestPlan = new ControlTestPlan();
        //MerryTestFramework.testitem.Headset.ButtonTest ButtonTest = new ButtonTest();
        ButtonTest ButtonTest = new ButtonTest();
        readonly VolumeTestPlan VolumeTestPlan = new VolumeTestPlan();

        string _path1 = ".\\AllDLL\\MenuStrip\\AddDeploy.ini";

        static Config Config => IniConfigHelper.ParseToEntity<Config>();

        public ReadINI dataIni = new ReadINI();
        //public static RFTestPlan RF = new RFTestPlan();

        /// <summary>
        /// 当前产品料号
        /// </summary>
        public string PN;

        /// <summary>
        /// 颜色ID
        /// </summary>
        int ColorID { get; set; }
        string FW { get; set; }
        ComPort DSerialDev => string.IsNullOrEmpty(Config.DongleComPort) ? GetSerialDev("VID_046D&PID_0AC4&MI_04") : new ComPort(Config.DongleComPort);
        ComPort HSerialDev => string.IsNullOrEmpty(Config.HeadsetComPort) ? GetSerialDev("VID_046D&PID_0AC5&MI_00") : new ComPort(Config.HeadsetComPort);
        ComPort ShieldingBox => new ComPort(Config.ShieldingBoxPort);
        Dictionary<string, object> Infomation;
        /// <summary>
        /// 区分是否为连板
        /// </summary>
        bool MerryTrsyFlag;
        public bool StartTest(Dictionary<string, object> Infomation)
        {
            this.Infomation = Infomation;
            MerryTrsyFlag = true;
            return true;
        }
        public MerryDll() {
            TestEnvironment.Setting.IsSaveLog2File = true;
            LoadTestitem();

            //DSerialDev.WriteOnly("01 a0 fc 03 0e 00 00");
        }
        //add CDC
        private Dictionary<string, object> STMFormConfg = new Dictionary<string, object>();
        public override void LoadTestitem() {
            testitemManager
                //PID VID
                .Add(GetHeadsetPidVid)
                .Add(GetDonglePidVid)

                // FW 验证
                .Add(GetDongleFW)
                .Add(GetHeadsetFW)

                // Read Test Flag
                .Add(ReadDongleTestFlag)
                .Add(ReadHeadsetTestFlag)

                // 进入/退出 测试模式
                .Add("OpenDongleTestMode", () => SetDongleTestMode(true))
                .Add("CloseDongleTestMode", () => SetDongleTestMode(false))

                .Add("OpenHeadsetTestMode", () => SetHeadsetTestMode(true))
                .Add("CloseHeadsetTestMode", () => SetHeadsetTestMode(false))

                // 按键测试
                .Add("PowerButtonTest", () => BtnTest(Config.ButtonStatus.Power, "请按开机键/ Vui lòng nhấn nút nguồn"))
                .Add("HPDButtonTest", () => BtnTest(Config.ButtonStatus.HPD, "请波动麦秆/Vui lòng gạt MIC"))
                .Add("FactoryResetButtonTest", () => BtnTest(Config.ButtonStatus.FactoryReset, "请按Reset按键/ Vui lòng nhấn nút Reset"))

                // 耳机LED灯测试
                .Add("OpenRedLED", () => OpenHeadsetLED(1))
                .Add("OpenGreenLED", () => OpenHeadsetLED(2))
                .Add("OpenOrangeLED", () => OpenHeadsetLED(3))
                .Add("OpenLED_RED", () => OpenLED(1))
                .Add("OpenLED_Green", () => OpenLED(2))
                .Add("OpenLED_Orange", () => OpenLED(3))
                .Add(CloseHeadsetLED)

                // 音量加减测试
                .Add("VolumeUp", () => VolumeTestPlan.VolumeTest(true, "上调音量/ Vui lòng vặn tăng âm lượng"))
                .Add("VolumeDown", () => VolumeTestPlan.VolumeTest(false, "下调音量/ Vui lòng vặn giảm âm lượng"))

                // 开关机测试
                .Add(PowerOn)

                // SN相关
                .Add("HeadsetSN", () => ReadHeadsetSN())
                .Add("DongleSN", () => ReadDongleSN())
                .Add("WriteDongleSN", () => WriteDongleSN((string)Config1["SN"]))
                .Add("WriteHeadsetSN", () => WriteHeadsetSN((string)Config1["SN"]))
                // BD
                .Add(ReadDonglePairedBD)

                // 充电相关
                .Add("EnableCharging", () => SetCharging(true)) // 启用充电
                .Add("DisableCharging", () => SetCharging(false)) // 禁用充电

                // 颜色相关
                .Add(DongleColor)
                .Add(HeadsetColor)

                // RF相关;
                //.Add("HeadsetOpenChannel", () => RFTestPlan.GetReady(HSerialDev, DSerialDev))
                .Add("HeadsetOpenChannel", () => OpenChannel(HSerialDev, DSerialDev, 5))
                .Add("HeadsetRSSI2402", () => RFTestPlan.RF2402(HSerialDev, DSerialDev, Config.Headset2402))
                .Add("HeadsetRSSI2440", () => RFTestPlan.RF2440(HSerialDev, DSerialDev, Config.Headset2440))
                .Add("HeadsetRSSI2480", () => RFTestPlan.RF2480(HSerialDev, DSerialDev, Config.Headset2480))
                .Add("PER2440", () => RFTestPlan.PER2440(HSerialDev, DSerialDev))

                //Add automatic RF calibration for Headset
                .Add("HeadsetRSSI2402_RFTEST", () => RFTestPlan.RF2402(HSerialDev, DSerialDev, Convert.ToDouble(INIGetStringValue(_path1, "RFTEST", (string)Config1["TestName"], "0"))))
                .Add("HeadsetRSSI2440_RFTEST", () => RFTestPlan.RF2440(HSerialDev, DSerialDev, Convert.ToDouble(INIGetStringValue(_path1, "RFTEST", (string)Config1["TestName"], "0"))))
                .Add("HeadsetRSSI2480_RFTEST", () => RFTestPlan.RF2480(HSerialDev, DSerialDev, Convert.ToDouble(INIGetStringValue(_path1, "RFTEST", (string)Config1["TestName"], "0"))))


                .Add("TestRF2402", () => RFTestPlan.RF2402(HSerialDev, DSerialDev, 40))

                //.Add("DongleOpenChannel", () => RFTestPlan.GetReady(DSerialDev, HSerialDev))

                .Add("DongleOpenChannel", () => OpenChannel(DSerialDev, HSerialDev, 5))
                .Add("DonglePower2402", () => RFTestPlan.RF2402(DSerialDev, HSerialDev, Config.Dongle2402))
                .Add("DonglePower2440", () => RFTestPlan.RF2440(DSerialDev, HSerialDev, Config.Dongle2440))
                .Add("DonglePower2480", () => RFTestPlan.RF2480(DSerialDev, HSerialDev, Config.Dongle2480))

                //Add automatic RF calibration for Dongle
                .Add("DonglePower2402_RFTEST", () => RFTestPlan.RF2402(DSerialDev, HSerialDev, Convert.ToDouble(INIGetStringValue(_path1, "RFTEST", (string)Config1["TestName"], "0"))))
                .Add("DonglePower2440_RFTEST", () => RFTestPlan.RF2440(DSerialDev, HSerialDev, Convert.ToDouble(INIGetStringValue(_path1, "RFTEST", (string)Config1["TestName"], "0"))))
                .Add("DonglePower2480_RFTEST", () => RFTestPlan.RF2480(DSerialDev, HSerialDev, Convert.ToDouble(INIGetStringValue(_path1, "RFTEST", (string)Config1["TestName"], "0"))))

                // 屏蔽箱
                .Add("OpenShieldingBox", () => SendCmd2ShieldingBox(Config.OpenBoxCmd))
                .Add("CloseShieldingBox", () => SendCmd2ShieldingBox(Config.CloseBoxCmd))

                .Add(GetVoltage) // 电压 // Only When Headset Power On
                .Add(SidetoneTest) // Sidetone测试
                .Add(SideToneOn)// side tone on CDC add 220715
                .Add(SideToneOff)// side tone off CDC add 220715

                .Add("ResetHeadset", () => ResetHeadset(DSerialDev)) // 重置耳机
                .Add("ResetHeadsetByHeadset", () => ResetHeadset(HSerialDev)) // 重置耳机 （从耳机）
                .Add(Pair)


                .Add(CheckDeviceComPortsState)
                .Add("PowerOffByHeadset", () => PowerOff(HSerialDev))
                .Add("PowerOff", () => PowerOff(DSerialDev))


                .Add(WrtieHeadsetTestFlag) // 写 耳机 testflag
                .Add(WrtieDongleTestFlag) // 写 Dongle TestFlag
                .Add(Delay) // 写 Dongle TestFlag

                // CAP
                .Add(WriteHeadsetRFCalibratedValue)
                .Add(WriteDongleRFCalibratedValue)
                .Add(DyTest)
                .Add(CheckDongleReady)
                .Add(CheckHeadsetReady)
            //声学
            ;
        }

        string WrtieHeadsetTestFlag() {
            string pathDirectory = string.Concat(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "\\Setting.ini");

            string testFlag = dataIni.readFlagHeadset(pathDirectory);

            //var testFlag = ControlUtils.ShowDialogInput("请写入标志位");
            HSerialDev.WriteOnly($"01 A2 FC 03 10 {testFlag} 00");
            if (ReadHeadsetTestFlag() == testFlag) {
                return testFlag;
            }

            return $"{false}_ 写入标志位不同";
        }
        string WrtieDongleTestFlag()
        {
            string pathDirectory = string.Concat(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "\\Setting.ini");

            string testFlag = dataIni.readFlagDongle(pathDirectory);
            //var testFlag = ControlUtils.ShowDialogInput("请写入标志位"); //program lastver
            //string testFlag = Config.DongleTestFlag;
            DSerialDev.WriteOnly($"01 A0 FC 03 10 {testFlag} 00");
            if (ReadDongleTestFlag() == testFlag)
            {
                return testFlag;
            }

            return $"{false}_ 写入标志位不同";
        }
        string Delay() {
            Thread.Sleep(100);
            return true.ToString();
        }

        /// <summary>
        /// 检查所有Dongle和耳机的COM完全准备（以可以获取FW来判断）
        /// </summary>
        /// <param name="comPorts"></param>
        /// <returns></returns>

        bool OpenChannel(ComPort sender, ComPort receiver, int retryCount = 3) {
            return Utils.WaitSomething(retryCount * 2000, 2000, () => {
                try {
                    return RFTestPlan.GetReady(sender, receiver);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            });
        }

        string CheckDeviceComPortsState() {
            var comPorts = new List<ComPort>();
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Config.DongleComPort)) {
                comPorts.Add(DSerialDev);
            }

            if (!string.IsNullOrEmpty(Config.HeadsetComPort)) {
                comPorts.Add(HSerialDev);
            }

            foreach (var com in comPorts) {
                var comState = GetDeviceComPortState(com);
                result.Add(com.PortName, comState);
            }
            return JsonConvert.SerializeObject(result);
        }

        //string WaitDeviceComPortsReady(params ComPort[] comPorts) {
        //    var comStates = "";
        //    var result = Utils.WaitSomething(5000, 1000, () => {
        //        comStates = CheckDeviceComPortsReady(comPorts);
        //        Console.WriteLine(comStates);
        //        return comStates == true.ToString();
        //    });
        //    return result ? result.ToString() : comStates;
        //}

        bool SendCmd2Switch(string cmd) {
            var port = new SerialPort();
            if (!port.IsOpen) {
                port.PortName = "COM42";
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Open();
            }

            port.Write(cmd);

            if (port.IsOpen) {
                port.Close();
                port.Dispose();
            }



            //if (!Switch.Port.IsOpen) {
            //    Switch.Port.Open();
            //}

            //Switch.Port.WriteLine(cmd);

            //Switch.Port.Close();
            //var result = Switch.Sends(cmd);
            //return result != false.ToString();
            return true;

        }

        bool SendCmd2ShieldingBox(string cmd) {
            try {
                ShieldingBox.WriteTextOnly(cmd);
            } catch (Exception) {
            }
            return true;
        }

        string ReadDongleTestFlag() {
            return DSerialDev.SendAndGetReport("01 9F FC 01 10", "8").ToBytesString();
        }

        string ReadHeadsetTestFlag() {
            return HSerialDev.SendAndGetReport("01 A1 FC 01 10", "8").ToBytesString();
        }

        //public bool WriteDongleTestFlag(string flag) {
        //    return DSerialDev.Send($"01 A0 FC 03 10 {flag} 00") == "04 0E 05 01 A0 FC 00 10";
        //}

        //public bool WritHeadsetTestFlag(string flag) {
        //    return HSerialDev.Send($"01 A2 FC 03 10 {flag} 00") == "04 0E 05 01 A2 FC 00 10";
        //}

        string GetHeadsetPidVid() {
            return HSerialDev == null ? false.ToString() : "V046D P0AC5";
        }

        string GetDonglePidVid() {
            return DSerialDev == null ? false.ToString() : "V046D P0AC4";
        }

        string CheckDongleReady()
        {
            int i = 0;
            while (i < 30)
            {
                if (DSerialDev!=null) return true.ToString();
                i++;
                Thread.Sleep(1000);
            }
            return false.ToString();
        }

        string CheckHeadsetReady()
        {
            int i = 0;
            while (i < 30)
            {
                if (HSerialDev != null) return true.ToString();
                i++;
                Thread.Sleep(1000);
            }
            return false.ToString();
        }

        string GetDongleFW() {
            string fw = "";
            var isSuccessed = Utils.WaitSomething(5000, 1000, () => {
                fw = DSerialDev?.SendAndGetReport("01 97 FC 00", "17 18 19 20").ToVersion();
                return fw != false.ToString();
            });
            if (FW == fw)
                return fw;
            return "False,当前FW为:" + fw + " 系统FW为:" + FW;
        }
       
        string GetHeadsetFW() {
            string fw = "";
            var isSuccessed = Utils.WaitSomething(5000, 1000, () => {
                fw = HSerialDev?.SendAndGetReport("01 9B FC 00", "17 18 19 20").ToVersion();
                return fw != false.ToString();
            });
            if (FW == fw)
                return fw;
            return "False,当前FW为:" + fw + " 系统FW为:" + FW;
        }

        /// <summary>
        /// 检查Donlge和耳机的状态;
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        string GetDeviceComPortState(ComPort com) {
            var pnpID = com.GetPNPDeviceID();
            if (pnpID == null) {
                return $"{false}_NotFound";
            }
            Func<string> checkFunc = null;
            if (pnpID.Contains("VID_046D&PID_0AC4&MI_04")) {
                checkFunc = DongleColor;
            }

            // 耳机
            if (pnpID.Contains("VID_046D&PID_0AC5&MI_00")) {
                checkFunc = HeadsetColor;
            }

            if (checkFunc == null) {
                return $"{false}_UnknowDevice";
            }

            try {
                var checkResult = checkFunc();
            } catch (Exception ex) {
                return $"{false}_{ex.Message}";
            }
            return true.ToString();
        }

        bool SetHeadsetTestMode(bool enable) {
            var result = HSerialDev?.Send($"01 A2 FC 03 11 0{(enable ? 1 : 0)} 00");
            if (result != "04 0E 05 01 A2 FC 00 11") return false;

            result = HSerialDev.Send("01 A1 FC 01 11");
            return result == $"04 0E 07 01 A1 FC 00 11 0{(enable ? 1 : 0)} 00";
        }

        bool SetDongleTestMode(bool enable) {
            var result = DSerialDev?.Send($"01 A0 FC 03 11 0{(enable ? 1 : 0)} 00");
            if (result != "04 0E 05 01 A0 FC 00 11") return false;

            result = DSerialDev.Send("01 9F FC 01 11");
            return result == $"04 0E 07 01 9F FC 00 11 0{(enable ? 1 : 0)} 00";
        }

        bool BtnTest(Config.ButtonStatus status, string message) {
            bool func() {
                var result = HSerialDev.Send("01 07 FD 01 00");
                if (result == null) return false;
                var value = Convert.ToInt32(result.Split(' ')[8], 16);
                return (value & (byte)status) != 0;
            }

            return ButtonTest.Buttontest(func, message);
        }

        bool OpenHeadsetLED(int colorValue)
        {
            string tipText;
            switch (colorValue)
            {
                case 1: tipText = "耳机红色LED是否打开/Xác nhận đèn LED đỏ của tai nghe sáng đang sáng";
                    break;
                case 2:
                    tipText = "耳机绿色LED是否打开/Xác nhận đèn LED xanh lá của tai nghe đang sáng";
                    break;
                case 3:
                    tipText = "耳机橙色LED是否打开/Xác nhận đèn LED màu cam của tai nghe đang sáng";
                    break;
                default:
                    tipText = "";
                    break;
            }
            if (!HSerialDev.Send($"01 06 FD 02 0{colorValue} 00").Equals($"04 0E 05 01 06 FD 00 0{colorValue}")) return false;
            //else return true;
            if (ControlTestPlan.JudgeBox(tipText))
            return true;
            return false;
            //return ControlTestPlan.JudgeBox(tipText);
        }
        bool OpenLED(int colorValue)
        {
            string tipText;
            if (!HSerialDev.Send($"01 06 FD 02 0{colorValue} 00").Equals($"04 0E 05 01 06 FD 00 0{colorValue}")) return false;
            return true;
        }
        bool CloseHeadsetLED() {
            return HSerialDev.Send("01 06 FD 02 00 00").Equals("04 0E 05 01 06 FD 00 00");
        }

        bool PowerOn()
        {
            string result = HSerialDev.Send("01 06 FD 02 04 00");
            Thread.Sleep(200);
            string[] re = result.Split(' ');
            if (re[7] == "04")
            {
                return true;
            }
            return false;


        }
        bool PowerOff(ComPort dev) {

            string result = dev.Send("01 06 FD 02 06 00");
            Thread.Sleep(200);
            string[] re = result.Split(' ');
            if (re[7] == "06")
            {
                return true;
            }
            return false;

            //var result = dev.Send("01 06 FD 02 06 00");
            //return result == "04 0E 05 01 06 FD 00 06";
        }
        bool SideToneOn()
        {
            if (HSerialDev?.Send("01 00 FD 02 E4 80") != null)
            {
                if(HSerialDev?.Send("01 00 FD 02 E8 05") != null)
                {
                    if(HSerialDev?.Send("01 00 FD 02 E9 06") != null)
                    {
                        if(HSerialDev?.Send("01 00 FD 02 E9 06") != null)
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        bool SideToneOff()
        {
            if(HSerialDev.Send("01 00 FD 02 E6 00") != null)
            {
                return true;

            }
            return false;
        }

        bool SidetoneTest() {
            if (HSerialDev?.Send("01 00 FD 02 E4 80") == null) return false;
            if (!ControlTestPlan.JudgeBox("检查侧音功能/ Kiểm tra chức năng SideTone")) return false;
            return HSerialDev.Send("01 00 FD 02 E6 00") != null;
            //return true;
        }

        bool SidetoneTest1()
        {
            if (HSerialDev?.Send("01 00 FD 02 E8 05") == null) return false;
            if (!ControlTestPlan.JudgeBox("检查侧音功能/ Kiểm tra chức năng SideTone")) return false;
            return HSerialDev.Send("01 00 FD 02 E6 00") != null;
            //return true;
        }

        bool SidetoneTest2()
        {
            if (HSerialDev?.Send("01 00 FD 02 E9 06") == null) return false;
            if (!ControlTestPlan.JudgeBox("检查侧音功能/ Kiểm tra chức năng SideTone")) return false;
            return HSerialDev.Send("01 00 FD 02 E6 00") != null;
            //return true;
        }

        bool HeadsetChangeSidetone()
        {
            if (HSerialDev?.Send("01 00 FD 02 E6 14") == null) return false;
            return true;
        }

        string WriteHeadsetSN(string SN) {
            if (SN.Contains("TE_BZP")) return true.ToString();
            HSerialDev.Send($"01 a2 fc 03 12 {SN.Substring(0, 2)} {SN.Substring(2, 2)}");
            HSerialDev.Send($"01 a2 fc 03 13 {SN.Substring(4, 2)} {SN.Substring(6, 2)}");
            HSerialDev.Send($"01 a2 fc 03 14 {SN.Substring(8, 2)} {SN.Substring(10, 2)}");
            var sn = ReadHeadsetSN();
            Thread.Sleep(300);
            return sn == SN ? SN : $"False {sn}";
        }
        string WriteDongleSN(string SN) {
            if (SN.Contains("TE_BZP")) return true.ToString();
            DSerialDev.Send($"01 a0 fc 03 12 {SN.Substring(0, 2)} {SN.Substring(2, 2)}");
            DSerialDev.Send($"01 a0 fc 03 13 {SN.Substring(4, 2)} {SN.Substring(6, 2)}");
            DSerialDev.Send($"01 a0 fc 03 14 {SN.Substring(8, 2)} {SN.Substring(10, 2)}");
            var sn = ReadDongleSN();
            return sn == SN ? SN : $"False {sn}";
        }
        string ReadHeadsetSN() {
            var result = HSerialDev.SendAndGetReport("01 09 FD 00", "7 8 9 10 11 12").ToBytesString("");
            return result == "000000000000" ? result : result;// edit null -> result cdc 220728
        }

        string ReadDongleSN() {
            var result = DSerialDev.SendAndGetReport("01 08 FD 00", "7 8 9 10 11 12").ToBytesString("");
            return result == "000000000000" ? result : result;
        }

        /// <summary>
        /// 读取Dongle中的耳机SN
        /// </summary>
        /// <returns></returns>
        string ReadDonglePairedBD() {
            try {
                var result = DSerialDev.SendAndGetReport("01 09 FD 00", "7 8 9 10 11 12").ToBytesString("");
                if (result == "000000000000") {
                    Console.WriteLine("[WRAN] Paired is 000000000000");
                }
                return result;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false.ToString();
            }
        }

        bool SetCharging(bool isEnable) {
            if (!isEnable) {
                var reoprt = HSerialDev.Send("01 9D FC 02 00 00");
                if (reoprt != "04 0E 04 01 9D FC 00") return false;
            }

            var result = HSerialDev?.Send($"01 0A FD 01 0{Convert.ToInt32(isEnable)}");
            return result == "04 0E 04 01 0A FD 00";
        }
        
        string DongleColor()
        {
            var result = DSerialDev.SendAndGetReport("01 9F FC 01 0E", "8").ToBytesString();
            string colorHS = "";
            switch (result)
            {
                case "00": colorHS = " BLACK";
                    break;
                case "01":
                    colorHS = " WHITE";
                    break;
                case "02":
                    colorHS = " MINT";
                    break;
                case "03":
                    colorHS = "PS5";
                    break;
            }
            return result == "0"+ ColorID ? result +colorHS : false.ToString() + "返回值" + result + "  正确值：0" + ColorID;
        }

        string HeadsetColor()
        {
            var result = HSerialDev.SendAndGetReport("01 A1 FC 01 0E", "8").ToBytesString();
            string colorHS = "";
            switch (result)
            {
                case "00":
                    colorHS = " BLACK";
                    break;
                case "01":
                    colorHS = " WHITE";
                    break;
                case "02":
                    colorHS = " MINT";
                    break;
                case "03":
                    colorHS = "PS5";
                    break;
            }
            return result == "0" + ColorID ? result + colorHS : false.ToString() + "返回值" + result + "  正确值：0" + ColorID;
        }

        //string DongleColor() {
        //    var result = DSerialDev.SendAndGetReport("01 9F FC 01 0E", "8").ToBytesString();
        //    return result;
        //}

        //string HeadsetColor() {
        //    var result = HSerialDev.SendAndGetReport("01 A1 FC 01 0E", "8").ToBytesString();
        //    return result;
        //}

        double GetVoltage() {
            var result = DSerialDev.Send("01 03 FD 00").Split(' ');
            var value = Convert.ToInt32((result[8] + result[7]), 16);
            return ((double)value / 1000);
        }

        string Pair() {
            bool result = false;
            for (int i = 0; i < 2; i++) {
                try {
                    DSerialDev.Send("01 99 FC 00"); // 配对指令
                } catch (Exception) {
                }
                Thread.Sleep(2000);
                result = Utils.WaitSomething(15 * 1000, 500, () => {
                    var DonglePairedId = ReadDonglePairedBD();
                    var HeadsetSN = ReadHeadsetSN();
                    return DonglePairedId != null && DonglePairedId != false.ToString() && DonglePairedId.Equals(HeadsetSN);
                });
                if (result) break;
            }
            return result ? ReadDonglePairedBD() : false.ToString();
        }

        bool ResetHeadset(ComPort dev) {
            string result = dev.Send("01 06 FD 02 05 00");
            Thread.Sleep(200);
            string[] re = result.Split(' ');
            if (re[7] == "05")
            {
                return true;
            }
            return false;

            //var result = dev.Send("01 06 FD 02 05 00");
            //return result == "04 0E 05 01 06 FD 00 05";
        }

       string CheckDongleSN()
        {
            if (FormData[0].Equals("TE_BZP"))
            {
                return "标准品";
            }
            //var writtenBD = GetDongleID();
            var SN = STMFormConfg["BindingSN"].ToString();
            string req = mesVN.GetPart(SN);
            if (req.Contains("TX"))
                return true.ToString();
            else return ($"SN异常/Mã SN sai{SN}, 料号返回值/mã liệu phản hồi là: {req}");
        }

        //Begin Calibration
        /// <summary>
        /// 写入DonlgeCap值
        /// </summary>
        /// <param name="cap">16进制数，不带0x</param>
        /// <returns></returns>
        string WriteDongleCap(string cap)
        {
            // Cap-in
            var result = DSerialDev.Send($"01 a0 fc 03 00 {cap} 00");
            Assert(result.ToLower() != "04 0e 04 01 a0 fc 01", "写入Dongle Cap-in 失败");

            // Cap-out
            result = DSerialDev.Send($"01 a0 fc 03 01 {cap} 00");
            Assert(result.ToLower() != "04 0e 04 01 a0 fc 01", "写入Dongle Cap-out 失败");

            var nowCap = ReadDongleCap();
            Assert(nowCap != cap, $"[读写cap不匹配] read:{nowCap}, write:{cap}");
            return cap;


        }

        /// <summary>
        /// 写入耳机Cap值
        /// </summary>
        /// <param name="cap">16进制数，不带0x</param>
        /// <returns></returns>
        string WriteHeadsetCap(string cap)
        {
            // Cap-in
            var result = HSerialDev.Send($"01 a2 fc 03 00 {cap} 00");
            Assert(result.ToLower() != "04 0e 05 01 a2 fc 00 00", "写入耳机 Cap-in 失败");

            // Cap-out
            result = HSerialDev.Send($"01 a2 fc 03 01 {cap} 00");
            Assert(result.ToLower() != "04 0e 05 01 a2 fc 00 01", "写入耳机 Cap-out 失败");

            var nowCap = ReadHeadsetCap();
            Assert(nowCap != cap, $"[读写cap不匹配] read:{nowCap}, write:{cap}");
            return cap;

        }

        /// <summary>
        /// 读Dongle cap值
        /// </summary>
        /// <returns>16进制校准值</returns>
        string ReadDongleCap()
        {
            var capIn = DSerialDev.SendAndGetReport("01 9f fc 01 00", "8").ToBytesString();

            var capOut = DSerialDev.SendAndGetReport("01 9f fc 01 01", "8").ToBytesString();

            Assert(capIn != capOut, $"[cap-in 与 cap-out值不相等] cap-in: {capIn} cap-out: {capOut}");
            return capIn;
        }

        /// <summary>
        /// 读耳机cap值
        /// </summary>
        /// <returns>16进制校准值</returns>
        string ReadHeadsetCap()
        {
            var capIn = HSerialDev.SendAndGetReport("01 a1 fc 01 00", "8").ToBytesString();

            var capOut = HSerialDev.SendAndGetReport("01 a1 fc 01 01", "8").ToBytesString();

            Assert(capIn != capOut, $"[cap-in 与 cap-out值不相等] cap-in: {capIn} cap-out: {capOut}");
            return capIn;
        }

        /// <summary>
        /// 写入耳机Cap校准值
        /// </summary>
        /// <returns></returns> 
        string WriteHeadsetRFCalibratedValue()
        {
            var intCap = int.Parse(FormData[4]).ToString("X2").PadLeft(2, '0');
            return WriteHeadsetCap($"{intCap }");
        }

        /// <summary>
        /// 写入DongleCap校准值
        /// </summary>
        /// <returns></returns>
        string WriteDongleRFCalibratedValue()
        {
            var intCap = int.Parse(FormData[4]).ToString("X2").PadLeft(2, '0');
            return WriteDongleCap($"{intCap}");
        }
        string DyTest() { return FormData[4]; }

        //End Calibration

        static Dictionary<string, object> Config1 = new Dictionary<string, object>();
        //接口
        public object Interface(Dictionary<string, object> keys) => (Config1 = keys);
        public bool Start(List<string> formsData, IntPtr _handel)
        {
            base.Start(formsData, _handel);
            Dictionary<string, string> PartNumberInfos = (Dictionary<string, string>)Config1["PartNumberInfos"];
            var a = Config1;
            if (Config.CheckColorIDBy == "WO")
            {

                string color = "";
                try
                {
                    FW = PartNumberInfos["FW"].ToString().Trim();
                    color = PartNumberInfos["color"].ToString().Trim();
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"此新料号在后台还没管控颜色，请联系TE处理: {Config1["OrderNumberInformation"]} / Mã liệu mới này chưa được quản chế màu sắc, vui lòng liên hệ TE xử lí\n\n");
                }
        
                switch (color)
                {
                   
                    case "BLACK":
                        ColorID = 0x00;
                        break;
                    case "WHITE":
                        ColorID = 0x01;
                        break;
                    case "BLUE":
                        ColorID = 0x02;
                        break;
                    case "lara":
                        ColorID = 0x03;
                        break;
                    default:
                        ControlTestPlan.ColorBox("料号异常: 没有找到此料号对应的机型颜色", Color.Red);
                        return false;
                }
                return true;
            }
            else
            {
                ColorSelect color1 = new ColorSelect();
                color1.ShowDialog();
                switch (color1.ColorReturn())
                {
                    case "00":
                        ColorID = 0x00;
                        FW = "V90.2.1.0";
                        break;
                    case "01":
                        ColorID = 0x01;
                        FW = "V90.2.1.0";
                        break;
                    case "02":
                        ColorID = 0x02;
                        FW = "V90.2.1.0";
                        break;
                    case "03":
                        ColorID = 0x03;
                        FW = "V90.2.2.0";
                        break;
                    default:
                        ControlTestPlan.ColorBox("料号异常: 没有找到此料号对应的机型颜色", Color.Red);
                        return false;
                }
                return true;
            }
           
            
            //switch (Config.CheckColorIDBy)
            //{
            //    case "WO": // 输入工单设置颜色ID
            //        ControlTestPlan.BarCodeBox("请输入当前生产工单/ Vui lòng nhập công đơn", 12, out workOrder);
            //        var req = mesVN.GetPart(workOrder);
            //        if (req.Contains("NG"))
            //        {
            //            ControlTestPlan.ColorBox($"工单异常{req}/ Công đơn sai, mã liệu phản hồi {req}", Color.Red);
            //            return false;
            //        }
            //        PN = req;
            //        goto case "PN";
            //    case "PN":
            //        if (string.IsNullOrEmpty(PN))
            //        {
            //            ControlTestPlan.BarCodeBox("请输入当前产品料号", 12, out PN);
            //        }

            //        if (PN.Contains("85X660001") || PN.Contains("85X660004") || PN.Contains("6602000AZ") || PN.Contains("6602000BZ") || PN.Contains("6601700DZ") || PN.Contains("6601700EZ"))//6601700DZ
            //        {
            //            ColorID = 0x00;
            //        }
            //        else if (PN.Contains("85X660002") || PN.Contains("85X660005") || PN.Contains("6602000DZ") || PN.Contains("6602000CZ") || PN.Contains("6601700FZ") || PN.Contains("6601700HZ"))
            //        {
            //            ColorID = 0x01;
            //        }
            //        else if (PN.Contains("85X660003") || PN.Contains("85X660006")||PN.Contains("6602000EZ") || PN.Contains("6602000FZ") || PN.Contains("6601700JZ") || PN.Contains("6601700GZ"))
            //        {
            //            ColorID = 0x02;
            //        }
            //        else
            //        {
            //            ControlTestPlan.ColorBox("料号异常: 没有找到此料号对应的机型颜色", Color.Red);
            //            return false;
            //        }

            //        return true;
            //    default:
            //        return true;

            //}
        }

        public string[] GetDllInfo()
        {
            //get
            {
                string dllname = "DLL 名称       ：HDT660";
                string dllfunction = "Dll功能说明 ：HDT660机型测试Dll";
                string dllHistoryVersion = "历史Dll版本：无";
                string dllVersion = "当前Dll版本：V23.12.12.0";
                string dllChangeInfo = "Dll改动信息：v1.0.220315";
                string dllChangeInfo2 = "Dll改动信息：v1.1.230314 增加输入工单确认Dongle颜色";
                string dllChangeInfo3 = "Dll改动信息：v1.1.230327 增加写入Dongle与HeadsetCalibratedValue";
                string dllChangeInfo4 = "Dll改动信息：V23.04.27.4 根据后台的料号确认产品颜色";
                string dllChangeInfo5 = "Dll改动信息：V23.10.30.1 添加自动射频补偿";
                string dllChangeInfo6 = "Dll改动信息：V23.12.31.0 添加新数据代码";
                string[] info = { dllname,
                dllfunction,
                dllHistoryVersion,
                dllVersion,
                dllChangeInfo, dllChangeInfo2, dllChangeInfo3, dllChangeInfo4, dllChangeInfo5 };
                return info;
            }
        }
        /// <summary>
        /// 自定义断言方法
        /// </summary>
        /// <param name="func"></param>
        /// <param name="errMsg"></param>
        /// <remarks>系统断言不能在 Release 版保留，用这个方法替代</remarks>
        public static void Assert(Func<bool> func, string errMsg)
        {
            Assert(func(), errMsg);
        }

        /// <summary>
        /// 自定义断言方法， result == true 抛出异常
        /// </summary>
        /// <param name="func"></param>
        /// <param name="errMsg"></param>
        /// <remarks>系统断言不能在 Release 版保留，用这个方法替代</remarks>
        public static void Assert(bool result, string errMsg)
        {
            if (result) throw new Exception(errMsg);
        }
    }
}