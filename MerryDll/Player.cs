using MerryTestFramework.testitem.Computer;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace MerryDllFramework {
    public class Player {

        private static SoundPlayer player = new SoundPlayer();
        /// <summary>
        /// 开始播放音乐
        /// </summary>
        public static void StartMusic(string music) {
            player.SoundLocation = music;
            player.Load();
            player.PlayLooping();
        }
        public static void StopMusic() {
            player.Stop();
            player.Dispose();
        }
        static ControlTestPlan messageBox = new ControlTestPlan();
        /// <summary>
        /// 开始录音
        /// </summary>
        /// <returns></returns>
        public static bool RecordTest() {
            StartRecord(@"C:\Users\ch180265\Desktop\Crush\Type_Name DLL\MerryDllFramework_Debug\bin\Debug\rec.wav");
            messageBox.JudgeBox("录音");
            StopRecord();

            player.SoundLocation = @".\rec.wav";
            player.Load();
            player.PlayLooping();
            bool flag = messageBox.JudgeBox("播放录音");
            player.Stop();
            player.Dispose();
            return flag;
        }

        #region 录音
        public static WaveIn mWavIn;
        public static WaveFileWriter mWavWriter;

        /// <summary>
        /// 开始录音
        /// </summary>
        /// <param name="filePath"></param>
        public static void StartRecord(string filePath) {

            mWavIn = new WaveIn(new System.Windows.Forms.Control().Handle);
            mWavIn.DataAvailable += new EventHandler<WaveInEventArgs>((sender, e) => {
                mWavWriter.Write(e.Buffer, 0, e.BytesRecorded);
                int secondsRecorded = (int)mWavWriter.Length / mWavWriter.WaveFormat.AverageBytesPerSecond;
            });
            mWavWriter = new WaveFileWriter(filePath, mWavIn.WaveFormat);
            mWavIn.StartRecording();
        }

        /// <summary>
        /// 停止录音
        /// </summary>
        public static void StopRecord() {
            mWavIn?.StopRecording();
            mWavIn?.Dispose();
            mWavIn = null;
            mWavWriter?.Close();
            mWavWriter = null;
        }

        private static void MWavIn_DataAvailable(object sender, WaveInEventArgs e) {
            mWavWriter.Write(e.Buffer, 0, e.BytesRecorded);
            int secondsRecorded = (int)mWavWriter.Length / mWavWriter.WaveFormat.AverageBytesPerSecond;
        }
        #endregion
    }
}
