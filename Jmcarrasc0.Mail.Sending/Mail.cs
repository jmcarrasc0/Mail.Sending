using System.Net;
using System.Net.Mail;

namespace Jmcarrasc0.Mail.Sending
{

    public class Attached
    {
        public byte[] file { get; set; }
        public string name { get; set; }
        public string MediaType { get; set; }

    }

    public class Addressee
    {
        public string ShowName { get; set; }
        public string Mail { get; set; }
    }

    public interface IEmail
    {
        List<Addressee> Addressees { get; set; }
        List<Addressee> AddresseesCC { get; set; }
        List<Addressee> AddresseesBCC { get; set; }
        string Title { get; set; }
        string Body { get; set; }
        string ScreenName { get; set; }
        int Port { get; set; }
        bool IsSSL { get; set; }
        bool BodyIsHtml { get; set; }
        List<Attached> Attachments { get; set; }
    }


    public class Email : IEmail
    {
        public List<Addressee> Addressees { get; set; }
        public List<Addressee> AddresseesCC { get; set; }
        public List<Addressee> AddresseesBCC { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ScreenName { get; set; }
        public int Port { get; set; }
        public bool IsSSL { get; set; }
        public bool BodyIsHtml { get; set; }
        public List<Attached> Attachments { get; set; }
        public string MailServer { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

    }

    public class EmailHost : IEmail
    {
        public string Host { get; set; }
        public List<Addressee> Addressees { get; set; }
        public List<Addressee> AddresseesCC { get; set; }
        public List<Addressee> AddresseesBCC { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string ScreenName { get; set; }
        public int Port { get; set; }
        public bool IsSSL { get; set; }
        public bool BodyIsHtml { get; set; }
        public List<Attached> Attachments { get; set; }


    }

    public class Mail
    {
        public bool SendMail(Email mail)
        {

            try
            {


                SmtpClient server = new SmtpClient(mail.MailServer)
                {

                    Port = mail.Port,
                    EnableSsl = mail.IsSSL,
                    Credentials = new NetworkCredential(mail.Account, mail.Password)


                };

                var mssg = new MailMessage
                {
                    Subject = mail.Title,
                    From = new MailAddress(mail.Account, mail.ScreenName),
                    IsBodyHtml = mail.BodyIsHtml,
                    Body = mail.Body

                };



                foreach (var to in mail.Addressees)
                {
                    mssg.To.Add(new MailAddress(to.Mail, to.ShowName));
                }


                if (mail.AddresseesCC != null)
                {
                    foreach (var cc in mail.AddresseesCC)
                    {
                        mssg.CC.Add(new MailAddress(cc.Mail, cc.ShowName));
                    }
                }

                if (mail.AddresseesBCC != null)
                {

                    foreach (var bcc in mail.AddresseesBCC)
                    {
                        mssg.Bcc.Add(new MailAddress(bcc.Mail, bcc.ShowName));
                    }

                }

                if (mail.Attachments!= null)
                {
                    foreach (var files in mail.Attachments)
                    {
                        var ms = new MemoryStream(files.file);
                        mssg.Attachments.Add(new Attachment(ms, files.name, files.MediaType));
                    }
                }


                /* Send*/
                server.Send(mssg);
                return true;

            }
            catch (Exception ex)
            {

                var argEx = new ArgumentException("A problem occurred while sending mail", ex);
                throw argEx;
            }

        }


        public bool SendMailbyHost(EmailHost mail)
        {
            try
            {

                SmtpClient server = new SmtpClient()
                {
                    Host = mail.Host,
                    Port = mail.Port,
                    EnableSsl = mail.IsSSL,
                };


                var mssg = new MailMessage
                {
                    Subject = mail.Title,
                    From = new MailAddress(mail.From, mail.ScreenName),
                    Body = mail.Body,
                    IsBodyHtml = mail.BodyIsHtml

                };



                foreach (var to in mail.Addressees)
                {
                    mssg.To.Add(new MailAddress(to.Mail, to.ShowName));
                }


                if (mail.AddresseesCC != null)
                {
                    foreach (var cc in mail.AddresseesCC)
                    {
                        mssg.CC.Add(new MailAddress(cc.Mail, cc.ShowName));
                    }
                }

                if (mail.AddresseesBCC != null)
                {

                    foreach (var bcc in mail.AddresseesBCC)
                    {
                        mssg.Bcc.Add(new MailAddress(bcc.Mail, bcc.ShowName));
                    }

                }

                if (mail.Attachments != null)
                {
                    foreach (var files in mail.Attachments)
                    {
                        var ms = new MemoryStream(files.file);
                        mssg.Attachments.Add(new Attachment(ms, files.name, files.MediaType));
                    }
                }


                /* Enviar */
                server.Send(mssg);
                return true;

            }
            catch (Exception ex)
            {

                var argEx = new ArgumentException("A problem occurred while sending mail", ex);
                throw argEx;
            }

        }
    }

}