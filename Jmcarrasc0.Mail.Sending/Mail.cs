using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

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

                var email = new MimeMessage();

                email.From.Add(new MailboxAddress($"{mail.ScreenName}", $"{mail.Account}"));
                email.Subject = mail.Title;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $"{mail.Body}"
                };

                foreach (var to in mail.Addressees)
                {
                    email.To.Add(new MailboxAddress($"{to.ShowName}", $"{to.Mail}"));
                }

                if (mail.AddresseesCC != null)
                {
                    foreach (var cc in mail.AddresseesCC)
                    {
                        email.Cc.Add(new MailboxAddress(cc.Mail, cc.ShowName));
                    }
                }

                if (mail.AddresseesBCC != null)
                {

                    foreach (var bcc in mail.AddresseesBCC)
                    {
                        email.Bcc.Add(new MailboxAddress(bcc.Mail, bcc.ShowName));
                    }

                }


                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(mail.MailServer, mail.Port,mail.IsSSL);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate(mail.Account, mail.Password);

                    smtp.Send(email);
                    return true;
                    smtp.Disconnect(true);


                }


            }
            catch (Exception ex)
            {

                var argEx = new ArgumentException("A problem occurred while sending mail", ex);
                throw argEx;
            }

        }


      
    }

}