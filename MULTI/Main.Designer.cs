namespace MULTI
{
    partial class main
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.eFolio = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.eDate = new System.Windows.Forms.DateTimePicker();
            this.cbxType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.eFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFolder = new System.Windows.Forms.Button();
            this.chbxAll = new System.Windows.Forms.CheckBox();
            this.selectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.lResults = new System.Windows.Forms.ListView();
            this.select = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.docEntry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.cbxDataSource = new System.Windows.Forms.ComboBox();
            this.eSeries = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.btnReload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // eFolio
            // 
            this.eFolio.Location = new System.Drawing.Point(472, 12);
            this.eFolio.Name = "eFolio";
            this.eFolio.Size = new System.Drawing.Size(100, 20);
            this.eFolio.TabIndex = 0;
            this.eFolio.TextChanged += new System.EventHandler(this.NoPaste);
            this.eFolio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IsNumber);
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(497, 83);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.Search);
            // 
            // eDate
            // 
            this.eDate.CustomFormat = "DD/MM/YYYY";
            this.eDate.Location = new System.Drawing.Point(366, 38);
            this.eDate.Name = "eDate";
            this.eDate.Size = new System.Drawing.Size(206, 20);
            this.eDate.TabIndex = 4;
            // 
            // cbxType
            // 
            this.cbxType.FormattingEnabled = true;
            this.cbxType.Location = new System.Drawing.Point(13, 85);
            this.cbxType.Name = "cbxType";
            this.cbxType.Size = new System.Drawing.Size(185, 21);
            this.cbxType.TabIndex = 6;
            this.cbxType.SelectedIndexChanged += new System.EventHandler(this.SelectType);
            this.cbxType.TextChanged += new System.EventHandler(this.SupressPast);
            this.cbxType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SupressKey);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Tipo de documento:";
            // 
            // btnPrint
            // 
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(513, 429);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(59, 23);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Guardar";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Export);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(448, 429);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(59, 23);
            this.btnSend.TabIndex = 10;
            this.btnSend.Text = "Enviar";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.viewMailDialog);
            // 
            // btnView
            // 
            this.btnView.Enabled = false;
            this.btnView.Location = new System.Drawing.Point(376, 429);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(66, 23);
            this.btnView.TabIndex = 11;
            this.btnView.Text = "Visualizar";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Visible = false;
            this.btnView.Click += new System.EventHandler(this.ViewReport);
            // 
            // eFolder
            // 
            this.eFolder.Location = new System.Drawing.Point(12, 429);
            this.eFolder.Name = "eFolder";
            this.eFolder.Size = new System.Drawing.Size(185, 20);
            this.eFolder.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(458, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "-";
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(203, 427);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(26, 23);
            this.btnFolder.TabIndex = 16;
            this.btnFolder.Text = "...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.getFolder);
            // 
            // chbxAll
            // 
            this.chbxAll.AutoSize = true;
            this.chbxAll.Location = new System.Drawing.Point(319, 433);
            this.chbxAll.Name = "chbxAll";
            this.chbxAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chbxAll.Size = new System.Drawing.Size(51, 17);
            this.chbxAll.TabIndex = 19;
            this.chbxAll.Text = "Todo";
            this.chbxAll.UseVisualStyleBackColor = true;
            this.chbxAll.Click += new System.EventHandler(this.SelectAll);
            // 
            // lResults
            // 
            this.lResults.CheckBoxes = true;
            this.lResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.select,
            this.nm,
            this.date,
            this.info,
            this.docEntry});
            this.lResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lResults.FullRowSelect = true;
            this.lResults.Location = new System.Drawing.Point(13, 112);
            this.lResults.Name = "lResults";
            this.lResults.Size = new System.Drawing.Size(559, 293);
            this.lResults.TabIndex = 20;
            this.lResults.UseCompatibleStateImageBehavior = false;
            this.lResults.View = System.Windows.Forms.View.Details;
            // 
            // select
            // 
            this.select.DisplayIndex = 3;
            this.select.Text = "";
            this.select.Width = 35;
            // 
            // nm
            // 
            this.nm.DisplayIndex = 0;
            this.nm.Text = "No#";
            this.nm.Width = 51;
            // 
            // date
            // 
            this.date.DisplayIndex = 1;
            this.date.Text = "Fecha";
            this.date.Width = 106;
            // 
            // info
            // 
            this.info.DisplayIndex = 2;
            this.info.Text = "Información";
            this.info.Width = 360;
            // 
            // docEntry
            // 
            this.docEntry.Text = "";
            this.docEntry.Width = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Base de datos:";
            // 
            // cbxDataSource
            // 
            this.cbxDataSource.FormattingEnabled = true;
            this.cbxDataSource.Location = new System.Drawing.Point(13, 25);
            this.cbxDataSource.Name = "cbxDataSource";
            this.cbxDataSource.Size = new System.Drawing.Size(185, 21);
            this.cbxDataSource.TabIndex = 21;
            this.cbxDataSource.SelectedIndexChanged += new System.EventHandler(this.SelectDataSource);
            this.cbxDataSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SupressKey);
            // 
            // eSeries
            // 
            this.eSeries.FormattingEnabled = true;
            this.eSeries.Location = new System.Drawing.Point(366, 11);
            this.eSeries.Name = "eSeries";
            this.eSeries.Size = new System.Drawing.Size(86, 21);
            this.eSeries.TabIndex = 23;
            this.eSeries.LostFocus += new System.EventHandler(this.ValidateSerie);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(416, 83);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Limpiar";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.ClearList);
            // 
            // progBar
            // 
            this.progBar.Location = new System.Drawing.Point(12, 411);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(559, 10);
            this.progBar.TabIndex = 99999;
            // 
            // btnReload
            // 
            this.btnReload.Enabled = false;
            this.btnReload.Font = new System.Drawing.Font("Wingdings 3", 8.25F);
            this.btnReload.Location = new System.Drawing.Point(204, 85);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(21, 21);
            this.btnReload.TabIndex = 100000;
            this.btnReload.Text = "Q";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.ReloadRPT);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.eSeries);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbxDataSource);
            this.Controls.Add(this.lResults);
            this.Controls.Add(this.chbxAll);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.eFolder);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxType);
            this.Controls.Add(this.eDate);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.eFolio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 500);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "main";
            this.ShowIcon = false;
            this.Text = "Consultas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.beforeClosing);
            this.Shown += new System.EventHandler(this.firstShown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox eFolio;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker eDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TextBox eFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.CheckBox chbxAll;
        private System.Windows.Forms.FolderBrowserDialog selectFolder;
        private System.Windows.Forms.ListView lResults;
        private System.Windows.Forms.ColumnHeader select;
        private System.Windows.Forms.ColumnHeader nm;
        private System.Windows.Forms.ColumnHeader date;
        private System.Windows.Forms.ColumnHeader info;
        private System.Windows.Forms.ColumnHeader docEntry;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbxDataSource;
        private System.Windows.Forms.ComboBox eSeries;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.ComboBox cbxType;

    }
}

