using DocProAPI.Models;
using DocProUtil;
using System;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using DocProUtil.Cf;

namespace DocProAPI.Customs.Helper
{
    public class SendMailHelper
    {
        public static string GoogleMail(dynamic doc ,string url)
        {
            var configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            var mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            url =mailSettings.Smtp.Network.TargetName + url;
            var googleMail = "https://mail.google.com/mail/u/0/?view=cm&fs=1&tf=1" + "&su=" + doc.Name + "&body=" + "Mô tả : " + doc.Describe + "%0A" + "Đường dẫn : " + url;
            return googleMail;
        }
        public SendMailHelper()
        {
        }
    
        #region ------ Send Mail -------
        public static bool SendMail(MailModel mail)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                var configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
                var mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                if (Equals(mailSettings, null))
                {
                    Loger.Log("Mail setting not invalid!!");
                    return false;
                }
                int port = mailSettings.Smtp.Network.Port;
                string host = mailSettings.Smtp.Network.Host;
                string password = mailSettings.Smtp.Network.Password;
                string username = mailSettings.Smtp.Network.UserName;
                mail.displayname = GlobalConfig.GetStringSetting("DisplayNameEmail");
                using (MailMessage mm = new MailMessage())
                {
                    mm.Subject = mail.subject;
                    mm.Body = mail.body;
                    mm.IsBodyHtml = true;
                    mm.From = new MailAddress(username, mail.displayname);
                    mm.To.Add(mail.mailto);
                    smtp.Host = host;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.Port = port;
                    smtp.Send(mm);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Loger.Log(ex.ToString());
                return false;
            }
            finally
            {
                smtp.Dispose();
            }
        }

        #endregion

    }
}