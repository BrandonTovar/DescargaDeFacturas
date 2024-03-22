using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace MULTI
{
    static class Program
    {
        
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new main());
            MessageBox.Show("El programa main se lanzo y paso de esa iteracion.");
        }
    }

    public class ConfigManager
    {
        Configuration conf;

        public sealed class Section : ConfigurationSection
        {
            [ConfigurationProperty("type", IsKey = true)]
            public string Type
            {
                get { return (string)this["type"]; }
                set { this["type"] = value; }
            }

            [ConfigurationProperty("name")]
            public string Name
            {
                get { return (string)this["name"]; }
                set { this["name"] = value; }
            }

            [ConfigurationProperty("value")]
            public string Value
            {
                get { return (string)this["value"]; }
                set { this["value"] = value; }
            }

            public Section() { }

            public Section(string type, string name, string value)
            {
                this["value"] = value;
                this["type"] = type;
                this["name"] = name;
            }
        }

        public void addConnectionString(SqlConnectionStringBuilder conn)
        {
            if (conf.ConnectionStrings.ConnectionStrings[conn.InitialCatalog] == null)
            {
                conf.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(
                    conn.InitialCatalog, conn.ConnectionString
                    ));
                conf.Save();
            }
            else
            {
                conf.ConnectionStrings.ConnectionStrings[conn.InitialCatalog].ConnectionString = conn.ConnectionString;
                conf.Save();
            }
        }

        public ConnectionStringSettings ReadConnectionString(string name)
        {
            if (conf.ConnectionStrings.ConnectionStrings[name] != null)
                return conf.ConnectionStrings.ConnectionStrings[name];
            return null;
        }

        public ConnectionStringSettingsCollection getConnectionStrings()
        {
            if (conf.ConnectionStrings.ConnectionStrings == null)
                return new ConnectionStringSettingsCollection() { };

            SqlConnectionStringBuilder str;
            ConnectionStringSettingsCollection conections = conf.ConnectionStrings.ConnectionStrings;
            ConnectionStringSettingsCollection result = new ConnectionStringSettingsCollection();
            foreach (ConnectionStringSettings item in conections) {
                str = new SqlConnectionStringBuilder(item.ConnectionString);
                if (str.InitialCatalog != "") result.Add(item);
            }

            return result;
        }

        public Section readLayout(string type) { return readValue("Layouts", type); }

        public Section[] getLayouts() { return getValues("Layouts"); }

        public void addLayout(string type, string name, string file) => addValue("Layouts", type, name, file);

        /**
         * @Param: NameGroup, Type, Content
         * If no exists, create a sectionGroup in the configExe file 
         * and add a new value into the file.
         */
        public void addValue(string nmGroup, string type, string name, string value)
        {
            Section section = new Section(type, name, value);
            if (conf.SectionGroups[nmGroup] == null)
                conf.SectionGroups.Add(nmGroup, new ConfigurationSectionGroup());
            if (conf.SectionGroups[nmGroup].Sections[type.Replace(' ', '_')] == null)
            {
                conf.SectionGroups[nmGroup].Sections.Add(type.Replace(' ', '_'), section);
                Save();
            }
            else
            {
                conf.SectionGroups[nmGroup].Sections.Remove(type.Replace(' ', '_'));
                conf.SectionGroups[nmGroup].Sections.Add(type.Replace(' ', '_'), section);
                Save();
            }
        }

        public void addValues(string nmGroup, string[] type, string[] name, string[] value)
        {
            int values = type.Length > value.Length ? type.Length : value.Length;
            for (int i = 0; i < values; i++)
            {
                addValue(nmGroup,
                    type.Length < i ? "" : type[i],
                    name.Length < i ? "" : name[i],
                    value.Length < i ? "" : value[i]
                    );
            }

        }
        // Read value into a sectionGroup
        public Section[] getValues(string nmGroup)
        {
            if (conf.SectionGroups[nmGroup] != null)
            {
                ConfigurationSectionGroup group = conf.SectionGroups[nmGroup];

                if (group.Sections.Count == 0) { return new Section[] { }; }
                ConfigurationSectionCollection sections = group.Sections;
                Section[] path = new Section[sections.Count];
                for (int i = 0; i < sections.Count; i++)
                {
                    Section section = (Section)sections[i];
                    path[i] = (Section)sections[i];
                }
                return path;
            }
            else
            {
                return new Section[] { };
            }
        }

        public Section readValue(string nmGroup, string type)
        {
            if (conf.SectionGroups[nmGroup].Sections[type] != null) {
                Section val = (Section)conf.SectionGroups[nmGroup].Sections[type];
                return val;
            }
            return null;
        }

        public ConfigManager() {
            conf = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public Configuration GetCurrentConfig()
        {
            return conf;
        }

        public ConfigManager(string file) // Cargar archivo de configuracion remoto
        {
            if (File.Exists(file)) {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = new FileInfo(file).Extension != ".config"? file + ".config" : file;
                conf = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            }   
        }

        public void Save() {
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(conf.AppSettings.SectionInformation.Name);
        }

        public void SaveAs(string filename) { conf.SaveAs(filename); }

    }

    public partial class main
    {
        // Loading Configuration
        public static string E_LOAD_CONF_FILE = "No se pudo cargar la configuracion. Por favor, llama a soporte o selecciona el archivo de configuracion.";
        public static string E_LOAD_CONF_FILE_CLOSE = "Sin un archivo de configuracion el programa no funcionara, llama a soporte.";
        public static string E_LOAD_DBACCESS = "No se pudo obtener la configuracion de la base de datos.\nPor favor, llama a soporte.";

        // Save files
        public static string E_PATH_EXPORT = "Para guardar los documentos es necesaria una ruta valida.";
        public static string E_FILE_NOSELECT = "Porfavor, Selecciona al menos una factura.";
        public static string E_XML_NOEXIST = "El archivo xml no existe.";
        public static string E_XML_COPY = "El archivo xml no se pudo copiar, verifica tus permisos.";
        public static string E_PATH_OLD = "La ruta de los archivos xml no es valida.";
        public static string E_SAVEFILES = "Ocurrio un error al guardar los documentos. \n\nPorfavor, verifica los siguientes puntos:\n  "+
            "- Se tiene acceso a la ruta seleccionada.\n"+
            "- Se cuenta con los permisos necesarios de escritura.\n"+
            "- La ruta no esta comprometida por algun otro proceso.\n";

        // Entry validators
        public static string E_INVALID_SERIE = "Esta serie no existe, porfavor ingresa un valor valido.";
        public static string E_WITHOUT_TYPES = "No se encontraron reportes, Por seguridad se finalizara el programa.";

        // CrystalReport 
        public static string E_RPT_GETLAYOUTS = "No se pudieron obtener los reportes, verifica el archivo de configuracion.";
        public static string E_RPT_EXPORT = "No se cargo ningun rpt.";
        public static string E_RPT_LOAD = "El archivo seleccionado para el reporte no existe o no tiene un formato valido.";

        // Change Option
        public static string E_CBX_SELECTTYPE = "No se pudieron obtener los reportes, verifica el archivo de configuracion.";

        // SQL
        public static string E_SQL_SEARCH = "Esta consulta no devolvio ningun documento.";
        public static string E_SQL_DOCTYPE = "Porfavor, Selecciona el tipo de documento.";

        // FORM State
        public bool MAIN_STATE = false;

        public static void ShowError(string error)
        {
            MessageBox.Show(error);
        }

        // Get DocType for files name
        public static string docType(string val)
        {
            switch (val)
            {
                case "13":
                    return "F";
                case "14":
                    return "N_C";
                case "15":
                    return "E";
                case "16":
                    return "DEV";
                case "17":
                    return "O";
                case "18":
                    return "FACT_P";
                case "19":
                    return "N_PAG";
                case "20":
                    return "O_COM_MER";
                case "21":
                    return "DEV_BIEN";
                case "22":
                    return "O_COMP";
                case "23":
                    return "C";
                case "24":
                    return "P";
                case "46":
                    return "P_SAL";
                case "59":
                    return "EM";
                case "60":
                    return "SM";
                case "67":
                    return "TRANSF_INV";
                case "69":
                    return "COS_DES";
                case "88":
                    return "TIP_ENV";
                case "89":
                    return "S";
                case "90":
                    return "D_TRAN";
                case "112":
                    return "BORR";
                case "132":
                    return "FACT_CORR_COBR";
                case "140":
                    return "BORR_PAGO";
                case "162":
                    return "REV_INV";
                case "163":
                    return "FACT_CORR_PAGA";
                case "164":
                    return "R_FAC_CUEN_PAG";
                case "165":
                    return "FACT_CORR_CUE_COBR";
                case "166":
                    return "R_FAC_CUEN_COBR";
                case "202":
                    return "ORD_PROD";
                case "203":
                    return "F_ANT";
                case "204":
                    return "P_INI_PAGR";
                case "213":
                    return "D_RECOM";
                case "280":
                    return "F_IMP_VENT";
                case "281":
                    return "F_IMP_COBR";
                case "1179":
                    return "B_TRA_ACC";
                case "10000071":
                    return "PUB_INV";
                case "310000001":
                    return "SAL_INICIAL";
                case "540000005":
                    return "D_COD_TRIB";
                case "540000006":
                    return "C_COMP";
                case "1250000001":
                    return "S_TRANS_INV";
                case "1470000049":
                    return "CAP";
                case "1470000060":
                    return "N";
                case "1470000065":
                    return "CONT_INV";
                case "1470000113":
                    return "S_COMP";
                default:
                    return "D";
            }
        }

        public static string Table(string type) {
            switch (type)
            {
                case "13":
                    return "OINV";
                case "14":
                    return "ORIN";
                case "15":
                    return "ODLN";
                case "16":
                    return "ORDN";
                case "17":
                    return "ORDR";
                case "18":
                    return "OPCH";
                case "19":
                    return "ORPC";
                case "20":
                    return "OPDN";
                case "21":
                    return "ORPD";
                case "22":
                    return "OPOR";
                case "23":
                    return "OQUT";
                case "24":
                    return "ORCT";
                case "46":
                    return "OVPM";
                case "59":
                    return "OIGN";
                case "60":
                    return "OIGE";
                case "67":
                    return "OWTR";
                case "69":
                    return "OIPF";
                case "88":
                    return "OENT";
                case "89":
                    return "OSAL";
                case "90":
                    return "OTRA";
                case "112":
                    return "ODRF";
                case "132":
                    return "OCIN";
                case "140":
                    return "OPDF";
                case "162":
                    return "OMRV";
                case "163":
                    return "OCPI";
                case "164":
                    return "OCPV";
                case "165":
                    return "OCSI";
                case "166":
                    return "OCSV";
                case "202":
                    return "OWOR";
                case "203":
                    return "ODPI";
                case "204":
                    return "ODPO";
                case "213":
                    return "ORCM";
                case "280":
                    return "OTSI";
                case "281":
                    return "OTPI";
                case "1179":
                    return "ODRF";
                case "10000071":
                    return "OIQR";
                case "310000001":
                    return "OIQI";
                case "540000005":
                    return "OTCX";
                case "540000006":
                    return "OPQT";
                case "1250000001":
                    return "OWTQ";
                case "1470000049":
                    return "OACQ";
                case "1470000060":
                    return "OACD";
                case "1470000065":
                    return "OINC";
                case "1470000113":
                    return "OPRQ";
                default:
                    return "NULL";
            }
        }

        private string[] GetFiles(string path, string match) {
            return Directory.GetFiles(path, match);
        }

        // Form

        public static string exportPath = "";
        public static bool[] rptLoad;
        public static string oldXmlPath = "";
        public static string[] files;
        public static string[] types; // Codigo de sap para documentos
        public static string[] olds;
        public static string[] series;
        public static int maxDocs = 500;
        public static List<string[]> Attachemets;
        public static string[] Names = new string[2];
        public static string pathConf = @"C:\Users\ASAEL\Documents\MULTI\MULTI\MULTI.EXE"; // Ruta de pruebas
        public static ConfigManager conf;
        public static ConfigManager localConf;
        static int dbIndex;
        public static CrystalDecisions.CrystalReports.Engine.ReportDocument[] Reports;
        System.Threading.Thread[] loads;
        static string ErrorMessage = "";
        static SqlConnectionStringBuilder[] conn;

        public enum A
        {
            DOCENTRY = 0,
            FOLIO = 1,
            TYPE = 2,
            SERIE = 3,
            UUID = 4,
            DATE = 5
        };

        


        public static string serie = "SELECT SeriesName FROM NNM1 WHERE Series = {0}";

        public static ListView lMailValues;
        public static ComboBox lTypeMail;


    }

    public class FileManager
    {
        /* Documentacion de la clase
         
         Create - Create(TextWrite) - Create(TextWrite, ContentEdit)
         Exists - Exists(FileName) // Confirmara la existencia del documento
         Search - Search(folder, match) - Search(folder, match, subdirectory : true | false)
         Copy - Copy(Origen, Destiny) - Copy(Origen, Destiny, Replace : true | false)
         Open - Open(file, Method: edit, read, truncate, replace)
         CreateTempFile - CreateTempFile(FileName, Content) - CreateTempFile(FileName, Content, Method: Add, Truncate)
               // Por defecto createtmpfile no hara nada si el archivo ya existe a menos que se indique un method
         */

        public string FileName = "";

        public bool Exists() { return Exists(FileName); }

        public bool Exists(string FileName) { return File.Exists(FileName); }

        public string[] Search(string Folder, string Match, SearchOption SubDirectorys) {
            return Directory.GetFiles(Folder, Match, SubDirectorys);
        }

        public void Create(string Folder, string FileName, string content, FileMode method)
        {
            string document = Path.Combine(Folder, FileName);
            if (!File.Exists(document))
            {
                FileStream file = new FileStream(document, method);

                byte[] bContent = System.Text.Encoding.UTF8.GetBytes(content);

                try
                {
                    file.Write(bContent, 0, bContent.Length);
                    file.Flush();
                }
                catch { }

                file.Close();
            }

        }

        public void Create(string FileName, string Content)
        {
            string Folder = new FileInfo(FileName).DirectoryName;
            string File = new FileInfo(FileName).Name;

            Create(Folder, File, Content, FileMode.Truncate);
        }

        public void Create(string Content) => Create(FileName, Content);

        public bool CreateTempFile(string Name, string Content) {
            string folder = Path.Combine(Path.GetTempPath(), "MULTI");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            Create(folder, Name, Content, FileMode.OpenOrCreate);

            return File.Exists(Path.Combine(folder, Name));
        }

        public bool Copy(string FileName, string to) {
            string DestFile = Path.Combine(to, (new FileInfo(FileName)).Name);
            if (!Exists(FileName) && Directory.Exists(to))
                File.Copy(FileName, DestFile);
            else
                try
                {
                    Directory.CreateDirectory(to);
                    File.Copy(FileName, DestFile, false);
                }
                catch { }

            return File.Exists(DestFile);
        }
    }

    public class XML // Clase orientada a el extraer de la base de datos el xml o copiar el documento del servidor 20
    {
        /* Documentacion de la clase
        
        GetXML - GetXML(DocEntry) // Extraera de la base de datos el xml
        Exists - Exists(DocEntry) // Validara la existencia del xml en la ruta predeterminada en el servidor 20
        Copy - Copy(FileSource, FolderPaste) // Copiara usando FileManager el archivo XML del servidor 20 a la ruta asignada en el eSaveIn
        Create - Create() - Create(Replace: true | false ) // If Exists - true  : Copia el documento desde el 20
                                                              If Exists - false : Genera el documento desde DB
        CreateTempXML - CreateTempXML(FileName, Content) - CreateTempXML(FileName, Content, Method: Add, Truncate)
                          // Haciendo uso de Copy creara un archivo temporal

         */

        public string FileName = "MULTI_DOCUMENT_BLANK.xml";
        FileManager Files = new FileManager();
        string Table = "";
        string Folder = "";

        public bool Exists(string FileName) {
            return Files.Exists(this.FileName = FileName);   
        }

        public bool Copy(string OrigenFile, string To) {
            if (Files.Copy(OrigenFile, To))
            {
                FileName = Path.Combine(To, (new FileInfo(OrigenFile).Name));
                return true;
            }
            return false;
        }

        public bool CreateTempXML(string FileName, string Content) {
            string file = new FileInfo(FileName).Name;
            if (Files.CreateTempFile(file, Content))
            {
                this.FileName = FileName = Path.Combine(Path.GetTempPath(), file);
                return true;
            }
            return false;
        }

        public string GetXML(string StringConnection, string Table, string DocEntry)
        {
            SQL conn = new SQL(StringConnection);
            List<object> xml = conn.Query(conn.ConnectionString, string.Format(conn.P_GET_XML, Table, DocEntry));
            this.Table = Table;

            if (xml.Count() == 1) return ((List<string>)xml[0])[0];
            return "";
        }

        public bool Create(string ConnectionString, string Folder, string Table, string DocEntry) {
            this.Table = Table;
            string name = Name(ConnectionString, Table, DocEntry);

            if (Exists(FileName)) return true;
            else {
                this.Folder = Directory.Exists(Folder) ? Folder : Path.GetTempPath();

                return false;
            }
            
        }

        public string Name(string ConnectionString, string Table, string DocEntry) {
            SQL con = new SQL(ConnectionString);
            List<object> xml = con.Query(con.ConnectionString, string.Format(con.P_NAME_DOC, Table, DocEntry));
            List<string> vals = (List<string>)xml[0];
            this.Table = Table;

            return string.Format("{0}_{1}_{2}_{3}",
                    main.docType(vals[1]), con.GetStringSerie(con.ConnectionString, vals[2]), vals[0], vals[3]);
        }

        XML(string Folder) {
            if (Directory.Exists(Folder))
                FileName = Path.Combine(Folder, "MULTI_DOCUMENT_BLANK.xml");
            FileName = Path.Combine(Path.GetTempPath(), "MULTI_DOCUMENT_BLANK.xml");
        }

        XML() { FileName = Path.Combine(Path.GetTempPath(), "MULTI_DOCUMENT_BLANK.xml"); }

    }

    public class PDF // Generador de pdfs con ReportDocument de crystalreports, Tambien copiara los documentos del 20
    {
        /* Documentacion de la clase
        
        LoadReport - LoadReport(ReportFileName) - LoadReport(ReportFileName, SqlConnectionInfo) - LoadReport(File, Conn, DocEntry) -
                     LoadReport(File, Conn, DocEntry, TempFile: true | false)
                    // Usando ReportDocument cargara el archivo rpt para su uso, especificar si sera como documento temporal.
        UseDB - UseDB(SqlConnectionInfo)  // Sobrescribira la informacion de base de dato del documento de forma temporal
        AddParam - Find(ParamName, Value) // Asignara el parametro y hara refresh al documento.
        Exists - Exists(Folio) // Validara la existencia del PDF en la ruta predeterminada en el servidor 20
        Copy - Copy(FileSource, FolderPaste) // Copiara usando FileManager el archivo PDF del servidor 20 a la ruta asignada en el eSaveIn
        Export - Export() - Export(Replace: true | false ) // If Exists - true  : Copia el documento desde el 20
                                                              If Exists - false : Exporta el documento
        Create - Create() // Asignara el DocEntry y Report.Refresh() cargara el documento pero no lo guardara
                             Este metodo depende del resultado de Exists, true: Copia o Crea el archivo | false: No Hace nada
        CreateTempPDF - CreateTempPDf(FileName, Content) - CreateTempPDF(FileName, Content, Method: Add, Truncate)
                          // Haciendo uso de FileManager creara un archivo temporal para el PDF 

         */

    }
    
    public class SQL
    {

        private SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();

        public string ConnectionString
        {
            get { return conn.ConnectionString; }
        }



        // Codigos de error
        private string E_SQL_SEARCH = "Esta consulta no devolvio ningun documento.";
        private string E_SQL_DOCTYPE = "Porfavor, Selecciona el tipo de documento.";

        // Procedimientos
        private string P_GET_SERIE = "SELECT SeriesName FROM NNM1 WHERE Series = {0}";
        private string P_GET_SERIES = "SELECT distinct TOP {0} N.SeriesName, N.Series FROM NNM1 N JOIN {1} O ON O.Series = N.Series";

        // XML o PDF para construir el nombre
        public string P_NAME_DOC = "SELECT TOP 1 DocNum, ObjType, Series, U_UUID FROM {0} WHERE DocEntry = {1}";
        public string P_GET_XML = "SELECT TOP 10 U_Xml FROM {0} WHERE DocEntry = {1}";

        // Limites
        private int MAX_ROWS = 150;


        public List<object> Query(string ConnectionString, string consult)
        {
            SqlConnection sql = new SqlConnection(ConnectionString);
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
                        values.Add(reader.GetValue(i).ToString());

                    result.Add(values);
                }
            }
            else MessageBox.Show(E_SQL_SEARCH);

            reader.Close();
            sql.Close();

            return result;
        }

        public string[] GetSeries(string table)
        {
            try
            {
                List<object> result = Query(conn.ConnectionString, string.Format(P_GET_SERIES, MAX_ROWS, table));

                string[] vals = new string[result.Count];
                foreach (var list in result.Select((value, i) => { return new { value, i }; }))
                    vals[list.i] = ((List<string>)list.value)[0];

                return vals;
            }
            catch { }

            return new string[] { };
        }

        public string GetStringSerie(string ConnectionString, string serie)
        {
            List<object> result = Query(ConnectionString, string.Format(P_GET_SERIE, serie));
            return ((List<string>)result[0])[0];
        }

        public SQL(string StringConnection)
        {
            SqlConnectionStringBuilder local = new SqlConnectionStringBuilder(StringConnection);
            if (TestConnection(local)) conn = local; 
        }

        public bool TestConnection(SqlConnectionStringBuilder str)
        {
            SqlConnectionStringBuilder local = str;
            SqlConnection con = new SqlConnection(local.ConnectionString);

            try
            {
                con.Open();
                con.Close();
                return true;
            }
            catch { return false; }
        }
    }


}
