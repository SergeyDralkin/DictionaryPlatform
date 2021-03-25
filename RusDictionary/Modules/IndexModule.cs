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
        DataTable dt = new DataTable();
        public IndexModule()
        {
            InitializeComponent();
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

            string search = "Шифр_истоchника = \'"+ Item + "\'";

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
            tb_note.Text = foundRows[0][16].ToString() +" ; "+ foundRows[0][17].ToString();




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


            string[] row1 = new string[] { "1", "см. = [синоним]." };
            dgv_rule.Rows.Add(row1);

            for (int i = 0; i < dgv_rule.Rows.Count; i++)
            {
                DataGridViewRow row = dgv_rule.Rows[i];

                string lbl_name = row.Cells[1].Value.ToString();


                string[] subs = lbl_name.Split('=');

                subs[0] = subs[0].Remove(subs[0].Length - 1);
                subs[1] = subs[1].Substring(1);



                bool containsSearchResult = dt.Rows[0].ItemArray[1].ToString().Contains(subs[0]);

                if (containsSearchResult)
                {
                    string res = dt.Rows[0].ItemArray[1].ToString().TrimStart(subs[0].ToCharArray());
                }

            }



        }
    }
}
