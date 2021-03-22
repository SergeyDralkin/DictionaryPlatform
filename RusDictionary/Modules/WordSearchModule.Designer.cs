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
            this.buWordSearch_Read = new System.Windows.Forms.Button();
            this.tbWordSearch_Text = new System.Windows.Forms.TextBox();
            this.tcWordSearch_Main = new System.Windows.Forms.TabControl();
            this.tpWordSearch_ReadFiles = new System.Windows.Forms.TabPage();
            this.tpWordSearch_Search = new System.Windows.Forms.TabPage();
            this.tbWordSearch_FindedWords = new System.Windows.Forms.TextBox();
            this.buWordSearch_FindWord = new System.Windows.Forms.Button();
            this.tbWordSearch_SearchingWord = new System.Windows.Forms.TextBox();
            this.cbSearchType = new System.Windows.Forms.CheckBox();
            this.tcWordSearch_Main.SuspendLayout();
            this.tpWordSearch_ReadFiles.SuspendLayout();
            this.tpWordSearch_Search.SuspendLayout();
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
            // tbWordSearch_Text
            // 
            this.tbWordSearch_Text.Location = new System.Drawing.Point(35, 100);
            this.tbWordSearch_Text.Multiline = true;
            this.tbWordSearch_Text.Name = "tbWordSearch_Text";
            this.tbWordSearch_Text.Size = new System.Drawing.Size(246, 174);
            this.tbWordSearch_Text.TabIndex = 1;
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
            this.tpWordSearch_ReadFiles.Controls.Add(this.tbWordSearch_Text);
            this.tpWordSearch_ReadFiles.Controls.Add(this.buWordSearch_Read);
            this.tpWordSearch_ReadFiles.Location = new System.Drawing.Point(4, 22);
            this.tpWordSearch_ReadFiles.Name = "tpWordSearch_ReadFiles";
            this.tpWordSearch_ReadFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpWordSearch_ReadFiles.Size = new System.Drawing.Size(1010, 522);
            this.tpWordSearch_ReadFiles.TabIndex = 0;
            this.tpWordSearch_ReadFiles.Text = "Считывание файлов";
            this.tpWordSearch_ReadFiles.UseVisualStyleBackColor = true;
            // 
            // tpWordSearch_Search
            // 
            this.tpWordSearch_Search.Controls.Add(this.cbSearchType);
            this.tpWordSearch_Search.Controls.Add(this.tbWordSearch_FindedWords);
            this.tpWordSearch_Search.Controls.Add(this.buWordSearch_FindWord);
            this.tpWordSearch_Search.Controls.Add(this.tbWordSearch_SearchingWord);
            this.tpWordSearch_Search.Location = new System.Drawing.Point(4, 22);
            this.tpWordSearch_Search.Name = "tpWordSearch_Search";
            this.tpWordSearch_Search.Padding = new System.Windows.Forms.Padding(3);
            this.tpWordSearch_Search.Size = new System.Drawing.Size(1010, 522);
            this.tpWordSearch_Search.TabIndex = 1;
            this.tpWordSearch_Search.Text = "Поиск слов";
            this.tpWordSearch_Search.UseVisualStyleBackColor = true;
            // 
            // tbWordSearch_FindedWords
            // 
            this.tbWordSearch_FindedWords.Location = new System.Drawing.Point(6, 113);
            this.tbWordSearch_FindedWords.Multiline = true;
            this.tbWordSearch_FindedWords.Name = "tbWordSearch_FindedWords";
            this.tbWordSearch_FindedWords.ReadOnly = true;
            this.tbWordSearch_FindedWords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbWordSearch_FindedWords.Size = new System.Drawing.Size(790, 403);
            this.tbWordSearch_FindedWords.TabIndex = 2;
            // 
            // buWordSearch_FindWord
            // 
            this.buWordSearch_FindWord.Location = new System.Drawing.Point(302, 61);
            this.buWordSearch_FindWord.Name = "buWordSearch_FindWord";
            this.buWordSearch_FindWord.Size = new System.Drawing.Size(75, 23);
            this.buWordSearch_FindWord.TabIndex = 1;
            this.buWordSearch_FindWord.Text = "Поиск";
            this.buWordSearch_FindWord.UseVisualStyleBackColor = true;
            this.buWordSearch_FindWord.Click += new System.EventHandler(this.buWordSearch_FindWord_Click);
            // 
            // tbWordSearch_SearchingWord
            // 
            this.tbWordSearch_SearchingWord.Location = new System.Drawing.Point(107, 64);
            this.tbWordSearch_SearchingWord.Name = "tbWordSearch_SearchingWord";
            this.tbWordSearch_SearchingWord.Size = new System.Drawing.Size(189, 20);
            this.tbWordSearch_SearchingWord.TabIndex = 0;
            // 
            // cbSearchType
            // 
            this.cbSearchType.AutoSize = true;
            this.cbSearchType.Location = new System.Drawing.Point(383, 64);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(134, 17);
            this.cbSearchType.TabIndex = 3;
            this.cbSearchType.Text = "Точное соответствие";
            this.cbSearchType.UseVisualStyleBackColor = true;
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
            this.tpWordSearch_ReadFiles.PerformLayout();
            this.tpWordSearch_Search.ResumeLayout(false);
            this.tpWordSearch_Search.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buWordSearch_Read;
        private System.Windows.Forms.TextBox tbWordSearch_Text;
        private System.Windows.Forms.TabControl tcWordSearch_Main;
        private System.Windows.Forms.TabPage tpWordSearch_ReadFiles;
        private System.Windows.Forms.TabPage tpWordSearch_Search;
        private System.Windows.Forms.TextBox tbWordSearch_FindedWords;
        private System.Windows.Forms.Button buWordSearch_FindWord;
        private System.Windows.Forms.TextBox tbWordSearch_SearchingWord;
        private System.Windows.Forms.CheckBox cbSearchType;
    }
}
