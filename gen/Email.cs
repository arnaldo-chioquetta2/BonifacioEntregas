using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

public class Email
{
    public bool EnviarEmail(string remetente, string usuario, string senha, string destinatario, string anexo, string assunto, string corpo)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(remetente);
                mail.To.Add(destinatario);
                mail.Subject = assunto;
                mail.Body = corpo;
                mail.IsBodyHtml = true; // Permite o uso de HTML no corpo do e-mail

                if (!string.IsNullOrEmpty(anexo))
                {
                    mail.Attachments.Add(new Attachment(anexo));
                }

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential(usuario, senha);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                }
            }
            // MessageBox.Show("E-mail enviado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao enviar e-mail: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }
}