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
            StringReader sr = new StringReader(Properties.Resources.tom2);
            string line;
            bool next = true; // Переключатель завершения считывания
            int count = 0; // Количество строк для считывания
            int globCount = 0; // Номера словарных статей
            bool read = false; // Переключатель считывания статей
            string dictionaryEntry = "";
            while ((line = sr.ReadLine()) != null && next)
            {
                if (line.Contains("<p"))
                {
                    read = true;
                }
                if (line.Contains("<b>"))
                {
                    globCount++;
                    DictionaryEntryDivide(dictionaryEntry);
                    dictionaryEntry = "";
                }
                if (read)
                {
                    dictionaryEntry += line;
                }
                if (line.Contains("</p"))
                {
                    read = false;
                }
                count++;
                if (count > 1500)
                {
                    next = false;
                }
            }
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
            while (forExit && i < s.Length - 4) // Поиск конечного индекса
            {
                if (s.Substring(i, 4) == "</b>")
                {
                    finishIndex = i - 1;
                    forExit = false;
                }
                i++;
            }
            if (finishIndex != 0 && finishIndex > startIndex) // Вывод заголовочного слова
            {
                name = s.Substring(startIndex, finishIndex - startIndex) + "\r\n";
                name = ClearTags(name);
                tbWordSearch_Text.Text += name;
                forExit = true;
                i = finishIndex;
                int endOfPometIndex = 0;
                while (forExit && i < s.Length - 4) // Поиск помет
                {
                    if (s.Substring(i, 4) == "</i>")
                    {
                        endOfPometIndex = i - 1;
                        forExit = false;
                    }
                    i++;
                }
                if (finishIndex < endOfPometIndex)
                {
                    pomet = s.Substring(finishIndex + 5, endOfPometIndex - (finishIndex + 5)) + "\r\n"; // Вывод помет
                    pomet = ClearTags(pomet);
                    tbWordSearch_Text.Text += pomet;
                    description = s.Substring(endOfPometIndex + 5, s.Length - (endOfPometIndex + 5)) + "\r\n"; // Вывод описания
                    description = ClearTags(description);
                    tbWordSearch_Text.Text += description;
                }
            }
        }
        string ClearTags(string s) // Очищение тагов
        {
            string outValue = s;

            bool findTag;
            int i;
            int tagBegin;
            int tagEnd;
            string tag;
            while (outValue.Contains("<"))
            {
                tagBegin = -1;
                tagEnd = -1;
                findTag = true;
                i = 0;
                while (findTag && i < outValue.Length)
                {
                    if (outValue[i].ToString() == "<")
                    {
                        tagBegin = i;
                    }
                    if (outValue[i].ToString() == ">")
                    {
                        tagEnd = i + 1;
                        findTag = false;
                    }
                    i++;
                }
                if (tagBegin != -1 /*&& tagEnd != -1*/)
                {
                    if (tagEnd == -1) tagEnd = outValue.Length - 1;
                    tag = outValue.Substring(tagBegin, tagEnd - tagBegin);
                    outValue = outValue.Replace(tag, "");
                }

            }
            return outValue;
        }

        private void buWordSearch_Read_Click(object sender, EventArgs e)
        {
            ReadingHTM();
        }
    }
}
