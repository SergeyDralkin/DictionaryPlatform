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
        string CardSeparator;
        /// <summary>
        /// Слово карточки
        /// </summary>
        string CardWord;
        /// <summary>
        /// Связанные со словом словосочения
        /// </summary>
        string CardRelatedCombinations;
        /// <summary>
        /// Значние слова
        /// </summary>
        string CardValue;
        /// <summary>
        /// Шифр Источника
        /// </summary>
        string CardSourceCode;
        /// <summary>
        /// Уточнение к источнику
        /// </summary>
        string CardSourceClarification;
        /// <summary>
        /// Пагинация карточки
        /// </summary>
        string CardPagination;
        /// <summary>
        /// Дата источника
        /// </summary>
        string CardSourceDate;
        /// <summary>
        /// Уточненная дата
        /// </summary>
        string CardSourceDateClarification;


        public CardIndexModule()
        {
            InitializeComponent();
        }

        void VisibleDown3button(bool parameter)
        {
            buCardIndexListChange.Visible = parameter;
            buCardIndexListAdd.Visible = parameter;
            buCardIndexListDelete.Visible = parameter;
        }
        void ActiveDown3button(bool parameter)
        {
            if (parameter == true)
            {
                foreach (Button button in MainForm.GetAll(this, typeof(Button)))
                {
                    if (button.Tag == "Insert" && MainForm.CanDoItList[0].CanInsert == 1.ToString() || button.Tag == "Update" && MainForm.CanDoItList[0].CanUpdate == 1.ToString() || button.Tag == "Delete" && MainForm.CanDoItList[0].CanDelete == 1.ToString() || button.Tag == "Select" && MainForm.CanDoItList[0].CanSelect == 1.ToString())
                    {
                        button.Enabled = parameter;
                    }                    
                }
            }
            else
            {
                buCardIndexListAdd.Enabled = parameter;
                buCardIndexListChange.Enabled = parameter;
                buCardIndexListDelete.Enabled = parameter;
            }
                          
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
                        VisibleDown3button(true);
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                        }
                        VisibleDown3button(false);
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                        }
                        VisibleDown3button(false);
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                        }
                        VisibleDown3button(false);
                        ActiveDown3button(false);
                        break;
                    }
            }            
            
            
            lbCardIndexList.Update();
            tcCards.SelectedTab = tpList;
            EnableElement(true);
            switch (NameClickButton)
            {
                case "buCardIndexMenuMarker":
                    {
                        VisibleDown3button(true);
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {
                        VisibleDown3button(false);
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        VisibleDown3button(false);
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        VisibleDown3button(false);
                        ActiveDown3button(false);
                        break;
                    }
            }
            Program.f1.PictAndLableWait(false);
        }
        void CreateSecondListItems(object NameButton)
        {
            SecondListItems.Clear();
            switch (NameClickButton)
            {
                case "buCardIndexMenuSeparator":
                    {                       
                        string query = "SELECT Marker FROM cardindex WHERE CardSeparator = " + ListBoxSelectedIndex;
                        JSON.Send(query, JSONFlags.Select);
                        SecondListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuBox":
                    {                       
                        string query = "SELECT Marker FROM cardindex WHERE NumberBox = " + ListBoxSelectedIndex;
                        JSON.Send(query, JSONFlags.Select);
                        SecondListItems = JSON.Decode();                       
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {                        
                        string query = "SELECT Marker FROM cardindex WHERE Symbol = " + ListBoxSelectedIndex;
                        JSON.Send(query, JSONFlags.Select);
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
                        JSON.Send(query, JSONFlags.Select);
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
                        JSON.Send(query, JSONFlags.Select);
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
                        JSON.Send(query, JSONFlags.Select);
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
                        JSON.Send(query, JSONFlags.Select);
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
                VisibleDown3button(false);
                ActiveDown3button(false);
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
                textBox.ReadOnly = !parameter;
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
                laCardsNumberCard.Text = "Текст карточки №" + CardMarker + ":";
                laCardsFirstSeparator.Text = CardSeparator;
                laCardsNumberBox.Text = CardNumberBox;
                laCardsLetter.Text = CardSymbol;
                pbPictCard.BackgroundImage = CardImage;   
                laCardsWord.Text = CardWord;
                tbCardText.Text = CardText;
                tbCardRelatedCombinations.Text = CardRelatedCombinations;
                tbCardValue.Text = CardValue;
                tbCardSourceCode.Text = CardSourceCode;
                tbCardSourceClarification.Text = CardSourceClarification;
                tbCardPagination.Text = CardPagination;
                tbCardSourceDate.Text = CardSourceDate;
                tbCardSourceDateClarification.Text = CardSourceDateClarification;
                tbCardNotes.Text = CardNotes;

                EnableOnCardPage(false);
                tcCards.SelectedTab = tpCards;
                Program.f1.PictAndLableWait(false);
                EnableElement(true);
            }
            else
            {
                EnableElement(false);
                Program.f1.PictAndLableWait(true);
                Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                myThread.Start(NameClickButton); // Запускаем поток
                while (myThread.IsAlive)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
                ClearMainList();
                EnableElement(true);
                switch (NameClickButton)
                {
                    case "buCardIndexMenuSeparator":
                        {
                            for (int i = 0; i < SecondListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Marker);
                            }
                            VisibleDown3button(true);
                            ActiveDown3button(false);
                            break;
                        }
                    case "buCardIndexMenuBox":
                        {
                            for (int i = 0; i < SecondListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Marker);
                            }
                            VisibleDown3button(true);
                            ActiveDown3button(false);
                            break;
                        }
                    case "buCardIndexMenuLetter":
                        {
                            for (int i = 0; i < SecondListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Marker);
                            }
                            VisibleDown3button(true);
                            ActiveDown3button(false);
                            break;
                        }
                }
                
                lbCardIndexList.Update();
                CardIndexMenuMarker = true;
                lbCardIndexList.Visible = true;
                Program.f1.PictAndLableWait(false);
            }            
        }

        void ShowCards(object Number)
        {            
            string query = "SELECT * FROM cardindex WHERE Marker = '" + Number + "'";
            JSON.Send(query, JSONFlags.Select);
            CardItems = JSON.Decode();
            
            //Подумать над сепаратором между карточками (отображаются на форме внизу при открытой карточке)

            CardMarker = CardItems[0].Marker;
            query = "SELECT CardSeparator FROM cardseparator WHERE ID = " + CardItems[0].CardSeparator;
            JSON.Send(query, JSONFlags.Select);
            CardSeparator = "Разделитель: " + JSON.Decode()[0].CardSeparator;
            query = "SELECT NumberBox FROM box WHERE ID = " + CardItems[0].NumberBox;
            JSON.Send(query, JSONFlags.Select);
            CardNumberBox = "Ящик: " + JSON.Decode()[0].NumberBox;
            query = "SELECT Symbol FROM letter WHERE ID = " + CardItems[0].Symbol;
            JSON.Send(query, JSONFlags.Select);
            CardSymbol = "Буква: " + JSON.Decode()[0].Symbol;
            CardImage = DecodeImageFromDB(CardItems[0].Img);
            CardWord = "Слово: " + CardItems[0].Word;
            CardText = CardItems[0].ImgText;
            CardRelatedCombinations = CardItems[0].RelatedCombinations;
            CardValue = CardItems[0].Value;
            CardSourceCode = CardItems[0].SourceCode;
            CardSourceClarification = CardItems[0].SourceClarification;
            CardPagination = CardItems[0].Pagination;
            CardSourceDate = CardItems[0].SourceDate;
            CardSourceDateClarification = CardItems[0].SourceDateClarification;
            CardNotes = CardItems[0].Notes;                       
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
            JSON.Send(query, JSONFlags.Update);
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
                JSON.Send(query, JSONFlags.Update);
            }
        }

        private void lbCardIndexList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveDown3button(true);
        }

        private void buCardIndexListChange_Click(object sender, EventArgs e)
        {
            EnableElement(false);
            ListBoxSelectedIndex = lbCardIndexList.SelectedIndex + 1;
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
            laCardsFirstSeparator.Text = CardSeparator;
            laCardsWord.Text = CardWord;
            tbCardText.Text = CardText;
            tbCardNotes.Text = CardNotes;
            EnableOnCardPage(true);
            tcCards.SelectedTab = tpCards;
            Program.f1.PictAndLableWait(false);
            EnableElement(true);            
        }

        private void buCardIndexCardsSave_Click(object sender, EventArgs e)
        {

        }
    }
}
