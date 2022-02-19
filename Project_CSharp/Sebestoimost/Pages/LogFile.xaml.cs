using Sebestoimost.Model;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Sebestoimost.Pages
{
    public partial class LogFile : Page
    {
        public LogFile()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<ALog> sourceList = new List<ALog>();
            using (StreamReader sr = new StreamReader(App.path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sourceList.Add(new ALog(line));
                }
            }
            sourceList.Sort(delegate (ALog a, ALog b) { return b.Date.CompareTo(a.Date); });
            GrdItems.ItemsSource = sourceList;
        }
    }
}
