using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using MVCWebApplication2.Models;
using System.Net.Mail;
using System.Data.SqlClient;

namespace MVCWebApplication2.Controllers
{
    public class SessionsController : Controller
    {
        const string sessionToken = "{Session.Passcode}";
        const string innerHtml = "< style type='text/css'>.auto-style1 {	color: #737487;}.auto-style3 {	color: #737487;	font-family: Arial, Helvetica, sans-serif;	font-size: 10pt;}</style></head><body> <div style='direction:ltr'>  	<table border='1' cellpadding='0' cellspacing='0' style='direction:ltr;   border-collapse:collapse;border-style:solid;border-color:#A3A3A3;border-width:   1pt' valign='top'>  		<tr>  			<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;    vertical-align:top;width:2.8576in;padding:4pt 4pt 4pt 4pt'>  			<p style='margin:0in'>  				 &nbsp;</p>	 <p style='margin:0in'>  			<img height='256' src='https://test.geek-ish.com/nothelp/images/geek-ish.png' width='256' /></p>  			<p style='margin:0in;font-family:Calibri;font-size:11.0pt'>	 &nbsp;</p>  			</td>  			<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;    vertical-align:top;width:5.6548in;padding:4pt 4pt 4pt 4pt'>  			<div style='direction:ltr'>  				<table border='1' cellpadding='0' cellspacing='0' style='direction:ltr;     border-collapse:collapse;border-style:solid;border-color:#A3A3A3;border-width:     1pt' valign='top'>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;font-family:Arial;font-size:27.75pt;color:black'>  						Your one-time&nbsp;passcode.</p>  						</td>  					</tr>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;font-family:Arial;font-size:10.5pt;color:#737487'>  						This&nbsp;passcode&nbsp;can only be used once and will expire in   						15 minutes:</p>  						</td>  					</tr>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;line-height:19pt;font-family:Arial;font-size:13.5pt;      color:#0E133B'>{Session.Passcode}</p>  						</td>  					</tr>  					<tr>  						<td style='border-style:solid;border-color:#A3A3A3;border-width:1pt;      background-color:white;vertical-align:top;width:5.5423in;padding:4pt 4pt 4pt 4pt'>  						<p style='margin:0in;line-height:15pt;font-family:Arial;font-size:10.5pt'>  								 <span style='color:#737487'>Request details:</span></p>		 <table style='width: 100%' cellpadding='0' cellspacing='1'>			 <tr>				 <td class='auto-style3' style='width: 17px'>&nbsp;</td>				 <td class='auto-style3' style='width: 104px'>From:</td>				 <td>				 <span class='mention-variable' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-offset-key='8jem0-1-0' style='box-sizing: border-box; margin-left: 0px; background-color: rgb(230, 243, 255); line-height: 1.8; padding: 3px 0px; box-shadow: rgb(230, 243, 255) 3px 0px 0px, rgb(230, 243, 255) -3px 0px 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 {{flow.variables.from}}</span></span></span></td>			 </tr>			 <tr>				 <td class='auto-style3' style='width: 17px'>&nbsp;</td>				 <td class='auto-style3' style='width: 104px'>				 <span data-offset-key='fbu60-0-0' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-offset-key='4aq71-0-0' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 Caller City:</span></span></span></td>				 <td>				 <span data-offset-key='fbu60-0-0' style='box-sizing: border-box;'>				 <span class='mention-variable' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-offset-key='4aq71-1-0' style='box-sizing: border-box; margin-left: 0px; background-color: rgb(230, 243, 255); line-height: 1.8; padding: 3px 0px; box-shadow: rgb(230, 243, 255) 3px 0px 0px, rgb(230, 243, 255) -3px 0px 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 {{flow.variables.callerCity}}</span></span></span></span></td>			 </tr>			 <tr>				 <td class='auto-style3' style='width: 17px; height: 32px;'></td>				 <td class='auto-style3' style='width: 104px; height: 32px;'>				 <span data-offset-key='fmg7t-0-0' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 Caller State:</span></span></td>				 <td style='height: 32px'>				 <span data-offset-key='fmg7t-0-0' style='box-sizing: border-box;'>				 <span data-text='true' style='box-sizing: border-box;'>&nbsp;</span></span><span class='mention-variable' style='box-sizing: border-box; margin-left: 0px;'><span data-offset-key='fmg7t-1-0' style='box-sizing: border-box; margin-left: 0px; background-color: rgb(230, 243, 255); line-height: 1.8; padding: 3px 0px; box-shadow: rgb(230, 243, 255) 3px 0px 0px, rgb(230, 243, 255) -3px 0px 0px;'><span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>{{flow.variables.callerState}}</span></span></span><span data-offset-key='fmg7t-2-0' style='box-sizing: border-box; margin-left: 0px;'><span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 </span></span></td>			 </tr>			 <tr>				 <td class='auto-style3' style='width: 17px'>&nbsp;</td>				 <td class='auto-style3' style='width: 104px'>				 <span data-offset-key='7h3i9-0-0' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 Caller Country:</span></span></td>				 <td>				 <span class='mention-variable' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-offset-key='7h3i9-1-0' style='box-sizing: border-box; margin-left: 0px; background-color: rgb(230, 243, 255); line-height: 1.8; padding: 3px 0px; box-shadow: rgb(230, 243, 255) 3px 0px 0px, rgb(230, 243, 255) -3px 0px 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 {{flow.variables.callerCountry}}</span></span></span></td>			 </tr>			 <tr>				 <td style='height: 23px; width: 17px'>&nbsp;</td>				 <td style='height: 23px; width: 104px'>				 <span data-offset-key='4mpp4-0-0' style='box-sizing: border-box; margin-left: 0px;'>				 <span class='auto-style3				 ' data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 Caller Zip:</span></span></td>				 <td style='height: 23px'>				 <span class='mention-variable' style='box-sizing: border-box; margin-left: 0px;'>				 <span data-offset-key='4mpp4-1-0' style='box-sizing: border-box; margin-left: 0px; background-color: rgb(230, 243, 255); line-height: 1.8; padding: 3px 0px; box-shadow: rgb(230, 243, 255) 3px 0px 0px, rgb(230, 243, 255) -3px 0px 0px;'>				 <span data-text='true' style='box-sizing: border-box; margin-left: 0px;'>				 {{flow.variables.callerZip}}</span></span></span></td>			 </tr>		 </table>		 <p style='margin:0in;line-height:15pt;font-family:Arial;font-size:10.5pt'>  								 <span class='auto-style1'>If</span><span style='color:#737487'> you did not make this   						request, please visit our&nbsp;</span><a href='https://test.geek-ish.com/nothelp/index.htm'>Help   						Center</a><span style='color:#737487'>&nbsp;for more   						information.</span></p>  						</td>  					</tr>  				</table>  			</div>  			<p style='margin:0in;font-family:Arial;font-size:27.75pt;color:black'>  				 &nbsp;</p>  			</td>  		</tr>  	</table>  </div>";
        public static string _connectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";
        //_connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=DTC;Integrated Security=True";
        private SessionsContext db = new SessionsContext();

        public void ResendPasscode(Session session)
        { 
            Action<Session> SendMailAsync = Mailer.SendPasscode;
            HostingEnvironment.QueueBackgroundWorkItem(cancelToken => SendMailAsync(session));
        }

        public void ResendPasscode(string passcode, string email, string sessionId)
        {
            Action<string, string, string> SendMailAsync = SendMail;
            HostingEnvironment.QueueBackgroundWorkItem(cancelToken => SendMailAsync(email, passcode, sessionId));
        }
        
        [HttpGet]
        public JsonResult Init2(int clientId, string from, string city, string state, string country, string zip)
        {
            bool isRegisteredDevice = IsRegisteredDevice(clientId, from);
            Models.Session session = new Models.Session()
            {
                Caller = new Models.Caller()
                {
                    From = from,
                    City = city,
                    State = state,
                    Country = country,
                    Zip = zip
                }
            };

            if (!isRegisteredDevice)
            {
                session = FindSessionByClientUnregisteredNumber(clientId, from);
                if (session == null)
                {
                    session = GetSession(clientId, from); if (session == null) throw new Exception("Session object is null.");
                    session.Passcode = int.Parse(GetRandomString(RandomDatatype.Numeric));
                    session.UnregisteredNumber = from;
                    UpdateDBSession(session);
                }
            }
            return new JsonResult() { Data = session, JsonRequestBehavior = JsonRequestBehavior.AllowGet };            
        }

        [HttpGet]
        public JsonResult Init(int clientId, string phone)
        {
            //Ref, Delegates :  Code Editor - https://www.tutorialsteacher.com/codeeditor?cid=cs-Th00fz
            //Note: Needed because the Twilio HTTP Request widget was running so slow the response was throwing an error.
            //  I had to find a way to improve the performance/speed and deicded to put the slow SendMail() function
            //  on a background thread to run async and the construct for that (QueueBackgroundWorkItem) required a delegate
            //  be used in its implementation.
            //  More time consuming tasks constructs: How to run Background Tasks in ASP.NET - Scott Hanselman, https://www.hanselman.com/blog/HowToRunBackgroundTasksInASPNET.aspx
            //  Twilio widget error details: https://www.twilio.com/docs/api/errors/81016
            Action<string, string, string> SendMailAsync = SendMail;

            bool isRegisteredDevice = IsRegisteredDevice(clientId, phone);
            Session session = null;

            if (!isRegisteredDevice)
            {
                //if numberIsUnregistered(number), lookup the unregistered number in the Sessions
                session = FindSessionByClientUnregisteredNumber(clientId, phone);
                if (session == null)
                {
                    //if a session with the unregistered number is NOT found then set the session, 
                    //write the temporary passcode & the unregistered number into the session, and
                    //write the session to db.
                    session = GetSession(clientId, phone); if (session == null) throw new Exception("Session object is null.");
                    session.Passcode = int.Parse(GetRandomString(RandomDatatype.Numeric));
                    session.UnregisteredNumber = phone;
                    UpdateDBSession(session);
                }      
            }

            try
            {
                if (session == null) session = GetSession(clientId, phone); if (session == null) throw new Exception("Session object is null.");
                //if (session.Passcode == null) throw new Exception("Session Passcode is null.");

                //if (!(PhoneNumber.Parse(phone).Equals(PhoneNumber.Parse(session.User.PhoneNumber))))
                if (!isRegisteredDevice)
                {
                    if (session.PasscodeSent == new DateTime())
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(cancelToken => SendMailAsync(session.User.EmailAddress, 
                            session.Passcode.ToString(), session.SessionId));
                        session.PasscodeSent = DateTime.Now;
                        TimestampPasscode(session.SessionId, session.PasscodeSent);
                    }
                }
                return new JsonResult() { Data = session, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) { throw ex; }
        }

        //static void Init(int clientId, string phone)
        //{
        //    bool isRegisteredDevice = IsRegisteredDevice(clientId, phone);
        //    MVCWebApplication2.Models.Session session = null;

        //    if (!isRegisteredDevice)
        //    {
        //        //if numberIsUnregistered(number), lookup the unregistered number in the Sessions
        //        session = FindSessionByClientUnregisteredNumber(clientId, phone);
        //        if (session == null)
        //        {
        //            //if a session with the unregistered number is NOT found...
        //            session = GetSession(clientId, phone);//set the session and ...
        //            if (session == null) throw new Exception("Session object is null.");
        //        }
        //        session.UnregisteredNumber = phone;//...write the unregistered number into the session & db.
        //        UpdateDBSession(session);
        //    }
        //    try
        //    {
        //        if (session.Passcode == null) throw new Exception("Session Passcode is null.");
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        static void AddRegisteredDevice(int clientId, string phoneNumber)
        {
            string updateQuery = string.Format("UPDATE ClientDevices SET DeviceList = cast ( DeviceList as nvarchar(max))  + cast ('; {0}' as nvarchar(max) )where ClientId = {1}",
                phoneNumber, clientId);
            string insertQuery = string.Format("INSERT INTO[dbo].[ClientDevices]([ClientId],[DeviceList]) VALUES({0},'{1}');",
                clientId, phoneNumber);
            string query = ClientHasRegisteredDevice(clientId) ? updateQuery : insertQuery;

            if (!IsRegisteredDevice(clientId, phoneNumber))
                ExecuteSqlServerNonQueryDb(query);
        }

        static bool ClientHasRegisteredDevice(int clientId)
        {
            string query = string.Format("select * from ClientDevices where clientId = {0}", clientId);
            DataTable dt = GetFilledSqlServerDataTable(query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                    return true;

            return false;
        }

        static bool IsRegisteredDevice(int clientId, string phoneNumber)
        {
            string query = string.Format("select DeviceList from ClientDevices where ClientId = {0}", clientId);
            DataTable dt = GetFilledSqlServerDataTable(query); if (dt == null || dt.Rows.Count < 1) return false;

            return (dt.Rows[0][0].ToString().Contains(phoneNumber));
        }

        private void TimestampPasscode(string sessionId, DateTime? timestamp)
        {
            if (sessionId == null) throw new NullReferenceException("Session ID is null");
            if (timestamp == null) timestamp = DateTime.Now; 
            string q = string.Format("update Sessions set [PasscodeSent] = " +
                 "convert(varchar, '{0}', 22) where [SessionId] = '{1}'", timestamp, sessionId);

            try { int retVal = ExecuteSqlServerNonQueryDb(q); }
            catch (Exception ex) { throw ex; }
        }

        private void Session_Start()
        {

        }

        private void Session_Destroy()
        {

        }

        static /*MVCWebApplication2.Models.*/Session FindSessionByClientUnregisteredNumber(int clientId, string unregisteredNumber)
        {
            string query = string.Format("select top 1 " +
                 "ClientId, Phone, CreationTime, Passcode, Email, TextNumber, FirstName, LastName, PasscodeSent, CallerId, " +
                  "LoginTime, LogoutTime, UnregisteredNumber, SessionId from Sessions where clientId = {0} and UnregisteredNumber = " +
                  "'{1}' order by CreationTime desc", clientId, unregisteredNumber);
            DataTable dt = GetFilledSqlServerDataTable(query); if (dt == null || dt.Rows.Count < 1) return null;
            int clientId1 = !string.IsNullOrEmpty(dt.Rows[0][0].ToString()) ? int.Parse(dt.Rows[0][0].ToString()) : clientId;
            string phoneNumber = dt.Rows[0][1].ToString();
            string emailAddress = dt.Rows[0][4].ToString();
            string textNumber = dt.Rows[0][5].ToString();
            string firstName = dt.Rows[0][6].ToString();
            string lastName = dt.Rows[0][7].ToString();
            DateTime creationTime = !string.IsNullOrEmpty(dt.Rows[0][2].ToString()) ? DateTime.Parse(dt.Rows[0][2].ToString()) : new DateTime();
            int? passcode = !string.IsNullOrEmpty(dt.Rows[0][3].ToString()) ? int.Parse(dt.Rows[0][3].ToString()) : (int?)null;
            DateTime passcodeSent = !string.IsNullOrEmpty(dt.Rows[0][8].ToString()) ? DateTime.Parse(dt.Rows[0][8].ToString()) : new DateTime();
            long? callerId = !string.IsNullOrEmpty(dt.Rows[0][9].ToString()) ? long.Parse(dt.Rows[0][9].ToString()) : (long?)null;
            DateTime loginTime = !string.IsNullOrEmpty(dt.Rows[0][10].ToString()) ? DateTime.Parse(dt.Rows[0][10].ToString()) : new DateTime(); //DateTime.Parse(dt.Rows[0][10].ToString());
            DateTime logoutTime = !string.IsNullOrEmpty(dt.Rows[0][11].ToString()) ? DateTime.Parse(dt.Rows[0][11].ToString()) : new DateTime();//DateTime.Parse(dt.Rows[0][11].ToString());
            string unregisteredNumber1 = dt.Rows[0][12].ToString();
            string sessionId = dt.Rows[0][13].ToString();

            /*MVCWebApplication2.Models.*/Session session = new /*MVCWebApplication2.Models.*/Session()
            {
                User = new /*MVCWebApplication2.Models.*/User()
                {
                    ClientId = clientId1,
                    PhoneNumber = phoneNumber,
                    EmailAddress = emailAddress,
                    TextNumber = textNumber,
                    FirstName = firstName,
                    LastName = lastName
                },
                CreationTime = creationTime,
                Passcode = passcode,
                PasscodeSent = passcodeSent,
                CallerId = callerId,
                LoginTime = loginTime,
                LogoutTime = logoutTime,
                UnregisteredNumber = unregisteredNumber1,
                SessionId = sessionId
            };

            return session;
        }

        static void UpdateDBSession(/*MVCWebApplication2.Models.*/Session session)
        {
            //Ref, "CAST AS": SQLSERVER Tryit Editor v1.0: https://www.w3schools.com/sql/trysqlserver.asp?filename=trysql_func_sqlserver_cast3
            string query = string.Format("update Sessions set ClientId = {0}, Phone = '{1}'," +
                //"CreationTime = CAST('{2}' AS datetime)," +
                "CreationTime = CONVERT(VARCHAR, '{2}', 103), " + 
                "Passcode = {3}," +
                "Email = '{4}'," +
                "TextNumber = '{5}'," +
                "FirstName = '{6}'," +
                "LastName = '{7}'," +
                //"PasscodeSent = CAST('{8}' AS datetime)," +
                "PasscodeSent = CONVERT(VARCHAR, '{2}', 103), " + 
                "CallerId = {9}," +
                //"LoginTime = CAST('{10}' AS datetime)," +
                "LoginTime = CONVERT(VARCHAR, '{2}', 103), " +
                //"LogoutTime = CAST('{11}' AS datetime)," +
                "LogoutTime = CONVERT(VARCHAR, '{11}', 103), " +
                "UnregisteredNumber = '{12}' " +
                "where SessionId = '{13}'",
                session.User.ClientId, session.User.PhoneNumber, session.CreationTime.ToString(),
                session.Passcode, session.User.EmailAddress, session.User.TextNumber,
                session.User.FirstName, session.User.LastName, session.PasscodeSent.ToString(),//.Passcode.ToString(),
                session.CallerId, session.LoginTime.ToString(), session.LogoutTime.ToString(),
                session.UnregisteredNumber,
                session.SessionId);

            int retVal = ExecuteSqlServerNonQueryDb(query);
        }

        private string TrimPhoneNumber(string phoneNumber)
        {
            string s = new string(phoneNumber.Where(c => char.IsNumber(c)).ToArray());
            return s.Substring((s.Length - 10));
        }
        
        public void SendMail(string recipient, string passcode, string sessionId)
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
                        Response.Write("<BR><BR>* Please double check the From Address and Password to confirm that both of them are correct. <br>");
                        Response.Write("<BR><BR>If you are using gmail smtp to send email for the first time, please refer to this KB to setup your gmail account: http://www.smarterasp.net/support/kb/a1546/send-email-from-gmail-with-smtp-authentication-but-got-5_5_1-authentication-required-error.aspx?KBSearchID=137388");
                        //Console/*Response*/.End();
                        throw ex;
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

        /*
         * TODO: As lookup & EF...
         * */
        private Session GetSession(int clientId, string phone)
        {
            Session session = null;
            try
            {
                User user = GetUser(clientId); if (user == null) throw new NullReferenceException("Session User object is null");
                string sessionId = GetRandomString(); if (string.IsNullOrEmpty(sessionId)) throw new NullReferenceException("Session Id is null or empty.");
                Int64 callerId = PhoneNumber.Parse(phone);

                session = new Session()
                   {
                       User = user,
                       SessionId = sessionId,
                       //ClientId = clientId,
                       //Phone = phoneNumber,
                       CreationTime = DateTime.Now,
                       Passcode =  user.Passcode,
                       CallerId = callerId
                   };

                    string query = string.Format("INSERT INTO [dbo].[Sessions] " +
                        "([SessionId] ,[ClientId] ,[Phone] ,[CreationTime], [Passcode] " +
                        ", [Email], [FirstName], [LastName], [TextNumber], [CallerId])" +
                       " VALUES ('{0}', {1}, '{2}', convert(varchar, '{3}', 22), {4}, " +
                        "'{5}', '{6}', '{7}', '{8}', {9})",
                       session.SessionId, session.User.ClientId, session.User.PhoneNumber,
                       session.CreationTime, session.Passcode,  
                       session.User.EmailAddress, session.User.FirstName, 
                       session.User.LastName, session.User.TextNumber, session.CallerId);
                    int ret = ExecuteSqlServerNonQueryDb(query);
                
                return session;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
         TODO: As lookup & EF...
         */
        private User GetUser(int clientId)
        {
            User user = null; 
            string q = string.Format("select [CLIENTID],[USERID], [PHONENUMBER], [EMAILADDRESS], [FIRSTNAME], [LASTNAME], [TEXTNUMBER], [PASSCODE] from Users where clientid = {0}", clientId);
            SqlConnection conn = new SqlConnection(_connectionString); if (conn == null) throw new NullReferenceException("DB connection is null");
            DataTable dt = GetFilledSqlServerDataTable(/*conn,*/ q); if (dt == null || dt.Rows.Count < 1) throw new NullReferenceException(string.Format("Client[ID={0}] not registered and is null", clientId));

            user = new User()
            {
                ClientId = int.Parse(dt.Rows[0][0].ToString()),
                UserId = int.Parse(dt.Rows[0][1].ToString()),
                PhoneNumber = dt.Rows[0][2].ToString(),
                EmailAddress = dt.Rows[0][3].ToString(),
                FirstName = dt.Rows[0][4].ToString(),
                LastName = dt.Rows[0][5].ToString(),
                TextNumber = dt.Rows[0][6].ToString(),
                Passcode = int.Parse(dt.Rows[0][7].ToString())
            };
            return user;
        }

        private static DataTable GetFilledSqlServerDataTable(/*SqlConnection conn,*/ string qry)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataAdapter oda = new SqlDataAdapter() { SelectCommand = new SqlCommand(qry, conn) };
                oda.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string errmsg = string.Empty;// "Connection Timeout: {0}\n";//conn.ConnectionTimeout
                throw new Exception(string.Format("{0}\n{1}", errmsg, ex.Message));
            }
        }

        //Ref, use LINQ's 'IsDigit' features to parse a string object as an array of char to filter the array 
        //of just Digits: asp.net - Stripping out non-numeric characters in string - Stack Overflow - https://stackoverflow.com/questions/3977497/stripping-out-non-numeric-characters-in-stringacters (using LINQ constructs) and p
        private string GetRandomString(RandomDatatype rdt = RandomDatatype.AlphaNumeric)
        {
            string alphaNumeric = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            string numeric = "0123456789";
            Random random = new Random();
            int length = rdt == RandomDatatype.AlphaNumeric ? 8 : 6;
            string key = rdt == RandomDatatype.AlphaNumeric ? alphaNumeric : numeric; 
            //string sessionId = new string(Enumerable.Repeat(alphaNumeric, numChars)
            //  .Select(s => s[random.Next(s.Length)]).ToArray());
            ////numChars = 6;
            return new string(Enumerable.Repeat(key, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*
         *For UPDATE, INSERT, and DELETE statements, the return value is the number of rows 
         *affected by the command. For all other types of statements, the return value is -1.
        */
        private static int ExecuteSqlServerNonQueryDb(/*string connectionString,*/ string query)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            using (conn)
            {
                try
                {
                    //Console.WriteLine("query: " + updateQuery);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Connection.Open();
                    return (cmd.ExecuteNonQuery());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // GET: Sessions
        public ActionResult Index1()
        {
            return View(db.Sessions.ToList());
        }

        // GET: Sessions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionId,ClientId,Phone,CreationTime,Passcode")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionId,ClientId,Phone,CreationTime,Passcode")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public enum RandomDatatype { Numeric, AlphaNumeric }
    public class TestResponseTime { public int ClientId; public string Phone; public string TestMessage; }
}
