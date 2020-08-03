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

namespace RusDictionary
{
    public partial class MainForm : Form
    {        
        /// <summary>
        /// IP сервера
        /// </summary>
        string IP = "127.0.0.1";
        /// <summary>
        /// Порт MySQL
        /// </summary>
        string Port = "3306";
        /// <summary>
        /// Логин пользователя DB
        /// </summary>
        string User = "mysql";
        /// <summary>
        /// Имя базы данных
        /// </summary>
        string DataBase = "db";
        /// <summary>
        /// Пароль пользователя DB
        /// </summary>
        string Password = "mysql";
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
        public MainForm()
        {
            Program.f1 = this;
            InitializeComponent(); 
            pbWait.Visible = false;
            laWait.Visible = false;
            Connnect = "server=" + IP + ";user=" + User + ";database=" + DataBase + ";port=" + Port + ";password=" + Password;
            ToolTip t = new ToolTip();
            t.SetToolTip(pbStatusConnect, "Принудительное подключение к базе данных");
            ChangeStatus();
            TimerStatusConnect.Start();
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
    }
}
