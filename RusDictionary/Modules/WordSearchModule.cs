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
        List<string> FileName = new List<string>();
        StreamReader sr;
        void ReadingHTM()
        {
            string query = "TRUNCATE TABLE dictionaryentries";
            JSON.Send(query, JSONFlags.Truncate);
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
                        bool forExit = true;
                        int i = 0;
                        int start = 0;
                        while (forExit && i <= line.Length - 3)
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
                        if (max > 1 || (nam.Length == 1 && max > 0))
                        {
                            mainWord = true;
                        }
                    }
                }
                if (read)
                {
                    tmp += " " + line;
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
                if (!read && dictionaryEntry != "" && newEntry)
                {
                    //globCount++;
                    DictionaryEntryDivide(dictionaryEntry);
                    dictionaryEntry = tmp;
                    tmp = "";
                    newEntry = false;
                }
                //count++;
                //if (count > 10000)
                //{
                //    next = false;
                //}
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
            string partOfSpeech = "";
            string rod = "";
            string num = "";
            string definition = "";

            while (forExit && i <= s.Length - 1/*- 3*/) // Поиск начального индекса
            {
                if (s.Substring(i, /*3*/1) == ">"/*"<b>"*/)
                {
                    startIndex = i + 1/*3*/;
                    forExit = false;
                }
                i++;
            }
            forExit = true;
            i = 0;
            while (forExit && i <= s.Length - 5) // Поиск конечного индекса
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
                while (forExit && i <= s.Length - 5) // Поиск конечного индекса
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
                while (forExit && i <= s.Length - 5) // Поиск конечного индекса
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
                while (forExit && i <= s.Length - 5) // Поиск конечного индекса
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
                i = 0;
                string tmpName = "";
                while (forExit && i < name.Length) // Поиск конечного индекса
                {
                    string ss = name[i].ToString();
                    if (!ss.Any(char.IsUpper))
                    {
                        tmpName = name.Substring(i);
                    }
                    else
                    {
                        forExit = false;
                    }
                    i++;
                }
                if (tmpName != "")
                {
                    name = tmpName;
                }
                forExit = true;
                i = finishIndex;
                int endOfPometIndex = 0;
                while (forExit && i <= s.Length - 4) // Поиск помет
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
                    tmp = s.Length - (endOfPometIndex + 4);
                    if (tmp < 0) tmp = 0;
                    description = s.Substring(endOfPometIndex + 4, tmp); // Вывод описания
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
                name = name.Replace(" ", " ");
                pomet = pomet.Replace(" ", " ");
                description = description.Replace(" ", " ");






                // перенос см. в описание
                if (name.Contains("см. "))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    while (forExit && i <= name.Length - 4)
                    {
                        if (name.Substring(i, 4) == "см. ")
                        {
                            st = i;
                            forExit = false;
                        }
                        i++;
                    }
                    definition = name.Substring(st, name.Length - st) + pomet;
                    name = name.Substring(0, st);
                }
                forExit = true;
                i = 0;
                while (forExit && i < name.Length) // Поиск конечного индекса
                {
                    string ss = name[i].ToString();
                    if (!ss.Any(char.IsUpper) && ss != "[" && ss != "]" && ss != "#" && ss != "(" &&
                        ss != ")" && ss != "," && ss != "." && ss != " " && ss != "-" && ss != "1" &&
                        ss != "2" && ss != "3" && ss != "4" && ss != "5" && ss != "6" && ss != "7" &&
                        ss != "8" && ss != "9" && ss != "0" && ss != "?" && ss != "!")
                    {
                        forExit = false;
                        name = name.Substring(0, i);
                        pomet = name.Substring(i) + pomet;
                    }
                    i++;
                }
                // перенос прил. к (слово) в пометы
                if (pomet.Contains("прил. к"))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") || description.Contains(" зная.") || description.Contains(" знал.") ||
                        description.Contains(" зкач.") || description.Contains(" энач."))
                    {
                        zn = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && zn)
                        {
                            zn = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос Действие по глаг. (слово) в пометы
                if (pomet.Contains("Действие по глаг."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") || description.Contains(" зная.") || description.Contains(" знал.") ||
                        description.Contains(" зкач.") || description.Contains(" энач."))
                    {
                        zn = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && zn)
                        {
                            zn = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос многокр. (слово) в пометы
                if (pomet.Contains("многокр."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") || description.Contains(" зная.") || description.Contains(" знал.") ||
                        description.Contains(" зкач.") || description.Contains(" энач."))
                    {
                        zn = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && zn)
                        {
                            zn = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос То же, что (слово) в пометы
                if (pomet.Contains("То же, что") || pomet.Contains("То ме, что"))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") || description.Contains(" зная.") || description.Contains(" знал.") ||
                        description.Contains(" зкач.") || description.Contains(" энач."))
                    {
                        zn = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && zn)
                        {
                            zn = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос закртыие скобки в пометы
                if (pomet.Contains("(") && !pomet.Contains(")"))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.')
                        {
                            st = i + 1;
                            forExit = false;
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос уменъш. к страд, к несов. к (слово) в пометы
                if (pomet.Contains("уменъш. к") || pomet.Contains("Уменъш. к") || pomet.Contains("уменъш.-ласкат. к") ||
                    pomet.Contains("уменьш к") || pomet.Contains("несое.") || pomet.Contains(" Лесов, к") ||
                    pomet.Contains("несов. к") ||
                    pomet.Contains("Страд, к") || pomet.Contains("страд, к") || pomet.Contains(" страД.") ||
                    pomet.Contains(" уничиж."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") || description.Contains(" зная.") || description.Contains(" знал.") ||
                        description.Contains(" зкач.") || description.Contains(" энач."))
                    {
                        zn = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && zn)
                        {
                            zn = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // Убираем пробелы в начале description
                if (description != "" && description.Length > 0)
                {
                    forExit = true;
                    while (forExit)
                    {
                        if (description.Length == 0)
                        {
                            forExit = false;
                        }
                        else
                        {
                            if (description[0] == ' ')
                            {
                                description = description.Substring(1);
                            }
                            else
                            {
                                forExit = false;
                            }
                        }
                    }
                }
                // очистка от пробелов пометы в конце
                if (pomet != "" && pomet.Length > 1)
                {
                    forExit = true;
                    while (forExit)
                    {
                        if (pomet.Length == 0)
                        {
                            forExit = false;
                        }
                        else
                        {
                            if (pomet[pomet.Length - 1] == ' ')
                            {
                                pomet = pomet.Substring(0, pomet.Length - 1);
                            }
                            else
                            {
                                forExit = false;
                            }
                        }
                    }
                }
                // перенос первого предложения (слово) в пустые пометы
                if (pomet == "" && description != "")
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool nm = false;
                    if (description[0] == '1' || description[0] == '2' || description[0] == '3')
                    {
                        nm = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && nm)
                        {
                            nm = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }

                // преренос рода и номеров в пометы
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
                // Очистка заголовочного слова от мусора
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
                string tmpStr;
                // Определение рода
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 2)
                {
                    if (pomet.Substring(i, 2) == "с.")
                    {
                        rod = "с.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 2 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 2);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    else
                    {
                        if (pomet.Substring(i, 2) == "ж.")
                        {
                            rod = "ж.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 2 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 2);
                            }
                            pomet = tmpStr;
                            forExit = false;
                        }
                        else
                        {
                            if (pomet.Substring(i, 2) == "м.")
                            {
                                rod = "м.";
                                tmpStr = pomet.Substring(0, i);
                                if (i + 2 < pomet.Length)
                                {
                                    tmpStr += pomet.Substring(i + 2);
                                }
                                pomet = tmpStr;
                                forExit = false;
                            }
                        }
                    }
                    i++;
                }
                // Определение числа
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 3) // мн.
                {
                    if (pomet.Substring(i, 3) == "мн.")
                    {
                        num = "мн.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 3 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 3);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                // Определение части речи
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 5) // прил.
                {
                    if (pomet.Substring(i, 5) == "прил.")
                    {
                        partOfSpeech = "прил.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 5 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 5);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    else
                    {
                        if (pomet.Substring(i, 5) == "лрил.")
                        {
                            partOfSpeech = "прил.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 5 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 5);
                            }
                            pomet = tmpStr;
                            forExit = false;
                        }
                        else
                        {
                            if (pomet.Substring(i, 5) == "нрил.")
                            {
                                partOfSpeech = "прил.";
                                tmpStr = pomet.Substring(0, i);
                                if (i + 5 < pomet.Length)
                                {
                                    tmpStr += pomet.Substring(i + 5);
                                }
                                pomet = tmpStr;
                                forExit = false;
                            }
                            else
                            {
                                if (pomet.Substring(i, 5) == "прич.")
                                {
                                    partOfSpeech = "прич.";
                                    tmpStr = pomet.Substring(0, i);
                                    if (i + 5 < pomet.Length)
                                    {
                                        tmpStr += pomet.Substring(i + 5);
                                    }
                                    pomet = tmpStr;
                                    forExit = false;
                                }
                            }
                        }
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 6) // нареч.
                {
                    if (pomet.Substring(i, 6) == "нареч.")
                    {
                        partOfSpeech = "нареч.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 6 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 6);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    else
                    {
                        if (pomet.Substring(i, 6) == "нвреч.")
                        {
                            partOfSpeech = "нареч.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                        }
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 7) // предлог.
                {
                    if (pomet.Substring(i, 7) == "предлог")
                    {
                        partOfSpeech = "предлог";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 7 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 7);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 5) // мест.
                {
                    if (pomet.Substring(i, 5) == "мест.")
                    {
                        partOfSpeech = "мест.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 5 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 5);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 5) // союз.
                {
                    if (pomet.Substring(i, 5) == "союз.")
                    {
                        partOfSpeech = "союз";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 5 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 5);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 8) // частица.
                {
                    if (pomet.Substring(i, 8) == "частица.")
                    {
                        partOfSpeech = "частица";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 5 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 8);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 5) // межд.
                {
                    if (pomet.Substring(i, 5) == "межд.")
                    {
                        partOfSpeech = "межд.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 5 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 5);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                // очистка от пробелов пометы в конце
                if (pomet != "" && pomet.Length > 0)
                {
                    forExit = true;
                    while (forExit)
                    {
                        if (pomet.Length == 0)
                        {
                            pomet = "";
                            forExit = false;
                        }
                        else
                        {
                            if (pomet[pomet.Length - 1] == ' ')
                            {
                                pomet = pomet.Substring(0, pomet.Length - 1);
                            }
                            else
                            {
                                forExit = false;
                            }
                        }
                    }
                }
                // перенос первого предложения (слово) в пустые пометы еще раз
                if (pomet == "" && description != "")
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool nm = false;
                    if (description[0] == '1' || description[0] == '2' || description[0] == '3')
                    {
                        nm = true;
                    }
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.' && nm)
                        {
                            nm = false;
                        }
                        else
                        {
                            if (description[i] == '.')
                            {
                                st = i + 1;
                                forExit = false;
                            }
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос первого предложения (слово) в пометы после спец символов точнее если нет точки
                if (pomet != "" && description != "")
                {
                    if (pomet.Last<char>() != '.'/*pomet.Last<char>() == ',' || pomet.Last<char>() == ';' || pomet.Last<char>() == ':'*/)
                    {
                        forExit = true;
                        i = 0;
                        int st = 0;
                        bool nm = false;
                        if (description[0] == '1' || description[0] == '2' || description[0] == '3' ||
                            description[0] == '4' || description[0] == '5' || description[0] == '6' ||
                            description[0] == '7' || description[0] == '8' || description[0] == '9')
                        {
                            nm = true;
                        }
                        while (forExit && i <= description.Length - 1)
                        {
                            if (description[i] == '.' && nm)
                            {
                                nm = false;
                            }
                            else
                            {
                                if (description[i] == '.')
                                {
                                    st = i + 1;
                                    forExit = false;
                                }
                            }
                            i++;
                        }
                        pomet += description.Substring(0, st);
                        pomet = ClearTags(pomet);
                        description = description.Substring(st);
                    }
                }
                // перенос закртыие скобки в пометы еще раз
                if /*while */(pomet.Contains("(") && !pomet.Contains(")"))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.')
                        {
                            st = i + 1;
                            forExit = false;
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // добавление части предложения после чего-л. и т.д.
                if (pomet.Contains("что-л.") || pomet.Contains("чего-л.") || pomet.Contains(" перен.") ||
                    pomet.Contains("Тот кто"))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    while (forExit && i <= description.Length - 1)
                    {
                        if (description[i] == '.')
                        {
                            st = i + 1;
                            forExit = false;
                        }
                        i++;
                    }
                    pomet += description.Substring(0, st);
                    pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }



                //string tmpStr;
                // Определение рода
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 3)
                {
                    if (pomet.Substring(i, 3) == " с.")
                    {
                        rod = "с.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 3 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 3);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    else
                    {
                        if (pomet.Substring(i, 3) == " ж.")
                        {
                            rod = "ж.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                        }
                        else
                        {
                            if (pomet.Substring(i, 3) == " м.")
                            {
                                rod = "м.";
                                tmpStr = pomet.Substring(0, i);
                                if (i + 3 < pomet.Length)
                                {
                                    tmpStr += pomet.Substring(i + 3);
                                }
                                pomet = tmpStr;
                                forExit = false;
                            }
                        }
                    }
                    i++;
                }
                // Определение числа
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 4) // мн.
                {
                    if (pomet.Substring(i, 4) == " мн.")
                    {
                        num = "мн.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 4 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 4);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                // Определение части речи
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 6) // прил.
                {
                    if (pomet.Substring(i, 5) == " прил.")
                    {
                        partOfSpeech = "прил.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 6 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 6);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    else
                    {
                        if (pomet.Substring(i, 6) == " лрил.")
                        {
                            partOfSpeech = "прил.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                        }
                        else
                        {
                            if (pomet.Substring(i, 6) == " нрил.")
                            {
                                partOfSpeech = "прил.";
                                tmpStr = pomet.Substring(0, i);
                                if (i + 6 < pomet.Length)
                                {
                                    tmpStr += pomet.Substring(i + 6);
                                }
                                pomet = tmpStr;
                                forExit = false;
                            }
                            else
                            {
                                if (pomet.Substring(i, 6) == " прич.")
                                {
                                    partOfSpeech = "прич.";
                                    tmpStr = pomet.Substring(0, i);
                                    if (i + 6 < pomet.Length)
                                    {
                                        tmpStr += pomet.Substring(i + 6);
                                    }
                                    pomet = tmpStr;
                                    forExit = false;
                                }
                            }
                        }
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 7) // нареч.
                {
                    if (pomet.Substring(i, 7) == " нареч.")
                    {
                        partOfSpeech = "нареч.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 7 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 7);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    else
                    {
                        if (pomet.Substring(i, 7) == " нвреч.")
                        {
                            partOfSpeech = "нареч.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 7 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 7);
                            }
                            pomet = tmpStr;
                            forExit = false;
                        }
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 8) // предлог.
                {
                    if (pomet.Substring(i, 8) == " предлог")
                    {
                        partOfSpeech = "предлог";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 8 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 8);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 6) // мест.
                {
                    if (pomet.Substring(i, 6) == " мест.")
                    {
                        partOfSpeech = "мест.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 6 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 6);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 6) // союз.
                {
                    if (pomet.Substring(i, 6) == " союз.")
                    {
                        partOfSpeech = "союз";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 6 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 6);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 9) // частица.
                {
                    if (pomet.Substring(i, 9) == " частица.")
                    {
                        partOfSpeech = "частица";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 5 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 9);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 6) // межд.
                {
                    if (pomet.Substring(i, 6) == " межд.")
                    {
                        partOfSpeech = "межд.";
                        tmpStr = pomet.Substring(0, i);
                        if (i + 6 < pomet.Length)
                        {
                            tmpStr += pomet.Substring(i + 6);
                        }
                        pomet = tmpStr;
                        forExit = false;
                    }
                    i++;
                }


                // Заполнение описания
                if (definition == "")
                {
                    definition = pomet;
                }
                // Очистка в начале defenition
                if (definition != "" && definition.Length > 1)
                {
                    forExit = true;
                    while (forExit)
                    {
                        if (definition.Length == 0)
                        {
                            forExit = false;
                        }
                        else
                        {
                            if (definition[0] == ' ' || definition[0] == ',')
                            {
                                definition = definition.Substring(1);
                            }
                            else
                            {
                                forExit = false;
                            }
                        }
                    }
                }
                //description = ClearTags(description);
                List<string> tmpEXMP = new List<string>(); // Заполнение цитат
                List<string> EXMP = new List<string>(); // Соединение разорваных частей цитат
                string[] defs = new string[1];
                if (description != "")
                {
                    description = description.Replace(" г.", " г./");
                    description = description.Replace(" в.", " в./");
                    description = description.Replace(" вв.", " вв./");
                    string[] separatingStrings = { "/" };
                    tmpEXMP.AddRange(description.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries));
                    for (int j = 0; j < tmpEXMP.Count; j++)
                    {
                        // Очистка в начале example
                        if (tmpEXMP[j] != "" && tmpEXMP[j].Length > 1)
                        {
                            forExit = true;
                            while (forExit)
                            {
                                if (tmpEXMP[j].Length == 0)
                                {
                                    forExit = false;
                                }
                                else
                                {
                                    if (tmpEXMP[j][0] == ' ')
                                    {
                                        tmpEXMP[j] = tmpEXMP[j].Substring(1);
                                    }
                                    else
                                    {
                                        forExit = false;
                                    }
                                }
                            }
                        }
                    }
                    EXMP.Add(tmpEXMP[0]);
                    for (int j = 1; j < tmpEXMP.Count; j++)
                    {
                        if (tmpEXMP[j].Length > 1)
                        {
                            if (tmpEXMP[j].Substring(0, 1) == "~" || tmpEXMP[j].Substring(0, 2) == "со" ||
                                tmpEXMP[j].Substring(0, 1) == "X" || tmpEXMP[j].Substring(0, 1) == "I" ||
                                tmpEXMP[j].Substring(0, 1) == "V")
                            {
                                EXMP[EXMP.Count - 1] += " " + tmpEXMP[j];
                            }
                            else
                            {
                                EXMP.Add(tmpEXMP[j]);
                            }
                        }
                    }
                    defs = new string[EXMP.Count]; // Заполнение описаний для цитат
                    defs[0] = definition;
                    string subTmp;
                    for (int j = 1; j < defs.Length; j++)
                    {
                        defs[j] = "";
                        int st;
                        if (EXMP[j].Length > 2)
                        {
                            subTmp = EXMP[j].Substring(0, 3);
                            if (subTmp == "1. " || subTmp == "2. " || subTmp == "3. " || subTmp == "4. " || subTmp == "5. " ||
                                subTmp == "6. " || subTmp == "7. " || subTmp == "8. " || subTmp == "9. ")
                            {
                                forExit = true;
                                i = 2;
                                st = 0;
                                while (forExit && i <= EXMP[j].Length - 1)
                                {
                                    if (EXMP[j][i] == '.')
                                    {
                                        st = i + 1;
                                        forExit = false;
                                    }
                                    i++;
                                }
                                defs[j] = EXMP[j].Substring(0, st);
                                EXMP[j] = EXMP[j].Substring(st);
                                if (defs[j].Contains(" перен."))
                                {
                                    forExit = true;
                                    i = 2;
                                    st = 0;
                                    while (forExit && i <= EXMP[j].Length - 1)
                                    {
                                        if (EXMP[j][i] == '.')
                                        {
                                            st = i + 1;
                                            forExit = false;
                                        }
                                        i++;
                                    }
                                    defs[j] += EXMP[j].Substring(0, st);
                                    EXMP[j] = EXMP[j].Substring(st);
                                }
                            }
                            if (EXMP[j].Contains(" перен."))
                            {
                                forExit = true;
                                i = 0;
                                st = 0;
                                while (forExit && i <= EXMP[j].Length - 1)
                                {
                                    if (EXMP[j][i] == '.')
                                    {
                                        st = i + 1;
                                        forExit = false;
                                    }
                                    i++;
                                }
                                defs[j] += EXMP[j].Substring(0, st);
                                EXMP[j] = EXMP[j].Substring(st);
                            }
                            if (EXMP[j].Contains("— Ср."))
                            {
                                defs[j] += EXMP[j].Substring(0);
                                EXMP[j] = "";
                            }
                            if (EXMP[j].Contains(" знач."))
                            {
                                forExit = true;
                                i = 0;
                                st = 0;
                                bool zn = true;
                                //if (description.Contains(" знач.") || description.Contains(" зная.") || description.Contains(" знал.") ||
                                //    description.Contains(" зкач.") || description.Contains(" энач."))
                                //{
                                //    zn = true;
                                //}
                                while (forExit && i <= EXMP[j].Length - 1)
                                {
                                    if (EXMP[j][i] == '.' && zn)
                                    {
                                        zn = false;
                                    }
                                    else
                                    {
                                        if (EXMP[j][i] == '.')
                                        {
                                            st = i + 1;
                                            forExit = false;
                                        }
                                    }
                                    i++;
                                }
                                defs[j] += EXMP[j].Substring(0, st);
                                EXMP[j] = EXMP[j].Substring(st);
                            }
                        }
                    }
                }
                else
                {
                    EXMP.Add("");
                    defs[0] = definition;
                }
                AddBD(name, partOfSpeech, rod, num, defs, EXMP);
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
            outValue = outValue.Replace("&nbsp;", "");
            outValue = outValue.Replace("&lt;", "<");
            outValue = outValue.Replace("&gt;", ">");
            outValue = outValue.Replace("&quot;", "\"");
            return outValue;
        }
        void AddBD(string nam, string par, string rod, string num, string[] def, List<string> exm)
        {
            string query = "INSERT INTO dictionaryentries (NAME, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE) VALUES ('" +
                    nam + "', '" + par + "', '" + rod + "', '" + num + "', '" + def[0] + "', '" + exm[0] + "')";
            // объект для выполнения SQL-запроса
            JSON.Send(query, JSONFlags.Insert);
            if (exm.Count > 1)
            {
                for (int i = 1; i < exm.Count; i++)
                {
                    query = "INSERT INTO dictionaryentries (NAME, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE) VALUES ('" +
                    nam + "', '', '', '', '" + def[i] + "', '" + exm[i] + "')";
                    JSON.Send(query, JSONFlags.Insert);
                }
            }
        }
        private void buWordSearch_Read_Click(object sender, EventArgs e)
        {
            FileName.Clear();
            //string query = "INSERT INTO dictionaryentries (NAME, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE) VALUES ('1', '1', '1', '1', '1', '1')";
            //JSON.Send(query, JSONFlags.Insert);
            OpenFileDialog OPF = new OpenFileDialog(); // Инициализация диалогового окна
            OPF.Filter = "HTM|*.htm"; // Фильтр в диалоговом окне
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                FileName.Add(OPF.FileName); // Добавление в массив пути картинки
            }
            sr = new StreamReader(FileName[0], Encoding.Default);
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
        void FindWords()
        {
            List<string> Names = new List<string>();
            List<string> PartsOfSpeech = new List<string>();
            List<string> Rods = new List<string>();
            List<string> Nums = new List<string>();
            List<string> Definitions = new List<string>();
            List<string> Examples = new List<string>();
            List<JSONArray> jNames = new List<JSONArray>();

            tbWordSearch_FindedWords.Text = "";

            string query = "SELECT * FROM dictionaryentries";
            JSON.Send(query, JSONFlags.Select);
            jNames = JSON.Decode();
            for (int i = 0; i < jNames.Count; i++)
            {
                Names.Add(jNames[i].Name);
                PartsOfSpeech.Add(jNames[i].Partofspeech);
                Rods.Add(jNames[i].Rod);
                Nums.Add(jNames[i].Num);
                Definitions.Add(jNames[i].Definition);
                Examples.Add(jNames[i].Example);
            }

            string tmp = "";
            bool newWord = false;
            for (int i = 0; i < Names.Count; i++)
            {
                if (Names[i].Contains(tbWordSearch_SearchingWord.Text))
                {
                    if (tmp != Names[i])
                    {
                        tmp = Names[i];
                        newWord = true;
                    }
                    if (newWord)
                    {
                        tbWordSearch_FindedWords.Text += "_____\r\n" + Names[i] + "\r\n";
                        if (PartsOfSpeech[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += PartsOfSpeech[i] + "\r\n";
                        }
                        if (Rods[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += Rods[i] + "\r\n";
                        }
                        if (Nums[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += Nums[i] + "\r\n";
                        }
                        if (Definitions[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += Definitions[i] + "\r\n";
                        }
                        if (Examples[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += Examples[i] + "\r\n";
                        }
                        newWord = false;
                    }
                    else
                    {
                        if (Definitions[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += Definitions[i] + "\r\n";
                        }
                        if (Examples[i] != "")
                        {
                            tbWordSearch_FindedWords.Text += Examples[i] + "\r\n";
                        }
                    }
                }
            }
        }
        private void buWordSearch_FindWord_Click(object sender, EventArgs e)
        {
            FindWords();
        }
    }
}
