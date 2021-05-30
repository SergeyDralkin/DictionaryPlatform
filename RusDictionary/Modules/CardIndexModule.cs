using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RusDictionary.Modules
{
    public partial class CardIndexModule : UserControl
    {
        int ListBoxSelectedIndex;
        int ListBoxSelectedIndexForUpdate;
        bool ListBoxPrev = true;
        string NameClickButtonInMenu;
        string NameClickButtonInListPage;
        string TagButtonChange;
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
        /// Список элементов всех карточек
        /// </summary>
        List<JSONArray> AllCardItems = new List<JSONArray>();
        /// <summary>
        /// Список элементов всех ящиков
        /// </summary>
        List<JSONArray> AllBoxItems = new List<JSONArray>();
        /// <summary>
        /// Список элементов всех букв
        /// </summary>
        List<JSONArray> AllLetterItems = new List<JSONArray>();
        /// <summary>
        /// Список элементов всех карточек-разделителей
        /// </summary>
        List<JSONArray> AllCardSeparatorItems = new List<JSONArray>();
        /// <summary>
        /// Список ящиков
        /// </summary>
        List<JSONArray> BoxItems = new List<JSONArray>();
        /// <summary>
        /// Список букв
        /// </summary>
        List<JSONArray> LetterItems = new List<JSONArray>();
        /// <summary>
        /// Список элементов всех указателей
        /// </summary>
        List<JSONArray> AllUkazItems = new List<JSONArray>();
        /// <summary>
        /// Список элементов всех указателей
        /// </summary>
        List<JSONArray> UkazItems = new List<JSONArray>();
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
        /// Отслеживание нажатия на кнопку "Слово"
        /// </summary>
        public static bool CardIndexMenuWord = false;
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
        /// ID ящика карточки
        /// </summary>
        string CardNumberBoxID;
        /// <summary>
        /// Разделитель ящика
        /// </summary>
        string CardSeparator;
        /// <summary>
        /// ID разделителя ящика
        /// </summary>
        string CardSeparatorID;
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
        /// <summary>
        /// Использовался ли список второй раз
        /// </summary>
        bool UseSecondList = false;

        public CardIndexModule()
        {
            InitializeComponent();
            SetupSettingForElements();
            tlpWord.Width = panelWord.Width - 6;
            tlpWord.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            tlpCard.Width = panelCard.Width - 6;
            tlpCard.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        }
        /// <summary>
        /// Установка настроек элементов данного модуля
        /// </summary>
        void SetupSettingForElements()
        {
            foreach (ComboBox comboBox in MainForm.GetAll(this, typeof(ComboBox)))
            {
                comboBox.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);                
            }
            foreach (TextBox textbox in MainForm.GetAll(this, typeof(TextBox)))
            {
                textbox.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
                textbox.ScrollBars = ScrollBars.Vertical;
            }
            foreach (ListBox listbox in MainForm.GetAll(this, typeof(ListBox)))
            {
                listbox.Font = new Font("Izhitsa", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            }
            foreach (Label label in MainForm.GetAll(this, typeof(Label)))
            {
                if (label.Name == "laCardsFirstSeparator" || label.Name == "laCardsLastSeparator" || label.Name == "laCardsLetter" || label.Name == "laCardsNumberBox")
                {
                    label.Font = new Font("Izhitsa", 9.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
                else
                {
                    label.Font = new Font("Izhitsa", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
            foreach (Button button in MainForm.GetAll(this, typeof(Button)))
            {
                if (button.Name == "buCardIndexCardsPrev" || button.Name == "buSelectWordPrev")
                {
                    button.Font = new Font("Izhitsa", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
                }
                else
                {
                    button.Font = new Font("Izhitsa", 16F, FontStyle.Regular, GraphicsUnit.Point, 204);
                }
            }
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
            NameClickButtonInMenu = (sender as Button).Name.ToString();
            ClearMainList();
            Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
            myThread.IsBackground = true;
            myThread.Start(NameClickButtonInMenu); // Запускаем поток
            while (myThread.IsAlive)
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }            
            switch (NameClickButtonInMenu)
            {
                case "buCardIndexMenuMarker":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Marker);
                        }
                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                        }
                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuBox":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                        }
                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                        }
                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuWord":
                    {
                        for (int i = 0; i < FirstListItems.Count; i++)
                        {
                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Word);
                        }
                        
                        ActiveDown3button(false);
                        break;
                    }
            }
            lbCardIndexList.Update();
            tcCards.SelectedTab = tpList;
            EnableElement(true);
            switch (NameClickButtonInMenu)
            {
                case "buCardIndexMenuMarker":
                    {                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuSeparator":
                    {                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuBox":
                    {                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {                        
                        ActiveDown3button(false);
                        break;
                    }
                case "buCardIndexMenuWord":
                    {                        
                        ActiveDown3button(false);
                        break;
                    }
            }
            Program.f1.PictAndLableWait(false);
        }
        void CreateSecondListItems(object NameButton)
        {
            if (SecondListItems != null)
            {
                SecondListItems.Clear();
            }
            
            switch (NameClickButtonInMenu)
            {
                case "buCardIndexMenuSeparator":
                    {                       
                        string query = "SELECT * FROM flotation WHERE CardSeparator = " + ListBoxSelectedIndex;
                        JSON.Send(query, JSONFlags.Select);
                        SecondListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuBox":
                    {                       
                        string query = "SELECT * FROM flotation WHERE NumberBox = " + ListBoxSelectedIndex;
                        JSON.Send(query, JSONFlags.Select);
                        SecondListItems = JSON.Decode();                       
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {                        
                        string query = "SELECT * FROM flotation WHERE Symbol = " + ListBoxSelectedIndex;
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
                        CardIndexMenuWord = false;
                        string query = "SELECT ID, Marker FROM cardindex";
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
                        CardIndexMenuWord = false;
                        string query = "SELECT * FROM cardseparator";
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
                        CardIndexMenuWord = false;
                        string query = "SELECT * FROM box";
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
                        CardIndexMenuWord = false;
                        string query = "SELECT * FROM letter";
                        JSON.Send(query, JSONFlags.Select);
                        FirstListItems = JSON.Decode();
                        break;
                    }
                case "buCardIndexMenuWord":
                    {
                        CardIndexMenuMarker = false;
                        CardIndexMenuSeparator = false;
                        CardIndexMenuBox = false;
                        CardIndexMenuLetter = false;
                        CardIndexMenuWord = true;
                        string query = "SELECT ID, Word, Value FROM flotation";
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
            if (NameClickButtonInMenu == "buCardIndexMenuMarker" || NameClickButtonInMenu == "buCardIndexMenuWord")
            {
                tcCards.SelectedTab = tpCardsMenu;
            }
            else
            {                
                ActiveDown3button(false);
                UseSecondList = false;
                if (ListBoxPrev != true)
                {
                    CardIndexMenuWord = false;
                    ClearMainList();
                    ListBoxPrev = true;
                    switch (NameClickButtonInMenu)
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
        void EnableOnCardPage(bool parameter, TabPage tab)
        {            
            foreach (Button button in MainForm.GetAll(tab, typeof(Button)))
            {
                if (button.Name == "buCardIndexCardsPrev")
                {
                    button.Enabled = !parameter;
                }
                else if (button.Name == "buSelectWordPrev")
                {
                    button.Enabled = !parameter;
                }
                else
                {
                    button.Enabled = parameter;
                }
            }
            foreach (TextBox textBox in MainForm.GetAll(tab, typeof(TextBox)))
            {
                textBox.ReadOnly = !parameter;
            }
        }
        private void lbCardIndexList_DoubleClick(object sender, EventArgs e)
        {
            if (lbCardIndexList.SelectedItem != null)
            {
                EnableElement(false);
                ListBoxSelectedIndex = lbCardIndexList.SelectedIndex + 1;
                ListBoxSelectedIndexForUpdate = lbCardIndexList.SelectedIndex + 1;
                ListBoxPrev = false;
                if (CardIndexMenuMarker == true)
                {
                    Program.f1.PictAndLableWait(true);
                    Thread myThread = new Thread(new ParameterizedThreadStart(ShowCards)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                    string[] mass = { ") " };
                    string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                    myThread.IsBackground = true;
                    myThread.Start(SplitItem.Last()); // Запускаем поток
                    while (myThread.IsAlive)
                    {
                        Thread.Sleep(1);
                        Application.DoEvents();
                    }
                    laCardsNumberCard.Text = "Текст карточки №" + CardMarker + ":";
                    laCardsNumberBox.Text = "Ящик: " + CardNumberBox;
                    pbPictCard.BackgroundImage = CardImage;
                    tbCardText.Text = CardText;
                    tbCardSourceCode.Text = CardSourceCode;
                    tbCardSourceClarification.Text = CardSourceClarification;
                    tbCardPagination.Text = CardPagination;
                    tbCardSourceDate.Text = CardSourceDate;
                    tbCardSourceDateClarification.Text = CardSourceDateClarification;
                    tbCardNotes.Text = CardNotes;

                    EnableOnCardPage(false, tpCardsSelectCard);
                    tcCards.SelectedTab = tpCardsSelectCard;
                    Program.f1.PictAndLableWait(false);
                    EnableElement(true);
                }
                else if (CardIndexMenuWord == true)
                {
                    if (CardIndexMenuSeparator == false && CardIndexMenuBox == false && CardIndexMenuLetter == false)
                    {
                        Program.f1.PictAndLableWait(true);
                        string[] SplitItem = lbCardIndexList.SelectedItem.ToString().Split(new string[] { ") " }, StringSplitOptions.RemoveEmptyEntries);

                        int ID = lbCardIndexList.SelectedIndex;
                        Thread myThread = new Thread(() => ShowWordWithCard(FirstListItems[ID].ID)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        myThread.IsBackground = true;
                        myThread.Start(); // Запускаем поток
                        while (myThread.IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        laCardsSelectWordNumberCard.Text = "Текст карточки №" + CardMarker + ":";
                        laCardsSelectWordSeparator.Text = "Разделитель: " + CardSeparator;
                        laCardsSelectWordNumberBox.Text = "Ящик: " + CardNumberBox;
                        laCardsSelectWordLetter.Text = "Буква: " + CardSymbol;                        
                        laCardsSelectWordWord.Text = "Слово: " + CardWord;
                        pbCardsSelectWordImage.BackgroundImage = CardImage;
                        tbCardsSelectWordText.Text = CardText;
                        tbCardsSelectWordValue.Text = CardValue;
                        tbCardsSelectWordSourceCode.Text = CardSourceCode;
                        tbCardsSelectWordSourceCodeClarification.Text = CardSourceClarification;
                        tbCardsSelectWordPagination.Text = CardPagination;
                        tbCardsSelectWordSourceDate.Text = CardSourceDate;
                        tbCardsSelectWordSourceDateClarification.Text = CardSourceDateClarification;
                        tbCardsSelectWordRelatedCombinations.Text = CardRelatedCombinations;
                        tbCardsSelectWordNotes.Text = CardNotes;
                        tcCards.SelectedTab = tpCardsSelectWord;

                        EnableOnCardPage(false, tpCardsSelectWord);
                        Program.f1.PictAndLableWait(false);
                        EnableElement(true);
                    }
                    else
                    {
                        Program.f1.PictAndLableWait(true);
                        string[] SplitItem = lbCardIndexList.SelectedItem.ToString().Split(new string[] { ") " }, StringSplitOptions.RemoveEmptyEntries);

                        int ID = lbCardIndexList.SelectedIndex;
                        Thread myThread = new Thread(() => ShowWordWithCard(SecondListItems[ID].ID)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        myThread.IsBackground = true;
                        myThread.Start(); // Запускаем поток
                        while (myThread.IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        laCardsSelectWordNumberCard.Text = "Текст карточки №" + CardMarker + ":";
                        laCardsSelectWordSeparator.Text = "Разделитель: " + CardSeparator;
                        laCardsSelectWordNumberBox.Text = "Ящик: " + CardNumberBox;
                        laCardsSelectWordLetter.Text = "Буква: " + CardSymbol;
                        laCardsSelectWordWord.Text = "Слово: " + CardWord;
                        pbCardsSelectWordImage.BackgroundImage = CardImage;                        
                        tbCardsSelectWordText.Text = CardText;
                        tbCardsSelectWordValue.Text = CardValue;
                        tbCardsSelectWordSourceCode.Text = CardSourceCode;
                        tbCardsSelectWordSourceCodeClarification.Text = CardSourceClarification;
                        tbCardsSelectWordPagination.Text = CardPagination;
                        tbCardsSelectWordSourceDate.Text = CardSourceDate;
                        tbCardsSelectWordSourceDateClarification.Text = CardSourceDateClarification;
                        tbCardsSelectWordRelatedCombinations.Text = CardRelatedCombinations;
                        tbCardsSelectWordNotes.Text = CardNotes;
                        tcCards.SelectedTab = tpCardsSelectWord;

                        EnableOnCardPage(false, tpCardsSelectWord);
                        Program.f1.PictAndLableWait(false);
                        EnableElement(true);
                    }
                    //CardIndexMenuWord = false;
                    
                }
                else
                {
                    EnableElement(false);
                    Program.f1.PictAndLableWait(true);
                    Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                    myThread.IsBackground = true;
                    myThread.Start(NameClickButtonInMenu); // Запускаем поток
                    while (myThread.IsAlive)
                    {
                        Thread.Sleep(1);
                        Application.DoEvents();
                    }
                    ClearMainList();
                    EnableElement(true);
                    int CountList;
                    if (SecondListItems == null)
                    {
                        CountList = 0;
                    }
                    else
                    {
                        CountList = SecondListItems.Count;
                    }
                    
                    switch (NameClickButtonInMenu)
                    {
                        case "buCardIndexMenuSeparator":
                            {
                                for (int i = 0; i < CountList; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                }
                                ActiveDown3button(false);
                                break;
                            }
                        case "buCardIndexMenuBox":
                            {
                                for (int i = 0; i < CountList; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                }
                                ActiveDown3button(false);
                                break;
                            }
                        case "buCardIndexMenuLetter":
                            {
                                for (int i = 0; i < CountList; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                }
                                ActiveDown3button(false);
                                break;
                            }
                    }
                    UseSecondList = true;
                    lbCardIndexList.Update();
                    CardIndexMenuWord = true;
                    lbCardIndexList.Visible = true;
                    Program.f1.PictAndLableWait(false);
                }
            }
        }
        /// <summary>
        /// Выбрать все ящики из БД
        /// </summary>
        void SelectAllBox()
        {
            string query = "SELECT * FROM box";
            JSON.Send(query, JSONFlags.Select);
            AllBoxItems = JSON.Decode();
        }
        /// <summary>
        /// Выбрать все карточки разделители
        /// </summary>
        void SelectAllCardSeparator()
        {
            string query = "SELECT * FROM cardseparator";
            JSON.Send(query, JSONFlags.Select);
            AllCardSeparatorItems = JSON.Decode();
        }        
        /// <summary>
        /// Выбрать все буквы
        /// </summary>        
        void SelectAllLetter()
        {
            string query = "SELECT * FROM letter";
            JSON.Send(query, JSONFlags.Select);
            AllLetterItems = JSON.Decode();
        }
        void SelectAllCardNumber()
        {
            string query = "SELECT Marker FROM cardindex";
            JSON.Send(query, JSONFlags.Select);
            AllCardItems = JSON.Decode();
        }
        /// <summary>
        /// Выбирает нужный ящик по ID ящика
        /// </summary>
        /// <param name="BoxID"></param>
        void SelectBoxID(string BoxID)
        {
            string query = "SELECT * FROM box WHERE ID = " + BoxID;
            JSON.Send(query, JSONFlags.Select);
            BoxItems = JSON.Decode();
        }
        /// <summary>
        /// Выбрать букву по её ID
        /// </summary>
        /// <param name="LetterID"></param>
        void SelectLetter(string LetterID)
        {
            string query = "SELECT * FROM letter WHERE ID = " + LetterID;
            JSON.Send(query, JSONFlags.Select);
            LetterItems = JSON.Decode();
        }
        /// <summary>
        /// Выбрать указатель по его ID
        /// </summary>
        /// <param name="LetterID"></param>
        void SelectUkaz(string UkazID)
        {
            string query = "SELECT * FROM ukaz WHERE ID = " + UkazID;
            JSON.Send(query, JSONFlags.Select);
            UkazItems = JSON.Decode();
        }
        /// <summary>
        /// Выбрать все указатели
        /// </summary>
        /// <param name="LetterID"></param>
        void SelectAllUkaz()
        {
            string query = "SELECT * FROM ukaz";
            JSON.Send(query, JSONFlags.Select);
            AllUkazItems = JSON.Decode();
        }
        void ShowCards(object Number)
        {
            string query = "SELECT * FROM cardindex WHERE Marker = '" + Number + "'";
            JSON.Send(query, JSONFlags.Select);
            CardItems = JSON.Decode();
            CardMarker = CardItems[0].Marker;
            CardNumberBoxID = CardItems[0].NumberBox;
            query = "SELECT NumberBox FROM box WHERE ID = " + CardNumberBoxID;
            JSON.Send(query, JSONFlags.Select);
            CardItems[0].NumberBox = JSON.Decode()[0].NumberBox;
            CardNumberBox = CardItems[0].NumberBox;
            CardImage = DecodeImageFromDB(CardItems[0].Img);
            CardWord = CardItems[0].Word;
            CardText = CardItems[0].ImgText;
            CardRelatedCombinations = CardItems[0].RelatedCombinations;
            CardValue = CardItems[0].Value;
            CardSourceCode = CardItems[0].SourceCode;
            if (CardSourceCode != null)
            {
                query = "SELECT Name FROM ukaz WHERE ID = " + CardSourceCode;
                JSON.Send(query, JSONFlags.Select);
                CardSourceCode = JSON.Decode()[0].Name;
            }    
            CardSourceClarification = CardItems[0].SourceClarification;
            if (CardSourceClarification != null)
            {
                query = "SELECT Name FROM ukaz WHERE ID = " + CardSourceClarification;
                JSON.Send(query, JSONFlags.Select);
                CardSourceClarification = JSON.Decode()[0].Name;
            }
            CardPagination = CardItems[0].Pagination;
            CardSourceDate = CardItems[0].SourceDate;
            CardSourceDateClarification = CardItems[0].SourceDateClarification;
            CardNotes = CardItems[0].Notes;
        }
        
        void ShowWordWithCard(object ID)
        {              
            string query = "SELECT * FROM flotation WHERE ID = '" + ID + "'";
            JSON.Send(query, JSONFlags.Select);
            CardItems = JSON.Decode();
            List<JSONArray> AboutCard = new List<JSONArray>();
            query = "SELECT * FROM cardindex WHERE ID = " + CardItems[0].Card;
            JSON.Send(query, JSONFlags.Select);
            AboutCard = JSON.Decode();
            CardMarker = AboutCard[0].Marker;
            CardImage = DecodeImageFromDB(AboutCard[0].Img);
            CardText = AboutCard[0].ImgText;
            CardSourceCode = AboutCard[0].SourceCode;
            CardSourceClarification = AboutCard[0].SourceClarification;
            CardPagination = AboutCard[0].Pagination;
            CardSourceDate = AboutCard[0].SourceDate;
            CardSourceDateClarification = AboutCard[0].SourceDateClarification;
            CardNotes = AboutCard[0].Notes;
            //-------------\\
            query = "SELECT NumberBox FROM box WHERE ID = " + CardItems[0].NumberBox;
            JSON.Send(query, JSONFlags.Select);
            CardNumberBox = JSON.Decode()[0].NumberBox;
            //-------------\\
            query = "SELECT Symbol FROM letter WHERE ID = " + CardItems[0].Symbol;
            JSON.Send(query, JSONFlags.Select);
            CardSymbol = JSON.Decode()[0].Symbol;
            //-------------\\
            CardSeparatorID = CardItems[0].CardSeparator;
            query = "SELECT CardSeparator FROM cardseparator WHERE ID = " + CardSeparatorID;
            JSON.Send(query, JSONFlags.Select);
            CardSeparator = JSON.Decode()[0].CardSeparator;
            //-------------\\
            CardWord = CardItems[0].Word;
            CardValue = CardItems[0].Value;
            CardRelatedCombinations = CardItems[0].RelatedCombinations;
        }
        /// <summary>
        /// Кодирование изображения в base64
        /// </summary>
        /// <param name="FilePath">Путь к изображению</param>
        /// <returns>Закодированное изображение</returns>
        string CodeInBase64(string FilePath)
        {
            MemoryStream stream = new MemoryStream();
            Properties.Resources.noimage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] bytes = stream.ToArray();
            if (Convert.ToBase64String(File.ReadAllBytes(FilePath)).Equals(Convert.ToBase64String(bytes)))
            {
                return "";
            }
            else
            {
                return Convert.ToBase64String(File.ReadAllBytes(FilePath));
            }
            
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
            EnableOnCardPage(true, tpCardsSelectCard);
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
                NameClickButtonInListPage = (sender as Button).Name.ToString();
                EnableElement(false);
                ListBoxSelectedIndex = lbCardIndexList.SelectedIndex + 1;

                Program.f1.PictAndLableWait(true);
                TagButtonChange = (sender as Button).Tag.ToString();
                switch (NameClickButtonInMenu)
                {
                    case "buCardIndexMenuMarker":
                        {
                            string[] mass = { ") " };
                            string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string sql = "DELETE FROM `cardindex` WHERE Marker = '" + NumberCardForDelete.Last() + "'";
                            JSON.Send(sql, JSONFlags.Delete);
                            ClearMainList();
                            Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            myThread.IsBackground = true;
                            myThread.Start("buCardIndexMenuMarker"); // Запускаем поток
                            while (myThread.IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            for (int i = 0; i < FirstListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Marker);
                            }
                            ActiveDown3button(false);
                            lbCardIndexList.Update();

                            MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    case "buCardIndexMenuSeparator":
                        {
                            if (UseSecondList == true)
                            {
                                string[] mass = { ") " };
                                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                string sql = "DELETE FROM `flotation` WHERE Word = '" + NumberCardForDelete.Last() + "'";
                                JSON.Send(sql, JSONFlags.Delete);

                                ClearMainList();
                                Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                ListBoxSelectedIndex = ListBoxSelectedIndexForUpdate;
                                myThread.IsBackground = true;
                                myThread.Start("buCardIndexMenuSeparator"); // Запускаем поток
                                while (myThread.IsAlive)
                                {
                                    Thread.Sleep(1);
                                    Application.DoEvents();
                                }
                                for (int i = 0; i < SecondListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                }
                                ActiveDown3button(false);
                                lbCardIndexList.Update();

                                MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                string[] mass = { ") " };
                                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                string sql = "DELETE FROM `cardseparator` WHERE CardSeparator = '" + NumberCardForDelete.Last() + "'";
                                JSON.Send(sql, JSONFlags.Delete);
                                ClearMainList();
                                Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                myThread.IsBackground = true;
                                myThread.Start("buCardIndexMenuSeparator"); // Запускаем поток
                                while (myThread.IsAlive)
                                {
                                    Thread.Sleep(1);
                                    Application.DoEvents();
                                }
                                for (int i = 0; i < FirstListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                                }
                                ActiveDown3button(false);
                                lbCardIndexList.Update();
                                MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;
                        }
                    case "buCardIndexMenuBox":
                        {
                            if (UseSecondList == true)
                            {
                                string[] mass = { ") " };
                                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                string sql = "DELETE FROM `flotation` WHERE Word = '" + NumberCardForDelete.Last() + "'";
                                JSON.Send(sql, JSONFlags.Delete);

                                ClearMainList();
                                Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                ListBoxSelectedIndex = ListBoxSelectedIndexForUpdate;
                                myThread.IsBackground = true;
                                myThread.Start("buCardIndexMenuBox");                                
                                while (myThread.IsAlive)
                                {
                                    Thread.Sleep(1);
                                    Application.DoEvents();
                                }
                                for (int i = 0; i < SecondListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                }
                                ActiveDown3button(false);
                                lbCardIndexList.Update();

                                MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                string[] mass = { ") " };
                                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                string sql = "DELETE FROM `box` WHERE NumberBox = '" + NumberCardForDelete.Last() + "'";
                                JSON.Send(sql, JSONFlags.Delete);
                                ClearMainList();
                                Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                myThread.IsBackground = true;
                                myThread.Start("buCardIndexMenuBox"); // Запускаем поток
                                while (myThread.IsAlive)
                                {
                                    Thread.Sleep(1);
                                    Application.DoEvents();
                                }
                                for (int i = 0; i < FirstListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                                }
                                ActiveDown3button(false);
                                lbCardIndexList.Update();
                                MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;
                        }
                    case "buCardIndexMenuLetter":
                        {
                            if (UseSecondList == true)
                            {
                                string[] mass = { ") " };
                                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                string sql = "DELETE FROM `flotation` WHERE Word = '" + NumberCardForDelete.Last() + "'";
                                JSON.Send(sql, JSONFlags.Delete);

                                ClearMainList();
                                Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                ListBoxSelectedIndex = ListBoxSelectedIndexForUpdate;
                                myThread.IsBackground = true;
                                myThread.Start("buCardIndexMenuLetter");                                
                                while (myThread.IsAlive)
                                {
                                    Thread.Sleep(1);
                                    Application.DoEvents();
                                }
                                for (int i = 0; i < SecondListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                }
                                ActiveDown3button(false);
                                lbCardIndexList.Update();

                                MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                string[] mass = { ") " };
                                string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                string sql = "DELETE FROM `letter` WHERE Symbol = '" + NumberCardForDelete.Last() + "'";
                                JSON.Send(sql, JSONFlags.Delete);
                                ClearMainList();
                                Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                myThread.IsBackground = true;
                                myThread.Start("buCardIndexMenuLetter"); // Запускаем поток
                                while (myThread.IsAlive)
                                {
                                    Thread.Sleep(1);
                                    Application.DoEvents();
                                }
                                for (int i = 0; i < FirstListItems.Count; i++)
                                {
                                    lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                                }
                                ActiveDown3button(false);
                                lbCardIndexList.Update();
                                MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            break;
                        }
                    case "buCardIndexMenuWord":
                        {
                            string[] mass = { ") " };
                            string[] NumberCardForDelete = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string sql = "DELETE FROM `flotation` WHERE Word = '" + NumberCardForDelete.Last() + "'";
                            JSON.Send(sql, JSONFlags.Delete);
                            ClearMainList();
                            Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            myThread.IsBackground = true;
                            myThread.Start("buCardIndexMenuWord"); // Запускаем поток
                            while (myThread.IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            for (int i = 0; i < FirstListItems.Count; i++)
                            {
                                lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Word);
                            }
                            ActiveDown3button(false);
                            lbCardIndexList.Update();
                            MessageBox.Show("Операция выполнена успешно!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                }
                Program.f1.PictAndLableWait(false);
                EnableElement(true);
            }
        }
        private void lbCardIndexList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbCardIndexList.SelectedIndex != -1)
            {
                ActiveDown3button(true);
            }
        }
        private void buCardIndexListChange_Click(object sender, EventArgs e)
        {
            NameClickButtonInListPage = (sender as Button).Name.ToString();
            EnableElement(false);
            ListBoxSelectedIndex = lbCardIndexList.SelectedIndex + 1;

            Program.f1.PictAndLableWait(true);
            TagButtonChange = (sender as Button).Tag.ToString();            
            switch (NameClickButtonInMenu)
            {
                //Работает
                case "buCardIndexMenuMarker":
                    {
                        cbCardsInsertAndUpdateBox.Items.Clear();
                        cbCardsInsertAndUpdateSourceCode.Items.Clear();
                        cbCardsInsertAndUpdateSourceCodeClarification.Items.Clear();
                        List<Thread> MyThreads = new List<Thread>();
                        string[] mass = { ") " };
                        string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);                         
                        MyThreads.Add(new Thread(() => ShowCards(SplitItem.Last()))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllUkaz())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads[0].IsBackground = true;
                        MyThreads[0].Start();
                        while (MyThreads[0].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[1].IsBackground = true;
                        MyThreads[1].Start();
                        while (MyThreads[1].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[2].IsBackground = true;
                        MyThreads[2].Start();
                        while (MyThreads[2].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        tbCardsInsertAndUpdateMarker.Text = CardMarker;

                        for (int i = 0; i < AllBoxItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateBox.Items.Add(AllBoxItems[i].NumberBox);
                            if (CardNumberBox.Equals(AllBoxItems[i].NumberBox))
                            {
                                cbCardsInsertAndUpdateBox.SelectedIndex = i;
                            }
                        }
                        cbCardsInsertAndUpdateSourceCode.Items.Add("");
                        cbCardsInsertAndUpdateSourceCodeClarification.Items.Add("");
                        for (int i = 0; i < AllUkazItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateSourceCode.Items.Add(AllUkazItems[i].Name);
                            cbCardsInsertAndUpdateSourceCodeClarification.Items.Add(AllUkazItems[i].Name);
                            if (CardSourceCode != null)
                            {
                                if (CardSourceCode.Equals(AllUkazItems[i].Name))
                                {
                                    cbCardsInsertAndUpdateSourceCode.SelectedIndex = i + 1;
                                }
                            }
                            else
                            {
                                cbCardsInsertAndUpdateSourceCode.SelectedIndex = 0;
                            }
                            if (CardSourceClarification != null)
                            {
                                if (CardSourceClarification.Equals(AllUkazItems[i].Name))
                                {
                                    cbCardsInsertAndUpdateSourceCodeClarification.SelectedIndex = i + 1;
                                }
                            }
                            else
                            {
                                cbCardsInsertAndUpdateSourceCodeClarification.SelectedIndex = 0;
                            }
                        }
                        pbCardsInsertAndUpdateImage.Image = CardImage;
                        tbCardsInsertAndUpdateTextCard.Text = CardText;
                        tbCardsInsertAndUpdatePagination.Text = CardPagination;
                        tbCardsInsertAndUpdateSourceDate.Text = CardSourceDate;
                        tbCardsInsertAndUpdateSourceDateClarification.Text = CardSourceDateClarification;
                        tbCardsInsertAndUpdateNotes.Text = CardNotes;
                        EnableButtonDeleteImage();
                        tcCards.SelectedTab = tpCardsInsertAndUpdateCard;
                        break;
                    }
                    //Криво
                case "buCardIndexMenuSeparator":
                    {
                        if (UseSecondList == true)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Clear();
                            cbCardsInsertAndUpdateWordBox.Items.Clear();
                            cbCardsInsertAndUpdateWordLetter.Items.Clear();
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();
                            string[] mass = { ") " };
                            string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string idWord = (SecondListItems[lbCardIndexList.SelectedIndex].ID).ToString();

                            MyThreads.Add(new Thread(() => ShowWordWithCard(idWord))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)                                                                             
                            MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start(); // Запускаем поток
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[2].IsBackground = true;
                            MyThreads[2].Start(); // Запускаем поток
                            while (MyThreads[2].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[3].IsBackground = true;
                            MyThreads[3].Start(); // Запускаем поток
                            while (MyThreads[3].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[4].IsBackground = true;
                            MyThreads[4].Start(); // Запускаем поток
                            while (MyThreads[4].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }

                            for (int i = 0; i < AllCardItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);
                                if (AllCardItems[i].Marker == CardMarker)
                                {
                                    cbCardsInsertAndUpdateWordCard.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);
                                if (AllBoxItems[i].NumberBox == CardNumberBox)
                                {
                                    cbCardsInsertAndUpdateWordBox.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllLetterItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);
                                if (AllLetterItems[i].Symbol == CardSymbol)
                                {
                                    cbCardsInsertAndUpdateWordLetter.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);
                                if (AllCardSeparatorItems[i].CardSeparator == CardSeparator)
                                {
                                    cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = i;
                                }
                            }
                            pbCardsInsertAndUpdateWordImage.BackgroundImage = CardImage;
                            tbCardsInsertAndUpdateWordWord.Text = CardWord;
                            tbCardsInsertAndUpdateWordRelatedCombinations.Text = CardRelatedCombinations;
                            tbCardsInsertAndUpdateWordValue.Text = CardValue;
                            tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        }
                        else
                        {
                            cbCardsInsertAndUpdateCardSeparatorBox.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();
                            string[] mass = { ") " };
                            string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)

                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive )
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start();
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            int idSeparator = Convert.ToInt32(AllCardSeparatorItems[lbCardIndexList.SelectedIndex].ID);
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateCardSeparatorBox.Items.Add(AllBoxItems[i].NumberBox);
                                if (AllCardSeparatorItems[lbCardIndexList.SelectedIndex].NumberBox == AllBoxItems[i].ID)
                                {
                                    cbCardsInsertAndUpdateCardSeparatorBox.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                if (AllCardSeparatorItems[i].ID.Equals(idSeparator.ToString()))
                                {
                                    tbCardsInsertAndUpdateCardSeparatorLetter.Text = AllCardSeparatorItems[i].CardSeparator;
                                    break;
                                }
                            }
                            tcCards.SelectedTab = tpCardsInsertAndUpdateCardSeparator;
                        }
                        break;
                    }
                    //Криво
                case "buCardIndexMenuBox":
                    {
                        if (UseSecondList == true)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Clear();
                            cbCardsInsertAndUpdateWordBox.Items.Clear();
                            cbCardsInsertAndUpdateWordLetter.Items.Clear();
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();
                            string[] mass = { ") " };
                            string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string idWord = (SecondListItems[lbCardIndexList.SelectedIndex].ID).ToString();

                            MyThreads.Add(new Thread(() => ShowWordWithCard(idWord))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)                                                                             
                            MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start(); // Запускаем поток
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[2].IsBackground = true;
                            MyThreads[2].Start(); // Запускаем поток
                            while (MyThreads[2].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[3].IsBackground = true;
                            MyThreads[3].Start(); // Запускаем поток
                            while (MyThreads[3].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[4].IsBackground = true;
                            MyThreads[4].Start(); // Запускаем поток
                            while (MyThreads[4].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }

                            for (int i = 0; i < AllCardItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);
                                if (AllCardItems[i].Marker == CardMarker)
                                {
                                    cbCardsInsertAndUpdateWordCard.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);
                                if (AllBoxItems[i].NumberBox == CardNumberBox)
                                {
                                    cbCardsInsertAndUpdateWordBox.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllLetterItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);
                                if (AllLetterItems[i].Symbol == CardSymbol)
                                {
                                    cbCardsInsertAndUpdateWordLetter.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);
                                if (AllCardSeparatorItems[i].CardSeparator == CardSeparator)
                                {
                                    cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = i;
                                }
                            }
                            pbCardsInsertAndUpdateWordImage.BackgroundImage = CardImage;
                            tbCardsInsertAndUpdateWordWord.Text = CardWord;
                            tbCardsInsertAndUpdateWordRelatedCombinations.Text = CardRelatedCombinations;
                            tbCardsInsertAndUpdateWordValue.Text = CardValue;
                            tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        }
                        else
                        {
                            List<Thread> MyThreads = new List<Thread>();
                            string[] mass = { ") " };
                            string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string idbox = (FirstListItems[lbCardIndexList.SelectedIndex].ID).ToString();
                            MyThreads.Add(new Thread(() => SelectBoxID(idbox))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            tbCardsInsertAndUpdateBoxNumberBox.Text = BoxItems[0].NumberBox;
                            tcCards.SelectedTab = tpCardsInsertAndUpdateBox;
                        }
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        if (UseSecondList == true)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Clear();
                            cbCardsInsertAndUpdateWordBox.Items.Clear();
                            cbCardsInsertAndUpdateWordLetter.Items.Clear();
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();
                            string[] mass = { ") " };
                            string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string idWord = (SecondListItems[lbCardIndexList.SelectedIndex].ID).ToString();

                            MyThreads.Add(new Thread(() => ShowWordWithCard(idWord))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)                                                                             
                            MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start(); // Запускаем поток
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[2].IsBackground = true;
                            MyThreads[2].Start(); // Запускаем поток
                            while (MyThreads[2].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[3].IsBackground = true;
                            MyThreads[3].Start(); // Запускаем поток
                            while (MyThreads[3].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[4].IsBackground = true;
                            MyThreads[4].Start(); // Запускаем поток
                            while (MyThreads[4].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }

                            for (int i = 0; i < AllCardItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);
                                if (AllCardItems[i].Marker == CardMarker)
                                {
                                    cbCardsInsertAndUpdateWordCard.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);
                                if (AllBoxItems[i].NumberBox == CardNumberBox)
                                {
                                    cbCardsInsertAndUpdateWordBox.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllLetterItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);
                                if (AllLetterItems[i].Symbol == CardSymbol)
                                {
                                    cbCardsInsertAndUpdateWordLetter.SelectedIndex = i;
                                }
                            }
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);
                                if (AllCardSeparatorItems[i].CardSeparator == CardSeparator)
                                {
                                    cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = i;
                                }
                            }
                            pbCardsInsertAndUpdateWordImage.BackgroundImage = CardImage;
                            tbCardsInsertAndUpdateWordWord.Text = CardWord;
                            tbCardsInsertAndUpdateWordRelatedCombinations.Text = CardRelatedCombinations;
                            tbCardsInsertAndUpdateWordValue.Text = CardValue;
                            tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        }
                        else
                        {
                            List<Thread> MyThreads = new List<Thread>();
                            string[] mass = { ") " };
                            string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                            string idLetter = (FirstListItems[lbCardIndexList.SelectedIndex].ID).ToString();
                            MyThreads.Add(new Thread(() => SelectLetter(idLetter))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)                        
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            tbCardsInsertAndUpdateLetter.Text = LetterItems[0].Symbol;
                            tcCards.SelectedTab = tpCardsInsertAndUpdateLetter;
                        }
                        
                        break;
                    }
                case "buCardIndexMenuWord":
                    {
                        cbCardsInsertAndUpdateWordCard.Items.Clear();
                        cbCardsInsertAndUpdateWordBox.Items.Clear();
                        cbCardsInsertAndUpdateWordLetter.Items.Clear();
                        cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                        List<Thread> MyThreads = new List<Thread>();
                        string[] mass = { ") " };
                        string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                        string idWord = (FirstListItems[lbCardIndexList.SelectedIndex].ID).ToString();

                        MyThreads.Add(new Thread(() => ShowWordWithCard(idWord))); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)                                                                             
                        MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads[0].IsBackground = true;
                        MyThreads[0].Start(); // Запускаем поток
                        while (MyThreads[0].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[1].IsBackground = true;
                        MyThreads[1].Start(); // Запускаем поток
                        while (MyThreads[1].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[2].IsBackground = true;
                        MyThreads[2].Start(); // Запускаем поток
                        while (MyThreads[2].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[3].IsBackground = true;
                        MyThreads[3].Start(); // Запускаем поток
                        while (MyThreads[3].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[4].IsBackground = true;
                        MyThreads[4].Start(); // Запускаем поток
                        while (MyThreads[4].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }

                        for (int i = 0; i < AllCardItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);
                            if (AllCardItems[i].Marker == CardMarker)
                            {
                                cbCardsInsertAndUpdateWordCard.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i < AllBoxItems.Count; i++)
                        {                            
                            cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);
                            if (AllBoxItems[i].NumberBox == CardNumberBox)
                            {
                                cbCardsInsertAndUpdateWordBox.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i < AllLetterItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);
                            if (AllLetterItems[i].Symbol == CardSymbol)
                            {
                                cbCardsInsertAndUpdateWordLetter.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);
                            if (AllCardSeparatorItems[i].CardSeparator == CardSeparator)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = i;
                            }
                        }
                        pbCardsInsertAndUpdateWordImage.BackgroundImage = CardImage;
                        tbCardsInsertAndUpdateWordWord.Text = CardWord;
                        tbCardsInsertAndUpdateWordRelatedCombinations.Text = CardRelatedCombinations;
                        tbCardsInsertAndUpdateWordValue.Text = CardValue;
                        tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        break;
                    }
            }
            Program.f1.PictAndLableWait(false);
            EnableElement(true);            
        }

        private void buSelectWordPrev_Click(object sender, EventArgs e)
        {
            tcCards.SelectedTab = tpList;
            EnableOnCardPage(true, tpCardsSelectWord);
        }

        private void buttonInsertAndUpdatePrev_Click(object sender, EventArgs e)
        {
            switch (TagButtonChange)
            {
                case "Update":
                    {
                        DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти? Все изменения не будут сохранены", "Возврат назад", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            tcCards.SelectedTab = tpList;
                        }
                        break;
                    }
                case "Insert":
                    {
                        DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти? Введенные не будут сохранены!", "Возврат назад", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            tcCards.SelectedTab = tpList;
                        }
                        break;
                    }
            }
        }

        private void buttonInsertAndUpdateSave_Click(object sender, EventArgs e)
        {
            switch (tcCards.SelectedTab.Name)
            {
                case "tpCardsInsertAndUpdateCard":
                    {
                        switch (NameClickButtonInListPage)
                        {
                            case "buCardIndexListChange":
                                {                                   
                                    string sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateBox.SelectedItem.ToString() + "'";
                                    JSON.Send(sql, JSONFlags.Select);
                                    string BoxID = JSON.Decode()[0].ID;
                                    string[] mass = { ") " };
                                    string[] SplitItem = (lbCardIndexList.SelectedItem.ToString()).Split(mass, StringSplitOptions.RemoveEmptyEntries);
                                    sql = "SELECT ID FROM cardindex WHERE Marker = '" + SplitItem.Last() + "'";
                                    JSON.Send(sql, JSONFlags.Select);
                                    string CardID = JSON.Decode()[0].ID;

                                    string SourceCodeID;
                                    if (cbCardsInsertAndUpdateSourceCode.SelectedItem.ToString() != "")
                                    {
                                        sql = "SELECT ID FROM ukaz WHERE Name = '" + cbCardsInsertAndUpdateSourceCode.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        SourceCodeID = JSON.Decode()[0].ID;
                                    }
                                    else
                                    {
                                        SourceCodeID = "NULL";
                                    }
                                    string SourceCodeClarification;
                                    if (cbCardsInsertAndUpdateSourceCodeClarification.SelectedItem.ToString() != "")
                                    {
                                        sql = "SELECT ID FROM ukaz WHERE Name = '" + cbCardsInsertAndUpdateSourceCodeClarification.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        SourceCodeClarification = JSON.Decode()[0].ID;
                                    }
                                    else
                                    {
                                        SourceCodeClarification = "NULL";
                                    }                                   

                                    pbCardsInsertAndUpdateImage.Image.Save("tmpImage.jpg");
                                    string ImageBase64 = CodeInBase64("tmpImage.jpg");
                                    File.Delete("tmpImage.jpg");

                                    sql = "UPDATE `cardindex` SET `Marker` = '" + tbCardsInsertAndUpdateMarker.Text + "', `NumberBox` = '" + BoxID + "', `img` = '" + ImageBase64 + "', `imgText` = '" + tbCardsInsertAndUpdateTextCard.Text + "', `SourceCode` = " + SourceCodeID + ", `SourceClarification` = " + SourceCodeClarification + ", `Pagination` = '" + tbCardsInsertAndUpdatePagination.Text + "' ,`SourceDate` = '" + tbCardsInsertAndUpdateSourceDate.Text + "', `SourceDateClarification` = '" + tbCardsInsertAndUpdateSourceDateClarification.Text + "', `Notes` =  '" + tbCardsInsertAndUpdateNotes.Text + "' WHERE `ID` = " + CardID;
                                                                     
                                    JSON.Send(sql, JSONFlags.Update);
                                                                        
                                    ClearMainList();
                                    Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                    myThread.IsBackground = true;
                                    myThread.Start("buCardIndexMenuMarker"); // Запускаем поток
                                    while (myThread.IsAlive)
                                    {
                                        Thread.Sleep(1);
                                        Application.DoEvents();
                                    }                                        
                                    for (int i = 0; i < FirstListItems.Count; i++)
                                    {
                                        lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Marker);
                                    }
                                    ActiveDown3button(false);    
                                    lbCardIndexList.Update();

                                    MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    tcCards.SelectedTab = tpList;
                                    break;
                                }
                            case "buCardIndexListAdd":
                                {
                                    if (tbCardsInsertAndUpdateMarker.Text != null && tbCardsInsertAndUpdateMarker.Text != "")
                                    {
                                        string sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateBox.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string BoxID = JSON.Decode()[0].ID;

                                        sql = "SELECT ID FROM ukaz WHERE Name = '" + cbCardsInsertAndUpdateSourceCode.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string SourceCodeID = JSON.Decode()[0].ID;

                                        string SourceCodeClarification;
                                        if (cbCardsInsertAndUpdateSourceCodeClarification.SelectedItem.ToString() != "" || cbCardsInsertAndUpdateSourceCodeClarification.SelectedItem.ToString() != "")
                                        {
                                            sql = "SELECT ID FROM ukaz WHERE Name = '" + cbCardsInsertAndUpdateSourceCodeClarification.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            SourceCodeClarification = JSON.Decode()[0].ID;
                                        }
                                        else
                                        {
                                            SourceCodeClarification = null;
                                        }

                                        pbCardsInsertAndUpdateImage.Image.Save("tmpImage.jpg");
                                        string ImageBase64 = CodeInBase64("tmpImage.jpg");
                                        File.Delete("tmpImage.jpg");

                                        sql = "INSERT INTO `cardindex`(`Marker`, `NumberBox`, `img`, `imgText`, `SourceCode`, `SourceClarification`, `Pagination`, `SourceDate`, `SourceDateClarification`, `Notes`) VALUES ('" + tbCardsInsertAndUpdateMarker.Text + "', '" + BoxID + "', '" + ImageBase64 + "', '" + tbCardsInsertAndUpdateTextCard.Text + "', " + SourceCodeID + ", " + SourceCodeClarification + ", '" + tbCardsInsertAndUpdatePagination.Text + "','" + tbCardsInsertAndUpdateSourceDate.Text + "', '" + tbCardsInsertAndUpdateSourceDateClarification.Text + "', '" + tbCardsInsertAndUpdateNotes.Text + "')";
                                        JSON.Send(sql, JSONFlags.Insert);

                                        ClearMainList();
                                        Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                        myThread.IsBackground = true;
                                        myThread.Start("buCardIndexMenuMarker"); // Запускаем поток
                                        while (myThread.IsAlive)
                                        {
                                            Thread.Sleep(1);
                                            Application.DoEvents();
                                        }
                                        for (int i = 0; i < FirstListItems.Count; i++)
                                        {
                                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Marker);
                                        }
                                        ActiveDown3button(false);
                                        lbCardIndexList.Update();

                                        MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tcCards.SelectedTab = tpList;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Не все поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "tpCardsInsertAndUpdateWord":
                    {
                        switch (NameClickButtonInListPage)
                        {
                            case "buCardIndexListChange":
                                {
                                    string sql;
                                    if (NameClickButtonInMenu == "buCardIndexMenuWord")
                                    {
                                        sql = "SELECT ID FROM cardindex WHERE Marker = '" + cbCardsInsertAndUpdateWordCard.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string CardID = JSON.Decode()[0].ID;
                                        sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateWordBox.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string BoxID = JSON.Decode()[0].ID;
                                        sql = "SELECT ID FROM letter WHERE Symbol = '" + cbCardsInsertAndUpdateWordLetter.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string SymbolID = JSON.Decode()[0].ID;
                                        sql = "SELECT ID FROM cardseparator WHERE CardSeparator = '" + cbCardsInsertAndUpdateWordCardSeparator.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string CardSeparatorID = JSON.Decode()[0].ID;
                                        sql = "UPDATE `flotation` SET `Card`= '" + CardID + "',`NumberBox`= '" + BoxID + "',`Symbol`= '" + SymbolID + "',`CardSeparator`= '" + CardSeparatorID + "',`Word`= '" + tbCardsInsertAndUpdateWordWord.Text + "',`Value`= '" + tbCardsInsertAndUpdateWordValue.Text + "',`RelatedCombinations`= '" + tbCardsInsertAndUpdateWordRelatedCombinations.Text + "' WHERE `ID` = " + FirstListItems[lbCardIndexList.SelectedIndex].ID;
                                    }
                                    else
                                    {
                                        sql = "SELECT ID FROM cardindex WHERE Marker = '" + cbCardsInsertAndUpdateWordCard.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string CardID = JSON.Decode()[0].ID;
                                        sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateWordBox.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string BoxID = JSON.Decode()[0].ID;
                                        sql = "SELECT ID FROM letter WHERE Symbol = '" + cbCardsInsertAndUpdateWordLetter.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string SymbolID = JSON.Decode()[0].ID;
                                        sql = "SELECT ID FROM cardseparator WHERE CardSeparator = '" + cbCardsInsertAndUpdateWordCardSeparator.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string CardSeparatorID = JSON.Decode()[0].ID;
                                        sql = "UPDATE `flotation` SET `Card`= '" + CardID + "',`NumberBox`= '" + BoxID + "',`Symbol`= '" + SymbolID + "',`CardSeparator`= '" + CardSeparatorID + "',`Word`= '" + tbCardsInsertAndUpdateWordWord.Text + "',`Value`= '" + tbCardsInsertAndUpdateWordValue.Text + "',`RelatedCombinations`= '" + tbCardsInsertAndUpdateWordRelatedCombinations.Text + "' WHERE `ID` = " + SecondListItems[lbCardIndexList.SelectedIndex].ID;
                                    }
                                    JSON.Send(sql, JSONFlags.Update);
                                    ClearMainList();
                                    Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                    myThread.IsBackground = true;
                                    myThread.Start("buCardIndexMenuWord"); // Запускаем поток
                                    while (myThread.IsAlive)
                                    {
                                        Thread.Sleep(1);
                                        Application.DoEvents();
                                    }
                                    for (int i = 0; i < FirstListItems.Count; i++)
                                    {
                                        lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Word);
                                    }
                                    ActiveDown3button(false);
                                    lbCardIndexList.Update();

                                    MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    tcCards.SelectedTab = tpList;
                                    break;
                                }
                            case "buCardIndexListAdd":
                                {
                                    if (tbCardsInsertAndUpdateWordWord.Text != null && tbCardsInsertAndUpdateWordWord.Text != "")
                                    {
                                        string sql;
                                        if (NameClickButtonInMenu == "buCardIndexMenuWord")
                                        {
                                            sql = "SELECT ID FROM cardindex WHERE Marker = '" + cbCardsInsertAndUpdateWordCard.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string CardID = JSON.Decode()[0].ID;
                                            sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateWordBox.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string BoxID = JSON.Decode()[0].ID;
                                            sql = "SELECT ID FROM letter WHERE Symbol = '" + cbCardsInsertAndUpdateWordLetter.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string SymbolID = JSON.Decode()[0].ID;
                                            sql = "SELECT ID FROM cardseparator WHERE CardSeparator = '" + cbCardsInsertAndUpdateWordCardSeparator.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string CardSeparatorID = JSON.Decode()[0].ID;
                                            sql = "INSERT INTO `flotation`(`Card`, `NumberBox`, `Symbol`, `CardSeparator`, `Word`, `Value`, `RelatedCombinations`) VALUES ('" + CardID + "', '" + BoxID + "', '" + SymbolID + "', '" + CardSeparatorID + "', '" + tbCardsInsertAndUpdateWordWord.Text + "', '" + tbCardsInsertAndUpdateWordValue.Text + "', '" + tbCardsInsertAndUpdateWordRelatedCombinations.Text + "')";
                                            JSON.Send(sql, JSONFlags.Insert);
                                            ClearMainList();
                                            Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                            myThread.IsBackground = true;
                                            myThread.Start("buCardIndexMenuWord"); // Запускаем поток
                                            while (myThread.IsAlive)
                                            {
                                                Thread.Sleep(1);
                                                Application.DoEvents();
                                            }
                                            for (int i = 0; i < FirstListItems.Count; i++)
                                            {
                                                lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Word);
                                            }
                                            ActiveDown3button(false);
                                            lbCardIndexList.Update();
                                        }
                                        else
                                        {
                                            sql = "SELECT ID FROM cardindex WHERE Marker = '" + cbCardsInsertAndUpdateWordCard.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string CardID = JSON.Decode()[0].ID;
                                            sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateWordBox.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string BoxID = JSON.Decode()[0].ID;
                                            sql = "SELECT ID FROM letter WHERE Symbol = '" + cbCardsInsertAndUpdateWordLetter.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string SymbolID = JSON.Decode()[0].ID;
                                            sql = "SELECT ID FROM cardseparator WHERE CardSeparator = '" + cbCardsInsertAndUpdateWordCardSeparator.SelectedItem.ToString() + "'";
                                            JSON.Send(sql, JSONFlags.Select);
                                            string CardSeparatorID = JSON.Decode()[0].ID;
                                            sql = "INSERT INTO `flotation`(`Card`, `NumberBox`, `Symbol`, `CardSeparator`, `Word`, `Value`, `RelatedCombinations`) VALUES ('" + CardID + "', '" + BoxID + "', '" + SymbolID + "', '" + CardSeparatorID + "', '" + tbCardsInsertAndUpdateWordWord.Text + "', '" + tbCardsInsertAndUpdateWordValue.Text + "', '" + tbCardsInsertAndUpdateWordRelatedCombinations.Text + "')";
                                            JSON.Send(sql, JSONFlags.Insert);
                                            ClearMainList();
                                            Thread myThread = new Thread(new ParameterizedThreadStart(CreateSecondListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                            ListBoxSelectedIndex = ListBoxSelectedIndexForUpdate;
                                            switch (NameClickButtonInMenu)
                                            {
                                                case "buCardIndexMenuSeparator":
                                                    {
                                                        myThread.IsBackground = true;
                                                        myThread.Start("buCardIndexMenuSeparator"); // Запускаем поток
                                                        break;
                                                    }

                                                case "buCardIndexMenuBox":
                                                    {
                                                        myThread.IsBackground = true;
                                                        myThread.Start("buCardIndexMenuBox");
                                                        break;
                                                    }
                                                case "buCardIndexMenuLetter":
                                                    {
                                                        myThread.IsBackground = true;
                                                        myThread.Start("buCardIndexMenuLetter");
                                                        break;
                                                    }
                                            }
                                            while (myThread.IsAlive)
                                            {
                                                Thread.Sleep(1);
                                                Application.DoEvents();
                                            }
                                            for (int i = 0; i < SecondListItems.Count; i++)
                                            {
                                                lbCardIndexList.Items.Add((i + 1) + ") " + SecondListItems[i].Word);
                                            }
                                            ActiveDown3button(false);
                                            lbCardIndexList.Update();
                                        }
                                        
                                        MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tcCards.SelectedTab = tpList;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Не все поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "tpCardsInsertAndUpdateLetter":
                    {
                        switch (NameClickButtonInListPage)
                        {
                            case "buCardIndexListChange":
                                {
                                    string sql = "UPDATE `letter` SET `Symbol`= '" + tbCardsInsertAndUpdateLetter.Text + "' WHERE `ID` = " + FirstListItems[lbCardIndexList.SelectedIndex].ID;
                                    JSON.Send(sql, JSONFlags.Update);
                                    ClearMainList();
                                    Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                    myThread.IsBackground = true;
                                    myThread.Start("buCardIndexMenuLetter"); // Запускаем поток
                                    while (myThread.IsAlive)
                                    {
                                        Thread.Sleep(1);
                                        Application.DoEvents();
                                    }
                                    for (int i = 0; i < FirstListItems.Count; i++)
                                    {
                                        lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                                    }
                                    ActiveDown3button(false);
                                    lbCardIndexList.Update();
                                    MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    tcCards.SelectedTab = tpList;
                                    break;
                                }
                            case "buCardIndexListAdd":
                                {
                                    if (tbCardsInsertAndUpdateLetter.Text != null && tbCardsInsertAndUpdateLetter.Text != "")
                                    {
                                        string sql = "INSERT INTO `letter`(`Symbol`) VALUES ('" + tbCardsInsertAndUpdateLetter.Text + "')";
                                        JSON.Send(sql, JSONFlags.Insert);
                                        ClearMainList();
                                        Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                        myThread.IsBackground = true;
                                        myThread.Start("buCardIndexMenuLetter"); // Запускаем поток
                                        while (myThread.IsAlive)
                                        {
                                            Thread.Sleep(1);
                                            Application.DoEvents();
                                        }
                                        for (int i = 0; i < FirstListItems.Count; i++)
                                        {
                                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].Symbol);
                                        }
                                        ActiveDown3button(false);
                                        lbCardIndexList.Update();
                                        MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tcCards.SelectedTab = tpList;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Не все поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "tpCardsInsertAndUpdateBox":
                    {
                        switch (NameClickButtonInListPage)
                        {
                            case "buCardIndexListChange":
                                {
                                    string sql = "UPDATE `box` SET `NumberBox`= '" + tbCardsInsertAndUpdateBoxNumberBox.Text + "' WHERE `ID` = " + FirstListItems[lbCardIndexList.SelectedIndex].ID;
                                    JSON.Send(sql, JSONFlags.Update);
                                    ClearMainList();
                                    Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                    myThread.IsBackground = true;
                                    myThread.Start("buCardIndexMenuBox"); // Запускаем поток
                                    while (myThread.IsAlive)
                                    {
                                        Thread.Sleep(1);
                                        Application.DoEvents();
                                    }
                                    for (int i = 0; i < FirstListItems.Count; i++)
                                    {
                                        lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                                    }
                                    ActiveDown3button(false);
                                    lbCardIndexList.Update();
                                    MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    tcCards.SelectedTab = tpList;
                                    break;
                                }
                            case "buCardIndexListAdd":
                                {
                                    if (tbCardsInsertAndUpdateBoxNumberBox.Text != null && tbCardsInsertAndUpdateBoxNumberBox.Text != "")
                                    {
                                        string sql = "INSERT INTO `box`(`NumberBox`) VALUES ('" + tbCardsInsertAndUpdateBoxNumberBox.Text + "')";
                                        JSON.Send(sql, JSONFlags.Insert);
                                        ClearMainList();
                                        Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                        myThread.IsBackground = true;
                                        myThread.Start("buCardIndexMenuBox"); // Запускаем поток
                                        while (myThread.IsAlive)
                                        {
                                            Thread.Sleep(1);
                                            Application.DoEvents();
                                        }
                                        for (int i = 0; i < FirstListItems.Count; i++)
                                        {
                                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].NumberBox);
                                        }
                                        ActiveDown3button(false);
                                        lbCardIndexList.Update();
                                        MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tcCards.SelectedTab = tpList;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Не все поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "tpCardsInsertAndUpdateCardSeparator":
                    {
                        switch (NameClickButtonInListPage)
                        {
                            case "buCardIndexListChange":
                                {
                                    string sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateCardSeparatorBox.SelectedItem.ToString() + "'";
                                    JSON.Send(sql, JSONFlags.Select);
                                    string NumberBoxID = JSON.Decode()[0].ID;
                                    sql = "UPDATE `cardseparator` SET `CardSeparator`= '" + tbCardsInsertAndUpdateCardSeparatorLetter.Text + "',`NumberBox`= " + NumberBoxID + " WHERE `ID` = " + Convert.ToInt32(AllCardSeparatorItems[lbCardIndexList.SelectedIndex].ID);
                                    JSON.Send(sql, JSONFlags.Update);
                                    ClearMainList();
                                    Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                    myThread.IsBackground = true;
                                    myThread.Start("buCardIndexMenuSeparator"); // Запускаем поток
                                    while (myThread.IsAlive)
                                    {
                                        Thread.Sleep(1);
                                        Application.DoEvents();
                                    }
                                    for (int i = 0; i < FirstListItems.Count; i++)
                                    {
                                        lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                                    }
                                    ActiveDown3button(false);
                                    lbCardIndexList.Update();
                                    MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    tcCards.SelectedTab = tpList;
                                    break;
                                }
                            case "buCardIndexListAdd":
                                {
                                    if (tbCardsInsertAndUpdateCardSeparatorLetter.Text !=null && tbCardsInsertAndUpdateCardSeparatorLetter.Text != "")
                                    {
                                        string sql = "SELECT ID FROM box WHERE NumberBox = '" + cbCardsInsertAndUpdateCardSeparatorBox.SelectedItem.ToString() + "'";
                                        JSON.Send(sql, JSONFlags.Select);
                                        string NumberBoxID = JSON.Decode()[0].ID;
                                        sql = "INSERT INTO `cardseparator`(`CardSeparator`, `NumberBox`) VALUES ('" + tbCardsInsertAndUpdateCardSeparatorLetter.Text + "', " + NumberBoxID + ")";
                                        JSON.Send(sql, JSONFlags.Insert);
                                        ClearMainList();
                                        Thread myThread = new Thread(new ParameterizedThreadStart(CreateFirstListItems)); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                                        myThread.IsBackground = true;
                                        myThread.Start("buCardIndexMenuSeparator"); // Запускаем поток
                                        while (myThread.IsAlive)
                                        {
                                            Thread.Sleep(1);
                                            Application.DoEvents();
                                        }
                                        for (int i = 0; i < FirstListItems.Count; i++)
                                        {
                                            lbCardIndexList.Items.Add((i + 1) + ") " + FirstListItems[i].CardSeparator);
                                        }
                                        ActiveDown3button(false);
                                        lbCardIndexList.Update();
                                        MessageBox.Show("Операция выполнена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        tcCards.SelectedTab = tpList;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Не все поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }            
        }

        private void CardIndexModule_SizeChanged(object sender, EventArgs e)
        {
            /*foreach (ComboBox comboBox in MainForm.GetAll(tpCardsInsertAndUpdateCard, typeof(ComboBox)))
            {
                comboBox.Size = new Size(comboBox.Size.Width, tbCardsInsertAndUpdateMarker.Height);
                //SetComboBoxHeight(comboBox.Handle, tbCardsInsertAndUpdateMarker.Height);
                comboBox.Refresh();
            }
            foreach (ComboBox comboBox in MainForm.GetAll(tpCardsInsertAndUpdateWord, typeof(ComboBox)))
            {
                SetComboBoxHeight(comboBox.Handle, buCardsInsertAndUpdateWordWord.Height);
                comboBox.Refresh();
            }
            foreach (ComboBox comboBox in MainForm.GetAll(tpCardsInsertAndUpdateCardSeparator, typeof(ComboBox)))
            {
                SetComboBoxHeight(comboBox.Handle, tbCardsInsertAndUpdateCardSeparatorLetter.Height);
                comboBox.Refresh();
            }*/
        }

        private void buCardIndexListAdd_Click(object sender, EventArgs e)
        {
            NameClickButtonInListPage = (sender as Button).Name.ToString();
            EnableElement(false);
            ListBoxSelectedIndex = lbCardIndexList.SelectedIndex + 1;
            Program.f1.PictAndLableWait(true);
            TagButtonChange = (sender as Button).Tag.ToString();
            switch (NameClickButtonInMenu)
            {
                //Работает
                case "buCardIndexMenuMarker":
                    {
                        cbCardsInsertAndUpdateBox.Items.Clear();
                        cbCardsInsertAndUpdateSourceCode.Items.Clear();
                        cbCardsInsertAndUpdateSourceCodeClarification.Items.Clear();
                        List<Thread> MyThreads = new List<Thread>();                        
                        MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllUkaz())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads[0].IsBackground = true;
                        MyThreads[0].Start();
                        while (MyThreads[0].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[1].IsBackground = true;
                        MyThreads[1].Start();
                        while (MyThreads[1].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }

                        for (int i = 0; i < AllBoxItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateBox.Items.Add(AllBoxItems[i].NumberBox);
                        }
                        for (int i = 0; i < AllUkazItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateSourceCode.Items.Add(AllUkazItems[i].Name);
                        }
                        cbCardsInsertAndUpdateSourceCodeClarification.Items.Add("");
                        for (int i = 0; i < AllUkazItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateSourceCodeClarification.Items.Add(AllUkazItems[i].Name);
                        }
                        cbCardsInsertAndUpdateBox.SelectedIndex = 0;
                        cbCardsInsertAndUpdateSourceCode.SelectedIndex = 0;
                        cbCardsInsertAndUpdateSourceCode.SelectedIndex = 0;
                        tbCardsInsertAndUpdateMarker.Text = "";
                        pbCardsInsertAndUpdateImage.Image = Properties.Resources.noimage;
                        tbCardsInsertAndUpdateTextCard.Text = "";
                        tbCardsInsertAndUpdatePagination.Text = "";
                        tbCardsInsertAndUpdateSourceDate.Text = "";
                        tbCardsInsertAndUpdateSourceDateClarification.Text = "";
                        tbCardsInsertAndUpdateNotes.Text = "";
                        tcCards.SelectedTab = tpCardsInsertAndUpdateCard;
                        break;
                    }
                //Криво
                case "buCardIndexMenuSeparator":
                    {
                        if (UseSecondList == true)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Clear();
                            cbCardsInsertAndUpdateWordBox.Items.Clear();
                            cbCardsInsertAndUpdateWordLetter.Items.Clear();
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();
                            
                            MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start(); // Запускаем поток
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[2].IsBackground = true;
                            MyThreads[2].Start(); // Запускаем поток
                            while (MyThreads[2].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[3].IsBackground = true;
                            MyThreads[3].Start(); // Запускаем поток
                            while (MyThreads[3].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }                            

                            for (int i = 0; i < AllCardItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);                                
                            }
                            cbCardsInsertAndUpdateWordCard.SelectedIndex = 0;
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);                                
                            }
                            cbCardsInsertAndUpdateWordBox.SelectedIndex = 0;
                            for (int i = 0; i < AllLetterItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);                                
                            }
                            cbCardsInsertAndUpdateWordLetter.SelectedIndex = 0;
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);                                
                            }
                            cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = 0;
                            tbCardsInsertAndUpdateWordWord.Text = "";
                            tbCardsInsertAndUpdateWordRelatedCombinations.Text = "";
                            tbCardsInsertAndUpdateWordValue.Text = "";
                            tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        }
                        else
                        {
                            cbCardsInsertAndUpdateCardSeparatorBox.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();

                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start();
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateCardSeparatorBox.Items.Add(AllBoxItems[i].NumberBox);                                
                            }
                            cbCardsInsertAndUpdateCardSeparatorBox.SelectedIndex = 0;                            
                            tbCardsInsertAndUpdateCardSeparatorLetter.Text = "";
                            tcCards.SelectedTab = tpCardsInsertAndUpdateCardSeparator;
                        }
                        break;
                    }
                //Криво
                case "buCardIndexMenuBox":
                    {
                        if (UseSecondList == true)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Clear();
                            cbCardsInsertAndUpdateWordBox.Items.Clear();
                            cbCardsInsertAndUpdateWordLetter.Items.Clear();
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();

                            MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start(); // Запускаем поток
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[2].IsBackground = true;
                            MyThreads[2].Start(); // Запускаем поток
                            while (MyThreads[2].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[3].IsBackground = true;
                            MyThreads[3].Start(); // Запускаем поток
                            while (MyThreads[3].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }                            

                            for (int i = 0; i < AllCardItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);                                
                            }
                            cbCardsInsertAndUpdateWordCard.SelectedIndex = 0;
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);                                
                            }
                            cbCardsInsertAndUpdateWordBox.SelectedIndex = 0;
                            for (int i = 0; i < AllLetterItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);                                
                            }
                            cbCardsInsertAndUpdateWordLetter.SelectedIndex = 0;
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);                                
                            }
                            cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = 0;
                            tbCardsInsertAndUpdateWordWord.Text = "";
                            tbCardsInsertAndUpdateWordRelatedCombinations.Text = "";
                            tbCardsInsertAndUpdateWordValue.Text = "";
                            tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        }
                        else
                        {
                            tbCardsInsertAndUpdateBoxNumberBox.Text = "";
                            tcCards.SelectedTab = tpCardsInsertAndUpdateBox;
                        }
                        break;
                    }
                case "buCardIndexMenuLetter":
                    {
                        if (UseSecondList == true)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Clear();
                            cbCardsInsertAndUpdateWordBox.Items.Clear();
                            cbCardsInsertAndUpdateWordLetter.Items.Clear();
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                            List<Thread> MyThreads = new List<Thread>();

                            MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                            MyThreads[0].IsBackground = true;
                            MyThreads[0].Start(); // Запускаем поток
                            while (MyThreads[0].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[1].IsBackground = true;
                            MyThreads[1].Start(); // Запускаем поток
                            while (MyThreads[1].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[2].IsBackground = true;
                            MyThreads[2].Start(); // Запускаем поток
                            while (MyThreads[2].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }
                            MyThreads[3].IsBackground = true;
                            MyThreads[3].Start(); // Запускаем поток
                            while (MyThreads[3].IsAlive)
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                            }                            

                            for (int i = 0; i < AllCardItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);
                            }
                            cbCardsInsertAndUpdateWordCard.SelectedIndex = 0;
                            for (int i = 0; i < AllBoxItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);                                
                            }
                            cbCardsInsertAndUpdateWordBox.SelectedIndex = 0;
                            for (int i = 0; i < AllLetterItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);
                            }
                            cbCardsInsertAndUpdateWordLetter.SelectedIndex = 0;
                            for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                            {
                                cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);
                            }
                            cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = 0;
                            tbCardsInsertAndUpdateWordWord.Text = "";
                            tbCardsInsertAndUpdateWordRelatedCombinations.Text = "";
                            tbCardsInsertAndUpdateWordValue.Text = "";
                            tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        }
                        else
                        {                            
                            tbCardsInsertAndUpdateLetter.Text = "";
                            tcCards.SelectedTab = tpCardsInsertAndUpdateLetter;
                        }

                        break;
                    }
                case "buCardIndexMenuWord":
                    {
                        cbCardsInsertAndUpdateWordCard.Items.Clear();
                        cbCardsInsertAndUpdateWordBox.Items.Clear();
                        cbCardsInsertAndUpdateWordLetter.Items.Clear();
                        cbCardsInsertAndUpdateWordCardSeparator.Items.Clear();
                        List<Thread> MyThreads = new List<Thread>();
                        MyThreads.Add(new Thread(() => SelectAllCardNumber())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllBox())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllLetter())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads.Add(new Thread(() => SelectAllCardSeparator())); //Создаем новый объект потока (функция, которая должна выпонится в фоновом режиме)
                        MyThreads[0].IsBackground = true;
                        MyThreads[0].Start(); // Запускаем поток
                        while (MyThreads[0].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[1].IsBackground = true;
                        MyThreads[1].Start(); // Запускаем поток
                        while (MyThreads[1].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[2].IsBackground = true;
                        MyThreads[2].Start(); // Запускаем поток
                        while (MyThreads[2].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        MyThreads[3].IsBackground = true;
                        MyThreads[3].Start(); // Запускаем поток
                        while (MyThreads[3].IsAlive)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }                        

                        for (int i = 0; i < AllCardItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordCard.Items.Add(AllCardItems[i].Marker);
                        }
                        cbCardsInsertAndUpdateWordCard.SelectedIndex = 0;
                        for (int i = 0; i < AllBoxItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordBox.Items.Add(AllBoxItems[i].NumberBox);                            
                        }
                        cbCardsInsertAndUpdateWordBox.SelectedIndex = 0;
                        for (int i = 0; i < AllLetterItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordLetter.Items.Add(AllLetterItems[i].Symbol);                            
                        }
                        cbCardsInsertAndUpdateWordLetter.SelectedIndex = 0;
                        for (int i = 0; i < AllCardSeparatorItems.Count; i++)
                        {
                            cbCardsInsertAndUpdateWordCardSeparator.Items.Add(AllCardSeparatorItems[i].CardSeparator);                            
                        }
                        cbCardsInsertAndUpdateWordCardSeparator.SelectedIndex = 0;
                        tbCardsInsertAndUpdateWordWord.Text = "";
                        tbCardsInsertAndUpdateWordRelatedCombinations.Text = "";
                        tbCardsInsertAndUpdateWordValue.Text = "";
                        tcCards.SelectedTab = tpCardsInsertAndUpdateWord;
                        break;
                    }
            }
            Program.f1.PictAndLableWait(false);
            EnableElement(true);
        }

        private void cbCardsInsertAndUpdateWordCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT img FROM cardindex WHERE Marker = " + cbCardsInsertAndUpdateWordCard.SelectedItem.ToString();
            JSON.Send(sql, JSONFlags.Select);
            pbCardsInsertAndUpdateWordImage.BackgroundImage = DecodeImageFromDB(JSON.Decode()[0].Img);
        }

        private void buCardsInsertAndUpdateOpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Файлы изображений (*.bmp, *.jpg, *.jpeg, *.png, *.gif, *.tiff)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                pbCardsInsertAndUpdateImage.Image = Image.FromFile(OPF.FileName);
            }
            EnableButtonDeleteImage();
        }
        void EnableButtonDeleteImage()
        {
            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            pbCardsInsertAndUpdateImage.Image.Save(stream1, System.Drawing.Imaging.ImageFormat.Png);
            Properties.Resources.noimage.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);
            byte[] bytes1 = stream1.ToArray();
            byte[] bytes2 = stream2.ToArray();
            if (Convert.ToBase64String(bytes1).Equals(Convert.ToBase64String(bytes2)))
            {
                buCardsInsertAndUpdateDeleteImage.Enabled = false;
            }
            else
            {
                buCardsInsertAndUpdateDeleteImage.Enabled = true;
            }
        }
        private void buCardsInsertAndUpdateDeleteImage_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить изображение? Отменить данное действие будет невозможно!", "Удаление изображения", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                pbCardsInsertAndUpdateImage.Image = Properties.Resources.noimage;
                EnableButtonDeleteImage();
            }
        }
    }
}