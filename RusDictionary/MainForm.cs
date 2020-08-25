using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace RusDictionary
{
    public partial class MainForm : Form
    {        
        /// <summary>
        /// IP сервера
        /// </summary>
        string IP;
        /// <summary>
        /// Порт MySQL
        /// </summary>
        string Port;
        /// <summary>
        /// Логин пользователя DB
        /// </summary>
        string User;
        /// <summary>
        /// Имя базы данных
        /// </summary>
        string DataBase;
        /// <summary>
        /// Пароль пользователя DB
        /// </summary>
        string Password;
        /// <summary>
        /// Путь к БД
        /// </summary>
        public static string Connnect;
        /// <summary>
        /// Принудительное подключение
        /// </summary>
        bool ForcedConnect = false;
        /// <summary>
        /// Статус подключения к БД
        /// </summary>
        bool StatusConnect = false;
        /// <summary>
        /// Вспомогательная переменная
        /// </summary>
        int TMP = 0;
        /// <summary>
        /// Вспомогательная переменная для точки
        /// </summary>
        int TMPDot = 0;
        /// <summary>
        /// Вспомогательная переменная количества точек
        /// </summary>
        int TMPMaxCountDot = 3;
        /// <summary>
        /// Видимость пароля на форме
        /// </summary>
        bool SeePass = false;
        public MainForm()
        {
            Program.f1 = this;
            InitializeComponent(); 
            pbWait.Visible = false;
            laWait.Visible = false;
            FillSetting();
            Connnect = "server=" + IP + ";user=" + User + ";database=" + DataBase + ";port=" + Port + ";password=" + Password;
            ToolTip t = new ToolTip();
            t.SetToolTip(pbStatusConnect, "Принудительное подключение к базе данных");
            StatusConnectionMethod();
            ChangeStatus();            
            TimerStatusConnect.Start();
        }       
        void FillSetting()
        {
            IP = Properties.Settings.Default.IP;
            Port = Properties.Settings.Default.Port;
            User = Properties.Settings.Default.User;
            DataBase = Properties.Settings.Default.NameDB;
            Password = Properties.Settings.Default.Password;

            tbIP.Text = IP;
            tbPort.Text = Port;
            tbUser.Text = User;
            tbPassword.Text = Password;
            tbNameDB.Text = DataBase;

        }
        /// <summary>
        /// Фоновый запрос в БД 
        /// </summary>
        void StatusConnectionMethod()
        {
            // Создаем экземпляр подключения к БД
            MySqlConnection conn = new MySqlConnection(Connnect);
            try
            {
                // Подключаемся к БД
                conn.Open();
                StatusConnect = true;
            }
            catch
            {
                StatusConnect = false;
            }
            conn.Close();
        }
        /// <summary>
        /// Проверка подключения к БД
        /// </summary>
        void ChangeStatus()
        {            
            if (StatusConnect == true)
            {
                pbStatusConnect.BackgroundImage = Properties.Resources.StatusTrue;
                ForcedConnect = false;
                laStatus.Text = "Статус подключения к Базе Данных: Подключено";
            }
            else
            {
                pbStatusConnect.BackgroundImage = Properties.Resources.StatusFalse;
                ForcedConnect = true;
                laStatus.Text = "Статус подключения к Базе Данных: Подключение отсутствует";
            }
        }      
        /// <summary>
        /// Срабатывание таймера
        /// </summary>
        private void TimerStatusConnect_Tick(object sender, EventArgs e)
        {
            Thread myThread = new Thread(StatusConnectionMethod); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
            myThread.IsBackground = true; // Делаем поток фоновым
            myThread.Start(); // Запускаем поток
            ChangeStatus();
        }
        /// <summary>
        /// Принудительное подключение к БД
        /// </summary>
        private void pbStatusConnect_Click(object sender, EventArgs e)
        {
            StatusConnectionMethod();            
            ChangeStatus();
            if (ForcedConnect == true)
            {
                MessageBox.Show("Нет подключения к базе данных. Обратитесь к администратору!");
            }
        }

        private void buAuthors_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpAuthors;
        }

        private void buAuthorPrev_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpMain;
        }
        /// <summary>
        /// Отключение курсора при нажатии на TextBox
        /// </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);        
        /// <summary>
        /// Открытие ссылок в браузере
        /// </summary>
        private void textBox_DoubleClick(object sender, EventArgs e)
        {
            HideCaret((sender as TextBox).Handle);
            switch ((sender as TextBox).Name.ToString())
            {
                case "textBox2":
                    {
                        System.Diagnostics.Process.Start("https://vk.com/warpack_sega6257");
                        break;
                    }
                case "textBox3":
                    {
                        System.Diagnostics.Process.Start("http://warpacksega67.std-996.ist.mospolytech.ru");
                        break;
                    }
                case "textBox4":
                    {
                        System.Diagnostics.Process.Start("");//Страница Вадима
                        break;
                    }
                case "textBox5":
                    {
                        System.Diagnostics.Process.Start("https://vk.com/vadyaboy");
                        break;
                    }
                case "textBox7":
                    {
                        System.Diagnostics.Process.Start("https://www.it-claim.ru/Persons/PhilippovichYuriy/PhilippovichYuriy.htm");
                        break;
                    }               
                case "textBox10":
                    {
                        System.Diagnostics.Process.Start("http://mag.std-997.ist.mospolytech.ru");
                        break;
                    }
                case "textBox11":
                    {
                        System.Diagnostics.Process.Start("https://vk.com/rick_351");
                        break;
                    }
            }
        }

        private void buCardIndexModule_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpCardIndex;
        }

        public void TCPrev()
        {
            MainTC.SelectedTab = tpMain;
        }
        /// <summary>
        /// Включение видимости элементов загрузки
        /// </summary>
        public void PictAndLableWait(bool Status)
        {
            switch (Status)
            {
                case true:
                    pbWait.Visible = true;
                    laWait.Visible = true;
                    break;
                case false:
                    pbWait.Visible = false;
                    laWait.Visible = false;
                    break;
            }
        }
        private void textBoxAuthor_Enter(object sender, EventArgs e)
        {
            HideCaret((sender as TextBox).Handle);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Location = new Point(130, 10);
        }

        private void buMainFormSetting_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpSettings;
            //DialogResult result = MessageBox.Show("Настройки были изменены.\n\n Сохранить изменения?", "Настройки", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //switch (result)
            //{
            //    case DialogResult.Yes:
            //        break;
            //    case DialogResult.No:
            //        break;
            //}              
        }

        private void tbIP_Leave(object sender, EventArgs e)
        {
            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
            string OldIP = tbIP.Text;
            string NewIP = null;
            for (int i = 0; i < OldIP.Length; i++)
            {                
                if (OldIP[i].ToString() != " ")
                {
                    if (OldIP[i].ToString() == ",")
                    {
                        NewIP += ".";
                    }
                    else
                    {
                        NewIP += OldIP[i];
                    }
                }
            }
            tbIP.Text = NewIP;
            if (Regex.IsMatch(NewIP, pattern) != true)
            {
                MessageBox.Show("IP-адрес введен не корректно!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
            else
            {
                if (tbIP.Text.Length < 15)
                {
                    if (tbIP.Text.Length == 0 && e.KeyChar == 46)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        if (tbIP.Text.Length == 0 && e.KeyChar == 8)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (e.KeyChar != 8)
                            {
                                TMPMaxCountDot = 3;
                                for (int i = 0; i < tbIP.Text.Length; i++)
                                {
                                    if (tbIP.Text[i] == '.')
                                    {
                                        TMPMaxCountDot--;
                                    }
                                }    
                                TMPDot = 0;
                                for (int i = tbIP.Text.Length - 1; i >= 0; i--)
                                {
                                    if (tbIP.Text[i] != '.')
                                    {
                                        TMPDot++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (TMPDot > 2 && TMPMaxCountDot != 0)
                                {
                                    TMPDot = 0;
                                    tbIP.Text += '.';
                                    tbIP.SelectionStart = tbIP.Text.Length;
                                    TMPMaxCountDot--;
                                }
                                if (e.KeyChar == 46 && TMPMaxCountDot <= 0)
                                {
                                    e.Handled = true;
                                }
                                else
                                {
                                    if (TMPDot > 2)
                                    {
                                        e.Handled = true;
                                    }
                                    TMPDot++;
                                }
                            }

                        }
                    }
                }
            }
        }

        private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
        }

        private void buSeePass_Click(object sender, EventArgs e)
        {
            switch (SeePass)
            {
                case true:                    
                    SeePass = false;
                    buSeePass.Image = Properties.Resources.EyeClose;
                    tbPassword.PasswordChar = '*';
                    break;
                case false:
                    SeePass = true;
                    buSeePass.Image = Properties.Resources.EyeOpen;
                    tbPassword.PasswordChar = '\0';
                    break;
            }
        }

        private void buSeePass_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (SeePass)
            {
                case false:
                    buSeePass.Image = Properties.Resources.EyeOpen;
                    break;
                case true:
                    buSeePass.Image = Properties.Resources.EyeClose;
                    break;
            }
        }

        private void buSeePass_MouseLeave(object sender, EventArgs e)
        {
            switch (SeePass)
            {
                case true:
                    buSeePass.Image = Properties.Resources.EyeOpen;
                    break;
                case false:
                    buSeePass.Image = Properties.Resources.EyeClose;
                    break;
            }
        }
    }
}
