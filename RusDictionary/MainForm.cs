using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace RusDictionary
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Имя вкладки, на которую нужно перейти
        /// </summary>
        public static string NameTabPage;
        /// <summary>
        /// IP сервера
        /// </summary>
        public static string IP;
        /// <summary>
        /// Порт MySQL
        /// </summary>
        public static string Port;      
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
        /// Цвет текста
        /// </summary>
        Color ColorText;
        /// <summary>
        /// Цвет фона
        /// </summary>
        Color ColorBackground;
        /// <summary>
        /// Цвет текстового поля
        /// </summary>
        Color ColorTextBox;
        /// <summary>
        /// Цвет фона кнопки
        /// </summary>
        Color ColorButton;
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public static string URL = null;
        /// <summary>
        /// Залогинен
        /// </summary>
        bool IfLogin = false;
        /// <summary>
        /// Количество попыток подключиться. Данная переменная нужна для отображения задержки отключения от сервера 
        /// (если вдруг были потеряны пакеты и программа сразу же не отключила пользоватя от сервера) 
        /// </summary>
        int tmpConnect = 12;
        /// <summary>
        /// Список того, что может делать роль пользователя
        /// </summary>
        public static List<JSONArray> CanDoItList = new List<JSONArray>();
        /// <summary>
        /// Отобразить пароль
        /// </summary>
        bool VisiblePass = false;
        public MainForm()
        {
            InitializeComponent();
            //Для тестов закоментировать строчку снизу
            HideTabs();
            //Для тестов закоментировать строчку сверху
            SetupFont();
            Program.f1 = this;            
            pbWait.Visible = false;
            laWait.Visible = false;
            FillSetting();            
            ToolTip t = new ToolTip();
            t.SetToolTip(pbStatusConnect, "Принудительное подключение к базе данных");
            //StatusConnectionMethod();
            //ChangeStatus();
            TimerStatusConnect.Start();
        }
        /// <summary>
        /// Скрыть вкладки
        /// </summary>
        void HideTabs()
        {
            foreach (TabControl tabControl in GetAll(this, typeof(TabControl)))
            {
                tabControl.Appearance = TabAppearance.FlatButtons;
                tabControl.ItemSize = new Size(0, 1);
                tabControl.SizeMode = TabSizeMode.Fixed;
                tabControl.TabStop = false;
            }
        }
        /// <summary>
        /// Установить шрифты на форме
        /// </summary>
        void SetupFont()
        {
            laStatus.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            laWait.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            #region Вкладка "Логин"
            laLogin.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            laPassword.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buLogin.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            #endregion
            #region Вкладка "Главная"
            label1.Font = new Font("Izhitsa", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);            
            foreach (Button button in GetAll(tpMain, typeof(Button)))
            {
                if (button.Name == "buCardIndexCardsPrev" || button.Name == "buCardIndexCardsSave")
                {
                    button.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
                }
                else
                {
                    button.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
                }
            }
            #endregion
            #region Вкладка "Авторы"
            foreach (Label label in GetAll(tpAuthors, typeof(Label)))
            {
                if (label.Name == "label3" || label.Name == "label9" || label.Name == "label22" || label.Name == "label16")
                {
                    label.Font = new Font("Izhitsa", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
                else if (label.Name == "label12" || label.Name == "label6" || label.Name == "label8" || label.Name == "label10" || label.Name == "label14" || label.Name == "label13" || label.Name == "label2" || label.Name == "label26" || label.Name == "label25" || label.Name == "label24" || label.Name == "label20" || label.Name == "label18")
                {
                    label.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
                }
                else if (label.Name == "label15")
                {
                    label.Font = new Font("Izhitsa", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }                
                else
                {
                    label.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
            buAuthorPrev.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
            #endregion
            #region Вкладка "Настройки"
            foreach (Label label in GetAll(tpSettings, typeof(Label)))
            {
                label.Font = new Font("Izhitsa", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            }
            foreach (Button button in GetAll(tpSettings, typeof(Button)))
            {
                if (button.Name == "buTextBoxColor" || button.Name == "buColorDefault")
                {
                    button.Font = new Font("Izhitsa", 13.5F, FontStyle.Regular, GraphicsUnit.Point, 204);
                }
                else
                {
                    button.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
                }
            }
            #endregion        
        }
        void FillSetting()
        {
            IP = Properties.Settings.Default.IP;
            Port = Properties.Settings.Default.Port;
            ColorText = Properties.Settings.Default.ColorText;
            ColorBackground = Properties.Settings.Default.ColorBackground;
            ColorTextBox = Properties.Settings.Default.ColorTextBox;
            ColorButton = Properties.Settings.Default.ColorButton;

            string[] massIP = IP.Split('.');
            tbIP.FirstBox.Text = massIP[0];
            tbIP.SecondBox.Text = massIP[1];
            tbIP.ThirdBox.Text = massIP[2];
            tbIP.FourthBox.Text = massIP[3];
            tbPort.Text = Port;

            foreach (Label label in GetAll(this, typeof(Label)))
            {
                label.ForeColor = Properties.Settings.Default.ColorText;
            }
            foreach (Button button in GetAll(this, typeof(Button)))
            {
                button.ForeColor = Properties.Settings.Default.ColorText;
            }
            foreach (TextBox textbox in GetAll(this, typeof(TextBox)))
            {
                textbox.ForeColor = Properties.Settings.Default.ColorText;
            }
            foreach (TabPage tabPage in GetAll(this, typeof(TabPage)))
            {
                tabPage.BackColor = Properties.Settings.Default.ColorBackground;
            }
            foreach (TextBox textbox in GetAll(this, typeof(TextBox)))
            {
                textbox.BackColor = Properties.Settings.Default.ColorTextBox;
            }
            foreach (ListBox listBox in GetAll(this, typeof(ListBox)))
            {
                listBox.BackColor = Properties.Settings.Default.ColorTextBox;
            }
            foreach (Button button in GetAll(this, typeof(Button)))
            {
                button.BackColor = Properties.Settings.Default.ColorButton;
            }
        }
        /// <summary>
        /// Фоновый запрос в БД 
        /// </summary>
        void StatusConnectionMethod()
        {                   
            try
            {
                string query = "SELECT 1";
                JSON.Send(query, JSONFlags.Select);
                StatusConnect = true;
            }
            catch
            {
                StatusConnect = false;
            }
        }
        /// <summary>
        /// Проверка подключения к БД
        /// </summary>
        void ChangeStatus()
        {
            if (StatusConnect == true)
            {
                tmpConnect = 0;
                pbStatusConnect.BackgroundImage = Properties.Resources.StatusTrue;
                ForcedConnect = false;
                laStatus.Text = "Статус подключения к Базе Данных: Подключено";
                buCardIndexModule.Enabled = true;
                buWordSearchModule.Enabled = true;
                buIndexModule.Enabled = true;
            }
            else
            {
                tmpConnect++;                
                if (tmpConnect > 10)
                {
                    pbStatusConnect.BackgroundImage = Properties.Resources.StatusFalse;
                    ForcedConnect = true;
                    laStatus.Text = "Статус подключения к Базе Данных: Подключение отсутствует";
                    buCardIndexModule.Enabled = false;
                    buWordSearchModule.Enabled = false;
                    buIndexModule.Enabled = false;
                    if ((MainTC.SelectedTab == tpCardIndex || MainTC.SelectedTab == tpWordSearch || MainTC.SelectedTab == tpPointer) && tmpConnect == 11)
                    {
                        MainTC.SelectedTab = tpMain;
                        MessageBox.Show("Потеряно соединение с сервером");
                        this.Activate();
                    }
                }
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
            RepairBackgroundTextbox();
        }
        /// <summary>
        /// Принудительное подключение к БД
        /// </summary>
        private void pbStatusConnect_Click(object sender, EventArgs e)
        {
            Thread myThread = new Thread(StatusConnectionMethod); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
            myThread.IsBackground = true; // Делаем поток фоновым
            myThread.Start(); // Запускаем поток
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
        void RepairBackgroundTextbox()
        {
            textBox1.BackColor = tpAuthors.BackColor;
            textBox2.BackColor = tpAuthors.BackColor;
            textBox3.BackColor = tpAuthors.BackColor;
            textBox4.BackColor = tpAuthors.BackColor;
            textBox5.BackColor = tpAuthors.BackColor;
            textBox6.BackColor = tpAuthors.BackColor;
            textBox7.BackColor = tpAuthors.BackColor;
            textBox9.BackColor = tpAuthors.BackColor;
            textBox10.BackColor = tpAuthors.BackColor;
            textBox11.BackColor = tpAuthors.BackColor;
            textBox12.BackColor = tpAuthors.BackColor;
        }
        /// <summary>
        /// Отключение курсора при нажатии на TextBox
        /// </summary>
        [DllImport("user32.dll")]
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
                        System.Diagnostics.Process.Start("http://melkov.std-991.ist.mospolytech.ru");
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
            NameTabPage = MainTC.SelectedTab.Name;
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
        }

        private void tbIP_Leave(object sender, EventArgs e)
        {
            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";


            string OldIP = tbIP.FirstBox.Text + "." + tbIP.SecondBox.Text + "." + tbIP.ThirdBox.Text + "." + tbIP.FourthBox.Text;
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

            string[] massIP = NewIP.Split('.');
            tbIP.FirstBox.Text = massIP[0];
            tbIP.SecondBox.Text = massIP[1];
            tbIP.ThirdBox.Text = massIP[2];
            tbIP.FourthBox.Text = massIP[3];            
            if (Regex.IsMatch(NewIP, pattern) != true)
            {
                MessageBox.Show("IP-адрес введен не корректно!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
        }

        private void buChangeColor_Click(object sender, EventArgs e)
        {
            if (cdChangeColor.ShowDialog() == DialogResult.OK)
            {
                foreach (TabPage tabPage in GetAll(this, typeof(TabPage)))
                {
                    tabPage.BackColor = cdChangeColor.Color;
                }
                ColorBackground = cdChangeColor.Color;
            }
        }
        private void buTextColor_Click(object sender, EventArgs e)
        {
            if (cdChangeColor.ShowDialog() == DialogResult.OK)
            {
                foreach (Label label in GetAll(this,typeof(Label)))
                {
                    label.ForeColor = cdChangeColor.Color;
                }
                foreach (Button button in GetAll(this, typeof(Button)))
                {
                    button.ForeColor = cdChangeColor.Color;
                }
                foreach (TextBox textbox in GetAll(this, typeof(TextBox)))
                {
                    textbox.ForeColor = cdChangeColor.Color;
                }
                ColorText = cdChangeColor.Color;
            }
        }        

        private void buTextBoxColor_Click(object sender, EventArgs e)
        {
            if (cdChangeColor.ShowDialog() == DialogResult.OK)
            {
                foreach (TextBox textbox in GetAll(this, typeof(TextBox)))
                {
                    textbox.BackColor = cdChangeColor.Color;
                }
                foreach (ListBox listBox in GetAll(this, typeof(ListBox)))
                {
                    listBox.BackColor = cdChangeColor.Color;
                }
                ColorTextBox = cdChangeColor.Color;
            }
        }
        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

        private void buPrevSettings_Click(object sender, EventArgs e)
        {
            string ipNew = tbIP.FirstBox.Text + "." + tbIP.SecondBox.Text + "." + tbIP.ThirdBox.Text + "." + tbIP.FourthBox.Text;
            if (Properties.Settings.Default.IP == ipNew && Properties.Settings.Default.Port == tbPort.Text && Properties.Settings.Default.ColorText == label1.ForeColor && Properties.Settings.Default.ColorBackground == tpMain.BackColor && Properties.Settings.Default.ColorButton == buAuthors.BackColor)
            {
                if (IfLogin == true)
                {
                    MainTC.SelectedTab = tpMain;
                }
                else
                {
                    MainTC.SelectedTab = tpLogin;
                }
                
            }
            else
            {
                DialogResult result = MessageBox.Show("Настройки были изменены.\n\n Сохранить изменения?", "Настройки", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                switch (result)
                {
                    case DialogResult.Yes:                        
                        URL = null;
                        StatusConnect = false;
                        ChangeStatus();
                        IP = tbIP.FirstBox.Text + "." + tbIP.SecondBox.Text + "." + tbIP.ThirdBox.Text + "." + tbIP.FourthBox.Text;
                        Port = tbPort.Text;
                        Properties.Settings.Default.IP = IP;
                        Properties.Settings.Default.Port = Port;
                        Properties.Settings.Default.ColorText = ColorText;
                        Properties.Settings.Default.ColorBackground = ColorBackground;
                        Properties.Settings.Default.ColorTextBox = ColorTextBox;
                        Properties.Settings.Default.ColorButton = ColorButton;
                        Properties.Settings.Default.Save();
                        if (IfLogin == true)
                        {
                            MainTC.SelectedTab = tpMain;
                        }
                        else
                        {
                            MainTC.SelectedTab = tpLogin;
                        }
                        break;
                    case DialogResult.No:
                        FillSetting();
                        if (IfLogin == true)
                        {
                            MainTC.SelectedTab = tpMain;
                        }
                        else
                        {
                            MainTC.SelectedTab = tpLogin;
                        }
                        break;
                }
            }
        }

        private void buSaveSettings_Click(object sender, EventArgs e)
        {
            URL = null;
            StatusConnect = false;
            ChangeStatus();
            IP = tbIP.FirstBox.Text + "." + tbIP.SecondBox.Text + "." + tbIP.ThirdBox.Text + "." + tbIP.FourthBox.Text;
            Port = tbPort.Text;
            ColorText = label1.ForeColor;
            ColorBackground = tpMain.BackColor;            
            ColorButton = buAuthors.BackColor;
            ColorTextBox = tbPort.BackColor;
            Properties.Settings.Default.IP = IP;
            Properties.Settings.Default.Port = Port;
            Properties.Settings.Default.ColorText = ColorText;
            Properties.Settings.Default.ColorBackground = ColorBackground;
            Properties.Settings.Default.ColorTextBox = ColorTextBox;
            Properties.Settings.Default.ColorButton = ColorButton;
            Properties.Settings.Default.Save();
            MessageBox.Show("Настройки были сохранены", "Сохранение настроек", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buColorDefault_Click(object sender, EventArgs e)
        {
            foreach (Label label in GetAll(this, typeof(Label)))
            {
                label.ForeColor = Color.Black;
            }
            foreach (Button button in GetAll(this, typeof(Button)))
            {
                button.ForeColor = Color.Black;
            }
            foreach (TextBox textbox in GetAll(this, typeof(TextBox)))
            {
                textbox.ForeColor = Color.Black;
            }
            foreach (TabPage tabPage in GetAll(this, typeof(TabPage)))
            {
                tabPage.BackColor = Color.SandyBrown;
            }
            foreach (TextBox textbox in GetAll(this, typeof(TextBox)))
            {
                textbox.BackColor = Color.White;
            }
            foreach (ListBox listBox in GetAll(this, typeof(ListBox)))
            {
                listBox.BackColor = Color.White;
            }
            foreach (Button button in GetAll(this, typeof(Button)))
            {
                button.BackColor = Color.White;
            }
            Properties.Settings.Default.ColorText = Color.Black;
            Properties.Settings.Default.ColorBackground = Color.SandyBrown;
            Properties.Settings.Default.ColorTextBox = Color.White;
            Properties.Settings.Default.ColorButton = Color.White;
        }

        private void buWordSearchModule_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpWordSearch;
            NameTabPage = MainTC.SelectedTab.Name;
        }

        private void buIndexModule_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpPointer;
            NameTabPage = MainTC.SelectedTab.Name;
        }

        private void buButtonColor_Click(object sender, EventArgs e)
        {
            if (cdChangeColor.ShowDialog() == DialogResult.OK)
            {
                foreach (Button button in GetAll(this, typeof(Button)))
                {
                    button.BackColor = cdChangeColor.Color;
                }
                ColorTextBox = cdChangeColor.Color;
            }
        }

        private void buLogin_Click(object sender, EventArgs e)
        {
            StatusConnectionMethod();
            if (StatusConnect == true)
            {
                IfLogin = false;
                List<JSONArray> PasswordList = new List<JSONArray>();
                string Login = tbLogin.Text.ToString();
                string Password = tbPassword.Text.ToString();
                if (Login != null && Login != "" && Password != null && Password != "")
                {
                    string query = "SELECT * FROM users WHERE Login = '" + Login + "'";
                    if (StatusConnect == true)
                    {
                        JSON.Send(query, JSONFlags.Select);
                        PasswordList = JSON.Decode();
                        try
                        {
                            if (PasswordList != null)
                            {
                                if (PasswordList[0].Password.Equals(Password))
                                {
                                    query = "SELECT * FROM roles WHERE ID = '" + PasswordList[0].Role + "'";
                                    JSON.Send(query, JSONFlags.Select);
                                    CanDoItList = JSON.Decode();
                                    IfLogin = true;
                                    MainTC.SelectedTab = tpMain;
                                }
                            }
                            else
                            {
                                IfLogin = false;
                            }
                        }
                        catch
                        {
                            IfLogin = false;
                        }
                        if (IfLogin == false)
                        {
                            MessageBox.Show("Не правильно введён логин или пароль. Повторите попытку");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Отсутствует подключение к серверу!");
            }
        }
        private void buSettingLogin_Click(object sender, EventArgs e)
        {
            MainTC.SelectedTab = tpSettings;
        }
        private void buVisiblePass_MouseLeave(object sender, EventArgs e)
        {
            switch (VisiblePass)
            {
                case true:
                    buVisiblePass.Image = Properties.Resources.EyeOpen;
                    break;
                case false:
                    buVisiblePass.Image = Properties.Resources.EyeClose;
                    break;
            }
        }

        private void buVisiblePass_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (VisiblePass)
            {
                case false:
                    buVisiblePass.Image = Properties.Resources.EyeOpen;
                    break;
                case true:
                    buVisiblePass.Image = Properties.Resources.EyeClose;
                    break;
            }
        }
        private void buVisiblePass_Click(object sender, EventArgs e)
        {
            switch (VisiblePass)
            {
                case false:
                    buVisiblePass.Image = Properties.Resources.EyeOpen;
                    tbPassword.PasswordChar = '\0';
                    VisiblePass = true;
                    break;
                case true:
                    buVisiblePass.Image = Properties.Resources.EyeClose;
                    tbPassword.PasswordChar = '*';
                    VisiblePass = false;
                    break;
            }
        }
        private void buExitLogin_Click(object sender, EventArgs e)
        {
            IfLogin = false;
            MainTC.SelectedTab = tpLogin;
        }
        public static System.Threading.Thread MyGlobalVar1;
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(MyGlobalVar1 != null)
            {
                MyGlobalVar1.Abort();
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                HelpNavigator navigator = HelpNavigator.TableOfContents;
                Help.ShowHelp(this, "Resources/Help.chm", navigator);
            }
        }
    }
}