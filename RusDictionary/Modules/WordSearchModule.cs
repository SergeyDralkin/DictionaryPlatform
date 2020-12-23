using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace RusDictionary.Modules
{
    public partial class WordSearchModule : UserControl
    {
        public WordSearchModule()
        {
            InitializeComponent();
        }
        void ReadingHTM()
        {
            string query = "TRUNCATE TABLE dictionaryentries";
            JSON.Send(query, JSONFlags.Truncate);

            StringReader sr = new StringReader(Properties.Resources.tom2);
            string line;
            //bool next = true; // Переключатель завершения считывания
            //int count = 0; // Количество строк для считывания
           // int globCount = 0; // Количество строк для считывания
            bool read = false; // Переключатель считывания статей
            string dictionaryEntry = "";
            string tmp = "";
            bool newEntry = true;
            bool mainWord = false;
            bool table = false;
            while ((line = sr.ReadLine()) != null /*&& next*/)
            {
                if (line.Contains("<table")) // Отброс таблиц
                {
                    table = true;
                }
                if (line.Contains("</table>"))
                {
                    table = false;
                }
                if (line.Contains("<p") && !table) // Поиск заглавных слов
                {
                    read = true;
                    if (line.Contains("<b>"))
                    {
                        bool forExit = true;
                        int i = 0;
                        int start = 0;
                        while (forExit && i < line.Length - 3)
                        {
                            if (line.Substring(i, 3) == "<b>")
                            {
                                start = i + 3;
                                forExit = false;
                            }
                            i++;
                        }
                        string nam = line.Substring(start);
                        int check = 0;
                        int max = 0;
                        for (i = 0; i < nam.Length; i++)
                        {
                            string s = nam[i].ToString();
                            if (s.Any(char.IsUpper))
                            {
                                check++;
                            }
                            else
                            {
                                check = 0;
                            }
                            if (max < check)
                            {
                                max = check;
                            }
                        }
                        if (max > 1)
                        {
                            mainWord = true;
                        }
                    }
                }
                if (read)
                {
                    tmp += line;
                }
                if (line.Contains("</p") && !table) // Переход между абзацами
                {
                    if (newEntry)
                    {
                        dictionaryEntry = tmp;
                        tmp = "";
                        newEntry = false;
                    }
                    else
                    {
                        if (tmp.Contains("<b>") && mainWord)
                        {
                            newEntry = true;
                        }
                        else
                        {
                            if (!tmp.Contains("Словарь русского яз. XI—XVII"))
                            {
                                dictionaryEntry += tmp;
                            }
                            tmp = "";
                        }
                    }
                    mainWord = false;
                    read = false;
                }
                if (!read && dictionaryEntry != "" && newEntry) // Обработка статьи
                {
                    //globCount++;
                    //tbText.Text += dictionaryEntry + "\r\n" + "\r\n";
                    DictionaryEntryDivide(dictionaryEntry);
                    dictionaryEntry = tmp;
                    tmp = "";
                    newEntry = false;
                }
                //count++;
                //if (count > 2500)
                //{
                //    next = false;
                //}
            }
            //globCount = globCount;
        }
        /// <summary>
        /// Разбиение словарной статьи
        /// </summary>
        void DictionaryEntryDivide(string s) // Словарная статья
        {
            int startIndex = 0; // Индексы начала и конца заголовочного слова
            int finishIndex = 0;
            bool forExit = true;
            int i = 0;
            string name = "";
            string pomet = "";
            string description = "";
            //string partOfSpeech = "";
            //string rod = "";
            //string defenition = "";
            //string example = "";
            while (forExit && i < s.Length - 3) // Поиск начального индекса
            {
                if (s.Substring(i, 3) == "<b>")
                {
                    startIndex = i + 3;
                    forExit = false;
                }
                i++;
            }
            forExit = true;
            i = 0;
            while (forExit && i < s.Length - 5) // Поиск конечного индекса
            {
                if (s.Substring(i, 5) == ",</b>")
                {
                    finishIndex = i;
                    forExit = false;
                }
                i++;
            }
            if (forExit)
            {
                forExit = true;
                i = 0;
                while (forExit && i < s.Length - 4) // Поиск конечного индекса
                {
                    if (s.Substring(i, 5) == ".</b>")
                    {
                        finishIndex = i + 1;
                        forExit = false;
                    }
                    i++;
                }
            }
            if (forExit)
            {
                forExit = true;
                i = 0;
                while (forExit && i < s.Length - 5) // Поиск конечного индекса
                {
                    if (s.Substring(i, 4) == "</b>" && s.Substring(i, 5) != "</b>#")
                    {
                        finishIndex = i;
                        forExit = false;
                    }
                    i++;
                }
            }
            else
            {
                forExit = true;
                i = 0;
                while (forExit && i < s.Length - 5) // Поиск конечного индекса
                {
                    if (s.Substring(i, 4) == "</b>" && i < finishIndex && s.Substring(i, 5) != "</b>#")
                    {
                        finishIndex = i;
                        forExit = false;
                    }
                    i++;
                }
            }
            if (finishIndex != 0 && finishIndex > startIndex) // Вывод заголовочного слова
            {
                name = s.Substring(startIndex, finishIndex - startIndex);
                name = ClearTags(name);
                //tbText.Text += name /*+ "\r\n"*/;
                forExit = true;
                i = finishIndex;
                int endOfPometIndex = 0;
                while (forExit && i < s.Length - 4) // Поиск помет
                {
                    if (s.Substring(i, 4) == "</i>")
                    {
                        endOfPometIndex = i;
                        forExit = false;
                    }
                    i++;
                }
                if (finishIndex < endOfPometIndex)
                {
                    int tmp = endOfPometIndex - (finishIndex + 4);
                    if (tmp < 0) tmp = 0;
                    pomet = s.Substring(finishIndex + 4, tmp); // Вывод помет
                    pomet = ClearTags(pomet);
                    //tbText.Text += pomet /*+ "\r\n"*/;
                    tmp = s.Length - (endOfPometIndex + 4);
                    if (tmp < 0) tmp = 0;
                    description = s.Substring(endOfPometIndex + 4, tmp); // Вывод описания
                    description = ClearTags(description);
                    //tbText.Text += description/*+ "\r\n"*/;
                }
                if (endOfPometIndex == 0)
                {
                    description = s.Substring(finishIndex, s.Length - finishIndex);
                    description = ClearTags(description);
                }
                name = name.Replace("'", "");
                pomet = pomet.Replace("'", "");
                description = description.Replace("'", "");
                // Тут код
                if (name.Contains("см. "))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    while (forExit && i < name.Length - 4)
                    {
                        if (name.Substring(i, 4) == "см. ")
                        {
                            st = i;
                            forExit = false;
                        }
                        i++;
                    }
                    pomet = name.Substring(st, name.Length - st) + pomet;
                    name = name.Substring(0, st);
                }
                string[] pmt = { "м.", "с.", "ж.", "1.", "2.", "3.", "4.", "5.", "1", "2", "3", "4", "5" };
                for (int j = 0; j < pmt.Length; j++)
                {
                    if (name.Contains(pmt[j]))
                    {
                        name = name.Replace(pmt[j], "");
                        if (!pomet.Contains(pmt[j]))
                        {
                            pomet = pmt[j] + " " + pomet;
                        }
                    }
                }
                forExit = true;
                while (forExit)
                {
                    if (name[name.Length - 1] == ',' || name[name.Length - 1] == ' ' || name[name.Length - 1] == '.')
                    {
                        name = name.Substring(0, name.Length - 1);
                    }
                    else
                    {
                        forExit = false;
                    }
                }
                AddBD(name, pomet, description);
            }
        }
        string ClearTags(string s) // Очищение тагов
        {
            string outValue = s;
            int i;
            int tagBegin;
            int tagEnd;
            string tag;
            while (outValue.Contains("<"))
            {
                tagBegin = -1;
                tagEnd = -1;
                i = 0;
                while (i < outValue.Length)
                {
                    if (outValue[i].ToString() == "<")
                    {
                        tagBegin = i;

                    }
                    if (outValue[i].ToString() == ">")
                    {
                        tagEnd = i + 1;
                    }
                    i++;
                }
                if (tagBegin != -1)
                {
                    if (tagEnd <= tagBegin) tagEnd = outValue.Length - 1;
                    tag = outValue.Substring(tagBegin, tagEnd - tagBegin);
                    outValue = outValue.Replace(tag, "");
                }
            }
            outValue = outValue.Replace("i>", "");
            outValue = outValue.Replace("sup>", "");
            outValue = outValue.Replace("/p>", "");
            outValue = outValue.Replace(">", "");
            return outValue;
        }
        void AddBD(string nam, string pom, string def)
        {
            string query = "INSERT INTO dictionaryentries (`NAME`, `POMET`, `DEFINITION`) VALUES ('" + nam + "', '" + pom + "', '" + def + "')";
            // объект для выполнения SQL-запроса
            JSON.Send(System.Net.WebUtility.UrlEncode(query), JSONFlags.Insert);
        }
        private void buWordSearch_Read_Click(object sender, EventArgs e)
        {
            Program.f1.PictAndLableWait(true);
            List<Thread> massThread = new List<Thread>();
            massThread.Add(new Thread(() => ReadingHTM()));
            massThread[0].Start();
            while (massThread[0].IsAlive)
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }
            massThread.Clear();
            Program.f1.PictAndLableWait(false);
            MessageBox.Show("Готово");
        }
        List<JSONArray> MainWords = new List<JSONArray>();
        List<string> FindedWords = new List<string>();
        List<JSONArray> Pomets = new List<JSONArray>();
        List<string> FindedPomets = new List<string>();
        List<JSONArray> Descriptions = new List<JSONArray>();
        List<string> FindedDescriptions = new List<string>();
        void FillMainWordsList()
        {
            MainWords.Clear();
            Pomets.Clear();
            Descriptions.Clear();
            FindedWords.Clear();
            FindedPomets.Clear();
            FindedDescriptions.Clear();
            string query = "SELECT NAME FROM dictionaryentries";
            JSON.Send(query, JSONFlags.Select);
            MainWords = JSON.Decode();
            query = "SELECT POMET FROM dictionaryentries";
            JSON.Send(query, JSONFlags.Select);
            Pomets = JSON.Decode();
            query = "SELECT DEFINITION FROM dictionaryentries";
            JSON.Send(query, JSONFlags.Select);
            Descriptions = JSON.Decode();
            for (int i = 0; i < MainWords.Count; i++)
            {
                if(MainWords[i].Name.Contains(tbWordSearch_SearchingWord.Text))
                {
                    FindedWords.Add(MainWords[i].Name);
                    FindedPomets.Add(Pomets[i].Pomet);
                    FindedDescriptions.Add(Descriptions[i].Definition);
                }
            }
            MainWords.Clear();
            Pomets.Clear();
            Descriptions.Clear();
        }
        void ShowResults()
        {
            tbWordSearch_FindedWords.Text = "";
            if (FindedWords.Count != 0)
            {
                for (int i = 0; i < FindedWords.Count; i++)
                {
                    tbWordSearch_FindedWords.Text += FindedWords[i] + " | " + FindedPomets[i] + " | " + FindedDescriptions[i] + "\r\n";
                }
            }
        }
        private void buWordSearch_FindWord_Click(object sender, EventArgs e)
        {
            FillMainWordsList();
            ShowResults();
        }
    }
}
