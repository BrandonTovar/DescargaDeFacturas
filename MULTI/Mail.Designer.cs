namespace MULTI
{
    partial class Mail
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
            this.btnSend = new System.Windows.Forms.Button();
            this.eTo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.eSubject = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.eContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(319, 303);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Enviar";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSendMail);
            // 
            // eTo
            // 
            this.eTo.Location = new System.Drawing.Point(15, 25);
            this.eTo.Name = "eTo";
            this.eTo.Size = new System.Drawing.Size(378, 20);
            this.eTo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Para:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Asunto:";
            // 
            // eSubject
            // 
            this.eSubject.Location = new System.Drawing.Point(15, 64);
            this.eSubject.Name = "eSubject";
            this.eSubject.Size = new System.Drawing.Size(378, 20);
            this.eSubject.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Contenido:";
            // 
            // eContent
            // 
            this.eContent.Location = new System.Drawing.Point(15, 103);
            this.eContent.Multiline = true;
            this.eContent.Name = "eContent";
            this.eContent.Size = new System.Drawing.Size(378, 194);
            this.eContent.TabIndex = 5;
            // 
            // Mail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 339);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.eContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.eSubject);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eTo);
            this.Controls.Add(this.btnSend);
            this.MaximizeBox = false;
            this.Name = "Mail";
            this.ShowIcon = false;
            this.Text = "Configuracion de correo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox eTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox eSubject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox eContent;
    }
}