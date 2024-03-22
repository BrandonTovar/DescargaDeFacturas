namespace MULTI
{
    partial class Config
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.tbxPass = new System.Windows.Forms.TextBox();
            this.tbxUser = new System.Windows.Forms.TextBox();
            this.cbxBaseData = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxServer = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lFiles = new System.Windows.Forms.ListView();
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.tbxType = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbxPathFile = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.lItemMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.lItemMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 100;
            this.label1.Text = "Servidor:";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.tbxPass);
            this.groupBox1.Controls.Add(this.tbxUser);
            this.groupBox1.Controls.Add(this.cbxBaseData);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbxServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 175);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Base de datos";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(267, 133);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(105, 23);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Probar Conexión";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.Test);
            // 
            // tbxPass
            // 
            this.tbxPass.Location = new System.Drawing.Point(92, 78);
            this.tbxPass.Name = "tbxPass";
            this.tbxPass.Size = new System.Drawing.Size(281, 20);
            this.tbxPass.TabIndex = 2;
            // 
            // tbxUser
            // 
            this.tbxUser.Location = new System.Drawing.Point(92, 52);
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.Size = new System.Drawing.Size(281, 20);
            this.tbxUser.TabIndex = 1;
            // 
            // cbxBaseData
            // 
            this.cbxBaseData.FormattingEnabled = true;
            this.cbxBaseData.Location = new System.Drawing.Point(92, 106);
            this.cbxBaseData.Name = "cbxBaseData";
            this.cbxBaseData.Size = new System.Drawing.Size(281, 21);
            this.cbxBaseData.TabIndex = 3;
            this.cbxBaseData.DropDown += new System.EventHandler(this.GetDataBases);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 100;
            this.label4.Text = "Base de datos: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 100;
            this.label3.Text = "Contraseña:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 100;
            this.label2.Text = "Usuario:";
            // 
            // cbxServer
            // 
            this.cbxServer.FormattingEnabled = true;
            this.cbxServer.Location = new System.Drawing.Point(92, 25);
            this.cbxServer.Name = "cbxServer";
            this.cbxServer.Size = new System.Drawing.Size(281, 21);
            this.cbxServer.TabIndex = 0;
            this.cbxServer.DropDown += new System.EventHandler(this.GetServers);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.lFiles);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbxType);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.tbxPathFile);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Location = new System.Drawing.Point(12, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 263);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layouts";
            // 
            // lFiles
            // 
            this.lFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.type,
            this.path});
            this.lFiles.FullRowSelect = true;
            this.lFiles.Location = new System.Drawing.Point(6, 71);
            this.lFiles.Name = "lFiles";
            this.lFiles.Size = new System.Drawing.Size(365, 173);
            this.lFiles.TabIndex = 100;
            this.lFiles.TabStop = false;
            this.lFiles.UseCompatibleStateImageBehavior = false;
            this.lFiles.View = System.Windows.Forms.View.Details;
            this.lFiles.LostFocus += new System.EventHandler(this.OutOfForm);
            this.lFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listSelect);
            // 
            // type
            // 
            this.type.Text = "Documento";
            this.type.Width = 110;
            // 
            // path
            // 
            this.path.Text = "Archivo";
            this.path.Width = 251;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Tipo/Documento:";
            // 
            // tbxType
            // 
            this.tbxType.Location = new System.Drawing.Point(101, 19);
            this.tbxType.Name = "tbxType";
            this.tbxType.Size = new System.Drawing.Size(270, 20);
            this.tbxType.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(313, 44);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(58, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Agregar";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.AddRPT);
            // 
            // tbxPathFile
            // 
            this.tbxPathFile.Location = new System.Drawing.Point(6, 45);
            this.tbxPathFile.Name = "tbxPathFile";
            this.tbxPathFile.Size = new System.Drawing.Size(270, 20);
            this.tbxPathFile.TabIndex = 100;
            this.tbxPathFile.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(284, 44);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(23, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "...";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.SearchRPT);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(337, 470);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(58, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Guardar";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Guardar);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(273, 470);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Cancel);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(209, 470);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(58, 23);
            this.btnValidate.TabIndex = 2;
            this.btnValidate.Text = "Validar";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.Verify);
            // 
            // lItemMenu
            // 
            this.lItemMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarToolStripMenuItem});
            this.lItemMenu.Name = "lItemMenu";
            this.lItemMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.eliminarToolStripMenuItem.Text = "Eliminar";
            // 
            // chose
            // 
            this.chose.Location = new System.Drawing.Point(0, 0);
            this.chose.Name = "chose";
            this.chose.Size = new System.Drawing.Size(25, 20);
            this.chose.TabIndex = 0;
            this.chose.Text = "...";
            this.chose.UseVisualStyleBackColor = true;
            this.chose.Click += new System.EventHandler(this.ChangePath);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 505);
            this.ControlBox = false;
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(423, 544);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(423, 544);
            this.Name = "Config";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Configuración";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.lItemMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button chose;
        private System.Windows.Forms.TextBox tbxPass;
        private System.Windows.Forms.TextBox tbxUser;
        private System.Windows.Forms.ComboBox cbxBaseData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxServer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbxPathFile;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxType;
        private System.Windows.Forms.ListView lFiles;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader path;
        private System.Windows.Forms.ContextMenuStrip lItemMenu;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem;
    }
}

