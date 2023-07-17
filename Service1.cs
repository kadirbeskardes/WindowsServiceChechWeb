using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace WindowsServiceChechWeb
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        System.Timers.Timer timer1 = new System.Timers.Timer();
        protected override void OnStart(string[] args)
        {
            
            YazLog($"Hizmet çalışmaya başladı. Zaman: {DateTime.Now}");
            if (CheckForInternetConnection())
            {
                YazLog($"Aktif bir internet bağlantısı var. Zaman: {DateTime.Now}");
            }
            else
            {
                YazLog($"Aktif bir internet bağlantısı yok. Zaman: {DateTime.Now}");
                return;
            }
            try {
                int count = 0;
                Process[] procs = Process.GetProcessesByName("ConsoleApp1");
                if (procs.Length > 0)
                {
                    foreach (Process proc in procs)
                    {
                        count++;
                        //do other stuff if you need to find out if this is the correct proc instance if you have more than one
                        proc.Kill();
                    }
                }
                YazLog($"ConsoleApp1 isimli task {count} defa çalıştırılmış. Tüm task'ler kapatıldı.");
            }
            catch (Exception e)
            {
                YazHataLog($"Servis çalıştırılırken bir hata meydana geldi. Zaman:{DateTime.Now}\n{e.Message}");
                return;
            }
            
            timer1.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer1.Interval = 1800000;
            timer1.Enabled = true;
            
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            
            if (CheckForInternetConnection())
            {
                YazLog($"Aktif bir internet bağlantısı var. Zaman: {DateTime.Now}");
            }
            else
            {
                YazLog($"Aktif bir internet bağlantısı yok. Zaman: {DateTime.Now}");
                return;
            }
            string _url = "https://youtube.com";
            if (CheckForSiteConnection(_url))
            {
                YazLog($"{_url} sitesine bağlantı var. Zaman: {DateTime.Now}");
            }
            else
            {
                YazLog($"{_url} sitesine bağlantı yok. Zaman: {DateTime.Now}");
            }
        }
        //Yöntem 3
        public static bool CheckForInternetConnection(int timeoutMs = 10000)
        {
            try
            {
                string url = "http://www.gstatic.com/generate_204";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                YazHataLog($"Servis çalıştırılırken bir hata meydana geldi. Zaman:{DateTime.Now}\n{e.Message}");
                return false;
            }
        }

        /* Yöntem 1
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
        */


        /* Yöntem 2 
         public static bool IsAvailableNetworkActive()
         {
             if (NetworkInterface.GetIsNetworkAvailable())
             {
                 NetworkInterface[] interfaces =NetworkInterface.GetAllNetworkInterfaces();
                 return (from face in interfaces
                         where face.OperationalStatus == OperationalStatus.Up
                         where (face.NetworkInterfaceType != NetworkInterfaceType.Tunnel) && (face.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                         select face.GetIPv4Statistics()).Any(statistics => (statistics.BytesReceived > 0) && (statistics.BytesSent > 0));
             }

             return false;
         }*/
        public static bool CheckForSiteConnection(string url,int timeoutMs = 10000)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception e)
            {
                YazHataLog($"Servis çalıştırılırken bir hata meydana geldi. Zaman:{DateTime.Now}\n{e.Message}");
                return false;
            }
        }
        protected override void OnStop()
        {
            YazLog($"Hizmet çalışmayı sonlandırdı. Zaman: {DateTime.Now}");
        }
        public void YazLog(string girdi)
        {
            string path = @"C:\Users\kadir\OneDrive\Desktop\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = $@"{path}\ServisGirdi_{DateTime.Now.Date.ToShortDateString().Replace('/', '_')}.txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(girdi);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(girdi);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
        }

        public static void YazHataLog(string girdi)
        {
            string path = @"C:\Users\kadir\OneDrive\Desktop\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = $@"{path}\ServisHataGirdi_{DateTime.Now.Date.ToShortDateString().Replace('/', '_')}.txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(girdi);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(girdi);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
        }
    }
}
