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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace RusDictionary.Modules
{

    public partial class IndexModule : UserControl
    {
        string[] base_name = {"Шифр_источника", "Полное_описание", "Синоним", "Название_источника", "Автор", "Исследователь",
             "Дата_источника", "Уточненная_дата", "Язык_оригинала", "Оригинал_перевода", "Другие_списки",
             "Издание_рукописи", "Датировка_состава", "Переиздание", "Место_хранения"};

        string[] table_name = { "cipher", "description", "synonym", "name_source", "author", "researcher",
            "date_source", "refind_date", "language", "translation", "other_list",
            "publication", "date_structure", "reprint", "storage" };

        DataTable dt = new DataTable();

        char[] sep = { '[', ']', '(', ')' };
        //string single_sep = "//" ;

        string[] row1 = new string[] { "1", "см. = [Синоним]END" };
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
            string strDSN = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Resources/IndexModule/Source.mdb";
            string strSQL = "SELECT * FROM UKAZ";
            OleDbConnection myConn = new OleDbConnection(strDSN);
            OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, myConn);
            myConn.Open();
            DataSet dtSet = new DataSet();
            myCmd.Fill(dtSet, "UKAZ");
            dt = dtSet.Tables[0];

            myConn.Close();

            foreach (DataRow row in dt.Rows)
            {

                lbName.Items.Add(row[1].ToString());

            }
            tc_index.SelectedTab = tp_list_sign;


        }

        private void lbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Item = lbName.SelectedItem.ToString();

            string search = "Шифр_истоchника = \'" + Item + "\'";

            DataRow[] foundRows = dt.Select(search);

            tb_cipher.Text = foundRows[0][1].ToString();
            tb_description.Text = foundRows[0][2].ToString();
            tb_synonym.Text = foundRows[0][3].ToString();
            tb_name_source.Text = foundRows[0][4].ToString();
            tb_author.Text = foundRows[0][5].ToString();
            tb_researcher.Text = foundRows[0][6].ToString();
            tb_date_source.Text = foundRows[0][7].ToString();
            tb_refind_date.Text = foundRows[0][8].ToString();
            tb_language.Text = foundRows[0][9].ToString();
            tb_translation.Text = foundRows[0][10].ToString();
            tb_publication.Text = foundRows[0][11].ToString();
            tb_other_list.Text = foundRows[0][12].ToString();
            tb_date_structure.Text = foundRows[0][13].ToString();
            tb_reprint.Text = foundRows[0][14].ToString();
            tb_storage.Text = foundRows[0][15].ToString();
            tb_note.Text = foundRows[0][16].ToString() + " ; " + foundRows[0][17].ToString();




            tc_index.SelectedTab = tp_sign;

        }

        private void bu_open_doc_Click(object sender, EventArgs e)
        {
            string filepath = "Resources/IndexModule/Input.docx";
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

            Table table = body.Elements<Table>().First();

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
                    pos_list.Add(text.IndexOf(sym));
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
    }
}
