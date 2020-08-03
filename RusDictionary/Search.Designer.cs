namespace RusDictionary
{
    partial class Search
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbText = new System.Windows.Forms.TextBox();
            this.buSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(12, 12);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(349, 20);
            this.tbText.TabIndex = 3;
            // 
            // buSearch
            // 
            this.buSearch.Location = new System.Drawing.Point(230, 38);
            this.buSearch.Name = "buSearch";
            this.buSearch.Size = new System.Drawing.Size(131, 23);
            this.buSearch.TabIndex = 2;
            this.buSearch.Text = "Найти";
            this.buSearch.UseVisualStyleBackColor = true;
            this.buSearch.Click += new System.EventHandler(this.buSearch_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 71);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.buSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(389, 110);
            this.MinimumSize = new System.Drawing.Size(389, 110);
            this.Name = "Search";
            this.Text = "Поиск";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Search_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button buSearch;
    }
}