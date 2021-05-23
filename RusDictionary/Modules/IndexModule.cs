using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using CsvHelper;
using System.IO;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data.OleDb;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;

namespace RusDictionary.Modules
{
    // TODO:  добавление новых записей в распознавании, сохранение в файл

    public partial class IndexModule : UserControl
    {
        string[] base_name = {"Шифр_источника", "Полное_описание", "Синоним", "Название_источника", "Автор", "Исследователь",
             "Дата_источника", "Уточненная_дата", "Язык_оригинала", "Оригинал_перевода", "Другие_списки",
             "Издание_рукописи", "Датировка_состава", "Переиздание", "Место_хранения"};

        string[] table_name = { "cipher", "description", "synonym", "name_source", "author", "researcher",
            "date_source", "refind_date", "language", "translation", "other_list",
            "publication", "date_structure", "reprint", "storage" };

        List<JSONArray> Ukaz_item = new List<JSONArray>();

        List<JSONArray> In_item = new List<JSONArray>();

        List<JSONArray> Out_item = new List<JSONArray>();

        List<JSONArray> check_q = new List<JSONArray>();


        bool modify = false;
        string query;

        DataTable dt = new DataTable();

        char[] sep = { '[', ']', '(', ')' };
        //string single_sep = "//" ;

        string[] row1 = new string[] { "1", "см. = [Синоним]" };
        string[] row2 = new string[] { "2", "*// = [Название_источника]" };
        string[] row3 = new string[] { "3", "Хранится, Хранятся = [Место_хранения]" };
        string[] row4 = new string[] { "4", "переизд. = [Переиздание]" };
        string[] row5 = new string[] { "5", "ред. = [Датировка_состава]" };



        public IndexModule()
        {

            InitializeComponent();
            dgv_rule.Rows.Add(row1);
            dgv_rule.Rows.Add(row2);
            dgv_rule.Rows.Add(row3);
            dgv_rule.Rows.Add(row4);
            dgv_rule.Rows.Add(row5);
        }

        private void reload_table()
        {
            lbName.Items.Clear();
            query = "SELECT * FROM ukaz_tab ORDER BY cipher";
            JSON.Send(query, JSONFlags.Select);
            Ukaz_item = JSON.Decode();

            foreach (var r in Ukaz_item)
            {

                lbName.Items.Add(r.cipher);

            }

        }

        private void buIndexSource_Click(object sender, EventArgs e)
        {

            // ORDER BY
            // индекс тоже возвращается

            reload_table();
            tc_index.SelectedTab = tp_list_sign;

        }

        private void lbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // переписать на поиск в листе

            var item_find = Ukaz_item.Find(x => x.cipher.Contains(lbName.SelectedItem.ToString()));

            tb_cipher.Text = item_find.cipher;
            tb_description.Text = item_find.description;
            tb_synonym.Text = item_find.synonym;
            tb_name_source.Text = item_find.name_source;
            tb_author.Text = item_find.author;
            tb_researcher.Text = item_find.researcher;
            tb_date_source.Text = item_find.date_source;
            tb_refind_date.Text = item_find.refind_date;
            tb_language.Text = item_find.language;
            tb_translation.Text = item_find.translation;
            tb_publication.Text = item_find.publication;
            tb_other_list.Text = item_find.other_list;
            tb_date_structure.Text = item_find.date_structure;
            tb_reprint.Text = item_find.refind_date;
            tb_storage.Text = item_find.storage;


            buSaveToDB.Visible = true;
            buSaveToDB.Enabled = true;
            bu_Insert.Visible = false;
            bu_Insert.Enabled = false;

            modify = false;

            tc_index.SelectedTab = tp_sign;

        }

        private void bu_open_doc_Click(object sender, EventArgs e)
        {
            string filepath = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "docx files (*.docx)|*.docx|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    filepath = openFileDialog.FileName;

                    WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true);

                    DataTable dt = new DataTable();

                    Body body = doc.MainDocumentPart.Document.Body;


                    if (body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>() is null)
                    {
                        MessageBox.Show("Данный документ не содержит каких-либо таблиц");
                    }
                    else
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Table table = body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>().First();
                        IEnumerable<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>();

                        dt.Columns.Add("1");
                        dt.Columns.Add("2");

                        // To read data from rows and to add records to the temporary table  
                        foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow row in rows)
                        {
                            dt.Rows.Add();
                            int i = 0;
                            foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell cell in row.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>())
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.InnerText;
                                i++;
                            }
                        }
                        dgv_output.Rows.Clear();
                        dgv_output.Refresh();


                        for (int count = 0; count < dt.Rows.Count; count++)
                        {
                            var split_list = new List<string>();
                            var name_list = new List<string>();


                            string cipher = dt.Rows[count].ItemArray[0].ToString();

                            string text = dt.Rows[count].ItemArray[1].ToString();


                            var search_list = new List<string>(); ///слова для поиска в тексте

                            var param_list = new List<string>(); ///название полей в базе данных 

                            var search_trim_list = new List<string>(); /// слова без спецсимволов

                            var pos_list = new List<int>(); ///позиция найденных слов и сепараторов



                            for (int i = 0; i < dgv_rule.Rows.Count - 1; i++)
                            {
                                DataGridViewRow row = dgv_rule.Rows[i];

                                string lbl_name = row.Cells[1].Value.ToString();


                                foreach (string param in lbl_name.Split('=')[0].Split(','))
                                {
                                    string string_trim = lbl_name.TrimEnd("END".ToCharArray());
                                    param_list.Add(string_trim.Split('=')[1].Substring(1).Trim(sep));
                                    search_list.Add(param.Trim());
                                    search_trim_list.Add(param.Trim('*'));

                                }


                            }
                            foreach (string word in search_trim_list)
                            {
                                pos_list.Add(text.IndexOf(word));

                            }
                            foreach (char sym in sep)
                            {
                                //pos_list.Add(text.IndexOf(sym));

                                for (int index = 0; ; index += 1)
                                {
                                    int add_count = 0;
                                    index = text.IndexOf(sym, index);
                                    if (index == -1)
                                    {
                                        if (add_count == 0)
                                        {
                                            pos_list.Add(index);
                                        }

                                        break;
                                    }
                                    pos_list.Add(index);
                                }

                            }

                            List<int> text_pos = pos_list.ToList();///копия позиций для поиска в строке


                            text_pos.RemoveAll(item => item == -1);
                            text_pos.Sort();

                            string[] row_out = new string[15];

                            for (int i = 0; i < text_pos.Count; i++)
                            {
                                int first_pos = 0;


                                var a = pos_list.FindIndex(item => item == text_pos[i]);
                                if (a + 1 <= search_list.Count)
                                {
                                    if (search_list[a][0] == '*')
                                    {
                                        if (text_pos[i] != text_pos.First())
                                            row_out[Array.IndexOf(base_name, param_list[a])] = text.Substring(text_pos[i - 1], text_pos[i] - text_pos[i - 1]);
                                        else
                                            row_out[Array.IndexOf(base_name, param_list[a])] = text.Substring(first_pos, text_pos[i]);

                                    }
                                    else
                                    {
                                        if (text_pos[i] != text_pos.Last())
                                            row_out[Array.IndexOf(base_name, param_list[a])] = text.Substring(text_pos[i], text_pos[i + 1] - text_pos[i]);
                                        else
                                            row_out[Array.IndexOf(base_name, param_list[a])] = text.Substring(text_pos[i]);

                                        //Substring(2, 5);

                                    }
                                    if (a < dgv_rule.Rows.Count - 1)
                                        if (dgv_rule.Rows[a].Cells[1].Value.ToString().EndsWith("END"))
                                            break;
                                }


                            }

                            if (row_out[0] == null)
                                row_out[0] = cipher;

                            if (row_out[1] == null)
                                row_out[1] = text;

                            dgv_output.Rows.Add(row_out);
                        }

                        tc_index.SelectedTab = tp_result_doc;
                    }


                }
            }

        }

        private void buRecognition_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_read_doc;
        }

        private void bu_RulesLoad_Click(object sender, EventArgs e)
        {

            string filepath = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    filepath = openFileDialog.FileName;
                    StreamReader file = new StreamReader(filepath);
                    DataTable dt = new DataTable();
                    dt.Columns.Add("№");
                    dt.Columns.Add("Правило");
                    string newline;
                    int count = 1;
                    while ((newline = file.ReadLine()) != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = count;
                        dr[1] = newline;
                        count++;
                        dt.Rows.Add(dr);
                    }
                    file.Close();
                    if (count >= 2)
                    {
                        dgv_rule.Rows.Clear();
                        dgv_rule.Columns.Clear();
                        dgv_rule.Refresh();
                        dgv_rule.DataSource = dt;
                        dgv_rule.Columns[0].Width = 100;
                        dgv_rule.Columns[1].Width = 650;

                    }
                    else
                    {
                        MessageBox.Show("Данный файл пуст");
                    }

                }


            }


        }

        private void buSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            char separator = ';';
            sfd.Title = "Сохранить в...";
            sfd.ShowDialog();
            string fileName = sfd.FileName;
            DataTable dot = new DataTable();
            foreach (DataGridViewColumn col in dgv_output.Columns)
            {
                dot.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dgv_output.Rows)
            {
                DataRow dRow = dot.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dot.Rows.Add(dRow);
            }
            if (dot != null)
            {
                FileStream fs = null;
                try
                {
                    fs = File.OpenWrite(fileName);
                }
                catch
                {

                }
                using (TextWriter tw = new StreamWriter(fs, Encoding.GetEncoding(1251)))
                {
                    String line = "";
                    //Выводим имя таблицы
                    if (!String.IsNullOrEmpty(dot.TableName))
                        tw.WriteLine(dot.TableName);
                    //Вывод названий столбцов
                    foreach (DataColumn colName in dot.Columns)
                    {
                        line += String.Format("\"{0}\"{1}", colName.ColumnName, separator);
                    }
                    tw.WriteLine(line.TrimEnd(separator));
                    //Вывод данных
                    foreach (DataRow dr in dot.Rows)
                    {
                        line = "";
                        Array.ForEach(dr.ItemArray, obj => line += String.Format("\"{0}\"{1}", obj, separator));
                        tw.WriteLine(line.TrimEnd(separator));
                    }
                }
                fs.Close();
                fs.Dispose();

            }


        }

        private void buSaveToDB_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tb_cipher.Text) && !String.IsNullOrEmpty(tb_description.Text))
            {
                query = "UPDATE ukaz_tab SET `cipher`='" + tb_cipher.Text + "'," +
                "`description`= '" + tb_description.Text + "', " +
                "`synonym`= '" + tb_synonym.Text + "'," +
                "`name_source`= '" + tb_name_source.Text + "'," +
                "`author`= '" + tb_author.Text + "'," +
                "`researcher`= '" + tb_researcher.Text + "'," +
                "`date_source`= '" + tb_date_source.Text + "'," +
                "`refind_date`= '" + tb_refind_date.Text + "'," +
                "`language`= '" + tb_language.Text + "'," +
                "`translation`= '" + tb_translation.Text + "'," +
                "`publication`= '" + tb_publication.Text + "'," +
                "`other_list`= '" + tb_other_list.Text + "'," +
                "`date_structure`= '" + tb_date_structure.Text + "'," +
                "`refind_date`= '" + tb_refind_date.Text + "'," +
                "`storage`= '" + tb_storage.Text + "'" +
                "WHERE `ID` = " + Ukaz_item[lbName.SelectedIndex].ID;
                JSON.Send(query, JSONFlags.Update);

                reload_table();
                tc_index.SelectedTab = tp_list_sign;
                tb_cipher.Enabled = true;

            }
            else
                MessageBox.Show("Заполните поле Описание источника");

        }

        private void bu_to_bd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow data in dgv_output.Rows)
            {
                if (!String.IsNullOrEmpty(data.Cells[0].Value.ToString()) && !String.IsNullOrEmpty(data.Cells[1].Value.ToString()))
                {
                    query = "SELECT cipher FROM ukaz_tab WHERE cipher = '" + data.Cells[0].Value.ToString() + "'";
                    JSON.Send(query, JSONFlags.Select);
                    check_q = JSON.Decode();

                    if (check_q is null)
                    {
                        query = "INSERT INTO ukaz_tab (cipher, description, synonym, name_source, author," +
                            "researcher, date_source, refind_date, language, translation, other_list, publication," +
                            "date_structure, reprint, storage) " +
                            "VALUES('" + data.Cells[0].Value.ToString() + "','" + data.Cells[1].Value.ToString() + "','" + data.Cells[2].Value.ToString() + "'," +
                           "'" + data.Cells[3].Value.ToString() + "','" + data.Cells[4].Value.ToString() + "','" + data.Cells[5].Value.ToString() + "'," +
                           "'" + data.Cells[6].Value.ToString() + "','" + data.Cells[7].Value.ToString() + "','" + data.Cells[8].Value.ToString() + "'," +
                           "'" + data.Cells[9].Value.ToString() + "','" + data.Cells[10].Value.ToString() + "','" + data.Cells[11].Value.ToString() + "'," +
                           "'" + data.Cells[12].Value.ToString() + "','" + data.Cells[14].Value.ToString() + "','" + data.Cells[15].Value.ToString() + "')";

                        JSON.Send(query, JSONFlags.Insert);


                    }
                    else
                        MessageBox.Show("Данный источник уже находится в базе данных");
                }
                else
                    MessageBox.Show("Заполните основные поля: Шифр источника и Описание источника");

            }
            reload_table();
            tc_index.SelectedTab = tp_list_sign;
        }

        private void bu_Insert_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(tb_cipher.Text) && !String.IsNullOrEmpty(tb_description.Text))
            {
                query = "SELECT cipher FROM ukaz_tab WHERE cipher = '" + tb_cipher.Text + "'";
                JSON.Send(query, JSONFlags.Select);
                check_q = JSON.Decode();

                if (check_q is null)
                {
                    query = "INSERT INTO ukaz_tab (cipher, description, synonym, name_source, author," +
                        "researcher, date_source, refind_date, language, translation, other_list, publication," +
                        "date_structure, reprint, storage) " +
                        "VALUES('" + tb_cipher.Text + "','" + tb_description.Text + "','" + tb_synonym.Text + "'," +
                       "'" + tb_name_source.Text + "','" + tb_author.Text + "','" + tb_researcher.Text + "'," +
                       "'" + tb_date_source.Text + "','" + tb_refind_date.Text + "','" + tb_language.Text + "'," +
                       "'" + tb_translation.Text + "','" + tb_publication.Text + "','" + tb_other_list.Text + "'," +
                       "'" + tb_date_structure.Text + "','" + tb_reprint.Text + "','" + tb_storage.Text + "')";



                    JSON.Send(query, JSONFlags.Insert);

                    reload_table();
                    tc_index.SelectedTab = tp_list_sign;
                }
                else
                    MessageBox.Show("Данный источник уже находится в базе данных");
            }
            else
                MessageBox.Show("Заполните основные поля: Шифр источника и Описание источника");
        }

        private void bu_Create_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_sign;
            tb_cipher.Text = "";
            tb_description.Text = "";
            tb_synonym.Text = "";
            tb_name_source.Text = "";
            tb_author.Text = "";
            tb_researcher.Text = "";
            tb_date_source.Text = "";
            tb_refind_date.Text = "";
            tb_language.Text = "";
            tb_translation.Text = "";
            tb_publication.Text = "";
            tb_other_list.Text = "";
            tb_date_structure.Text = "";
            tb_reprint.Text = "";
            tb_storage.Text = "";
            buSaveToDB.Visible = false;
            buSaveToDB.Enabled = false;
            bu_Insert.Visible = true;
            bu_Insert.Enabled = true;
        }

        private void bu_delete_Click(object sender, EventArgs e)
        {
            if (lbName.SelectedIndex != -1)
            {
                DialogResult result = MessageBox.Show(
                "Если вы удалите запись, вы не сможете восстановить данные. Продолжить?",
                "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    query = "DELETE FROM ukaz_tab WHERE ID = '" + Ukaz_item[lbName.SelectedIndex].ID + "'";
                    JSON.Send(query, JSONFlags.Delete);
                    reload_table();
                    tc_index.SelectedTab = tp_list_sign;
                }
            }

        }

        private void buIndexBack_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_menu;
        }

        private void bu_back_list_Click(object sender, EventArgs e)
        {
            if (bu_Insert.Visible || modify)
            {
                DialogResult result = MessageBox.Show(
                "Если вы покинете данную страницу, данные не будут сохранены. Продолжить?",
                 "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    tb_cipher.Enabled = true;
                    tc_index.SelectedTab = tp_list_sign;
                }

            }
            else
            {
                tb_cipher.Enabled = true;
                tc_index.SelectedTab = tp_list_sign;
            }

        }

        private void bu_back_desc_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_menu;
        }

        private void bu_back_rule_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_read_doc;
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            modify = true;

        }

        private void bu_comp_Click(object sender, EventArgs e)
        {
            rb_showAll.Checked = true;

            tc_index.SelectedTab = tp_comparison;

        }

        void Clear_reload()
        {
            lb_table_in.Items.Clear();
            lb_table_out.Items.Clear();

            query = "SELECT * FROM ukaz ORDER BY Name";
            JSON.Send(query, JSONFlags.Select);
            Out_item = JSON.Decode();

            foreach (var r in Out_item)
            {

                lb_table_out.Items.Add(r.Name);

            }


            query = "SELECT * FROM ukaz_tab ORDER BY cipher";
            JSON.Send(query, JSONFlags.Select);
            In_item = JSON.Decode();

            foreach (var r in In_item)
            {

                lb_table_in.Items.Add(r.cipher);

            }


            if (rb_notExist.Checked)
            {

                foreach (var r in In_item)
                {
                    if (lb_table_out.Items.IndexOf(r.cipher) != -1)
                    {
                        lb_table_in.Items.Remove(r.cipher);
                    }
                }
            }

        }

        private void rb_showAll_CheckedChanged(object sender, EventArgs e)
        {
            rb_notExist.Checked = !rb_showAll.Checked;
            bu_addIndex.Enabled = rb_notExist.Checked;

            Clear_reload();
        }

        private void bu_addIndex_Click(object sender, EventArgs e)
        {
            if (lb_table_in.SelectedIndex != -1)
            {
                query = "INSERT INTO ukaz (Name) " +
                   "VALUES('" + lb_table_in.SelectedItem.ToString() + "')";
                JSON.Send(query, JSONFlags.Insert);

                Clear_reload();

            }
        }

        private void bu_deleteOut_Click(object sender, EventArgs e)
        {
            if (lb_table_out.SelectedIndex != -1)
            {
                DialogResult result = MessageBox.Show(
                               "Если вы удалите запись, вы не сможете восстановить данные. Продолжить?",
                               "Внимание",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning,
                               MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    query = "DELETE FROM ukaz WHERE ID = '" + Out_item[lb_table_out.SelectedIndex].ID + "'";
                    JSON.Send(query, JSONFlags.Delete);
                    Clear_reload();
                }

            }
        }

        private void bu_BackComp_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_menu;
            rb_showAll.Checked = false;
        }

        private void buIndexReturn_Click(object sender, EventArgs e)
        {
            Program.f1.TCPrev();
        }
        void SaveCSV()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Сохранить в...";
            sfd.ShowDialog();
            string fileName = sfd.FileName;


            DataTable table = new DataTable("Указатель");
            DataColumn cipherColumn = new DataColumn("cipher", Type.GetType("System.String"));
            DataColumn descriptionColumn = new DataColumn("description", Type.GetType("System.String"));
            DataColumn synonymColumn = new DataColumn("synonym", Type.GetType("System.String"));
            DataColumn name_sourceColumn = new DataColumn("name_source", Type.GetType("System.String"));
            DataColumn authorColumn = new DataColumn("author", Type.GetType("System.String"));
            DataColumn researcherColumn = new DataColumn("researcher", Type.GetType("System.String"));
            DataColumn date_sourceColumn = new DataColumn("date_source", Type.GetType("System.String"));
            DataColumn refind_dateColumn = new DataColumn("refind_date", Type.GetType("System.String"));
            DataColumn languageColumn = new DataColumn("language", Type.GetType("System.String"));
            DataColumn translationColumn = new DataColumn("translation", Type.GetType("System.String"));
            DataColumn other_listColumn = new DataColumn("other_list", Type.GetType("System.String"));
            DataColumn publicationColumn = new DataColumn("publication", Type.GetType("System.String"));
            DataColumn date_structureColumn = new DataColumn("date_structure", Type.GetType("System.String"));
            DataColumn reprintColumn = new DataColumn("reprint", Type.GetType("System.String"));
            DataColumn storageColumn = new DataColumn("storage", Type.GetType("System.String"));

            table.Columns.Add(cipherColumn);
            table.Columns.Add(descriptionColumn);
            table.Columns.Add(synonymColumn);
            table.Columns.Add(name_sourceColumn);
            table.Columns.Add(authorColumn);
            table.Columns.Add(researcherColumn);
            table.Columns.Add(date_sourceColumn);
            table.Columns.Add(refind_dateColumn);
            table.Columns.Add(languageColumn);
            table.Columns.Add(translationColumn);
            table.Columns.Add(other_listColumn);
            table.Columns.Add(publicationColumn);
            table.Columns.Add(date_structureColumn);
            table.Columns.Add(reprintColumn);
            table.Columns.Add(storageColumn);




            foreach (var it in Ukaz_item)
            {
                table.Rows.Add(new object[] { it.cipher, it.description,it.synonym,it.name_source,it.author,it.researcher,it.date_source,it.refind_date,it.language,it.translation, it.publication,it.other_list,it.date_structure,it.reprint,it.storage });
            }


            char separator = ';';

            if (table != null)
            {
                FileStream fs = null;
                try
                {
                    fs = File.OpenWrite(fileName);
                }
                catch
                {

                }
                using (TextWriter tw = new StreamWriter(fs, Encoding.GetEncoding(1251)))
                {
                    String line = "";
                    //Выводим имя таблицы
                    if (!String.IsNullOrEmpty(table.TableName))
                        tw.WriteLine(table.TableName);
                    //Вывод названий столбцов
                    foreach (DataColumn colName in table.Columns)
                    {
                        line += String.Format("\"{0}\"{1}", colName.ColumnName, separator);
                    }
                    tw.WriteLine(line.TrimEnd(separator));
                    //Вывод данных
                    foreach (DataRow dr in table.Rows)
                    {
                        line = "";
                        Array.ForEach(dr.ItemArray, obj => line += String.Format("\"{0}\"{1}", obj, separator));
                        tw.WriteLine(line.TrimEnd(separator));
                    }
                }
                fs.Close();
                fs.Dispose();

            }


        }

        private void bu_saveTofile_Click(object sender, EventArgs e)
        {
            SaveCSV();
        }
    }
}
