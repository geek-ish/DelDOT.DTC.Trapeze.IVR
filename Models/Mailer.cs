using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace MVCWebApplication2.Models
{
    public class Mailer
    {
        const string sessionToken = "{Session.Passcode}", FromToken = "{{flow.variables.from}}",
            CallerCityToken = "{{flow.variables.callerCity}}", CallerStateToken = "{{flow.variables.callerState}}",
            CallerCountryToken = "{{flow.variables.callerCountry}}", CallerZipToken = "{{flow.variables.callerZip}}";
        const string innerHtml = "  <div style='direction:ltr'>  	<table border='1' cellpadding='0' cellspacing='0' style='direction:ltr;   border-collapse:collapse;border-style:solid;border-color:#A3A3A3;border-width:   1pt' valign='top'>  		<tr>  			<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;    vertical-align:top;width:2.8576in;padding:4pt 4pt 4pt 4pt'>  			<p style='margin:0in'>  			<img height='256' src='https://test.geek-ish.com/nothelp/images/geek-ish.png' width='256' /></p>  			<p style='margin:0in;font-family:Calibri;font-size:11.0pt'>&nbsp;</p>  			</td>  			<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;    vertical-align:top;width:5.6548in;padding:4pt 4pt 4pt 4pt'>  			<p style='margin:0in;font-family:Calibri;font-size:11.0pt'>&nbsp;</p>  			<p style='margin:0in;font-family:Calibri;font-size:11.0pt'>&nbsp;</p>  			<div style='direction:ltr'>  				<table border='1' cellpadding='0' cellspacing='0' style='direction:ltr;     border-collapse:collapse;border-style:solid;border-color:#A3A3A3;border-width:     1pt' valign='top'>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;font-family:Arial;font-size:27.75pt;color:black'>  						Your one-time&nbsp;passcode.</p>  						</td>  					</tr>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;font-family:Arial;font-size:10.5pt;color:#737487'>  						This&nbsp;passcode&nbsp;can only be used once and will expire in   						15 minutes:</p>  						</td>  					</tr>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;line-height:19pt;font-family:Arial;font-size:13.5pt;      color:#0E133B'>{Session.Passcode}</p>  						</td>  					</tr>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;line-height:15pt;font-family:Arial;font-size:10.5pt'>  						<span style='color:#737487'>If you did not make this   						request, please visit our&nbsp;</span><a href='https://test.geek-ish.com/nothelp/index.htm'>Help   						Center</a><span style='color:#737487'>&nbsp;for more   						information.</span></p>  						</td>  					</tr>  				</table>  			</div>  			<p style='margin:0in;font-family:Arial;font-size:27.75pt;color:black'>  			&nbsp;</p>  			</td>  		</tr>  	</table>  </div>  ";
        public static string _connectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
        
        /* TODO: parse mail fields

        */

        public static void SendPasscode(Models.Session session)
        {
            if (!string.IsNullOrEmpty(session.User.EmailAddress))
            {
                string body = GetBodyAsHtml(session);// innerHtml.Replace(sessionToken, passcode);
                string sender = "smtp@geek-ish.com";
                string pwd = "helloWorld55!";
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress(sender);//"tirus.brown@delaware.gov");// sender/*txtFrom.Text*/);
                m.To.Add(session.User.EmailAddress/*txtTo.Text*/);
                m.Subject = "Your one-time passcode";// "This is a test";
                m.Body = body;// "This is a sample message using SMTP authentication";
                m.IsBodyHtml = true;
                sc.Host = "mail.geek-ish.com";// txtMailServer.Text;
                string str1 = "gmail.com";
                string str2 = sender/*txtFrom.Text*/.ToLower();
                if (str2.Contains(str1))
                {
                    try
                    {
                        sc.Port = 587;
                        sc.Credentials = new System.Net.NetworkCredential(sender/*txtFrom.Text*/, pwd /*txtPass.Text*/);
                        sc.EnableSsl = true;
                        sc.Send(m);
                        //Response.Write("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        string message = /*Response.Write("<BR><BR>* */ "Please double check the From Address and Password to confirm that both of them are correct.";/* <br>");*/
                        message += /*Response.Write("<BR><BR>*/"If you are using gmail smtp to send email for the first time, please refer to this KB to setup your gmail account: http://www.smarterasp.net/support/kb/a1546/send-email-from-gmail-with-smtp-authentication-but-got-5_5_1-authentication-required-error.aspx?KBSearchID=137388";/*);*/
                        throw new Exception(message, ex);
                    }
                }
                else
                {
                    try
                    {
                        sc.Port = 25;
                        sc.Credentials = new System.Net.NetworkCredential(sender/*txtFrom.Text*/, pwd /*txtPass.Text*/);
                        sc.EnableSsl = false;
                        sc.Send(m);
                        //Response.Write(string.Format("Passcode email sent successfully to: {0}\n\n", recipient));
                    }
                    catch (Exception ex)
                    {
                        Console/*Response*/.Write("<BR><BR>* Please double check the From Address and Password to confirm that both of them are correct. <br>");
                        //Response.End();
                        throw ex;
                    }
                }
            }
        }

        private static string GetBodyAsHtml(Models.Session session)
        {
            string body = innerHtml.Replace(sessionToken, session.Passcode == null ? session.Passcode.ToString() : null);
            body = body.Replace(FromToken, session.Caller.From);
            body = body.Replace(CallerCityToken, session.Caller.City);
            body = body.Replace(CallerStateToken, session.Caller.State);
            body = body.Replace(CallerCountryToken, session.Caller.Country);
            body = body.Replace(CallerZipToken, session.Caller.Zip);
            return body;
        }

        public static void SendPasscode(string recipient, string passcode)
        {
            if (!string.IsNullOrEmpty(recipient))
            {
                string body = innerHtml.Replace(sessionToken, passcode);
                string sender = "smtp@geek-ish.com";
                string pwd = "helloWorld55!";
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                m.From = new MailAddress(sender);//"tirus.brown@delaware.gov");// sender/*txtFrom.Text*/);
                m.To.Add(recipient/*txtTo.Text*/);
                m.Subject = "Your one-time passcode";// "This is a test";
                m.Body = body;// "This is a sample message using SMTP authentication";
                m.IsBodyHtml = true;
                sc.Host = "mail.geek-ish.com";// txtMailServer.Text;
                string str1 = "gmail.com";
                string str2 = sender/*txtFrom.Text*/.ToLower();
                if (str2.Contains(str1))
                {
                    try
                    {
                        sc.Port = 587;
                        sc.Credentials = new System.Net.NetworkCredential(sender/*txtFrom.Text*/, pwd /*txtPass.Text*/);
                        sc.EnableSsl = true;
                        sc.Send(m);
                        //Response.Write("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        string message = /*Response.Write("<BR><BR>* */ "Please double check the From Address and Password to confirm that both of them are correct.";/* <br>");*/
                        message += /*Response.Write("<BR><BR>*/"If you are using gmail smtp to send email for the first time, please refer to this KB to setup your gmail account: http://www.smarterasp.net/support/kb/a1546/send-email-from-gmail-with-smtp-authentication-but-got-5_5_1-authentication-required-error.aspx?KBSearchID=137388";/*);*/        
                        throw new Exception(message, ex);
                    }
                }
                else
                {
                    try
                    {
                        sc.Port = 25;
                        sc.Credentials = new System.Net.NetworkCredential(sender/*txtFrom.Text*/, pwd /*txtPass.Text*/);
                        sc.EnableSsl = false;
                        sc.Send(m);
                        //Response.Write(string.Format("Passcode email sent successfully to: {0}\n\n", recipient));
                    }
                    catch (Exception ex)
                    {
                        Console/*Response*/.Write("<BR><BR>* Please double check the From Address and Password to confirm that both of them are correct. <br>");
                        //Response.End();
                        throw ex;
                    }
                }
            }
        }
    }
}

