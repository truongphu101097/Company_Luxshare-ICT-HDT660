namespace MerryDllFramework {
    /// <summary>
    /// Config.txt程序配置
    /// </summary>
    public class Config {

        /// <summary>
        /// 判定从工单(WO)或料号(PN)来检查ColorID，
        /// </summary>
        public string CheckColorIDBy { get; set; }

        public string DongleComPort { get; set; }

        public string HeadsetComPort { get; set; }

        public string SwichComPort { get; set; }

        public string ShieldingBoxPort { get; set; }
        //public string DongleTestFlag { get; set; }

        /// <summary>
        /// 开启屏蔽箱的指令
        /// </summary>
        public string OpenBoxCmd {  get; set; }
        
        /// <summary>
        /// 关闭屏蔽箱的指令
        /// </summary>
        public string CloseBoxCmd {  get; set; }

        //public string DongleTestFlag { get; set; }
        public int Dongle2402 { get; set; }
        public int Dongle2440 { get; set; }
        public int Dongle2480 { get; set; }
        public int Headset2402 { get; set; }
        public int Headset2440 { get; set; }
        public int Headset2480 { get; set; }

        

        /// <summary>
        /// 打开控制Dongle的Switch的指令
        /// </summary>
        //public string OpenDongleSwitchCmd { get; set; }

        ///// <summary>
        ///// 关闭控制Dongle的Switch的指令
        ///// </summary>
        //public string CloseDongleSwitchCmd { get; set; }

        ///// <summary>
        ///// 打开控制Headset的Switch的指令
        ///// </summary>
        //public string OpenHeadsetSwitchCmd { get; set; }

        ///// <summary>
        ///// 关闭控制Headset的Switch的指令
        ///// </summary>
        //public string CloseHeadsetSwitchCmd { get; set; }

        /// <summary>
        /// 按键返回值
        /// </summary>
        public enum ButtonStatus : byte {
            Power = 01,
            HPD = 02,
            FactoryReset = 04,
            VOL_PIN_A = 08,
            VOL_PIN_B = 10
        };
    }
}
