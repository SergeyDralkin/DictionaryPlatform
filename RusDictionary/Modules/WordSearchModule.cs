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
        int mainWordNum;
        List<string> Ukaz;
        void ReadingHTM()
        {
            string line;
            //bool next = true; // Переключатель завершения считывания
            //int count = 0; // Количество строк для считывания
            //int globCount = 0; // Номера словарных статей
            bool read = false; // Переключатель считывания статей
            string dictionaryEntry = "";
            string tmp = "";
            bool newEntry = true;
            bool mainWord = false;
            bool table = false;

            List<JSONArray> jNames = new List<JSONArray>(); // подсчет кол-ва статей в БД
            string query = "SELECT * FROM mainwords";
            JSON.Send(query, JSONFlags.Select);
            jNames = JSON.Decode();
            if (jNames != null)
            {
                mainWordNum = jNames.Count + 1;
                jNames.Clear();
            }
            else
            {
                mainWordNum = 1;
            }

            Ukaz = new List<string>();
            jNames = new List<JSONArray>();
            query = "SELECT * FROM ukaz";
            JSON.Send(query, JSONFlags.Select);
            jNames = JSON.Decode();
            if (jNames != null)
            {
                for (int i = 0; i < jNames.Count; i++)
                {
                    Ukaz.Add(jNames[i].Name);
                }
            }

            while ((line = sr.ReadLine()) != null /*&& next*/)
            {
                if (line.Contains("<table"))
                {
                    table = true;
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
                        forExit = true;
                        i = 0;
                        int end = 0;
                        while (forExit && i <= line.Length - 4)
                        {
                            if (line.Substring(i, 4) == "</b>")
                            {
                                end = i;
                                forExit = false;
                            }
                            i++;
                        }
                        if (end > start)
                        {
                            string nam = line.Substring(start, end - start);
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
                }
                if (read)
                {
                    tmp += " " + line;
                }
                if (line.Contains("</p>") && !table)
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
                if (line.Contains("</table>"))
                {
                    table = false;
                }
                //count++;
                //if (count > 7000)
                //{
                //    next = false;
                //}
            }
            Ukaz.Clear();
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
            string semant = "";
            string pomet = "";
            string description = "";
            string partOfSpeech = "";
            string rod = "";
            string num = "";
            string definition = "";
            string sr = "";

            // приведение спеиальных слов к нормальной форме
            s = s.Replace(" зная.", " знач.");
            s = s.Replace(" знал.", " знач.");
            s = s.Replace(" зкач.", " знач.");
            s = s.Replace(" энач.", " знач.");
            s = s.Replace(" анач.", " знач.");
            s = s.Replace("уменъш. к", "уменьш. к");
            s = s.Replace("Уменъш. к", "уменьш. к");
            s = s.Replace("уменьш к", "уменьш. к");
            s = s.Replace("уменъш.-ласкат. к", "уменьш.-ласкат. к");
            s = s.Replace("несое.", "несов. к");
            s = s.Replace("Лесов, к", "несов. к");
            s = s.Replace("Страд, к", "страд. к");
            s = s.Replace("страд, к", "страд. к");
            s = s.Replace(" страД.", "страд. к");
            s = s.Replace(" страд.", "страд. к");
            s = s.Replace(" лрил.", " прил.");
            s = s.Replace(" нрил.", " прил.");
            s = s.Replace(" gрил.", " прил.");
            s = s.Replace(" нвреч.", " нареч.");
            s = s.Replace("То ме, что", "То же, что");

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
            while (forExit && i <= s.Length - 5) // Поиск конечного индекса (с зяпятой)
            {
                if (s.Substring(i, 5) == ",</b>")
                {
                    finishIndex = i;
                    forExit = false;
                }
                i++;
            }
            if (forExit) // если не найдено в предыдущем цикле
            {
                forExit = true;
                i = 0;
                while (forExit && i <= s.Length - 5) // Поиск конечного индекса (с точкой)
                {
                    if (s.Substring(i, 5) == ".</b>")
                    {
                        finishIndex = i + 1;
                        forExit = false;
                    }
                    i++;
                }
            }
            if (forExit) // если не найдено в предыдущих циклах
            {
                forExit = true;
                i = 0;
                while (forExit && i <= s.Length - 5) // Поиск конечного индекса (без точки, запятой и хештега)
                {
                    if (s.Substring(i, 4) == "</b>" && s.Substring(i, 5) != "</b>#")
                    {
                        finishIndex = i;
                        forExit = false;
                    }
                    i++;
                }
            }
            else // если найдено в предыдущих циклах
            {
                forExit = true;
                i = 0;
                while (forExit && i <= s.Length - 5) // Поиск конечного индекса (без точки, запятой и хештега)
                {
                    if (s.Substring(i, 4) == "</b>" && i < finishIndex && s.Substring(i, 5) != "</b>#")
                    {
                        finishIndex = i;
                        forExit = false;
                    }
                    i++;
                }
            }
            // для семантики
            int semStartIndex = -1;
            int semEndIndex = -1;
            if (finishIndex != 0 && finishIndex > startIndex) // Вывод заголовочного слова
            {
                name = s.Substring(startIndex, finishIndex - startIndex);

                forExit = true;
                i = 0;
                while (forExit && i <= name.Length - 5) // начало семантики
                {
                    if (name.Substring(i, 5) == "<sup>")
                    {
                        semStartIndex = i + 5;
                        forExit = false;
                    }
                    i++;
                }
                forExit = true;
                i = 0;
                while (forExit && i <= name.Length - 6) // конец семантики
                {
                    if (name.Substring(i, 6) == "</sup>")
                    {
                        semEndIndex = i;
                        forExit = false;
                    }
                    i++;
                }
                if (semStartIndex != -1 && semEndIndex != -1)
                {
                    semant = name.Substring(semStartIndex, semEndIndex - semStartIndex);
                    name = name.Substring(0, semStartIndex - 5) + name.Substring(semEndIndex);
                }
                string[] smt = { "1,", "2,", "3,", "4,", "5," };
                for (int j = 0; j < smt.Length; j++)
                {
                    if (name.Contains(smt[j]))
                    {
                        name = name.Replace(smt[j], "");
                        semant = smt[j];
                        semant = semant.Replace(",", "");
                    }
                }
                semant = ClearTags(semant);
                // очистка заг слова от маленьких букв в начале
                name = ClearTags(name);
                forExit = true;
                i = 0;
                string tmpName = "";
                while (forExit && i < name.Length)
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
                // Поиск помет
                forExit = true;
                i = finishIndex;
                int endOfPometIndex = 0;
                while (forExit && i <= s.Length - 4)
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

                    if (semant == "")
                    {
                        semStartIndex = -1;
                        semEndIndex = -1;
                        forExit = true;
                        i = 0;
                        while (forExit && i <= pomet.Length - 5) // начало семантики
                        {
                            if (pomet.Substring(i, 5) == "<sup>")
                            {
                                semStartIndex = i + 5;
                                forExit = false;
                            }
                            i++;
                        }
                        forExit = true;
                        i = 0;
                        while (forExit && i <= pomet.Length - 6) // конец семантики
                        {
                            if (pomet.Substring(i, 6) == "</sup>")
                            {
                                semEndIndex = i;
                                forExit = false;
                            }
                            i++;
                        }
                        if (semStartIndex != -1 && semEndIndex != -1)
                        {
                            semant = pomet.Substring(semStartIndex, semEndIndex - semStartIndex);
                            pomet = pomet.Substring(0, semStartIndex - 5) + pomet.Substring(semEndIndex);
                        }
                        semant = ClearTags(semant);
                    }
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
                // перенос символов нижнего регистра в поле pomet
                forExit = true;
                i = 0;
                while (forExit && i < name.Length)
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
                    if (description.Contains(" знач.") /*|| description.Contains(" зная.") || description.Contains(" знал.") || description.Contains(" зкач.") || description.Contains(" энач.")*/)
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
                    //pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос Действие по глаг. (слово) в пометы
                if (pomet.Contains("Действие по глаг."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") /*|| description.Contains(" зная.") || description.Contains(" знал.") || description.Contains(" зкач.") || description.Contains(" энач.")*/)
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
                    //pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос многокр. (слово) в пометы
                if (pomet.Contains("многокр."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") /*|| description.Contains(" зная.") || description.Contains(" знал.") || description.Contains(" зкач.") || description.Contains(" энач.")*/)
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
                    pomet = pomet.Replace("многокр.", "");
                    num = "многокр.";
                    //pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос однокр. (слово) в пометы
                if (pomet.Contains("однокр."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") /*|| description.Contains(" зная.") || description.Contains(" знал.") || description.Contains(" зкач.") || description.Contains(" энач.")*/)
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
                    pomet = pomet.Replace("однокр.", "");
                    num = "однокр.";
                    //pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос То же, что (слово) в пометы
                if (pomet.Contains("То же, что") /*|| pomet.Contains("То ме, что")*/)
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") /*|| description.Contains(" зная.") || description.Contains(" знал.") || description.Contains(" зкач.") || description.Contains(" энач.")*/)
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
                    //pomet = ClearTags(pomet);
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
                    //pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // перенос уменъш. к страд, к несов. к (слово) в пометы
                if (pomet.Contains("уменьш. к") || pomet.Contains("уменьш.-ласкат. к") || pomet.Contains("несов. к") || pomet.Contains("страд. к") || pomet.Contains(" уничиж."))
                {
                    forExit = true;
                    i = 0;
                    int st = 0;
                    bool zn = false;
                    if (description.Contains(" знач.") /*|| description.Contains(" зная.") || description.Contains(" знал.") || description.Contains(" зкач.") || description.Contains(" энач.")*/)
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
                    //pomet = ClearTags(pomet);
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
                    //pomet = ClearTags(pomet);
                    description = description.Substring(st);
                }
                // преренос рода и номеров в пометы
                string[] pmt = { "1.", "2.", "3.", "4.", "5.", "м.", "с.", "ж." };
                for (int j = 0; j < pmt.Length; j++)
                {
                    if (name.Contains(pmt[j]))
                    {
                        name = name.Replace(pmt[j], "");
                        if (!pomet.Contains(pmt[j]))
                        {
                            pomet = " " + pmt[j] + " " + pomet;
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
                // Определение рода
                string tmpStr;
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 3)
                {
                    switch (pomet.Substring(i, 3))
                    {
                        case " с.":
                            rod = "с.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " ж.":
                            rod = "ж.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " м.":
                            rod = "м.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                // Определение числа
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 3) // мн.
                {
                    switch (pomet.Substring(i, 3))
                    {
                        case "мн.":
                            num = "мн.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case "дв.":
                            num = "дв.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                // Определение части речи
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 5) // прил. прич.
                {
                    switch (pomet.Substring(i, 5))
                    {
                        case "прил.":
                            partOfSpeech = "прил.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 5 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 5);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case "прич.":
                            partOfSpeech = "прич.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 5 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 5);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case "мест.":
                            partOfSpeech = "мест.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 5 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 5);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case "союз.":
                            partOfSpeech = "союз";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 5 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 5);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case "межд.":
                            partOfSpeech = "межд.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 5 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 5);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
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
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 8) // дееприч.
                {
                    switch (pomet.Substring(i, 8))
                    {
                        case "дееприч.":
                            partOfSpeech = "дееприч.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 8 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 8);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case "частица.":
                            partOfSpeech = "частица";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 8 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 8);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
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
                // перенос первого предложения (слово) в пустые пометы еще раз, если пусто
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
                //ПОВТОРНАЯ ПРОВЕРКА ПОСЛЕ ПЕРЕНОСА
                // Определение рода
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 3)
                {
                    switch (pomet.Substring(i, 3))
                    {
                        case " с.":
                            rod = "с.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " ж.":
                            rod = "ж.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " м.":
                            rod = "м.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 3 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 3);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                // Определение числа
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 4) // мн.
                {
                    switch (pomet.Substring(i, 4))
                    {
                        case " мн.":
                            num = "мн.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 4 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 4);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " дв.":
                            num = "дв.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 4 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 4);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                // Определение части речи
                forExit = true;
                i = 0;
                while (forExit && i <= pomet.Length - 6) // прил.
                {
                    switch (pomet.Substring(i, 6))
                    {
                        case " прил.":
                            partOfSpeech = "прил.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " прич.":
                            partOfSpeech = "прич.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " мест.":
                            partOfSpeech = "мест.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " союз.":
                            partOfSpeech = "союз";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " межд.":
                            partOfSpeech = "межд.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 6 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 6);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
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
                    i++;
                }
                i = 0;
                while (forExit && i <= pomet.Length - 9) // дееприч.
                {
                    switch (pomet.Substring(i, 9))
                    {
                        case " дееприч.":
                            partOfSpeech = "дееприч.";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 9 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 9);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        case " частица.":
                            partOfSpeech = "частица";
                            tmpStr = pomet.Substring(0, i);
                            if (i + 9 < pomet.Length)
                            {
                                tmpStr += pomet.Substring(i + 9);
                            }
                            pomet = tmpStr;
                            forExit = false;
                            break;
                        default:
                            break;
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
                if (definition == "см.")
                {
                    definition += description;
                    description = "";
                }
                List<string> tmpEXMP = new List<string>(); // Заполнение цитат
                List<string> EXMP = new List<string>(); // Соединение разорваных частей цитат
                string[] defs = new string[1];
                int st0;
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
                                tmpEXMP[j].Substring(0, 1) == "V" || tmpEXMP[j].Substring(0, 2) == "оо") // оо
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
                    // проверка на (1555) в цитате (первой)
                    forExit = true;
                    i = 0;
                    st0 = -1;
                    while (forExit && i <= EXMP[0].Length - 6)
                    {
                        if (EXMP[0][i] == '(' && EXMP[0][i + 1].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 2].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 3].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 4].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 5] == ')')
                        {
                            st0 = i;
                            forExit = false;
                        }
                        i++;
                    }
                    if (st0 != -1)
                    {
                        defs[0] = defs[0] + EXMP[0].Substring(0, st0);
                        EXMP[0] = EXMP[0].Substring(st0);
                    }
                    // проверка на (155) в цитате (первой)
                    forExit = true;
                    i = 0;
                    st0 = -1;
                    while (forExit && i <= EXMP[0].Length - 5)
                    {
                        if (EXMP[0][i] == '(' && EXMP[0][i + 1].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 2].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 3].ToString().Any(char.IsDigit) &&
                            EXMP[0][i + 4] == ')')
                        {
                            st0 = i;
                            forExit = false;
                        }
                        i++;
                    }
                    if (st0 != -1)
                    {
                        defs[0] = defs[0] + EXMP[0].Substring(0, st0);
                        EXMP[0] = EXMP[0].Substring(st0);
                    }

                    string subTmp;
                    int cnt = 1;
                    for (int j = 1; j < defs.Length; j++)
                    {
                        defs[j] = "";
                        int st;
                        if (EXMP[j].Length > 2)
                        {
                            subTmp = EXMP[j].Substring(0, 3);
                            if (subTmp == "1. " || subTmp == "2. " || subTmp == "3. " || subTmp == "4. " ||
                                subTmp == "5. " || subTmp == "6. " || subTmp == "7. " || subTmp == "8. " ||
                                subTmp == "9. " || subTmp == "|| " || subTmp == "() " || subTmp == "|) " ||
                                subTmp == "|I " || subTmp == "|[ ")
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
                                if (defs[j] != "")
                                {
                                    if (defs[j].Substring(0, 3) == "|| " || defs[j].Substring(0, 3) == "() " ||
                                    defs[j].Substring(0, 3) == "|) " || defs[j].Substring(0, 3) == "|I " ||
                                    defs[j].Substring(0, 3) == "|[ ")
                                    {
                                        defs[j] = cnt.ToString() + ") " + defs[j].Substring(3);
                                        cnt++;
                                    }
                                    else
                                    {
                                        cnt = 1;
                                    }
                                }

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
                            if (EXMP[j].Contains("— Ср.")) // исправить
                            {
                                //defs[j] += EXMP[j];
                                sr = EXMP[j];
                                EXMP[j] = "";
                            }
                            if (EXMP[j].Contains(" знач.") /*|| EXMP[j].Contains(" зная.") || EXMP[j].Contains(" знал.") || EXMP[j].Contains(" зкач.") || EXMP[j].Contains(" энач.")*/)
                            {
                                forExit = true;
                                i = 0;
                                st = 0;
                                bool zn = true;
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
                            // проверка на (1555) в цитате
                            forExit = true;
                            i = 0;
                            st = -1;
                            while (forExit && i <= EXMP[j].Length - 6)
                            {
                                subTmp = EXMP[j].Substring(i, 6);
                                if (subTmp[0] == '(' && subTmp[1].ToString().Any(char.IsDigit) &&
                                    subTmp[2].ToString().Any(char.IsDigit) &&
                                    subTmp[3].ToString().Any(char.IsDigit) &&
                                    subTmp[4].ToString().Any(char.IsDigit) &&
                                    subTmp[5] == ')')
                                {
                                    st = i;
                                    forExit = false;
                                }
                                i++;
                            }
                            if (st != -1)
                            {
                                defs[j] = defs[j] + EXMP[j].Substring(0, st);
                                EXMP[j] = EXMP[j].Substring(st);
                            }
                            // проверка на (155) в цитате
                            forExit = true;
                            i = 0;
                            st = -1;
                            while (forExit && i <= EXMP[j].Length - 5)
                            {
                                subTmp = EXMP[j].Substring(i, 5);
                                if (subTmp[0] == '(' && subTmp[1].ToString().Any(char.IsDigit) &&
                                    subTmp[2].ToString().Any(char.IsDigit) &&
                                    subTmp[3].ToString().Any(char.IsDigit) &&
                                    subTmp[4] == ')')
                                {
                                    st = i;
                                    forExit = false;
                                }
                                i++;
                            }
                            if (st != -1)
                            {
                                defs[j] = defs[j] + EXMP[j].Substring(0, st);
                                EXMP[j] = EXMP[j].Substring(st);
                            }
                            // проверка на (155) в описании
                            forExit = true;
                            i = 0;
                            st = -1;
                            while (forExit && i <= defs[j].Length - 5)
                            {
                                subTmp = defs[j].Substring(i, 5);
                                if (subTmp[0] == '(' && subTmp[1].ToString().Any(char.IsDigit) &&
                                    subTmp[2].ToString().Any(char.IsDigit) &&
                                    subTmp[3].ToString().Any(char.IsDigit) &&
                                    subTmp[4] == ')')
                                {
                                    st = i;
                                    forExit = false;
                                }
                                i++;
                            }
                            if (st != -1)
                            {
                                EXMP[j] = defs[j].Substring(st) + EXMP[j];
                                defs[j] = defs[j].Substring(0, st);
                            }
                            // проверка на (1555) в описании
                            forExit = true;
                            i = 0;
                            st = -1;
                            while (forExit && i <= defs[j].Length - 6)
                            {
                                subTmp = defs[j].Substring(i, 6);
                                if (subTmp[0] == '(' && subTmp[1].ToString().Any(char.IsDigit) &&
                                    subTmp[2].ToString().Any(char.IsDigit) &&
                                    subTmp[3].ToString().Any(char.IsDigit) &&
                                    subTmp[4].ToString().Any(char.IsDigit) &&
                                    subTmp[5] == ')')
                                {
                                    st = i;
                                    forExit = false;
                                }
                                i++;
                            }
                            if (st != -1)
                            {
                                EXMP[j] = defs[j].Substring(st) + EXMP[j];
                                defs[j] = defs[j].Substring(0, st);
                            }
                        }
                    }
                }
                else
                {
                    EXMP.Add("");
                    defs[0] = definition;
                }
                // проверка на (155) в описании для первого элемента
                forExit = true;
                i = 0;
                st0 = -1;
                while (forExit && i <= defs[0].Length - 5)
                {
                    if (defs[0][i] == '(' && defs[0][i + 1].ToString().Any(char.IsDigit) &&
                        defs[0][i + 2].ToString().Any(char.IsDigit) &&
                        defs[0][i + 3].ToString().Any(char.IsDigit) &&
                        defs[0][i + 4] == ')')
                    {
                        st0 = i;
                        forExit = false;
                    }
                    i++;
                }
                if (st0 != -1)
                {
                    EXMP[0] = defs[0].Substring(st0) + EXMP[0];
                    defs[0] = defs[0].Substring(0, st0);
                }
                // проверка на (1555) в описании для первого элемента
                forExit = true;
                i = 0;
                st0 = -1;
                while (forExit && i <= defs[0].Length - 6)
                {
                    if (defs[0][i] == '(' && defs[0][i + 1].ToString().Any(char.IsDigit) &&
                        defs[0][i + 2].ToString().Any(char.IsDigit) &&
                        defs[0][i + 3].ToString().Any(char.IsDigit) &&
                        defs[0][i + 4].ToString().Any(char.IsDigit) &&
                        defs[0][i + 5] == ')')
                    {
                        st0 = i;
                        forExit = false;
                    }
                    i++;
                }
                if (st0 != -1)
                {
                    EXMP[0] = defs[0].Substring(st0) + EXMP[0];
                    defs[0] = defs[0].Substring(0, st0);
                }

                // здесь текст цитаты источник и дата
                string[] sourceCode = new string[EXMP.Count];
                string[] sourceDate = new string[EXMP.Count];
                string[] sprt = { "///" };
                bool findUkaz;
                int uId = 0;
                string[] splitUkaz;
                for (i = 0; i < EXMP.Count; i++)
                {
                    findUkaz = false;
                    for (int j = 0; j < Ukaz.Count; j++)
                    {
                        if (EXMP[i].Contains(Ukaz[j]))
                        {
                            findUkaz = true;
                            uId = j;
                        }
                    }
                    if (findUkaz) // поиск по источникам в бд
                    {
                        string[] sprtU = { Ukaz[uId] };
                        splitUkaz = EXMP[i].Split(sprtU, StringSplitOptions.RemoveEmptyEntries);
                        EXMP[i] = splitUkaz[0];
                        sourceCode[i] = Ukaz[uId];
                        if (splitUkaz.Length > 1)
                        {
                            sourceDate[i] = splitUkaz[1];
                            sourceDate[i] = sourceDate[i].Replace(". ", ". ///");
                            string[] splitDate = sourceDate[i].Split(sprt, StringSplitOptions.RemoveEmptyEntries);
                            sourceDate[i] = "";
                            for (int j = 0; j < splitDate.Length; j++)
                            {
                                if (splitDate[j].Contains(" в.") || splitDate[j].Contains(" вв.") || splitDate[j].Contains(" г."))
                                {
                                    sourceDate[i] += splitDate[j];
                                }
                                else
                                {
                                    sourceCode[i] += splitDate[j];
                                }
                            }
                        }
                    }
                    else // внутренний алгоритм
                    {
                        EXMP[i] = EXMP[i].Replace(". ", ". ///");
                        string[] splitEXMP = EXMP[i].Split(sprt, StringSplitOptions.RemoveEmptyEntries);
                        EXMP[i] = "";
                        int textBorder = -1;
                        int dateBorder = -1;
                        bool firstDate = true;
                        for (int j = 0; j < splitEXMP.Length; j++)
                        {
                            if (splitEXMP[j].Contains("#"))
                            {
                                textBorder = j;
                            }
                            if (firstDate && (splitEXMP[j].Contains(" в.") || splitEXMP[j].Contains(" вв.") || splitEXMP[j].Contains(" г.")))
                            {
                                dateBorder = j;
                                firstDate = false;
                            }
                        }
                        if (textBorder != -1 || dateBorder != -1)
                        {
                            if (dateBorder != -1)
                            {
                                for (int j = 0; j < splitEXMP.Length; j++)
                                {
                                    if (j >= dateBorder)
                                    {
                                        sourceDate[i] += splitEXMP[j];
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < splitEXMP.Length; j++)
                                {
                                    if (j > textBorder)
                                    {
                                        sourceCode[i] += splitEXMP[j];
                                    }
                                }
                            }
                            if (textBorder != -1)
                            {
                                for (int j = 0; j < splitEXMP.Length; j++)
                                {
                                    if (j <= textBorder)
                                    {
                                        EXMP[i] += splitEXMP[j];
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < splitEXMP.Length; j++)
                                {
                                    if (j < dateBorder)
                                    {
                                        EXMP[i] += splitEXMP[j];
                                    }
                                }
                            }
                            if (dateBorder != -1 && textBorder != -1)
                            {
                                for (int j = 0; j < splitEXMP.Length; j++)
                                {
                                    if (j < dateBorder && j > textBorder)
                                    {
                                        sourceCode[i] += splitEXMP[j];
                                    }
                                }
                            }
                        }
                        else
                        {
                            // перенести все в EXMP обратно
                            for (int j = 0; j < splitEXMP.Length; j++)
                            {
                                EXMP[i] += splitEXMP[j];
                            }
                        }
                    }
                }
                AddBD(name, semant, partOfSpeech, rod, num, defs, EXMP, sourceCode, sourceDate, sr);
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
        void AddBD(string nam, string sem, string par, string rod, string num, string[] def, List<string> exm, string[] cod, string[] dat, string com)
        {
            string query = "INSERT INTO mainwords (MAINWORD) VALUES ('" + nam + "')";
            // объект для выполнения SQL-запроса
            JSON.Send(query, JSONFlags.Insert);
            query = "INSERT INTO dictionaryentries (NAME, SEMANTIC, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE, SOURCECODE, SOURCEDATE, COMPARE) VALUES (" +
                    mainWordNum.ToString() + ", '" + sem + "', '" + par + "', '" + rod + "', '" + num + "', '" + def[0] + "', '" + exm[0] + "', '" + cod[0] + "', '" + dat[0] + "', '" + com + "')";
            // объект для выполнения SQL-запроса
            JSON.Send(query, JSONFlags.Insert);
            if (exm.Count > 1)
            {
                for (int i = 1; i < exm.Count; i++)
                {
                    query = "INSERT INTO dictionaryentries (NAME, SEMANTIC, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE, SOURCECODE, SOURCEDATE, COMPARE) VALUES (" +
                                mainWordNum.ToString() + ", '', '', '', '', '" + def[i] + "', '" + exm[i] + "', '" + cod[i] + "', '" + dat[i] + "', '')";
                    JSON.Send(query, JSONFlags.Insert);
                }
            }
            mainWordNum++;
        }
        void ClearTable()
        {
            string query = "TRUNCATE TABLE dictionaryentries";
            JSON.Send(query, JSONFlags.Truncate);
            query = "TRUNCATE TABLE mainwords";
            JSON.Send(query, JSONFlags.Truncate);
        }
        bool Decomposition = true;
        private void buWordSearch_Read_Click(object sender, EventArgs e)
        {
            //tcWordSearch_Main.Enabled = false;
            buWordSearch_Read.Enabled = false;
            buClearDETable.Enabled = false;
            buScanFilesBack.Enabled = false;
            FileName.Clear();

            buStopDecomposition.Visible = true;
            buStopDecomposition.Enabled = true;

            OpenFileDialog OPF = new OpenFileDialog(); // Инициализация диалогового окна
            OPF.Filter = "HTM|*.htm"; // Фильтр в диалоговом окне
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                FileName.Add(OPF.FileName); // Добавление в массив пути картинки
                sr = new StreamReader(FileName[0], Encoding.Default);
                Program.f1.PictAndLableWait(true);
                //List<Thread> massThread = new List<Thread>();
                MainForm.MyGlobalVar1 = new Thread(() => ReadingHTM());
                //massThread.Add(new Thread(() => ReadingHTM()));
                //massThread[0].IsBackground = true;
                MainForm.MyGlobalVar1.IsBackground = true;
                //massThread[0].Start();
                MainForm.MyGlobalVar1.Start();

                while (MainForm.MyGlobalVar1.IsAlive/*massThread[0].IsAlive*/ && Decomposition)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
                //massThread.Clear();
                MainForm.MyGlobalVar1.Abort();
                Program.f1.PictAndLableWait(false);
                if(Decomposition)
                {
                    MessageBox.Show("Готово", "Декомпозиция");
                }
                else
                {
                    MessageBox.Show("Остановлена", "Декомпозиция");
                }
            }
            Decomposition = true;
            buStopDecomposition.Visible = false;
            buStopDecomposition.Enabled = false;
            buWordSearch_Read.Enabled = true;
            buClearDETable.Enabled = true;
            buScanFilesBack.Enabled = true;
            //tcWordSearch_Main.Enabled = true;
        }
        List<string[]> table;
        List<string[]> idMainWord;
        int cntMainWord;
        void FindWords()
        {
            List<string> ID = new List<string>();
            List<string> Names = new List<string>();
            List<string> Semantic = new List<string>();
            List<string> PartsOfSpeech = new List<string>();
            List<string> Rods = new List<string>();
            List<string> Nums = new List<string>();
            List<string> Definitions = new List<string>();
            List<string> Examples = new List<string>();
            List<string> SourceCode = new List<string>();
            List<string> SourceDate = new List<string>();
            List<string> Compare = new List<string>();
            List<string> MainWord = new List<string>();
            List<JSONArray> jNames = new List<JSONArray>();

            pageCount = 0;
            string query = "SELECT * FROM dictionaryentries";
            JSON.Send(query, JSONFlags.Select);
            jNames = JSON.Decode();
            if (jNames != null)
            {
                for (int i = 0; i < jNames.Count; i++)
                {
                    ID.Add(jNames[i].ID);
                    Names.Add(jNames[i].Name);
                    Semantic.Add(jNames[i].Semantic);
                    PartsOfSpeech.Add(jNames[i].Partofspeech);
                    Rods.Add(jNames[i].Rod);
                    Nums.Add(jNames[i].Num);
                    Definitions.Add(jNames[i].Definition);
                    Examples.Add(jNames[i].Example);
                    SourceCode.Add(jNames[i].SourceCode);
                    SourceDate.Add(jNames[i].SourceDate);
                    Compare.Add(jNames[i].Compare);
                }
            }
            query = "SELECT * FROM mainwords";
            JSON.Send(query, JSONFlags.Select);
            jNames = JSON.Decode();
            if (jNames != null)
            {
                for (int i = 0; i < jNames.Count; i++)
                {
                    MainWord.Add(jNames[i].Mainword);
                }
            }
            cntMainWord = MainWord.Count;
            if (Names != null)
            {
                string tmpSearch = tbWordSearch_SearchingWord.Text;
                string textSearch = "";
                for (int i = 0; i < tmpSearch.Length; i++)
                {
                    if (char.IsLetter(tmpSearch[i]))
                    {
                        textSearch += char.ToUpper(tmpSearch[i]);
                    }
                    else
                    {
                        textSearch += tmpSearch[i];
                    }
                }
                int tmp = 0;
                bool newWord = false;
                table = new List<string[]>();
                idMainWord = new List<string[]>();
                string[] line;
                if (cbSearchType.Checked == true)
                {
                    for (int i = 0; i < Names.Count; i++)
                    {
                        line = new string[12];
                        if (MainWord[Convert.ToInt32(Names[i]) - 1] == textSearch || textSearch == "")
                        {
                            if (tmp /*!=*/< Convert.ToInt32(Names[i]))
                            {
                                tmp = Convert.ToInt32(Names[i]);
                                newWord = true;
                            }
                            if (newWord)
                            {
                                line[0] = MainWord[Convert.ToInt32(Names[i]) - 1];
                                line[1] = Semantic[i];
                                line[2] = PartsOfSpeech[i];
                                line[3] = Rods[i];
                                line[4] = Nums[i];
                                line[5] = Definitions[i];
                                line[6] = Examples[i];
                                line[7] = SourceCode[i];
                                line[8] = SourceDate[i];
                                line[9] = Compare[i];
                                line[10] = ID[i];
                                line[11] = Names[i];
                                table.Add(line);
                                newWord = false;
                                pageCount++;
                                line = new string[3];
                                line[0] = ID[i];
                                line[1] = MainWord[Convert.ToInt32(Names[i]) - 1];
                                line[2] = Names[i];
                                idMainWord.Add(line);
                            }
                            else
                            {
                                line[0] = "";
                                line[1] = Semantic[i];
                                line[2] = PartsOfSpeech[i];
                                line[3] = Rods[i];
                                line[4] = Nums[i];
                                line[5] = Definitions[i];
                                line[6] = Examples[i];
                                line[7] = SourceCode[i];
                                line[8] = SourceDate[i];
                                line[9] = Compare[i];
                                line[10] = ID[i];
                                line[11] = Names[i];
                                table.Add(line);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Names.Count; i++)
                    {
                        line = new string[12];
                        if (MainWord[Convert.ToInt32(Names[i]) - 1].Contains(textSearch) || textSearch == "")
                        {
                            if (tmp /*!=*/< Convert.ToInt32(Names[i]))
                            {
                                tmp = Convert.ToInt32(Names[i]);
                                newWord = true;
                            }
                            if (newWord)
                            {
                                line[0] = MainWord[Convert.ToInt32(Names[i]) - 1];
                                line[1] = Semantic[i];
                                line[2] = PartsOfSpeech[i];
                                line[3] = Rods[i];
                                line[4] = Nums[i];
                                line[5] = Definitions[i];
                                line[6] = Examples[i];
                                line[7] = SourceCode[i];
                                line[8] = SourceDate[i];
                                line[9] = Compare[i];
                                line[10] = ID[i];
                                line[11] = Names[i];
                                table.Add(line);
                                newWord = false;
                                pageCount++;
                                line = new string[3];
                                line[0] = ID[i];
                                line[1] = MainWord[Convert.ToInt32(Names[i]) - 1];
                                line[2] = Names[i];
                                idMainWord.Add(line);
                            }
                            else
                            {
                                line[0] = "";
                                line[1] = Semantic[i];
                                line[2] = PartsOfSpeech[i];
                                line[3] = Rods[i];
                                line[4] = Nums[i];
                                line[5] = Definitions[i];
                                line[6] = Examples[i];
                                line[7] = SourceCode[i];
                                line[8] = SourceDate[i];
                                line[9] = Compare[i];
                                line[10] = ID[i];
                                line[11] = Names[i];
                                table.Add(line);
                            }
                        }
                    }
                }
            }
        }
        int page;
        int pageCount;
        void SearchClick()
        {
            buAddEntry.Enabled = false;
            buAddComplete.Enabled = false;
            buWordSearch_FindWord.Enabled = false;
            lbMainWords.Enabled = false;
            lbMainWords.Items.Clear();
            buChangeEntry.Enabled = false;
            buSaveEntry.Enabled = false;
            buCancelChanges.Enabled = false;
            dgvResults.Rows.Clear();
            dgvResults.Columns.Clear();
            dgvResults.Refresh();
            cmbPage.Items.Clear();
            buPageBack.Enabled = false;
            buPageNext.Enabled = false;
            cmbPage.Enabled = false;
            FindWords();
            if (table.Count > 0)
            {
                pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(pageCount) / 50.0));
                page = 1;
                for (int i = 0; i < pageCount; i++)
                {
                    cmbPage.Items.Add(i + 1);
                }
                if (pageCount > 1)
                {
                    buPageNext.Enabled = true;
                    cmbPage.Enabled = true;
                }
                cmbPage.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Результатов не найдено", "Оповещение");
            }
            lbMainWords.Enabled = true;
            buWordSearch_FindWord.Enabled = true;
            buAddEntry.Enabled = true;
        }
        private void buWordSearch_FindWord_Click(object sender, EventArgs e)
        {
            SearchClick();
        }
        
        private void buPageNext_Click(object sender, EventArgs e)
        {
            page++;
            if(page == pageCount)
            {
                buPageNext.Enabled = false;
            }
            buPageBack.Enabled = true;
            cmbPage.SelectedIndex = page - 1;
        }

        private void buPageBack_Click(object sender, EventArgs e)
        {
            page--;
            if (page == 1)
            {
                buPageBack.Enabled = false;
            }
            buPageNext.Enabled = true;
            cmbPage.SelectedIndex = page - 1;
        }

        private void cmbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            page = Convert.ToInt32(cmbPage.SelectedItem);
            buPageBack.Enabled = true;
            buPageNext.Enabled = true;
            if (page == 1)
            {
                buPageBack.Enabled = false;
            }
            if (page == pageCount)
            {
                buPageNext.Enabled = false;
            }
            //dgvResults.Rows.Clear();
            //dgvResults.Refresh();
            lbMainWords.Items.Clear();
            int i = 0 + (page - 1) * 50;
            while (i < 50 * page && i < idMainWord.Count)
            {
                lbMainWords.Items.Add(idMainWord[i][1]);
                //dgvResults.Rows.Add(table[i]);
                i++;
            }
        }
        int selectedWord;
        List<int> wordID;
        private void lbMainWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cancel();
            dgvResults.Columns.Add("Column1", "Заголовочное слово");
            dgvResults.Columns.Add("Column2", "Семантика");
            dgvResults.Columns.Add("Column3", "Часть речи");
            dgvResults.Columns.Add("Column4", "Род");
            dgvResults.Columns.Add("Column5", "Число");
            dgvResults.Columns.Add("Column6", "Определение");
            dgvResults.Columns.Add("Column7", "Цитата");
            dgvResults.Columns.Add("Column8", "Код цитаты");
            dgvResults.Columns.Add("Column9", "Дата цитаты");
            dgvResults.Columns.Add("Column10", "Сравнить");
            dgvResults.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            buChangeEntry.Enabled = true;
            wordID = new List<int>();
            for (int i = 0; i < table.Count; i++)
            {
                if(idMainWord[lbMainWords.SelectedIndex + (page - 1) * 50][2] == table[i][11])
                {
                    dgvResults.Rows.Add(table[i]);
                    selectedWord = Convert.ToInt32(table[i][11]);
                    wordID.Add(Convert.ToInt32(table[i][10]));
                }
            }
        }

        private void buChangeEntry_Click(object sender, EventArgs e)
        {
            buAddEntry.Enabled = false;
            buChangeEntry.Enabled = false;
            buCancelChanges.Enabled = true;
            buSaveEntry.Enabled = true;
            dgvResults.ReadOnly = false;
            buAddRow.Enabled = true;
            buDeleteRow.Enabled = true;
            buDeleteEntry.Enabled = true;
        }
        void Cancel()
        {
            buAddComplete.Enabled = false;
            buAddEntry.Enabled = true;
            buChangeEntry.Enabled = true;
            buCancelChanges.Enabled = false;
            buSaveEntry.Enabled = false;
            dgvResults.ReadOnly = true;
            buAddRow.Enabled = false;
            buDeleteRow.Enabled = false;
            buDeleteEntry.Enabled = false;
            lbMainWords.Enabled = true;
            dgvResults.Rows.Clear();
            dgvResults.Columns.Clear();
            dgvResults.Refresh();
        }
        private void buCancelChanges_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void buSaveEntry_Click(object sender, EventArgs e)
        {
            buAddEntry.Enabled = true;
            buWordSearch_FindWord.Enabled = false;
            buCancelChanges.Enabled = false;
            buSaveEntry.Enabled = false;
            dgvResults.ReadOnly = true;
            buAddRow.Enabled = false;
            buDeleteRow.Enabled = false;
            buDeleteEntry.Enabled = false;
            string query;
            int cnt = 0;
            for(int i = 0; i < table.Count; i++)
            {
                if(Convert.ToInt32(table[i][11]) == selectedWord)
                {
                    if(cnt < dgvResults.Rows.Count)
                    {
                        if (cnt == 0)
                        {
                            query = "UPDATE mainwords SET MAINWORD='" + dgvResults.Rows[cnt].Cells[0].Value + "' WHERE ID='" + selectedWord + "'";
                            JSON.Send(query, JSONFlags.Update);
                        }
                        query = "UPDATE dictionaryentries SET SEMANTIC='" + dgvResults.Rows[cnt].Cells[1].Value + "', PARTOFSPEECH='" + dgvResults.Rows[cnt].Cells[2].Value + "', ROD='" + dgvResults.Rows[cnt].Cells[3].Value + "', NUM='" + dgvResults.Rows[cnt].Cells[4].Value + "', DEFINITION='" + dgvResults.Rows[cnt].Cells[5].Value + "', EXAMPLE='" + dgvResults.Rows[cnt].Cells[6].Value + "', SOURCECODE='" + dgvResults.Rows[cnt].Cells[7].Value + "', SOURCEDATE='" + dgvResults.Rows[cnt].Cells[8].Value + "', COMPARE='" + dgvResults.Rows[cnt].Cells[9].Value + "' WHERE ID='" + wordID[cnt] + "'";
                        JSON.Send(query, JSONFlags.Update);
                        cnt++;
                    }
                    else
                    {
                        //query = "UPDATE dictionaryentries SET SEMANTIC='', PARTOFSPEECH='', ROD='', NUM='', DEFINITION='', EXAMPLE='', SOURCECODE='', SOURCEDATE='', COMPARE='' WHERE ID='" + wordID[cnt] + "'";
                        query = "DELETE FROM dictionaryentries WHERE ID='" + wordID[cnt] + "'";
                        JSON.Send(query, JSONFlags.Update);
                        cnt++;
                    }
                    
                }
            }
            if(cnt < dgvResults.Rows.Count /*- 1*/)
            {
                for(int i = cnt; i < dgvResults.Rows.Count; i++)
                {
                    query = "INSERT INTO dictionaryentries (NAME, SEMANTIC, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE, SOURCECODE, SOURCEDATE, COMPARE) VALUES (" +
                    selectedWord + ", '" + dgvResults.Rows[cnt].Cells[1].Value + "', '" + dgvResults.Rows[cnt].Cells[2].Value + "', '" + dgvResults.Rows[cnt].Cells[3].Value + "', '" + dgvResults.Rows[cnt].Cells[4].Value + "', '" + dgvResults.Rows[cnt].Cells[5].Value + "', '" + dgvResults.Rows[cnt].Cells[6].Value + "', '" + dgvResults.Rows[cnt].Cells[7].Value + "', '" + dgvResults.Rows[cnt].Cells[8].Value + "', '" + dgvResults.Rows[cnt].Cells[9].Value + "')";
                    // объект для выполнения SQL-запроса
                    JSON.Send(query, JSONFlags.Insert);
                }
            }
            buChangeEntry.Enabled = true;
            buWordSearch_FindWord.Enabled = true;
            SearchClick();
        }

        private void buAddRow_Click(object sender, EventArgs e)
        {
            dgvResults.Rows.Add();
            //dgvResults.Refresh();
        }

        private void buDeleteRow_Click(object sender, EventArgs e)
        {
            if(dgvResults.Rows.Count > 1)
            {
                dgvResults.Rows.RemoveAt(dgvResults.Rows.Count - 1);
                //dgvResults.Refresh();
            }
        }

        private void buDeleteEntry_Click(object sender, EventArgs e)
        {
            buAddEntry.Enabled = true;
            buDeleteEntry.Enabled = false;
            buCancelChanges.Enabled = false;
            buSaveEntry.Enabled = false;
            dgvResults.ReadOnly = true;
            buAddRow.Enabled = false;
            buDeleteRow.Enabled = false;
            string query;
            query = "DELETE FROM dictionaryentries WHERE NAME='" + selectedWord + "'";
            JSON.Send(query, JSONFlags.Update);
            buChangeEntry.Enabled = true;
            SearchClick();
        }

        private void buAddEntry_Click(object sender, EventArgs e)
        {
            lbMainWords.Enabled = false;
            dgvResults.Rows.Clear();
            dgvResults.Columns.Clear();
            dgvResults.Refresh();
            buAddEntry.Enabled = false;
            buChangeEntry.Enabled = false;
            dgvResults.ReadOnly = false;
            buAddRow.Enabled = true;
            buDeleteRow.Enabled = true;
            buCancelChanges.Enabled = true;
            dgvResults.Columns.Add("Column1", "Заголовочное слово");
            dgvResults.Columns.Add("Column2", "Семантика");
            dgvResults.Columns.Add("Column3", "Часть речи");
            dgvResults.Columns.Add("Column4", "Род");
            dgvResults.Columns.Add("Column5", "Число");
            dgvResults.Columns.Add("Column6", "Определение");
            dgvResults.Columns.Add("Column7", "Цитата");
            dgvResults.Columns.Add("Column8", "Код цитаты");
            dgvResults.Columns.Add("Column9", "Дата цитаты");
            dgvResults.Columns.Add("Column10", "Сравнить");
            dgvResults.Rows.Add();
            buAddComplete.Enabled = true;
        }

        private void buAddComplete_Click(object sender, EventArgs e)
        {
            buAddComplete.Enabled = false;
            dgvResults.ReadOnly = true;
            buAddRow.Enabled = false;
            buDeleteRow.Enabled = false;
            buCancelChanges.Enabled = false;

            List<JSONArray> jNames = new List<JSONArray>();
            string query = "SELECT * FROM mainwords";
            JSON.Send(query, JSONFlags.Select);
            jNames = JSON.Decode();
            cntMainWord = jNames.Count;

            int tmp = cntMainWord + 1;
            /*string*/ query = "INSERT INTO mainwords (MAINWORD) VALUES ('" + dgvResults.Rows[0].Cells[0].Value + "')";
            JSON.Send(query, JSONFlags.Insert);
            for (int i = 0; i < dgvResults.Rows.Count; i++)
            {
                query = "INSERT INTO dictionaryentries (NAME, SEMANTIC, PARTOFSPEECH, ROD, NUM, DEFINITION, EXAMPLE, SOURCECODE, SOURCEDATE, COMPARE) VALUES (" +
                    tmp + ", '" + dgvResults.Rows[i].Cells[1].Value + "', '" + dgvResults.Rows[i].Cells[2].Value + "', '" + dgvResults.Rows[i].Cells[3].Value + "', '" + dgvResults.Rows[i].Cells[4].Value + "', '" + dgvResults.Rows[i].Cells[5].Value + "', '" + dgvResults.Rows[i].Cells[6].Value + "', '" + dgvResults.Rows[i].Cells[7].Value + "', '" + dgvResults.Rows[i].Cells[8].Value + "', '" + dgvResults.Rows[i].Cells[9].Value + "')";
                JSON.Send(query, JSONFlags.Insert);
            }
            dgvResults.Rows.Clear();
            dgvResults.Columns.Clear();
            dgvResults.Refresh();
            //buChangeEntry.Enabled = true;
            buAddEntry.Enabled = true;
            lbMainWords.Enabled = true;
            if(tbWordSearch_SearchingWord.Text != "")
            {
                SearchClick();
            }
        }

        private void buClearDETable_Click(object sender, EventArgs e)
        {
            ClearTable();
            MessageBox.Show("Готово", "Очистка базы данных");
        }

        private void buWordSearchScanFiles_Click(object sender, EventArgs e)
        {
            tcWordSearch_Main.SelectedTab = tpWordSearch_ReadFiles;
        }

        private void buWordSearchFindWords_Click(object sender, EventArgs e)
        {
            tcWordSearch_Main.SelectedTab = tpWordSearch_Search;
        }

        private void buFindWordsBack_Click(object sender, EventArgs e)
        {
            tcWordSearch_Main.SelectedTab = tpWordSearchMenu;
        }

        private void buScanFilesBack_Click(object sender, EventArgs e)
        {
            tcWordSearch_Main.SelectedTab = tpWordSearchMenu;
        }

        private void buWordSearchModuleToMenu_Click(object sender, EventArgs e)
        {
            Program.f1.TCPrev();
        }

        private void buStopDecomposition_Click(object sender, EventArgs e)
        {
            Decomposition = false;
        }
    }
}
