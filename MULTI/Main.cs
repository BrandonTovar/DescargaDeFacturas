using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Reflection;

namespace MULTI
{
    public partial class main : Form
    {
        public delegate string Reverse(string s);

        public main() {
            bool exists = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1;
            if (!exists) {
                InitializeComponent();
                dbIndex = cbxDataSource.SelectedIndex;
                loadConfig();
                if (!MAIN_STATE) Environment.Exit(-1);
            }
            else Environment.Exit(-1);
        }

        // Crear archivos
        private bool CreateXML(ListViewItem item) => CreateXML(item, eFolder.Text);

        private bool CreateXML(ListViewItem item, string Folder) {
            string name = "";
            string docEntry = "";
            byte[] content;

            name = item.SubItems[3].Text;
            docEntry = item.SubItems[5].Text;
            content = Encoding.UTF8.GetBytes(GetXML(docEntry));
            if (content != null && content.Count() != 0) {
                FileStream xml = File.OpenWrite(Path.Combine(Folder, name + ".xml"));
                xml.Write(content, 0, content.Length);
                xml.Flush();
                xml.Close();
                xml.Dispose();
                Names[1] = Path.Combine(Folder, name + ".xml");
                return true;
            }
            else {
                if (!CopyXML(item.SubItems[1].Text, Folder)) return false;
                return true;
            }
        }

        private string GetXML(string DocEntry) {
            string folio = eFolio.Text;
            string serie = series[eSeries.SelectedIndex];
            string table = Table(types[cbxType.SelectedIndex]);

            string consulta = $"SELECT TOP 10 U_Xml FROM {table} WHERE DocEntry = {DocEntry}";

            List<object> xml = query(consulta);

            if (xml.Count() == 1)
            {
                return ((List<string>)xml[0])[0];
            }
            return null;
        }

        private bool CopyXML(string DocNum) => CopyXML(DocNum, eFolder.Text);

        private bool CopyXML(string DocNum, string Folder) {
            string folder = olds[cbxType.SelectedIndex];
            if (folder != "" && folder != null)
            {
                string path = Path.Combine(oldXmlPath, folder);
                if (Directory.Exists(path)) {
                    foreach (string item in Directory.GetFiles(path, $"*_{DocNum}_*"))
                        if (!File.Exists(Path.Combine(Folder, Path.GetFileName(item))))
                            try {
                                File.Copy(item, Path.Combine(Folder, Path.GetFileName(item)));
                                Names[1] = Path.Combine(Folder, Path.GetFileName(item));
                            }
                            catch { ShowError(E_XML_COPY); }
                }
                else {
                    ShowError(E_PATH_OLD);
                }

                return true;
            }

            return false;
        }

        private void loadConfig()
        {
            ErrorMessage = "";
            localConf = new ConfigManager();
            try
            {
                pathConf = localConf.readValue("Paths", "ConfigFile").Value;


                if (pathConf != null && pathConf != "")
                {
                    GetConf();
                }
                else
                {
                    ShowError(E_LOAD_CONF_FILE);
                    SelectConfigFile();

                    loadConfig();
                }
            }
            catch (Exception e)
            {
                ShowError(e.Message);
                SelectConfigFile();
                loadConfig();
            }
        }

        private void SelectConfigFile() {
            FileDialog file = new OpenFileDialog();
            file.Filter = "Archivo de configuracion (*.config;*.exe.config)|(*.config;*.exe.config;*.EXE.config;*.EXE.CONFIG;*.exe.CONFIG)|" +
                "Ejecutable de windows (*.EXE)|*.EXE";

            file.ShowDialog();
            if (file.FileName == "")
            {
                MessageBox.Show(E_LOAD_CONF_FILE_CLOSE);
                Environment.Exit(-1);
            }
            localConf.addValue("Paths", "ConfigFile", "", file.FileName);
        }
                
        private void GetConf() {
            conf = new ConfigManager(pathConf);
            LoadLayouts();
            LoadStringConnections();
            dbIndex = cbxDataSource.SelectedIndex;

            

            if (cbxDataSource.Items.Count == 0 && cbxType.Items.Count == 0) ErrorMessage = E_LOAD_CONF_FILE;
            if (ErrorMessage != "") {
                MessageBox.Show(ErrorMessage);
                Environment.Exit(-1);
            }
            cbxType.SelectedIndex = cbxDataSource.SelectedIndex = 0;

            Reports = new ReportDocument[files.Count()];
            loads = new Thread[files.Count()];
            rptLoad = new bool[files.Count()];

            for (int i = 0; i < files.Count(); i++)
            {
                string file = files[i];
                loads[i] = new Thread(() => load(file));
                loads[i].Start();
            }


            oldXmlPath = conf.readValue("Paths", "OldFiles").Value;
            try { eFolder.Text = localConf.readValue("Paths", "SaveIn").Value; }
            catch { }


            ConfigManager.Section[] oldFolders = conf.getValues("Folders");
            olds = new string[types.Count()];
            foreach (var tb in types.Select((val, i) => new { val, i }))
                foreach (ConfigManager.Section item in oldFolders)
                    olds[tb.i] = tb.val == item.Type ? item.Value : olds[tb.i];
        }

        private bool TestConnection()
        {
            SqlConnectionStringBuilder local = new SqlConnectionStringBuilder(conn[dbIndex].ConnectionString);
            // Validar informacion
            SqlConnection con = new SqlConnection(local.ConnectionString);
            try
            {
                con.Open();
                con.Close();
                conf.addConnectionString(local);
                return true;
            }
            catch { return false; }
        }

        private void saveConfig() => conf.Save();

        private void beforeClosing(object sender, FormClosingEventArgs e)
        {
            try {
                for (int i = 0; i < loads.Count(); i++) {
                    loads[i].Interrupt();
                    loads[i].Abort();
                }
            }
            catch { MessageBox.Show("No se pudo finalizar los procesos"); }
            // saveConfig();
        }

        public void load(string rpt) {
            int index = Array.FindIndex(files, s => s.Equals(rpt));
            bool isOnline = TestConnection();
            Reports[index] = new ReportDocument();
            Action btnState = delegate { SelectType(index); };
            bool isLoaded = false;
            Action Validate = delegate {
                rptLoad[index] = isLoaded;
                if (index == cbxType.SelectedIndex)
                    btnReload.Enabled = !rptLoad[index];
            };

            if (File.Exists(rpt) && new FileInfo(rpt).Extension == ".rpt" && isOnline) {
                try {
                    Reports[index].Load(rpt, OpenReportMethod.OpenReportByTempCopy);
                    foreach (IConnectionInfo item in Reports[index].DataSourceConnections) {
                        item.SetConnection(conn[dbIndex].DataSource,
                        conn[dbIndex].InitialCatalog,
                        conn[dbIndex].UserID,
                        conn[dbIndex].Password);
                    }
                    

                    Reports[index].VerifyDatabase();
                    isLoaded = true;
                }
                catch { try { Reports[index].Close(); } catch { } }
            }

            try { Invoke(btnState); }
            catch { }
            Invoke(Validate);
            
        }

        private int GetIndexCbx(string[] cbx, string rpt)
        {
            return Array.FindIndex(cbx, s => s.Equals(rpt));
        }

        private void getFolder(object sender, EventArgs e)
        {
            selectFolder.ShowDialog();
            eFolder.Text = selectFolder.SelectedPath;
            localConf.addValue("Paths", "SaveIn", "", eFolder.Text);
        }

        private void Search(object sender, EventArgs e)
        {
            string folio = eFolio.Text;
            string serie = "";
            try { serie = series[eSeries.SelectedIndex]; }
            catch { serie = ""; }
            string table = Table(types[cbxType.SelectedIndex]);
            chbxAll.Checked = false;

            string[] folios;
            string search = "";

            // Get table name for the query

            string chr = folio.Contains(",") ? "," :
                folio.Contains("*") ? "*" :
                folio.Contains("_") ? "_" :
                folio.Contains("-") ? "-" : "";

            if ((serie != "" || eDate.Text != "") && folio == "")
            {
                chr = "";
                if (eSeries.Text != "") chr += "S";
                if (eDate.Text != "") chr += "D";
            }


            switch (chr) {
                case ",":
                    //  Caso de lista
                    folio = Regex.Replace(folio, @"[^0-9|,]", string.Empty);
                    folios = folio.Split(",".ToCharArray());
                    search = "DocNum IN (";
                    foreach (var val in folios.Select((value, index) => new { value, index }))
                        search += val.value.Trim() +
                            (val.index + 1 == folios.Count() || val.value == "" ? "" : ", ");

                    search += ") ";
                    if (search == "DocNum IN () ") search = "DocNum = 1";
                    break;
                case "*":
                case "_":
                    folio = Regex.Replace(folio, @"[^0-9|*|_]", string.Empty);
                    //  Caso de coinsidencia
                    folio = folio.Replace(",", string.Empty);
                    folio = folio.Replace("*", "%");
                    search = $"DocNum LIKE '{folio}' ";
                    break;
                case "-":
                    //  Caso de rango
                    folio = Regex.Replace(folio, @"[^0-9|\-]", string.Empty);
                    folios = folio.Split("-".ToCharArray());
                    string[] valores = new string[2];
                    valores[0] = folios[0] != string.Empty ? folios[0] : "''";
                    valores[1] = folios[1] != string.Empty ? folios[1] : "''";
                    search = $"DocNum BETWEEN " +
                        $"{valores[0]} AND " +
                        $"{valores[1]}";
                    break;
                case "S":
                    //  Caso serie
                    search = $"Series = '{serie}' ";
                    break;
                case "D":
                    //  Caso fecha
                    search = $"CONVERT(varchar, DocDate, 103) LIKE '{eDate.Value.ToShortDateString()}%'";
                    break;
                case "SD":
                    //  Caso serie en fecha
                    search = $"Series = '{serie}' AND CONVERT(varchar, DocDate, 103) LIKE '{eDate.Value.ToShortDateString()}%'";
                    break;
                default:
                    //  Caso por defecto
                    folio = Regex.Replace(folio, @"[^0-9]", string.Empty);
                    search = $"DocNum = {folio} ";
                    break;
            }

            string consult = $"SELECT TOP {maxDocs} DocEntry, DocNum, ObjType, Series, U_UUID, " +
                "CONVERT(varchar,DocDate,103) + ' ' +" +
                "left(IIF(LEN(CAST(DocTime AS VARCHAR)) < 4, ('0'+CAST(DocTime AS VARCHAR)), CAST(DocTime AS VARCHAR)), 2)" +
                "+ ':' + right(DocTime, 2)" +
                $" FROM {table} WHERE {search}";

            

            if (lResults.Items.Count < maxDocs)
            {
                List<object> vals = query(consult);


                if (vals.Count() >= 1)
                {
                    foreach (List<string> item in vals)
                    {
                        string info = string.Format("{0}_{1}_{2}_{3}",
                        docType(item[(int)A.TYPE]),     // DocType
                        getSerie(item[(int)A.SERIE]),   // Serie
                        item[(int)A.FOLIO],             // DocNum
                        item[(int)A.UUID]);             // UUID
                        if (info.Trim().Last() == '_') info = info.Trim().Substring(0, info.Trim().Length - 1);
                        if (lResults.Items.Count <= maxDocs) // Modificar el valor de comparacion limita la cantidad maxima de valores en el ListView
                            addRow(new string[] { (lResults.Items.Count + 1).ToString(), item[(int)A.DATE], info, "", item[(int)A.DOCENTRY] }, lResults);
                            
                    }
                }
            }

        }

        public void Export(object sender, EventArgs e) => PreparateReports(eFolder.Text);

        public void PreparateReports(string Folder)
        {
            int[] selects = getRows();
            Attachemets = new List<string[]>();
            Names = new string[2] { "", "" };
            string name = "";
            string docEntry = "";

            try { if (!Directory.Exists(Folder) && Folder != "") Directory.CreateDirectory(Folder); }
            catch { ShowError(E_PATH_EXPORT); }
            

            if (Folder != "" && Directory.Exists(Folder))
            {
                progBar.Maximum = selects.Count();
                if (selects.Count() >= 1)
                {
                    Enabled = false;
                    foreach (int i in selects)
                    {
                        try
                        {
                            name = lResults.Items[i].SubItems[3].Text;
                            docEntry = lResults.Items[i].SubItems[5].Text;

                            try
                            {
                                CreateXML(lResults.Items[i], Folder);
                                Export(int.Parse(docEntry), Path.Combine(Folder, name + ".pdf"));
                            }
                            catch
                            {
                                ShowError(E_SAVEFILES);
                                break;
                            }
                            
                            
                            Attachemets.Add(new string[2] { Names[0], Names[1] });
                            
                            OneMore();
                        }
                        catch (CrystalReportsException err) { MessageBox.Show(err.ToString()); }
                    }
                    OneMore();
                    Enabled = true;
                }
                else ShowError(E_FILE_NOSELECT);
            }
            else ShowError(E_PATH_EXPORT);
            progBar.Value = 0;
        }

        public void Export(int docEntry, string pdf)
        {
            Reports[cbxType.SelectedIndex].SetParameterValue("DocKey@", docEntry);
            Reports[cbxType.SelectedIndex].ExportToDisk(ExportFormatType.PortableDocFormat, pdf);
            Names[0] = pdf;
        }

        private void ChangeDB() {
            Action btnState = delegate { SelectDB(); };
            Action change = delegate { ValidateButtons(false); };
            ErrorMessage = "";
            Invoke(change);

            if (TestConnection())
            {
                if (cbxDataSource.Items.Count >= 2)
                {
                    for (int i = 0; i < cbxType.Items.Count; i++)
                    {
                        try
                        {
                            IConnectionInfo connection = Reports[i].DataSourceConnections[0];
                            connection.SetConnection(conn[dbIndex].DataSource, conn[dbIndex].InitialCatalog,
                                conn[dbIndex].UserID, conn[dbIndex].Password);
                        }
                        catch { }

                    }
                    Invoke(btnState);
                }
            }
            else
            {
                MessageBox.Show("No se pudo acceder a la base de datos.");
            }
        }

        private bool rptAcces(ReportDocument file) {
            try { file.VerifyDatabase(); return true; }
            catch { return false; }
        }

        private int[] getRows()
        {
            ListView.CheckedIndexCollection indices = lResults.CheckedIndices;
            List<int> index = new List<int>();

            for (int i = 0; i < indices.Count; i++)
            {
                index.Add(indices[i]);
            }

            return index.ToArray();
        }

        private void GetSeries(object sender, EventArgs args) => GetSeries();

        private void GetSeries()
        {
            int i = cbxType.SelectedIndex;
            try
            {
                eSeries.Items.Clear();
                List<object> result = query(
                    $"SELECT distinct TOP 100 N.SeriesName, N.Series FROM NNM1 N JOIN {Table(types[i])} O ON O.Series = N.Series"
                    );
                series = new string[result.Count + 1];
                series[eSeries.Items.Add("")] = "";
                foreach (List<string> item in result)
                {
                    int e = eSeries.Items.Add(item[0]);
                    series[e] = item[1];
                }
            }
            catch { }
        }

        public static string getSerie(string serie)
        {
            List<object> result = query(string.Format(main.serie, serie));
            string val = "";
            foreach (List<string> item in result)
            {
                val = item[0].ToString();
            }
            return val;
        }

        private void addRow(string[] vals, ListView control)
        {
            ListViewItem listItem = new ListViewItem();

            for (int i = 0; i < vals.Length; i++)
            {
                listItem.SubItems.Add(vals[i]);
            }

            bool exist = false;

            try { exist = (control.FindItemWithText(vals[4]) != null); }
            catch {  }

            if (!exist) {
                control.Items.Add(listItem);
            }
            // 3006643
        }

        private void ClearList(object sender, EventArgs args) => ClearList();

        private void ClearList() => lResults.Items.Clear();

        public static List<object> query(string consult)
        {
            SqlConnection sql = new SqlConnection(conn[dbIndex].ConnectionString);
            sql.Open();
            SqlCommand command = new SqlCommand(consult, sql);
            SqlDataReader reader = command.ExecuteReader();

            List<object> result = new List<object>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<string> values = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values.Add(reader.GetValue(i).ToString());
                    }
                    result.Add(values);
                }
            }
            else ShowError(E_SQL_SEARCH);

            reader.Close();
            sql.Close();

            return result;
        }

        private void SelectAll(object sender, EventArgs e)
        {
            int rows = lResults.Items.Count;
            for (int i = 0; i < rows; i++) {
                lResults.Items[i].Checked = chbxAll.Checked;
            }

        }

        private void configMail(object sender, EventArgs e)
        {
            lMailValues = lResults;
            Mail mail = new Mail();
            mail.ShowDialog(this);
        }

        private void SelectType(int index)
        {

            if (index == cbxType.SelectedIndex) { ClearList(); SelectType(); }
        }
        private void SelectType() {
            int i = cbxType.SelectedIndex;
            try {
                eSeries.Items.Clear();
                GetSeries();
                eSeries.SelectedIndex = 0;
                if (!File.Exists(files[i])) ShowError(E_RPT_LOAD);
                ValidateButtons(Reports[i].IsLoaded);
            }
            catch {
                ValidateButtons(false);
            }
        }

        private void SelectDB()
        {
            int i = cbxType.SelectedIndex;
            try {
                Reports[i].VerifyDatabase();
                ValidateButtons(true);
                ClearList();
            }
            catch { ValidateButtons(false); }
        }

        private void SelectType(object sender, EventArgs e) {
            SelectType();
            try {
                btnReload.Enabled = !rptLoad[cbxType.SelectedIndex];
                ClearList();
            }
            catch { }
        }

        private void enableBtnRefresh()
        {
            int i = cbxType.SelectedIndex;
            try {
                if (Reports[i] != null && MAIN_STATE)
                    rptLoad[i] = !Reports[i].IsLoaded;
                btnReload.Visible = rptLoad[cbxType.SelectedIndex];
            }
            catch { btnReload.Visible = true; }

            
        }

        /* 
          Almacenar en un array si el reporte se cargo correctamente
          Tomar el valor para mostrar u ocultar el boton reload
          Enfoque en:  load -> rptLoads -> Evaluar el estado del reporte antes de habilitar
             
        */

        private void SelectDataSource(object sender, EventArgs e) {
            dbIndex = cbxDataSource.SelectedIndex;
           
            if (MAIN_STATE)
            {
                Thread change = new Thread(() => ChangeDB());
                change.Start();
            }
                
        }

        //  Load Configuration Init

        private void LoadLayouts()
        {
            ConfigManager.Section[] layouts = conf.getLayouts();
            files = new string[withoutNulls(layouts)];
            types = new string[withoutNulls(layouts)];
            foreach (ConfigManager.Section item in layouts) {
                if (item.Value != "")
                {
                    int i = cbxType.Items.Add(item.Name);
                    files[i] = item.Value;
                    types[i] = item.Type;
                }
                
            }

            try { cbxType.SelectedIndex = 1; }
            catch {
                ErrorMessage = E_RPT_GETLAYOUTS;
            }
        }

        private int withoutNulls(ConfigManager.Section[] files) {
            int i = 0;
            foreach (ConfigManager.Section item in files)
                if (item.Value != "" && item.Value != null)
                    i++;
            return i;
        }

        private void LoadStringConnections()
        {
            ConnectionStringSettingsCollection connections = conf.getConnectionStrings();
            conn = new SqlConnectionStringBuilder[connections.Count];
            foreach (ConnectionStringSettings item in connections)
            {
                SqlConnectionStringBuilder str = new SqlConnectionStringBuilder(item.ConnectionString);
                int i = cbxDataSource.Items.Add(str.InitialCatalog);
                conn[i] = new SqlConnectionStringBuilder(str.ConnectionString);
            }
            try { cbxDataSource.SelectedIndex = 0; }
            catch { ErrorMessage = E_LOAD_DBACCESS; }
        }

        private void ViewReport(object sender, EventArgs e) {
            int[] selects = getRows();
            string name = "";
            string docEntry = "";

            if (eFolder.Text != "" && Directory.Exists(eFolder.Text))
            {
                if (selects.Count() == 1)
                {
                    Enabled = false;
                    foreach (int i in selects)
                    {
                        try
                        {
                            name = lResults.Items[i].SubItems[3].Text;
                            docEntry = lResults.Items[i].SubItems[5].Text;
                            ViewReport(int.Parse(docEntry), Path.Combine(eFolder.Text, name + ".pdf"));
                        }
                        catch (CrystalReportsException err) { MessageBox.Show(err.ToString()); }
                    }
                    Enabled = true;
                }
                else ShowError(E_FILE_NOSELECT);
            }
            else ShowError(E_PATH_EXPORT);
        }

        private void ViewReport(int docEntry, string pdf) {
            //Reports[cbxType.SelectedIndex].SetParameterValue("DocKey@", docEntry);
            //using (Form form = new Form())
            //{
            //    CrystalReportViewer tempViewer = new CrystalReportViewer();

            //    tempViewer.ActiveViewIndex = -1;
            //    tempViewer.BorderStyle = BorderStyle.FixedSingle;
            //    tempViewer.Dock = DockStyle.Fill;
            //    tempViewer.Name = "tempViewer";
            //    tempViewer.SelectionFormula = "";
            //    tempViewer.TabIndex = 0;
            //    tempViewer.ViewTimeSelectionFormula = "";

            //    tempViewer.ReportSource = Reports[cbxType.SelectedIndex];
            //    tempViewer.AutoSize = true;
            //    tempViewer.Refresh();
            //    form.Controls.Add(tempViewer);
            //    form.AutoSize = true;
            //    form.ShowDialog();
            //    form.Width = 840;
            //    form.Height = 640;
            //}
        }

        private void firstShown(object sender, EventArgs e) => MAIN_STATE = true;

        // Keyboard controller : Validate values 

        private void IsNumber(object sender, KeyPressEventArgs args) {
            args.Handled = IsValid(args);
        }

        private void NoPaste(object sender, EventArgs args) {
            eFolio.Text = EraseNoNumeric(eFolio.Text);
        }

        private void SupressKey(object sender, KeyEventArgs args)
        {
            args.Handled = true;
            args.SuppressKeyPress = true;
        }

        private void SupressPast(object sender, EventArgs args) {
            if (!((ComboBox)sender).Items.Contains(((ComboBox)sender).Text))
            {
                try
                {
                    ((ComboBox)sender).SelectedIndex = 0;
                }
                catch
                {
                    ShowError(E_WITHOUT_TYPES);
                    Close();
                }

            }

        }

        private string EraseNoNumeric(string text) {
            return Regex.Replace(text, @"[^0-9|,|\-|_|*]", string.Empty);
        }

        private bool IsValid(KeyPressEventArgs args) {
            if ($"1234567890,_-*{(char)Keys.Back}".Contains(args.KeyChar) ||
                new KeyEventArgs((Keys)args.KeyChar).KeyValue == 22 ||
                new KeyEventArgs((Keys)args.KeyChar).KeyValue == 1 ||
                new KeyEventArgs((Keys)args.KeyChar).KeyValue == 3)
                return false;                
            return true;
        }

        // Validar serie

        private void ValidateSerie(object sender, EventArgs args) {
            if (((ComboBox)sender).Items.Count <= 0) ((ComboBox)sender).Items.Add("");
            ExistValue((ComboBox)sender, E_INVALID_SERIE);
        }

        private void ExistValue(ComboBox e, string ErrorCode) {
            if (!e.Items.Contains(e.Text))
            {
                e.SelectedIndex = 0;
                ShowError(ErrorCode);
            }
        }

        private void OneMore() { if ((progBar.Value + 1) <= progBar.Maximum) progBar.Value += 1; }

        private void ValidateButtons(bool status) {
            btnPrint.Enabled = btnSearch.Enabled = 
            eSeries.Enabled = eFolio.Enabled = eDate.Enabled = 
            btnView.Enabled = btnSend.Enabled = cbxDataSource.Enabled = status;
            chbxAll.Checked = false;
            
        }

        // Configuracion de correo, envio masivo de correos

        private void viewMailDialog(object sender, EventArgs e) {
            if (lResults.CheckedItems.Count >= 1)
            {
                bool isShift = (ModifierKeys == Keys.Shift);
                lMailValues = lResults;
                lTypeMail = cbxType;
                PreparateReports(Path.Combine(Path.GetTempPath(), "Multielectrico"));

                Mail mail = new Mail();
                if (isShift) mail.btnSendMail(sender, e);
                else mail.ShowDialog();

                mail.Close();

                foreach (string[] item in Attachemets)
                {

                    if (item[0] != "" && Path.GetDirectoryName(item[0]) == (Path.GetTempPath() + "Multielectrico"))
                        if (File.Exists(item[0])) File.Delete(item[0]);
                    if (item[1] != "" && Path.GetDirectoryName(item[1]) == (Path.GetTempPath() + "Multielectrico"))
                        if (File.Exists(item[1])) File.Delete(item[1]);
                }
            }
        }

        // Llamar nuevamente a load para cargar el reporte
        private void ReloadRPT() { // Solo vuelve a cargar el archivo rpt usando la misma ruta, si la ruta esta mal nunca cargara.
            int i = cbxType.SelectedIndex;

            string file = files[i];
            loads[i] = new Thread(() => load(file));
            loads[i].Start();
        }

        private void ReloadRPT(object sender, EventArgs e) => ReloadRPT();


    }
}