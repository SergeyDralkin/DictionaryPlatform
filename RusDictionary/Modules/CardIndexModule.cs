using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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
        List<JSONArray> FirstListItems = new List<JSONArray>();
        /// <summary>
        /// Второй список элементов
        /// </summary>
        List<JSONArray> SecondListItems = new List<JSONArray>();
        /// <summary>
        /// Список элементов одной карточки
        /// </summary>
        List<JSONArray> CardItems = new List<JSONArray>();
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
        /// <summary>
        /// Маркер карточки
        /// </summary>
        string CardMarker;
        /// <summary>
        /// Текст с карточки
        /// </summary>
        string CardText;
        /// <summary>
        /// Примечание с карточки
        /// </summary>
        string CardNotes;
        /// <summary>
        /// Изображение карточки
        /// </summary>
        Image CardImage;
        /// <summary>
        /// Буква карточки
        /// </summary>
        string CardSymbol;
        /// <summary>
        /// Номер ящика карточки
        /// </summary>
        string CardNumberBox;
        /// <summary>
        /// Первый разделитель ящика
        /// </summary>
        string CardFirstSeparator;
        /// <summary>
        /// Последний разделитель ящика
        /// </summary>
        string CardLastSeparator;

        public CardIndexModule()
        {
            InitializeComponent();
        }       
       
        private void buCardIndexInMenuButton_Click(object sender, EventArgs e)
        {
            EnableElement(false);
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
            switch (NameClickButton)
            {
                case "buCardIndexMenuMarker":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Marker);
                        }
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                        }
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                        }
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                        }
                        break;
                    }
            }            
            lbCardIndexList.Update();
            tcCards.SelectedTab = tpList; 
            Program.f1.PictAndLableWait(false);
            EnableElement(true);
        }
        void CreateSecondListItems(object NameButton)
        {
            SecondListItems.Clear();
            switch (NameClickButton)
            {
                case "buCardIndexMenuSeparator":
                    {                       
                        string query = "SELECT Marker FROM cardindex WHERE CardSeparator = " + ListBoxSelectedIndex;
                        JSON.Send(JSONFlags.Select, query);
                        SecondListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuBox":
                    {                       
                        string query = "SELECT Marker FROM cardindex WHERE NumberBox = " + ListBoxSelectedIndex;
                        JSON.Send(JSONFlags.Select, query);
                        SecondListItems = JSON.Decode();                       
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {                        
                        string query = "SELECT Marker FROM cardindex WHERE Symbol = " + ListBoxSelectedIndex;
                        JSON.Send(JSONFlags.Select, query);
                        SecondListItems = JSON.Decode();
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
                        CardIndexMenuMarker = true;
                        CardIndexMenuSeparator = false;
                        CardIndexMenuBox = false;
                        CardIndexMenuLetter = false;
                        string query = "SELECT Marker FROM cardindex";
                        JSON.Send(JSONFlags.Select, query);
                        FirstListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = true;
                        CardIndexMenuBox = false;
                        CardIndexMenuLetter = false;                        
                        string query = "SELECT CardSeparator FROM cardseparator";
                        JSON.Send(JSONFlags.Select, query);
                        FirstListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = false;
                        CardIndexMenuBox = true;
                        CardIndexMenuLetter = false;
                        string query = "SELECT NumberBox FROM box";
                        JSON.Send(JSONFlags.Select, query);
                        FirstListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = false;
                        CardIndexMenuBox = false;
                        CardIndexMenuLetter = true;                        
                        string query = "SELECT Symbol FROM letter";
                        JSON.Send(JSONFlags.Select, query);
                        FirstListItems = JSON.Decode();
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
                    switch (NameClickButton)
                    {
                        case "buCardIndexMenuSeparator":
                            {
                                for (int i = 0; i < FirstListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                                }
                                break;
                            }
                        case "buCardIndexMenuBox":
                            {
                                for (int i = 0; i < FirstListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                                }
                                break;
                            }
                        case "buCardIndexMenuLetter":
                            {
                                for (int i = 0; i < FirstListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                                }
                                break;
                            }
                    }
                }
                else
                {
                    tcCards.SelectedTab = tpCardsMenu;
                }                
            }            
        }
        void EnableElement(bool parameter)
        {
            foreach (Button button in MainForm.GetAll(tpList, typeof(Button)))
            {
                button.Enabled = parameter;
            }
            foreach (Button button in MainForm.GetAll(tpCardsMenu, typeof(Button)))
            {
                button.Enabled = parameter;
            }
            foreach (ListBox listbox in MainForm.GetAll(tpList, typeof(ListBox)))
            {
                listbox.Enabled = parameter;
            }
        }
        void EnableOnCardPage(bool parameter)
        {
            foreach (Button button in MainForm.GetAll(tpCards, typeof(Button)))
            {
                if (button.Name != "buCardIndexCardsPrev")
                {
                    button.Enabled = parameter;
                }
                if (button.Name == "buCardIndexCardsSave")
                {
                    button.Visible = parameter;
                }
            }
            foreach (TextBox textBox in MainForm.GetAll(tpCards, typeof(TextBox)))
            {
                textBox.Enabled = parameter;
            }
        }
        private void lbCardIndexList_DoubleClick(object sender, EventArgs e)
        {
            EnableElement(false);
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
                laCardsNumberCard.Text = "Текст карточки №" + CardMarker + ":";
                laCardsLetter.Text = CardSymbol;
                laCardsNumberBox.Text = CardNumberBox;
                laCardsFirstSeparator.Text = CardFirstSeparator;
                laCardsLastSeparator.Text = CardLastSeparator;
                tbTextCard.Text = CardText;
                tbCardNotes.Text = CardNotes;
                EnableOnCardPage(false);
                tcCards.SelectedTab = tpCards;
                Program.f1.PictAndLableWait(false);
                EnableElement(true);
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
                switch (NameClickButton)
                {
                    case "buCardIndexMenuSeparator":
                        {
                            for (int i = 0; i < SecondListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Marker);
                            }
                            break;
                        }
                    case "buCardIndexMenuBox":
                        {
                            for (int i = 0; i < SecondListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Marker);
                            }
                            break;
                        }
                    case "buCardIndexMenuLetter":
                        {
                            for (int i = 0; i < SecondListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Marker);
                            }
                            break;
                        }
                }
                EnableElement(true);
                lbCardIndexList.Update();
                CardIndexMenuMarker = true;
                lbCardIndexList.Visible = true;
                Program.f1.PictAndLableWait(false);
            }            
        }

        void ShowCards(object Number)
        {            
            string query = "SELECT * FROM cardindex WHERE Marker = '" + Number + "'";
            JSON.Send(JSONFlags.Select, query);
            CardItems = JSON.Decode();
            
            //Подумать над сепаратором между карточками

            CardMarker = CardItems[0].Marker;
            CardNotes = CardItems[0].Notes;
            CardNumberBox = CardItems[0].NumberBox;
            CardSymbol = CardItems[0].Symbol;
            CardImage = DecodeImageFromDB(CardItems[0].Img);
            CardText = CardItems[0].ImgText;
            query = "SELECT Symbol FROM letter WHERE ID = " + CardSymbol;
            JSON.Send(JSONFlags.Select, query);
            CardSymbol = "Буква: " + JSON.Decode()[0].Symbol;
            query = "SELECT CardSeparator FROM cardseparator WHERE BoxNumber = " + CardNumberBox;
            JSON.Send(JSONFlags.Select, query);
            CardFirstSeparator = "Первый разделитель: " + JSON.Decode()[0].CardSeparator;
            CardLastSeparator = "Последний разделитель: " + JSON.Decode().Last().CardSeparator;
            query = "SELECT NumberBox FROM box WHERE ID = " + CardNumberBox;
            JSON.Send(JSONFlags.Select, query);
            CardNumberBox = "Ящик: " + JSON.Decode()[0].NumberBox;            
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
            EnableOnCardPage(true);
        }

        private void buTest_Click(object sender, EventArgs e)
        {
            string query = "UPDATE `cardindex` SET `Notes` = 'Test' WHERE `Marker` = '5770005'";
            JSON.Send(JSONFlags.Update, query);
        }

        private void buCardIndexListDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {

            }
            if (result == DialogResult.Yes)
            {
                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(')', ' ');
                string query = "UPDATE `cardindex` SET `Marker` = is NULL,`CardSeparator` = is NULL,`NumberBox` = is NULL,`Symbol` = is NULL,`img` = is NULL, `imgText` = is NULL, `Notes` = is NULL, WHERE `Marker` = '" + NumberCardForDelete[2] + "'";                
                JSON.Send(JSONFlags.Update, query);
            }
        }
    }
}
