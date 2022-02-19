using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Sebestoimost
{
    public partial class App : Application
    {
        public static Model.dbContext db = new Model.dbContext();
        public static Model.User user;
        public static string path = Directory.GetCurrentDirectory() + "\\" + "log.txt";

        public static string GetMD5(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static void SetLogText(string text)
        {
            File.AppendAllText(path, DateTime.Now.ToString() + "\t" + text + "\r\n");
        }
    }
}
