using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Data.OleDb;

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
            string strDSN = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = Resources/Source.accdb";
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
    }
}
