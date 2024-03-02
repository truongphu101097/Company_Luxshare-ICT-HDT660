using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace MerryDllFramework
{
    class ReadINIfile
    {
        private string filePath;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
        string key,
        string val,
        string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
        string key,
        string def,
        StringBuilder retVal,
        int size,
        string filePath);
        public ReadINIfile(string filePath)
        {
            this.filePath = filePath;
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value.ToLower(), this.filePath);
        }

        public string Read(string section, string key)
        {
            StringBuilder SB = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", SB, 255, this.filePath);
            return SB.ToString();
        }

        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
    }


    public class ReadINI
    {
        public string readFlagDongle(string path)
        {
            var dataReadINI = new ReadINIfile(path);
            string dongleTF = dataReadINI.Read("Setting", "DongleTestFlag");
            return dongleTF;

        }

        public string readFlagHeadset(string path)
        {
            var dataReadINI = new ReadINIfile(path);
            string dongleTF = dataReadINI.Read("Setting", "HeadsetTestFlag");
            return dongleTF;

        }
    }

}
