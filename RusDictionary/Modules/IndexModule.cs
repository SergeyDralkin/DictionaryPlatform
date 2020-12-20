using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace RusDictionary.Modules
{
    public partial class IndexModule : UserControl
    {
        public IndexModule()
        {
            InitializeComponent();
        }
        public class Foo
        {
            public string SORT { get; set; }
            //public string Developer { get; set; }
        }

        private void buIndexSource_Click(object sender, EventArgs e)
        {
            string pathCsvFile = "Resources/Ukaz_23a.csv";

            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(pathCsvFile, Encoding.Default))
            {
                string[] headers = sr.ReadLine().Split('\t');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split('\t');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < rows.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }

            foreach (DataRow row in dt.Rows)
            {

                lbName.Items.Add(row[1].ToString());
                
            }


        }
    }
}
