using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace MULTI
{
    public partial class Config : Form
    {
        SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();

        public Config()
        {
            InitializeComponent();
            cbxServer.Focus();
        }

        private void Cancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Guardar(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (Validation())
            {
                main.conf.Save();
                MessageBox.Show("La informacion se guardo correctamente.");
                Close();
            }
        }

        private void Verify(object sender, EventArgs e) {
            if (Validation()) { MessageBox.Show("Verificacion exitosa."); }
        }
        private void Test(object sender, EventArgs e) {
            if (TestConnection()) MessageBox.Show("Conexion realizada con exito, se guardo la informacion.");
        }

        private bool TestConnection()
        {
            main.conf = new ConfigManager(main.pathConf);
            conn.InitialCatalog = cbxBaseData.Text;
            conn.DataSource = cbxServer.Text;
            conn.UserID = tbxUser.Text;
            conn.Password = tbxPass.Text;

            // Validar informacion
            SqlConnection con = new SqlConnection(conn.ConnectionString);
            try {
                con.Open();
                con.Close();
                main.conf.addConnectionString(conn);
                return true;
            }
            catch {
                MessageBox.Show("No hay conexion con la base de datos, verifica la informacion o la conexion con el servidor.");
                return false;
            }
        }

        private bool Validation() { return (TestConnection() && CheckFiles()); }

        private bool CheckFiles()
        {
            bool exists = true;
            if (lFiles.Items.Count == 0)
            {
                MessageBox.Show("No se encontro ningun archivo para los reportes.");
                return false;
            }
            foreach (ListViewItem item in lFiles.Items)
            {
                string type = item.SubItems[0].Text;
                string path = item.SubItems[1].Text;
                if (File.Exists(path)) { main.conf.addLayout(type, path); item.ForeColor = Color.Black; }
                else { item.ForeColor = Color.Red; exists = false; }
            }
            if (!exists) MessageBox.Show("Algunos de los archivos que ingresaste no existen o son inalcanzables, verifica la informacion.");
            return exists;
        }

        private void InsertButton(ListViewItem item)
        {
            Point focus = item.Position;
            Point subitem = new Point(
                focus.X + lFiles.Columns[0].Width + lFiles.Columns[1].Width - chose.Width - 5
                , focus.Y - 2);
            chose.Location = subitem;
            lFiles.Controls.Add(chose);
        }

        private void listSelect(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = lFiles.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    lItemMenu.Show(Cursor.Position);
                }
            }
            else if (e.Button == MouseButtons.Left) 
            {
                var focusedItem = lFiles.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    InsertButton(focusedItem);
                }
            }
        }

        private void Delete(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Eliminar")
            {
                foreach (ListViewItem item in lFiles.SelectedItems) {
                    item.Remove();
                }
            }

        }
        
        private string SearchRPT()
        {
            FileDialog file = new OpenFileDialog();
            file.Filter = "Crystal Reports (*.rpt)|*.rpt";
            file.ShowDialog();
            return file.FileName;
        }

        private void SearchRPT(object sender, EventArgs e)
        {
            tbxPathFile.Text = SearchRPT();
        }

        private void ChangePath(object sender, EventArgs e)
        {
            string path = SearchRPT();
            int item = lFiles.FocusedItem.Index;
            if (path != "")
            {
                lFiles.FocusedItem.SubItems[1].Text = path;
                lFiles.Controls.Remove(chose);
            }

            lFiles.Items[item].Selected = true;
            lFiles.Select();
        }

        private void AddRPT(object sender, EventArgs e)
        {
            if (tbxType.Text == "") { MessageBox.Show("Porfavor, Ingresa el tipo de documento."); tbxType.Focus(); }
            else if (tbxPathFile.Text == "") { MessageBox.Show("Es necesario seleccionar un archivo."); btnSearch.Focus(); }
            else
            {
                if (File.Exists(tbxPathFile.Text))
                {
                    if (lFiles.FindItemWithText(tbxType.Text.Trim()) != null)
                    {
                        MessageBox.Show("Este documento ya existe, verifica que no sea el mismo reporte o considera cambiar el nombre");
                        tbxType.Focus();
                    }
                    else
                    {
                        lFiles.Items.Add(new ListViewItem(new string[] { tbxType.Text.Trim(), tbxPathFile.Text.Trim() }));
                        tbxType.Text = tbxPathFile.Text = "";
                    }
                    
                }
                else
                {
                    MessageBox.Show("El archivo que se ingreso no existe o esta fuera de su alzance.");
                }
                
            }
        }

        private void OutOfForm(object sender, EventArgs e)
        {
            if (!chose.Focused)
            {
                groupBox2.Select();
                lFiles.Controls.Remove(chose);
            }            
        }

        private void GetServers(object sender, EventArgs e)
        {
            System.Data.Sql.SqlDataSourceEnumerator instances = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            foreach (System.Data.DataRow row in instances.GetDataSources().Rows)
            {
                if (!cbxServer.Items.Contains(row[0])) cbxServer.Items.Add(row[0]);
            }
        }

        private void GetDataBases(object sender, EventArgs e)
        {
            conn.DataSource = cbxServer.Text;
            conn.UserID = tbxUser.Text;
            conn.Password = tbxPass.Text;

            using (SqlConnection con = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (!cbxBaseData.Items.Contains(dr[0].ToString())) cbxBaseData.Items.Add(dr[0].ToString());

                            }
                        }
                    }
                    con.Close();
                }
                catch
                {
                    MessageBox.Show("Ocurrio un error al obtener las bases de datos. \nPorfavor, Verifica tu informacion.");
                }

            }
        }
    }
}
