using MailKit.Net.Smtp;
using MimeKit;

namespace MarketingAlliance.App.Service
{
    public class MailService
    {
        /// <summary>
        /// Адрес почтового сервера
        /// </summary>
        private readonly string server;

        /// <summary>
        /// Порт почтового сервера
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public MailService(
            string server,
            int port)
        {
            this.server = server;
            this.port = port;
        }

        /// <inheritdoc/>
        public async Task Send(string from, string user, string password, string[] to, string subject, string body, string[] attachFiles = null, bool isHtmlBody = true, string[] copyTo = null)

        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Маркетинговая Альянс", from));

            foreach (var el in to)
                message.To.Add(new MailboxAddress("", el));


            foreach (var el in copyTo ?? new string[] { })
                message.Cc.Add(new MailboxAddress("", el));

            message.Subject = subject;


            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = isHtmlBody ? body : null,
                TextBody = isHtmlBody ? null : body
            };

            foreach (var file in attachFiles ?? Array.Empty<string>())
                if (File.Exists(file))
                    bodyBuilder.Attachments.Add(file);

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(server, port, false);
                await client.AuthenticateAsync(user, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}