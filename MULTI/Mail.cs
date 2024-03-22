using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace MULTI
{
    public partial class Mail : Form
    {
        string Address, NameSender, Password, Subject, Body, MoreTwoFiles;
        string docEntry, table;


        ListView values;
        public Mail()
        {
            InitializeComponent();
            __initializeProgram();
        }

        public void btnSendMail(object sender, EventArgs e)
        {
            Enabled = false;
            bool AllForOne = (eTo.Text != "");
            string to = eTo.Text != "" ? eTo.Text : "";
            string subject = eSubject.Text != ""? eSubject.Text: Subject;
            string content = eContent.Text != "" ? eContent.Text : Body;

            if (AllForOne)
            {
                bool Send = false;
                docEntry = values.CheckedItems[0].SubItems[5].Text;
                table = main.Table(main.types[main.lTypeMail.SelectedIndex]);

                string[] files = new string[main.Attachemets.Count()*2];
                for (int i = 0, x = 0; i < (main.Attachemets.Count()*2); i += 2, x++) {
                    files[i] = main.Attachemets[x][0];
                    files[i+1] = main.Attachemets[x][1];
                }

                to = to.Replace(" ", string.Empty);
                to = to.Trim();
                to = to.Last() == ',' ? to.Substring(0, (to.Length - 1)) : to;
                string Msg = "";

                try { SendMail(to, subject, content, files); Send = true; }
                catch (Exception ex) { Msg = ex.Message; }
               

                if (Send) MessageBox.Show("El correo ha sido enviado.");
                else MessageBox.Show("No fue posible enviar el correo.");

                
            }
            else
            {
                bool Send = false;
                
                ListView.CheckedIndexCollection chkdIndx = values.CheckedIndices;
                List<int> index = new List<int>();
                for (int i = 0; i < chkdIndx.Count; i++) index.Add(chkdIndx[i]); // Se crea un array con los indices en un iterador
                int[] checks = index.ToArray(); // Se usa la variable index para una conversion en array ya que chkedindxcollect no lo permite
                bool One = checks.Count() == 1 ? true : false;
                int iFile = 0; // Se usara un iterador normal para los archivos que se adjuntaran
                foreach (int x in checks) // Este "checks" contiene los indices seleccionados en lResults del formulario inicial
                {
                    ListViewItem item = values.Items[x];
                    docEntry = item.SubItems[5].Text;
                    table = main.Table(main.types[main.lTypeMail.SelectedIndex]);
                    string remiters = "";
                    List<object> mails = main.query(
                        "SELECT TOP 25 T0.[E_MailL], T0.[U_CPN_SENDEMAIL] FROM OCPR T0 WHERE " +
                        $"T0.[CardCode] = (SELECT TOP 1 CardCode FROM {table} WHERE DocEntry = {docEntry})");

                    if (mails.Count() >= 1) {
                        foreach (var mail in mails.Select((val, i) => new { val, i }))
                            if (((List<string>)mail.val)[1] == "SI")
                                remiters += (((List<string>)mail.val)[0]) + ((mails.Count() - 1) > mail.i ? ", " : "");
                    }

                    remiters = remiters.Trim();
                    remiters = remiters.Last() == ',' ? remiters.Substring(0, (remiters.Length - 1)) : remiters;

                    if (mails.Count() == 0 || remiters == "") continue;

                    // Folio de prueba Javier: 1008103
                    try { SendMail(remiters, subject, content, new string[2] { (main.Attachemets[iFile])[0], (main.Attachemets[iFile])[1] }); Send = true; }
                    catch {  }

                    iFile++;
                }
                if (!One && Send) MessageBox.Show("Los correos han sido enviados.");
                else if (One && Send) MessageBox.Show("El correo ha sido enviado.");
                else MessageBox.Show("No fue posible enviar el correo, verifica que el cliente tenga un correo asignado y que la casilla enviar email tenga el valor \"SI\".");
            }

           

            Enabled = true;
        }

        void __initializeProgram() {
            ConfigManager conf = new ConfigManager(@"\\192.100.10.187\escaner\Reports\MULTI.EXE.config");
            ConfigManager.Section[] configuration = conf.getValues("CFDI");

            Address = conf.readValue("CFDI", "Address").Value;
            NameSender = conf.readValue("CFDI", "NameSender").Value;
            Password = conf.readValue("CFDI", "Password").Value; // Es necesario descifrar la contraseña
            Subject = conf.readValue("CFDI", "Subject").Value;
            Body = conf.readValue("CFDI", "Body").Value;
            MoreTwoFiles = conf.readValue("CFDI", "ContentForFiles").Value;

            values = main.lMailValues;
        }

        public void SendMail(string to, string subject, string content, string[] Attachemets) {
            using (var client = new SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(Address, Password);

                using (var message = new MailMessage()) {
                    message.From = new MailAddress(Address, NameSender);
                    message.To.Add(to);
                    message.Subject = subject;
                    message.IsBodyHtml = true;
                    // La regla es: Si son 2 archivos se toman los valores separados, si son mas de 2 solo se pega el nombre del archvio.

                    if (Attachemets.Count() <= 2)
                    {
                        if (Attachemets[0] != "" && Attachemets[0] != null && File.Exists(Attachemets[0]))
                            message.Attachments.Add(AddAtthFile(Attachemets[0]));
                        if (Attachemets[1] != "" && Attachemets[1] != null && File.Exists(Attachemets[1]))
                            message.Attachments.Add(AddAtthFile(Attachemets[1]));
                        
                        string[] file = new string[3];
                        List<object> document = main.query($"SELECT TOP 1 DocNum, Series, U_UUID FROM {table} WHERE DocEntry = {docEntry}");
                        if (document.Count() >= 1)
                            foreach (List<string> item in document)
                            {
                                file[0] = main.getSerie(item[1]);
                                file[1] = item[0];
                                file[2] = item[2];
                            }
                                 

                        // Orden: SaltoDeLinea, Serie, Folio, UUID, Comillas
                        message.Body = string.Format(content, "<br>", file[0], file[1], file[2], '"');
                    }
                    else
                    {
                        string files = "";
                        foreach (string item in Attachemets)
                            if (item != "" && item != null && File.Exists(item))
                            {
                                message.Attachments.Add(AddAtthFile(item));
                                files += Path.GetFileName(item) + "<br>";
                            }
                        // Orden: SaltoDeLinea, Files, Comillas
                        message.Body = string.Format(content != Body? content: MoreTwoFiles, "<br>", files, '"');
                    }
                    if (message.Attachments.Count >= 1)
                        client.Send(message);
                    else
                    {
                        MessageBox.Show("No fue posible enviar el correo porque no contiene archivos adjuntos.");
                    }
                }
            }


        }




        private Attachment AddAtthFile(string File) {
            ContentType contentType = new ContentType(MediaTypeNames.Application.Pdf);
            contentType.Name = Path.GetFileName(File);
            return new Attachment(File, contentType);
        }


    }
}
