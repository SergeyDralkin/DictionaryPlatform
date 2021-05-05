using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Data.OleDb;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;

namespace RusDictionary.Modules
{
    // TODO: Сохранение изменений, добавление новых записей(в основном и в распознавании), удаление, сохранение в файл

    public partial class IndexModule : UserControl
    {
        string[] base_name = {"Шифр_источника", "Полное_описание", "Синоним", "Название_источника", "Автор", "Исследователь",
             "Дата_источника", "Уточненная_дата", "Язык_оригинала", "Оригинал_перевода", "Другие_списки",
             "Издание_рукописи", "Датировка_состава", "Переиздание", "Место_хранения"};

        string[] table_name = { "cipher", "description", "synonym", "name_source", "author", "researcher",
            "date_source", "refind_date", "language", "translation", "other_list",
            "publication", "date_structure", "reprint", "storage" };

        List<JSONArray> Ukaz_item = new List<JSONArray>();

        char[] letters = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToArray();

        int index_db = 0;

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

        private void buIndexSource_Click(object sender, EventArgs e)
        {

            // ORDER BY
            // индекс тоже возвращается

            string query = "SELECT * FROM ukaz_tab ORDER BY cipher";
            JSON.Send(query, JSONFlags.Select);
            Ukaz_item = JSON.Decode();

            foreach (var r in Ukaz_item)
            {

                lbName.Items.Add(r.cipher);

            }
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

            tc_index.SelectedTab = tp_sign;

        }

        private void bu_open_doc_Click(object sender, EventArgs e)
        {
            string filepath = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filepath = openFileDialog.FileName;
                }
            }


            WordprocessingDocument doc = WordprocessingDocument.Open(filepath, false);

            DataTable dt = new DataTable();

            Body body = doc.MainDocumentPart.Document.Body;

            DocumentFormat.OpenXml.Wordprocessing.Table table = body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>().First();

            IEnumerable<TableRow> rows = table.Elements<TableRow>();

            dt.Columns.Add("1");
            dt.Columns.Add("2");

            // To read data from rows and to add records to the temporary table  
            foreach (TableRow row in rows)
            {
                dt.Rows.Add();
                int i = 0;
                foreach (TableCell cell in row.Descendants<TableCell>())
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

        private void buRecognition_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_read_doc;
        }

        private void bu_RulesLoad_Click(object sender, EventArgs e)
        {
            dgv_rule.Rows.Clear();
            dgv_rule.Columns.Clear();
            dgv_rule.Refresh();
            string filepath = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    filepath = openFileDialog.FileName;

            }
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
            dgv_rule.DataSource = dt;
            dgv_rule.Columns[0].Width = 100;
            dgv_rule.Columns[1].Width = 650;


        }

        private void buSave_Click(object sender, EventArgs e)
        {
            /*
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Document|*.xlsx";
            sfd.Title = "Save an Image File";
            sfd.ShowDialog();

            
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(sfd.FileName, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "mySheet" };
            sheets.Append(sheet);

            // Get the sheetData cell table.
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Add a row to the cell table.
            Row row;
            row = new Row() { RowIndex = 1 };
            sheetData.Append(row);

            // In the new row, find the column location to insert a cell in A1.  
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, "B1", true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            // Add the cell to the cell table at A1.
            Cell newCell = new Cell() { CellReference = "B1" };
            row.InsertAfter(newCell, refCell);

            // Set the cell value to be a numeric value of 100.
            newCell.CellValue = new CellValue("100");
            newCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, "B1", true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            Cell newCell1 = new Cell() { CellReference = "B2" };
            row.InsertAfter(newCell1, refCell);

            // Set the cell value to be a numeric value of 100.
            newCell1.CellValue = new CellValue("100");
            newCell1.DataType = new EnumValue<CellValues>(CellValues.Number);

            // Close the document.
            spreadsheetDocument.Close();

            
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
            Create(sfd.FileName, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());


            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Вывод"
            };
            sheets.Append(sheet);

            workbookpart.Workbook.Save();

            Worksheet worksheet = new Worksheet();
            SheetData sheetData = new SheetData();

            for (int i = 0; i < dgv_output.Rows.Count; i++)
            {
                Row row = new Row();
                for (int j = 0; j < dgv_output.Columns.Count; j++)
                {
                    if (dgv_output.Rows[i].Cells[j].Value != null)
                    {
                        Cell cell = new Cell()
                        {
                            CellReference = letters[j].ToString() + i.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(dgv_output.Rows[i].Cells[j].Value.ToString())
                        };
                        row.Append(cell);
                    }
                    else
                    { 
                    }
                }
                sheetData.Append(row);


            }

            worksheet.Append(sheetData);
            worksheetPart.Worksheet = worksheet;

            workbookpart.Workbook.Save();


            // Close the document.
            spreadsheetDocument.Close();
            */


        }

        private void buSaveToDB_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tb_cipher.Text))
            {
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    if (dr[base_name[0]].ToString() == tb_cipher.Text) // if id==2
                    {
                        dr[base_name[1]] = tb_description.Text;

                        dr[base_name[2]] = tb_synonym.Text;
                        dr[base_name[3]] = tb_name_source.Text;
                        dr[base_name[4]] = tb_author.Text;
                        dr[base_name[5]] = tb_researcher.Text;
                        dr[base_name[6]] = tb_date_source.Text;
                        dr[base_name[7]] = tb_refind_date.Text;
                        dr[base_name[8]] = tb_language.Text;
                        dr[base_name[9]] = tb_translation.Text;
                        dr[base_name[10]] = tb_publication.Text;
                        dr[base_name[11]] = tb_other_list.Text;
                        dr[base_name[12]] = tb_date_structure.Text;
                        dr[base_name[12]] = tb_reprint.Text;
                        dr[base_name[12]] = tb_storage.Text;

                    }
                }
            }
            else
                MessageBox.Show("Заполните поле Шифр Источника");


        }

        private void bu_to_bd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow data in dgv_output.Rows)
            {
                if (lbName.Items.IndexOf(data.Cells[0]) > 0)
                {
                    ///
                }

            }
        }



        private void bu_Insert_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tb_cipher.Text))
            {
                DataRow dr = dt.NewRow();
                dr[base_name[0]] = tb_cipher.Text;
                dr[base_name[1]] = tb_description.Text;
                dr[base_name[2]] = tb_synonym.Text;
                dr[base_name[3]] = tb_name_source.Text;
                dr[base_name[4]] = tb_author.Text;
                dr[base_name[5]] = tb_researcher.Text;
                dr[base_name[6]] = tb_date_source.Text;
                dr[base_name[7]] = tb_refind_date.Text;
                dr[base_name[8]] = tb_language.Text;
                dr[base_name[9]] = tb_translation.Text;
                dr[base_name[10]] = tb_publication.Text;
                dr[base_name[11]] = tb_other_list.Text;
                dr[base_name[12]] = tb_date_structure.Text;
                dr[base_name[12]] = tb_reprint.Text;
                dr[base_name[12]] = tb_storage.Text;


                dt.Rows.Add(dr);

                /*
                foreach (object item in lbName.Items)
                {
                    var it = item.ToString();
                    if ((tb_cipher.Text[0] - it[0]) != 0)
                    {
                        int curIndex = lbName.Items.IndexOf(it);

                        dt.Rows.InsertAt(dr, curIndex);
                        break;
                    }
                    if ((tb_cipher.Text[1] - it[1]) == 0)
                    {

                    }

                }
                */
            }
            else
                MessageBox.Show("Заполните поле Шифр Источника");


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

            DialogResult result = MessageBox.Show(
            "Если вы покинете данную страницу, данные не будут сохранены, продолжить?",
            "Внимание",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
                lbName.Items.Remove(lbName.SelectedItem);

        }

        private void buIndexBack_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_menu;
        }

        private void bu_back_list_Click(object sender, EventArgs e)
        {
            if (bu_Insert.Visible)
            {
                DialogResult result = MessageBox.Show(
                "Если вы покинете данную страницу, данные не будут сохранены, продолжить?",
                 "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                    tc_index.SelectedTab = tp_list_sign;
            }
            else
                tc_index.SelectedTab = tp_list_sign;



        }

        private void bu_back_desc_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_menu;
        }

        private void bu_back_rule_Click(object sender, EventArgs e)
        {
            tc_index.SelectedTab = tp_read_doc;
        }
    }
}
