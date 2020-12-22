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
                if (line.Contains("<table"))
                {
                    table = true;
                }
                if (line.Contains("</table>"))
                {
                    table = false;
                }
                if (line.Contains("<p") && !table)
                {
                    read = true;
                    if (line.Contains("<b>"))
                    {
                        mainWord = true;
                    }
                }
                if (read)
                {
                    tmp += line;
                    //dictionaryEntry += line;
                }
                if (line.Contains("</p") && !table)
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
                            dictionaryEntry += tmp;
                            tmp = "";
                        }
                    }
                    mainWord = false;
                    read = false;
                }
                if (!read && dictionaryEntry != "" && newEntry)
                {
                    //globCount++;
                    //tbText.Text += dictionaryEntry + "\r\n" + "\r\n";
                    DictionaryEntryDivide(dictionaryEntry);
                    dictionaryEntry = tmp;
                    //DictionaryEntryDivide(tmp);
                    //dictionaryEntry = "";
                    tmp = "";
                    newEntry = false;
                }
                //count++;
                //if (count > 1500)
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
                    int tmp = endOfPometIndex - (finishIndex + 5);
                    if (tmp < 0) tmp = 0;
                    pomet = s.Substring(finishIndex + 5, tmp); // Вывод помет
                    pomet = ClearTags(pomet);
                    tmp = s.Length - (endOfPometIndex + 5);
                    if (tmp < 0) tmp = 0;
                    description = s.Substring(endOfPometIndex + 4, tmp + 1); // Вывод описания
                    description = ClearTags(description);
                }
                if (endOfPometIndex == 0)
                {
                    description = s.Substring(finishIndex, s.Length - finishIndex);
                    description = ClearTags(description);
                }
                name = name.Replace("'", "");
                pomet = pomet.Replace("'", "");
                description = description.Replace("'", "");
                /*name = name.Replace("#", "");
                pomet = pomet.Replace("#", "");
                description = description.Replace("#", "");
                name = name.Replace("&lt;", "");
                pomet = pomet.Replace("&lt;", "");
                description = description.Replace("&lt;", "");
                name = name.Replace("&gt;", "");
                pomet = pomet.Replace("&gt;", "");
                description = description.Replace("&gt;", "");*/
                /*tbWordSearch_Text.Text += name + "---";
                tbWordSearch_Text.Text += pomet + "---";
                tbWordSearch_Text.Text += description + "\r\n";*/
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
            ReadingHTM();
            MessageBox.Show("Готово");
        }
        List<JSONArray> MainWords = new List<JSONArray>();
        List<string> FindedWords = new List<string>();
        void FillMainWordsList()
        {
            MainWords.Clear();
            FindedWords.Clear();
            string query = "SELECT NAME FROM dictionaryentries";
            JSON.Send(query, JSONFlags.Select);
            MainWords = JSON.Decode();
            for(int i = 0; i < MainWords.Count; i++)
            {
                if(MainWords[i].Name.Contains(tbWordSearch_SearchingWord.Text))
                {
                    FindedWords.Add(MainWords[i].Name);
                }
            }
        }
        void ShowResults()
        {
            tbWordSearch_FindedWords.Text = "";
            if (FindedWords.Count != 0)
            {
                string query;
                for (int i = 0; i < FindedWords.Count; i++)
                {
                    MainWords.Clear();
                    query = "SELECT * FROM dictionaryentries WHERE NAME = '" + FindedWords[i] + "'";
                    JSON.Send(System.Net.WebUtility.UrlEncode(query), JSONFlags.Select);
                    MainWords = JSON.Decode();
                    for (int j = 0; j < MainWords.Count; j++)
                    {
                        tbWordSearch_FindedWords.Text += MainWords[j].Name + " | " + MainWords[j].Pomet + " | " + MainWords[j].Definition + "\r\n";
                    }
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
