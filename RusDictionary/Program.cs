using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace RusDictionary
{
    static class Program
    {
        /// <summary>
        /// MainForm
        /// </summary>
        public static MainForm f1;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                SetupFont();
            }
            catch
            { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        static void SetupFont()
        {
            File.Copy(Path.Combine(Environment.CurrentDirectory, "Resources", "IZHITSA.TTF"), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "IZHITSA.TTF"), true);
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
            key.SetValue("IZHITSA", "IZHITSA.TTF");
            key.Close();
        }
    }
}
