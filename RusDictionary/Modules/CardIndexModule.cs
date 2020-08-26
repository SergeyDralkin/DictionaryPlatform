using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace RusDictionary.Modules
{
    public partial class CardIndexModule : UserControl
    {
        int ListBoxSelectedIndex;
        bool ListBoxPrev = true;
        string NameClickButton;
        /// <summary>
        /// Первый список элементов
        /// </summary>
        List<string> FirstListItems = new List<string>();
        /// <summary>
        /// Второй список элементов
        /// </summary>
        List<string> SecondListItems = new List<string>();
        /// <summary>
        /// Класс формы поиска
        /// </summary>
        public static Search f4 = new Search();
        /// <summary>
        /// Отслеживание нажатия на кнопку "Маркер"
        /// </summary>
        public static bool CardIndexMenuMarker = false;
        /// <summary>
        /// Отслеживание нажатия на кнопку "Карточка-разделитель"
        /// </summary>
        public static bool CardIndexMenuSeparator = false;
        /// <summary>
        /// Отслеживание нажатия на кнопку "Ящик"
        /// </summary>
        public static bool CardIndexMenuBox = false;
        /// <summary>
        /// Отслеживание нажатия на кнопку "Буква"
        /// </summary>
        public static bool CardIndexMenuLetter = false;

        Image CardImage;
        string CardLetter;
        string CardBox;
        string CardFirstSeparator;
        string CardLastSeparator;



        public CardIndexModule()
        {
            InitializeComponent();
        }

        private void buCardIndexListSearch_Click(object sender, EventArgs e)
        {
            ShowSearchForm();
        }
        public static void ShowSearchForm()
        {
            //f1.DGVInicilizationResult();
            //Если форма была закрыта
            if (f4.ActiveFormSearch == false)
            {
                f4.ActiveFormSearch = true; // Присвоить значение true для переменной ActiveForm2 второй формы
                f4.Show(); // Отобразить форму
            }
        }

        private void buCardIndexInMenuButton_Click(object sender, EventArgs e)
        {
            Program.f1.PictAndLableWait(true);
            NameClickButton = (sender as Button).Name.ToString();
            ClearMainList();
            Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
            myThread.Start(NameClickButton); // Запускаем поток
            while (myThread.IsAlive)
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }
            for (int i = 0; i < FirstListItems.Count; i++)
            {
                lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i]);
            }
            lbCardIndexList.Update();
            tcCards.SelectedTab = tpList; 
            Program.f1.PictAndLableWait(false);
        }
        void CreateSecondListItems(object NameButton)
        {
            SecondListItems.Clear();
            switch (NameClickButton)
            {
                case "buCardIndexMenuSeparator":
                    {
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            conn.Open();
                            string NewSql = "SELECT Marker FROM cardindex WHERE CardSeparator = " + ListBoxSelectedIndex;
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                SecondListItems.Add(reader["Marker"].ToString());
                            }
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            conn.Open();
                            string NewSql = "SELECT Marker FROM cardindex WHERE Box = " + ListBoxSelectedIndex;
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                SecondListItems.Add(reader["Marker"].ToString());
                            }
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            conn.Open();
                            string NewSql = "SELECT Marker FROM cardindex WHERE Letter = " + ListBoxSelectedIndex;
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                SecondListItems.Add(reader["Marker"].ToString());
                            }
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
            }
        }
        void CreateFirstListItems(object NameButton)
        {
            FirstListItems.Clear();
            switch (NameButton)
            {
                case "buCardIndexMenuMarker":
                    {                        
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            CardIndexMenuMarker = true;
                            CardIndexMenuSeparator = false;
                            CardIndexMenuBox = false;
                            CardIndexMenuLetter = false;
                            conn.Open();
                            string NewSql = "SELECT Marker FROM cardindex";
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                FirstListItems.Add(reader["Marker"].ToString());
                            }
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = true;
                        CardIndexMenuBox = false;
                        CardIndexMenuLetter = false;
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            conn.Open();
                            string NewSql = "SELECT CardSeparator FROM cardseparator";
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                FirstListItems.Add(reader["CardSeparator"].ToString());
                            }
                            lbCardIndexList.Update();
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = false;
                        CardIndexMenuBox = true;
                        CardIndexMenuLetter = false;
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            conn.Open();
                            string NewSql = "SELECT NumberBox FROM box";
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                FirstListItems.Add(reader["NumberBox"].ToString());
                            }
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = false;
                        CardIndexMenuBox = false;
                        CardIndexMenuLetter = true;
                        MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
                        try
                        {
                            conn.Open();
                            string NewSql = "SELECT Symbol FROM letter";
                            MySqlCommand command = new MySqlCommand(NewSql, conn);
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                FirstListItems.Add(reader["Symbol"].ToString());
                            }
                            conn.Close();
                        }
                        catch
                        { }
                        break;
                    }
            }
        }
        void ClearMainList()
        {            
            lbCardIndexList.ClearSelected();
            lbCardIndexList.Items.Clear();
        }
        private void buCardIndexMenuPrev_Click(object sender, EventArgs e)
        {
            Program.f1.TCPrev();
        }

        private void buCardIndexListPrev_Click(object sender, EventArgs e)
        {
            if (NameClickButton == "buCardIndexMenuMarker")
            {
                tcCards.SelectedTab = tpCardsMenu;
            }
            else
            {
                if (ListBoxPrev != true)
                {
                    CardIndexMenuMarker = false;
                    ClearMainList();
                    ListBoxPrev = true;
                    for (int i = 0; i < FirstListItems.Count; i++)
                    {
                        lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i]);
                    }
                }
                else
                {
                    tcCards.SelectedTab = tpCardsMenu;
                }                
            }            
        }

        private void lbCardIndexList_DoubleClick(object sender, EventArgs e)
        {
            ListBoxSelectedIndex = lbCardIndexList.SelectedIndex + 1;
            ListBoxPrev = false;
            if (CardIndexMenuMarker == true)
            {
                Program.f1.PictAndLableWait(true);
                Thread myThread = new Thread(new ParameterizedThreadStart(ShowCards)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                string[] SplitItem = lbCardIndexList.SelectedItem.ToString().Split(')', ' ');
                myThread.Start(SplitItem.Last()); // Запускаем поток
                while (myThread.IsAlive)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
                pbPictCard.BackgroundImage = CardImage;
                laCardsNumberCard.Text = "Текст карточки №" + SplitItem.Last() + ":";
                laCardsLetter.Text = CardLetter;
                laCardsNumberBox.Text = CardBox;
                laCardsFirstSeparator.Text = CardFirstSeparator;
                laCardsLastSeparator.Text = CardLastSeparator;
                tcCards.SelectedTab = tpCards;
                Program.f1.PictAndLableWait(false);
            }
            else
            {
                Program.f1.PictAndLableWait(true);
                Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                myThread.Start(NameClickButton); // Запускаем поток
                while (myThread.IsAlive)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
                ClearMainList();
                for (int i = 0; i < SecondListItems.Count; i++)
                {
                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i]);
                }
                lbCardIndexList.Update();
                CardIndexMenuMarker = true;
                lbCardIndexList.Visible = true;
                Program.f1.PictAndLableWait(false);
            }            
        }

        void ShowCards(object Number)
        {            
            MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
            try
            {
                conn.Open();
                string NewSql = "SELECT * FROM cardindex WHERE Marker = '" + Number + "'";
                MySqlCommand command = new MySqlCommand(NewSql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                CardImage = DecodeImageFromDB(reader["img"].ToString());
                string ReadSymbol = reader["Letter"].ToString();
                string ReadBox = reader["Box"].ToString();
                reader.Close();
                ////////////////////////
                string Sql = "SELECT Symbol FROM letter WHERE ID = " + ReadSymbol;                
                MySqlCommand command1 = new MySqlCommand(Sql, conn);
                MySqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    CardLetter = "Буква: " + reader1["Symbol"].ToString();
                }
                reader1.Close();
                ////////////////////////
                Sql = "SELECT NumberBox FROM box WHERE ID = " + ReadBox;
                command1 = new MySqlCommand(Sql, conn);
                reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    CardBox = "Ящик: " + reader1["NumberBox"].ToString();
                }
                reader1.Close();
                ////////////////////////
                Sql = "SELECT CardSeparator FROM cardseparator WHERE BoxNumber = " + ReadBox;
                command1 = new MySqlCommand(Sql, conn);
                reader1 = command1.ExecuteReader();
                bool tmp = false;
                while (reader1.Read())
                {
                    if (tmp == false)
                    {
                        CardFirstSeparator = "Первый разделитель: " + reader1["CardSeparator"].ToString();
                        tmp = !tmp;
                    }
                    else
                    {
                        CardLastSeparator = "Последний разделитель: " + reader1["CardSeparator"].ToString();
                    }
                }
                reader1.Close();
                conn.Close();                    
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }                     
        }
        /// <summary>
        /// Кодирование изображения в base64
        /// </summary>
        /// <param name="FilePath">Путь к изображению</param>
        /// <returns>Закодированное изображение</returns>
        string CodeInBase64(string FilePath)
        {
            return Convert.ToBase64String(File.ReadAllBytes(FilePath));
        }
        /// <summary>
        /// Декодирование изображения
        /// </summary>
        /// <param name="Newbase64FromBD">Закодированное изображение</param>
        /// <returns>Декодированное изображение</returns>
        Image DecodeImageFromDB(string Newbase64FromBD)
        {
            if (Newbase64FromBD != "")
            {
                return Image.FromStream(new MemoryStream(Convert.FromBase64String(Newbase64FromBD)));
            }
            else
            {
                return Properties.Resources.noimage;
            }
        }

        private void buCardIndexCardsPrev_Click(object sender, EventArgs e)
        {
            tcCards.SelectedTab = tpList;
        }
    }
}
