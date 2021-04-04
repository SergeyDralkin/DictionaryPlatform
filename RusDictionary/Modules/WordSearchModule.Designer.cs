namespace RusDictionary.Modules
{
    partial class WordSearchModule
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buWordSearch_Read = new System.Windows.Forms.Button();
            this.tcWordSearch_Main = new System.Windows.Forms.TabControl();
            this.tpWordSearch_ReadFiles = new System.Windows.Forms.TabPage();
            this.tpWordSearch_Search = new System.Windows.Forms.TabPage();
            this.buAddComplete = new System.Windows.Forms.Button();
            this.buAddEntry = new System.Windows.Forms.Button();
            this.buDeleteEntry = new System.Windows.Forms.Button();
            this.buDeleteRow = new System.Windows.Forms.Button();
            this.buAddRow = new System.Windows.Forms.Button();
            this.buCancelChanges = new System.Windows.Forms.Button();
            this.buSaveEntry = new System.Windows.Forms.Button();
            this.buChangeEntry = new System.Windows.Forms.Button();
            this.lbMainWords = new System.Windows.Forms.ListBox();
            this.cmbPage = new System.Windows.Forms.ComboBox();
            this.buPageNext = new System.Windows.Forms.Button();
            this.buPageBack = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.cbSearchType = new System.Windows.Forms.CheckBox();
            this.buWordSearch_FindWord = new System.Windows.Forms.Button();
            this.tbWordSearch_SearchingWord = new System.Windows.Forms.TextBox();
            this.tcWordSearch_Main.SuspendLayout();
            this.tpWordSearch_ReadFiles.SuspendLayout();
            this.tpWordSearch_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // buWordSearch_Read
            // 
            this.buWordSearch_Read.Location = new System.Drawing.Point(35, 31);
            this.buWordSearch_Read.Name = "buWordSearch_Read";
            this.buWordSearch_Read.Size = new System.Drawing.Size(128, 50);
            this.buWordSearch_Read.TabIndex = 0;
            this.buWordSearch_Read.Text = "Считывание файла";
            this.buWordSearch_Read.UseVisualStyleBackColor = true;
            this.buWordSearch_Read.Click += new System.EventHandler(this.buWordSearch_Read_Click);
            // 
            // tcWordSearch_Main
            // 
            this.tcWordSearch_Main.Controls.Add(this.tpWordSearch_ReadFiles);
            this.tcWordSearch_Main.Controls.Add(this.tpWordSearch_Search);
            this.tcWordSearch_Main.Location = new System.Drawing.Point(3, 3);
            this.tcWordSearch_Main.Name = "tcWordSearch_Main";
            this.tcWordSearch_Main.SelectedIndex = 0;
            this.tcWordSearch_Main.Size = new System.Drawing.Size(1018, 548);
            this.tcWordSearch_Main.TabIndex = 2;
            // 
            // tpWordSearch_ReadFiles
            // 
            this.tpWordSearch_ReadFiles.Controls.Add(this.buWordSearch_Read);
            this.tpWordSearch_ReadFiles.Location = new System.Drawing.Point(4, 22);
            this.tpWordSearch_ReadFiles.Name = "tpWordSearch_ReadFiles";
            this.tpWordSearch_ReadFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpWordSearch_ReadFiles.Size = new System.Drawing.Size(1010, 522);
            this.tpWordSearch_ReadFiles.TabIndex = 0;
            this.tpWordSearch_ReadFiles.Text = "Считывание файлов и заполнение БД";
            this.tpWordSearch_ReadFiles.UseVisualStyleBackColor = true;
            // 
            // tpWordSearch_Search
            // 
            this.tpWordSearch_Search.Controls.Add(this.buAddComplete);
            this.tpWordSearch_Search.Controls.Add(this.buAddEntry);
            this.tpWordSearch_Search.Controls.Add(this.buDeleteEntry);
            this.tpWordSearch_Search.Controls.Add(this.buDeleteRow);
            this.tpWordSearch_Search.Controls.Add(this.buAddRow);
            this.tpWordSearch_Search.Controls.Add(this.buCancelChanges);
            this.tpWordSearch_Search.Controls.Add(this.buSaveEntry);
            this.tpWordSearch_Search.Controls.Add(this.buChangeEntry);
            this.tpWordSearch_Search.Controls.Add(this.lbMainWords);
            this.tpWordSearch_Search.Controls.Add(this.cmbPage);
            this.tpWordSearch_Search.Controls.Add(this.buPageNext);
            this.tpWordSearch_Search.Controls.Add(this.buPageBack);
            this.tpWordSearch_Search.Controls.Add(this.dgvResults);
            this.tpWordSearch_Search.Controls.Add(this.cbSearchType);
            this.tpWordSearch_Search.Controls.Add(this.buWordSearch_FindWord);
            this.tpWordSearch_Search.Controls.Add(this.tbWordSearch_SearchingWord);
            this.tpWordSearch_Search.Location = new System.Drawing.Point(4, 22);
            this.tpWordSearch_Search.Name = "tpWordSearch_Search";
            this.tpWordSearch_Search.Padding = new System.Windows.Forms.Padding(3);
            this.tpWordSearch_Search.Size = new System.Drawing.Size(1010, 522);
            this.tpWordSearch_Search.TabIndex = 1;
            this.tpWordSearch_Search.Text = "Поиск слов по БД";
            this.tpWordSearch_Search.UseVisualStyleBackColor = true;
            // 
            // buAddComplete
            // 
            this.buAddComplete.Enabled = false;
            this.buAddComplete.Location = new System.Drawing.Point(282, 478);
            this.buAddComplete.Name = "buAddComplete";
            this.buAddComplete.Size = new System.Drawing.Size(101, 35);
            this.buAddComplete.TabIndex = 16;
            this.buAddComplete.Text = "Добавить статью";
            this.buAddComplete.UseVisualStyleBackColor = true;
            this.buAddComplete.Click += new System.EventHandler(this.buAddComplete_Click);
            // 
            // buAddEntry
            // 
            this.buAddEntry.Enabled = false;
            this.buAddEntry.Location = new System.Drawing.Point(282, 437);
            this.buAddEntry.Name = "buAddEntry";
            this.buAddEntry.Size = new System.Drawing.Size(101, 35);
            this.buAddEntry.TabIndex = 15;
            this.buAddEntry.Text = "Новая статья";
            this.buAddEntry.UseVisualStyleBackColor = true;
            this.buAddEntry.Click += new System.EventHandler(this.buAddEntry_Click);
            // 
            // buDeleteEntry
            // 
            this.buDeleteEntry.Enabled = false;
            this.buDeleteEntry.Location = new System.Drawing.Point(929, 466);
            this.buDeleteEntry.Name = "buDeleteEntry";
            this.buDeleteEntry.Size = new System.Drawing.Size(75, 23);
            this.buDeleteEntry.TabIndex = 14;
            this.buDeleteEntry.Text = "Удалить";
            this.buDeleteEntry.UseVisualStyleBackColor = true;
            this.buDeleteEntry.Click += new System.EventHandler(this.buDeleteEntry_Click);
            // 
            // buDeleteRow
            // 
            this.buDeleteRow.Enabled = false;
            this.buDeleteRow.Location = new System.Drawing.Point(818, 466);
            this.buDeleteRow.Name = "buDeleteRow";
            this.buDeleteRow.Size = new System.Drawing.Size(105, 23);
            this.buDeleteRow.TabIndex = 13;
            this.buDeleteRow.Text = "Удалить строку";
            this.buDeleteRow.UseVisualStyleBackColor = true;
            this.buDeleteRow.Click += new System.EventHandler(this.buDeleteRow_Click);
            // 
            // buAddRow
            // 
            this.buAddRow.Enabled = false;
            this.buAddRow.Location = new System.Drawing.Point(818, 437);
            this.buAddRow.Name = "buAddRow";
            this.buAddRow.Size = new System.Drawing.Size(105, 23);
            this.buAddRow.TabIndex = 12;
            this.buAddRow.Text = "Добавить строку";
            this.buAddRow.UseVisualStyleBackColor = true;
            this.buAddRow.Click += new System.EventHandler(this.buAddRow_Click);
            // 
            // buCancelChanges
            // 
            this.buCancelChanges.Enabled = false;
            this.buCancelChanges.Location = new System.Drawing.Point(737, 495);
            this.buCancelChanges.Name = "buCancelChanges";
            this.buCancelChanges.Size = new System.Drawing.Size(75, 23);
            this.buCancelChanges.TabIndex = 11;
            this.buCancelChanges.Text = "Отменить";
            this.buCancelChanges.UseVisualStyleBackColor = true;
            this.buCancelChanges.Click += new System.EventHandler(this.buCancelChanges_Click);
            // 
            // buSaveEntry
            // 
            this.buSaveEntry.Enabled = false;
            this.buSaveEntry.Location = new System.Drawing.Point(929, 495);
            this.buSaveEntry.Name = "buSaveEntry";
            this.buSaveEntry.Size = new System.Drawing.Size(75, 23);
            this.buSaveEntry.TabIndex = 10;
            this.buSaveEntry.Text = "Сохранить";
            this.buSaveEntry.UseVisualStyleBackColor = true;
            this.buSaveEntry.Click += new System.EventHandler(this.buSaveEntry_Click);
            // 
            // buChangeEntry
            // 
            this.buChangeEntry.Enabled = false;
            this.buChangeEntry.Location = new System.Drawing.Point(818, 495);
            this.buChangeEntry.Name = "buChangeEntry";
            this.buChangeEntry.Size = new System.Drawing.Size(105, 23);
            this.buChangeEntry.TabIndex = 9;
            this.buChangeEntry.Text = "Редактировать";
            this.buChangeEntry.UseVisualStyleBackColor = true;
            this.buChangeEntry.Click += new System.EventHandler(this.buChangeEntry_Click);
            // 
            // lbMainWords
            // 
            this.lbMainWords.FormattingEnabled = true;
            this.lbMainWords.Location = new System.Drawing.Point(6, 33);
            this.lbMainWords.Name = "lbMainWords";
            this.lbMainWords.Size = new System.Drawing.Size(270, 459);
            this.lbMainWords.TabIndex = 8;
            this.lbMainWords.SelectedIndexChanged += new System.EventHandler(this.lbMainWords_SelectedIndexChanged);
            // 
            // cmbPage
            // 
            this.cmbPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPage.Enabled = false;
            this.cmbPage.FormattingEnabled = true;
            this.cmbPage.Location = new System.Drawing.Point(87, 495);
            this.cmbPage.Name = "cmbPage";
            this.cmbPage.Size = new System.Drawing.Size(108, 21);
            this.cmbPage.TabIndex = 7;
            this.cmbPage.SelectedIndexChanged += new System.EventHandler(this.cmbPage_SelectedIndexChanged);
            // 
            // buPageNext
            // 
            this.buPageNext.Enabled = false;
            this.buPageNext.Location = new System.Drawing.Point(201, 493);
            this.buPageNext.Name = "buPageNext";
            this.buPageNext.Size = new System.Drawing.Size(75, 23);
            this.buPageNext.TabIndex = 6;
            this.buPageNext.Text = "Вперед";
            this.buPageNext.UseVisualStyleBackColor = true;
            this.buPageNext.Click += new System.EventHandler(this.buPageNext_Click);
            // 
            // buPageBack
            // 
            this.buPageBack.Enabled = false;
            this.buPageBack.Location = new System.Drawing.Point(6, 493);
            this.buPageBack.Name = "buPageBack";
            this.buPageBack.Size = new System.Drawing.Size(75, 23);
            this.buPageBack.TabIndex = 5;
            this.buPageBack.Text = "Назад";
            this.buPageBack.UseVisualStyleBackColor = true;
            this.buPageBack.Click += new System.EventHandler(this.buPageBack_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvResults.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvResults.Location = new System.Drawing.Point(282, 33);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.Size = new System.Drawing.Size(722, 398);
            this.dgvResults.TabIndex = 4;
            // 
            // cbSearchType
            // 
            this.cbSearchType.AutoSize = true;
            this.cbSearchType.Location = new System.Drawing.Point(282, 8);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(134, 17);
            this.cbSearchType.TabIndex = 3;
            this.cbSearchType.Text = "Точное соответствие";
            this.cbSearchType.UseVisualStyleBackColor = true;
            // 
            // buWordSearch_FindWord
            // 
            this.buWordSearch_FindWord.Location = new System.Drawing.Point(201, 4);
            this.buWordSearch_FindWord.Name = "buWordSearch_FindWord";
            this.buWordSearch_FindWord.Size = new System.Drawing.Size(75, 23);
            this.buWordSearch_FindWord.TabIndex = 1;
            this.buWordSearch_FindWord.Text = "Поиск";
            this.buWordSearch_FindWord.UseVisualStyleBackColor = true;
            this.buWordSearch_FindWord.Click += new System.EventHandler(this.buWordSearch_FindWord_Click);
            // 
            // tbWordSearch_SearchingWord
            // 
            this.tbWordSearch_SearchingWord.Location = new System.Drawing.Point(6, 6);
            this.tbWordSearch_SearchingWord.Name = "tbWordSearch_SearchingWord";
            this.tbWordSearch_SearchingWord.Size = new System.Drawing.Size(189, 20);
            this.tbWordSearch_SearchingWord.TabIndex = 0;
            // 
            // WordSearchModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcWordSearch_Main);
            this.Name = "WordSearchModule";
            this.Size = new System.Drawing.Size(1024, 554);
            this.tcWordSearch_Main.ResumeLayout(false);
            this.tpWordSearch_ReadFiles.ResumeLayout(false);
            this.tpWordSearch_Search.ResumeLayout(false);
            this.tpWordSearch_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buWordSearch_Read;
        private System.Windows.Forms.TabControl tcWordSearch_Main;
        private System.Windows.Forms.TabPage tpWordSearch_ReadFiles;
        private System.Windows.Forms.TabPage tpWordSearch_Search;
        private System.Windows.Forms.Button buWordSearch_FindWord;
        private System.Windows.Forms.TextBox tbWordSearch_SearchingWord;
        private System.Windows.Forms.CheckBox cbSearchType;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Button buPageNext;
        private System.Windows.Forms.Button buPageBack;
        private System.Windows.Forms.ComboBox cmbPage;
        private System.Windows.Forms.ListBox lbMainWords;
        private System.Windows.Forms.Button buSaveEntry;
        private System.Windows.Forms.Button buChangeEntry;
        private System.Windows.Forms.Button buCancelChanges;
        private System.Windows.Forms.Button buDeleteRow;
        private System.Windows.Forms.Button buAddRow;
        private System.Windows.Forms.Button buDeleteEntry;
        private System.Windows.Forms.Button buAddEntry;
        private System.Windows.Forms.Button buAddComplete;
    }
}
