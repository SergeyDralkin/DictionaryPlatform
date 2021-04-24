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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buWordSearch_Read = new System.Windows.Forms.Button();
            this.tcWordSearch_Main = new System.Windows.Forms.TabControl();
            this.tpWordSearch_ReadFiles = new System.Windows.Forms.TabPage();
            this.buClearDETable = new System.Windows.Forms.Button();
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
            this.tpWordSearchMenu = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.buWordSearchScanFiles = new System.Windows.Forms.Button();
            this.buWordSearchFindWords = new System.Windows.Forms.Button();
            this.buFindWordsBack = new System.Windows.Forms.Button();
            this.buScanFilesBack = new System.Windows.Forms.Button();
            this.buWordSearchModuleToMenu = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tcWordSearch_Main.SuspendLayout();
            this.tpWordSearch_ReadFiles.SuspendLayout();
            this.tpWordSearch_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.tpWordSearchMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // buWordSearch_Read
            // 
            this.buWordSearch_Read.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buWordSearch_Read.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buWordSearch_Read.Location = new System.Drawing.Point(334, 106);
            this.buWordSearch_Read.Name = "buWordSearch_Read";
            this.buWordSearch_Read.Size = new System.Drawing.Size(335, 97);
            this.buWordSearch_Read.TabIndex = 0;
            this.buWordSearch_Read.Text = "Считывание файла";
            this.buWordSearch_Read.UseVisualStyleBackColor = true;
            this.buWordSearch_Read.Click += new System.EventHandler(this.buWordSearch_Read_Click);
            // 
            // tcWordSearch_Main
            // 
            this.tcWordSearch_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcWordSearch_Main.Controls.Add(this.tpWordSearchMenu);
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
            this.tpWordSearch_ReadFiles.Controls.Add(this.tableLayoutPanel9);
            this.tpWordSearch_ReadFiles.Location = new System.Drawing.Point(4, 22);
            this.tpWordSearch_ReadFiles.Name = "tpWordSearch_ReadFiles";
            this.tpWordSearch_ReadFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpWordSearch_ReadFiles.Size = new System.Drawing.Size(1010, 522);
            this.tpWordSearch_ReadFiles.TabIndex = 0;
            this.tpWordSearch_ReadFiles.Text = "Считывание файлов и заполнение БД";
            this.tpWordSearch_ReadFiles.UseVisualStyleBackColor = true;
            // 
            // buClearDETable
            // 
            this.buClearDETable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buClearDETable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buClearDETable.Location = new System.Drawing.Point(334, 209);
            this.buClearDETable.Name = "buClearDETable";
            this.buClearDETable.Size = new System.Drawing.Size(335, 97);
            this.buClearDETable.TabIndex = 1;
            this.buClearDETable.Text = "Очистить таблицу";
            this.buClearDETable.UseVisualStyleBackColor = true;
            this.buClearDETable.Click += new System.EventHandler(this.buClearDETable_Click);
            // 
            // tpWordSearch_Search
            // 
            this.tpWordSearch_Search.Controls.Add(this.tableLayoutPanel1);
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
            this.buAddComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buAddComplete.Enabled = false;
            this.buAddComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buAddComplete.Location = new System.Drawing.Point(297, 45);
            this.buAddComplete.Name = "buAddComplete";
            this.buAddComplete.Size = new System.Drawing.Size(104, 37);
            this.buAddComplete.TabIndex = 16;
            this.buAddComplete.Text = "Добавить статью";
            this.buAddComplete.UseVisualStyleBackColor = true;
            this.buAddComplete.Click += new System.EventHandler(this.buAddComplete_Click);
            // 
            // buAddEntry
            // 
            this.buAddEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buAddEntry.Enabled = false;
            this.buAddEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buAddEntry.Location = new System.Drawing.Point(297, 3);
            this.buAddEntry.Name = "buAddEntry";
            this.buAddEntry.Size = new System.Drawing.Size(104, 36);
            this.buAddEntry.TabIndex = 15;
            this.buAddEntry.Text = "Новая статья";
            this.buAddEntry.UseVisualStyleBackColor = true;
            this.buAddEntry.Click += new System.EventHandler(this.buAddEntry_Click);
            // 
            // buDeleteEntry
            // 
            this.buDeleteEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buDeleteEntry.Enabled = false;
            this.buDeleteEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buDeleteEntry.Location = new System.Drawing.Point(627, 45);
            this.buDeleteEntry.Name = "buDeleteEntry";
            this.buDeleteEntry.Size = new System.Drawing.Size(107, 37);
            this.buDeleteEntry.TabIndex = 14;
            this.buDeleteEntry.Text = "Удалить";
            this.buDeleteEntry.UseVisualStyleBackColor = true;
            this.buDeleteEntry.Click += new System.EventHandler(this.buDeleteEntry_Click);
            // 
            // buDeleteRow
            // 
            this.buDeleteRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buDeleteRow.Enabled = false;
            this.buDeleteRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buDeleteRow.Location = new System.Drawing.Point(517, 45);
            this.buDeleteRow.Name = "buDeleteRow";
            this.buDeleteRow.Size = new System.Drawing.Size(104, 37);
            this.buDeleteRow.TabIndex = 13;
            this.buDeleteRow.Text = "Удалить строку";
            this.buDeleteRow.UseVisualStyleBackColor = true;
            this.buDeleteRow.Click += new System.EventHandler(this.buDeleteRow_Click);
            // 
            // buAddRow
            // 
            this.buAddRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buAddRow.Enabled = false;
            this.buAddRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buAddRow.Location = new System.Drawing.Point(517, 3);
            this.buAddRow.Name = "buAddRow";
            this.buAddRow.Size = new System.Drawing.Size(104, 36);
            this.buAddRow.TabIndex = 12;
            this.buAddRow.Text = "Добавить строку";
            this.buAddRow.UseVisualStyleBackColor = true;
            this.buAddRow.Click += new System.EventHandler(this.buAddRow_Click);
            // 
            // buCancelChanges
            // 
            this.buCancelChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buCancelChanges.Enabled = false;
            this.buCancelChanges.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buCancelChanges.Location = new System.Drawing.Point(407, 3);
            this.buCancelChanges.Name = "buCancelChanges";
            this.buCancelChanges.Size = new System.Drawing.Size(104, 36);
            this.buCancelChanges.TabIndex = 11;
            this.buCancelChanges.Text = "Отменить";
            this.buCancelChanges.UseVisualStyleBackColor = true;
            this.buCancelChanges.Click += new System.EventHandler(this.buCancelChanges_Click);
            // 
            // buSaveEntry
            // 
            this.buSaveEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buSaveEntry.Enabled = false;
            this.buSaveEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buSaveEntry.Location = new System.Drawing.Point(627, 3);
            this.buSaveEntry.Name = "buSaveEntry";
            this.buSaveEntry.Size = new System.Drawing.Size(107, 36);
            this.buSaveEntry.TabIndex = 10;
            this.buSaveEntry.Text = "Сохранить";
            this.buSaveEntry.UseVisualStyleBackColor = true;
            this.buSaveEntry.Click += new System.EventHandler(this.buSaveEntry_Click);
            // 
            // buChangeEntry
            // 
            this.buChangeEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buChangeEntry.Enabled = false;
            this.buChangeEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buChangeEntry.Location = new System.Drawing.Point(407, 45);
            this.buChangeEntry.Name = "buChangeEntry";
            this.buChangeEntry.Size = new System.Drawing.Size(104, 37);
            this.buChangeEntry.TabIndex = 9;
            this.buChangeEntry.Text = "Редактировать";
            this.buChangeEntry.UseVisualStyleBackColor = true;
            this.buChangeEntry.Click += new System.EventHandler(this.buChangeEntry_Click);
            // 
            // lbMainWords
            // 
            this.lbMainWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMainWords.FormattingEnabled = true;
            this.lbMainWords.Location = new System.Drawing.Point(3, 3);
            this.lbMainWords.Name = "lbMainWords";
            this.lbMainWords.Size = new System.Drawing.Size(237, 401);
            this.lbMainWords.TabIndex = 8;
            this.lbMainWords.SelectedIndexChanged += new System.EventHandler(this.lbMainWords_SelectedIndexChanged);
            // 
            // cmbPage
            // 
            this.cmbPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPage.Enabled = false;
            this.cmbPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbPage.FormattingEnabled = true;
            this.cmbPage.Location = new System.Drawing.Point(85, 3);
            this.cmbPage.Name = "cmbPage";
            this.cmbPage.Size = new System.Drawing.Size(65, 21);
            this.cmbPage.TabIndex = 7;
            this.cmbPage.SelectedIndexChanged += new System.EventHandler(this.cmbPage_SelectedIndexChanged);
            // 
            // buPageNext
            // 
            this.buPageNext.Dock = System.Windows.Forms.DockStyle.Top;
            this.buPageNext.Enabled = false;
            this.buPageNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buPageNext.Location = new System.Drawing.Point(156, 3);
            this.buPageNext.Name = "buPageNext";
            this.buPageNext.Size = new System.Drawing.Size(78, 23);
            this.buPageNext.TabIndex = 6;
            this.buPageNext.Text = "Вперед";
            this.buPageNext.UseVisualStyleBackColor = true;
            this.buPageNext.Click += new System.EventHandler(this.buPageNext_Click);
            // 
            // buPageBack
            // 
            this.buPageBack.Dock = System.Windows.Forms.DockStyle.Top;
            this.buPageBack.Enabled = false;
            this.buPageBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buPageBack.Location = new System.Drawing.Point(3, 3);
            this.buPageBack.Name = "buPageBack";
            this.buPageBack.Size = new System.Drawing.Size(76, 23);
            this.buPageBack.TabIndex = 5;
            this.buPageBack.Text = "Назад";
            this.buPageBack.UseVisualStyleBackColor = true;
            this.buPageBack.Click += new System.EventHandler(this.buPageBack_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvResults.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(3, 3);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.Size = new System.Drawing.Size(737, 356);
            this.dgvResults.TabIndex = 4;
            // 
            // cbSearchType
            // 
            this.cbSearchType.AutoSize = true;
            this.cbSearchType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbSearchType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSearchType.Location = new System.Drawing.Point(351, 25);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(542, 17);
            this.cbSearchType.TabIndex = 3;
            this.cbSearchType.Text = "Точное соответствие";
            this.cbSearchType.UseVisualStyleBackColor = true;
            // 
            // buWordSearch_FindWord
            // 
            this.buWordSearch_FindWord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buWordSearch_FindWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buWordSearch_FindWord.Location = new System.Drawing.Point(252, 20);
            this.buWordSearch_FindWord.Name = "buWordSearch_FindWord";
            this.buWordSearch_FindWord.Size = new System.Drawing.Size(93, 22);
            this.buWordSearch_FindWord.TabIndex = 1;
            this.buWordSearch_FindWord.Text = "Поиск";
            this.buWordSearch_FindWord.UseVisualStyleBackColor = true;
            this.buWordSearch_FindWord.Click += new System.EventHandler(this.buWordSearch_FindWord_Click);
            // 
            // tbWordSearch_SearchingWord
            // 
            this.tbWordSearch_SearchingWord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbWordSearch_SearchingWord.Location = new System.Drawing.Point(3, 22);
            this.tbWordSearch_SearchingWord.Name = "tbWordSearch_SearchingWord";
            this.tbWordSearch_SearchingWord.Size = new System.Drawing.Size(243, 20);
            this.tbWordSearch_SearchingWord.TabIndex = 0;
            // 
            // tpWordSearchMenu
            // 
            this.tpWordSearchMenu.Controls.Add(this.tableLayoutPanel8);
            this.tpWordSearchMenu.Location = new System.Drawing.Point(4, 22);
            this.tpWordSearchMenu.Name = "tpWordSearchMenu";
            this.tpWordSearchMenu.Padding = new System.Windows.Forms.Padding(3);
            this.tpWordSearchMenu.Size = new System.Drawing.Size(1010, 522);
            this.tpWordSearchMenu.TabIndex = 2;
            this.tpWordSearchMenu.Text = "Меню";
            this.tpWordSearchMenu.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1004, 516);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Controls.Add(this.tbWordSearch_SearchingWord, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buWordSearch_FindWord, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbSearchType, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buFindWordsBack, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(998, 45);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 54);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(998, 459);
            this.tableLayoutPanel3.TabIndex = 18;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lbMainWords, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(243, 453);
            this.tableLayoutPanel4.TabIndex = 18;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel5.Controls.Add(this.buPageBack, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmbPage, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.buPageNext, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 410);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(237, 40);
            this.tableLayoutPanel5.TabIndex = 18;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.dgvResults, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(252, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(743, 453);
            this.tableLayoutPanel6.TabIndex = 18;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 5;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel7.Controls.Add(this.buAddEntry, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.buDeleteEntry, 4, 1);
            this.tableLayoutPanel7.Controls.Add(this.buAddComplete, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.buSaveEntry, 4, 0);
            this.tableLayoutPanel7.Controls.Add(this.buDeleteRow, 3, 1);
            this.tableLayoutPanel7.Controls.Add(this.buCancelChanges, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.buAddRow, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.buChangeEntry, 2, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 365);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(737, 85);
            this.tableLayoutPanel7.TabIndex = 18;
            // 
            // buWordSearchScanFiles
            // 
            this.buWordSearchScanFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buWordSearchScanFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buWordSearchScanFiles.Location = new System.Drawing.Point(334, 106);
            this.buWordSearchScanFiles.Name = "buWordSearchScanFiles";
            this.buWordSearchScanFiles.Size = new System.Drawing.Size(335, 97);
            this.buWordSearchScanFiles.TabIndex = 0;
            this.buWordSearchScanFiles.Text = "Считывание файлов словаря и занесение в БД";
            this.buWordSearchScanFiles.UseVisualStyleBackColor = true;
            this.buWordSearchScanFiles.Click += new System.EventHandler(this.buWordSearchScanFiles_Click);
            // 
            // buWordSearchFindWords
            // 
            this.buWordSearchFindWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buWordSearchFindWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buWordSearchFindWords.Location = new System.Drawing.Point(334, 209);
            this.buWordSearchFindWords.Name = "buWordSearchFindWords";
            this.buWordSearchFindWords.Size = new System.Drawing.Size(335, 97);
            this.buWordSearchFindWords.TabIndex = 1;
            this.buWordSearchFindWords.Text = "Поиск словарных статей";
            this.buWordSearchFindWords.UseVisualStyleBackColor = true;
            this.buWordSearchFindWords.Click += new System.EventHandler(this.buWordSearchFindWords_Click);
            // 
            // buFindWordsBack
            // 
            this.buFindWordsBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buFindWordsBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buFindWordsBack.Location = new System.Drawing.Point(899, 3);
            this.buFindWordsBack.Name = "buFindWordsBack";
            this.buFindWordsBack.Size = new System.Drawing.Size(96, 39);
            this.buFindWordsBack.TabIndex = 4;
            this.buFindWordsBack.Text = "Назад";
            this.buFindWordsBack.UseVisualStyleBackColor = true;
            this.buFindWordsBack.Click += new System.EventHandler(this.buFindWordsBack_Click);
            // 
            // buScanFilesBack
            // 
            this.buScanFilesBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buScanFilesBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buScanFilesBack.Location = new System.Drawing.Point(334, 312);
            this.buScanFilesBack.Name = "buScanFilesBack";
            this.buScanFilesBack.Size = new System.Drawing.Size(335, 97);
            this.buScanFilesBack.TabIndex = 2;
            this.buScanFilesBack.Text = "Назад";
            this.buScanFilesBack.UseVisualStyleBackColor = true;
            this.buScanFilesBack.Click += new System.EventHandler(this.buScanFilesBack_Click);
            // 
            // buWordSearchModuleToMenu
            // 
            this.buWordSearchModuleToMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buWordSearchModuleToMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buWordSearchModuleToMenu.Location = new System.Drawing.Point(334, 312);
            this.buWordSearchModuleToMenu.Name = "buWordSearchModuleToMenu";
            this.buWordSearchModuleToMenu.Size = new System.Drawing.Size(335, 97);
            this.buWordSearchModuleToMenu.TabIndex = 2;
            this.buWordSearchModuleToMenu.Text = "Назад в меню";
            this.buWordSearchModuleToMenu.UseVisualStyleBackColor = true;
            this.buWordSearchModuleToMenu.Click += new System.EventHandler(this.buWordSearchModuleToMenu_Click);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel8.Controls.Add(this.buWordSearchScanFiles, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.buWordSearchModuleToMenu, 1, 3);
            this.tableLayoutPanel8.Controls.Add(this.buWordSearchFindWords, 1, 2);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 5;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(1004, 516);
            this.tableLayoutPanel8.TabIndex = 3;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel9.Controls.Add(this.buWordSearch_Read, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.buClearDETable, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.buScanFilesBack, 1, 3);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 5;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1004, 516);
            this.tableLayoutPanel9.TabIndex = 4;
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.tpWordSearchMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
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
        private System.Windows.Forms.Button buClearDETable;
        private System.Windows.Forms.TabPage tpWordSearchMenu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button buWordSearchFindWords;
        private System.Windows.Forms.Button buWordSearchScanFiles;
        private System.Windows.Forms.Button buFindWordsBack;
        private System.Windows.Forms.Button buScanFilesBack;
        private System.Windows.Forms.Button buWordSearchModuleToMenu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    }
}
