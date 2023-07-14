using System;
using System.IO;
using System.Net;
using System.ServiceProcess;

namespace WindowsServiceChechWeb
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            YazLog($"Hizmet çalışmaya başladı. Zaman: {DateTime.Now}");
            if (CheckForInternetConnection())
            {
                YazLog($"Aktif bir internet bağlantısı var.");
            }
            else
            {
                YazLog($"Aktif bir internet bağlantısı yok.");
                return;
            }
            //string _url = "http://serkanogutcuoglu.website/";
            string _url = "http://thissiteiscreatedbyimagination.site/";
            if (CheckForSiteConnection(_url))
            {
                YazLog($"{_url} sitesine bağlantı var.");
            }
            else
            {
                YazLog($"{_url} sitesine bağlantı yok.");
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
            catch
            {
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
            catch
            {
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
    }
}
