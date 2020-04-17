using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ShoppingPortal.Utility
{
    public class EmailManager
    {
        public string SenderEmailAddress { get; set; }
        public string SenderPassword { get; set; }
        public string SenderHostName { get; set; }
        public int SenderPort { get; set; }
        public bool EnableSSL { get; set; }

        public EmailManager()
        {
            SenderEmailAddress = ConfigurationManager.AppSettings.Get("username");
            SenderPassword = ConfigurationManager.AppSettings.Get("Password");
            SenderPort =Convert.ToInt32(ConfigurationManager.AppSettings.Get("port"));
            SenderHostName = ConfigurationManager.AppSettings.Get("Host");
            EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("Ssl"));
        }

        public bool SendEmail(string Subject, string Body, string EmailTO, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                ListEmailTO.Add(EmailTO);
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, AttchementPath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool SendEmailMessage(string Subject, string Body, List<string> ListEmailTO, List<string> ListEmailCC = null, List<string> ListEmailBCC = null, MailPriority Priority = MailPriority.Normal, List<string> ListAttchementPath = null, string LinkedResources = null)
        {
            try
            {
                System.Net.Mail.MailMessage objMail = new System.Net.Mail.MailMessage();
                //Set from address
                objMail.From = new MailAddress(this.SenderEmailAddress);

                // Set to addresses
                if (ListEmailTO != null)
                {
                    if (ListEmailTO.Count > 0)
                    {
                        foreach (string emailTO in ListEmailTO)
                        {
                            if (!string.IsNullOrEmpty(emailTO))
                            {
                                objMail.To.Add(new MailAddress(emailTO));
                            }
                        }
                    }
                }

                // Set CC addresses
                if (ListEmailCC != null)
                {
                    if (ListEmailCC.Count > 0)
                    {
                        foreach (string emailCC in ListEmailCC)
                        {
                            objMail.CC.Add(new MailAddress(emailCC));
                        }
                    }
                }

                // Set BCC addresses
                if (ListEmailBCC != null)
                {
                    if (ListEmailBCC.Count > 0)
                    {
                        foreach (string emailBCC in ListEmailBCC)
                        {
                            objMail.Bcc.Add(new MailAddress(emailBCC));
                        }
                    }
                }

                // Set the content
                // Set subject
                objMail.Subject = Subject;
                // Set body/message
                // objMail.Body = Body;
                AlternateView htmlMail = null;
                htmlMail = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);

                // Set priority default is "Normal"
                objMail.Priority = Priority;

                if (!string.IsNullOrEmpty(LinkedResources))
                {
                    List<string> _LinkedResources = LinkedResources.Split(',').ToList();
                    foreach (string Cid in _LinkedResources)
                    {
                        if (!string.IsNullOrEmpty(Cid))
                        {
                            string[] _Cid = Cid.Split('^').ToArray();
                            string path = System.Web.HttpContext.Current.Server.MapPath(_Cid[0]);
                            string contentid = _Cid[1];

                            LinkedResource myimage = new LinkedResource(path);
                            myimage.ContentId = contentid;

                            // Create HTML view
                            // Set ContentId property. Value of ContentId property must be the same as
                            // the src attribute of image tag in email body. 
                            htmlMail.LinkedResources.Add(myimage);
                        }
                    }
                }

                objMail.AlternateViews.Add(htmlMail);
                objMail.IsBodyHtml = true;

                // Set priority default is "Normal"
                objMail.Priority = Priority;

                // Set attachement 
                if (ListAttchementPath != null)
                {
                    if (ListAttchementPath.Count > 0)
                    {
                        foreach (string path in ListAttchementPath)
                        {
                            objMail.Attachments.Add(new Attachment(path));
                        }
                    }
                }

                SmtpClient SMTPClientObj = new SmtpClient();
                SMTPClientObj.Credentials = new System.Net.NetworkCredential(this.SenderEmailAddress, this.SenderPassword);
                SMTPClientObj.Host = this.SenderHostName;
                SMTPClientObj.Port = this.SenderPort;
                SMTPClientObj.EnableSsl = this.EnableSSL;
                SMTPClientObj.Send(objMail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}