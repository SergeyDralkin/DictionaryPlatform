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
            this.SuspendLayout();
            // 
            // buWordSearch_Read
            // 
            this.buWordSearch_Read.Location = new System.Drawing.Point(37, 78);
            this.buWordSearch_Read.Name = "buWordSearch_Read";
            this.buWordSearch_Read.Size = new System.Drawing.Size(75, 23);
            this.buWordSearch_Read.TabIndex = 0;
            this.buWordSearch_Read.Text = "button1";
            this.buWordSearch_Read.UseVisualStyleBackColor = true;
            this.buWordSearch_Read.Click += new System.EventHandler(this.buWordSearch_Read_Click);
            // 
            // tbWordSearch_Text
            // 
            this.tbWordSearch_Text.Location = new System.Drawing.Point(296, 78);
            this.tbWordSearch_Text.Multiline = true;
            this.tbWordSearch_Text.Name = "tbWordSearch_Text";
            this.tbWordSearch_Text.Size = new System.Drawing.Size(383, 334);
            this.tbWordSearch_Text.TabIndex = 1;
            // 
            // WordSearchModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbWordSearch_Text);
            this.Controls.Add(this.buWordSearch_Read);
            this.Name = "WordSearchModule";
            this.Size = new System.Drawing.Size(1024, 554);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buWordSearch_Read;
        private System.Windows.Forms.TextBox tbWordSearch_Text;
    }
}
