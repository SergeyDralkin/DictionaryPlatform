using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RusDictionary
{
    public partial class Search : Form
    {
        /// <summary>
        /// Состояние формы
        /// </summary>
        public bool ActiveFormSearch = false;
        public Search()
        {
            InitializeComponent();
        }

        private void Search_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ActiveFormSearch = false; //Состояние активации формы равно false
                e.Cancel = true; //Отменяем событие уничтожения формы
                Hide(); // Скрываем форму от пользователя
            }
        }

        private void buSearch_Click(object sender, EventArgs e)
        {
            //MySqlConnection conn = new MySqlConnection(MainForm.Connnect);
            try
            {
                // Подключаемся к БД
                //conn.Open();
                //StatusConnect = true;
            }
            catch
            {
                //StatusConnect = false;
            }
            //conn.Close();
        }
    }
}
