using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Turbo.MVC.Base3.Models;

namespace WDACC.Commons
{
    public class MailSender
    {
        public IList<Tuple<string, string>> ToAddress { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }

        public void Send()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("共通核心職能專區系統", ConfigModel.MailSenderAddr));
            message.Subject = "共通核心職能專區系統-忘記密碼通知信";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = this.HtmlBody;
            message.Body = bodyBuilder.ToMessageBody();

            foreach (Tuple<string, string> item in ToAddress)
            {
                if (ConfigModel.IsProductionMode)
                {
                    // 正式模式，送出信件
                    message.To.Add(new MailboxAddress(item.Item1, item.Item2));
                }
                else
                {
                    // 測試模式，寄至測試信箱
                    string email = ConfigModel.TestMailTo;
                    message.To.Add(new MailboxAddress(item.Item1, email));
                }
            }

            using (var client = new SmtpClient())
            {
                // client.Connect(ConfigModel.MailServer, 465, true);
                client.Connect(ConfigModel.MailServer, ConfigModel.MailServerPort, SecureSocketOptions.None);
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(ConfigModel.MailSenderAddr, ConfigModel.MailSenderPwd);
                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}
