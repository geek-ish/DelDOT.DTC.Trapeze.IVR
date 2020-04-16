using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCWebApplication2.Models;
using System.Data.SqlClient;

namespace MVCWebApplication2.Controllers
{
    public class ClientDevicesController : Controller
    {
        public string _connectionString = "Data Source=sql7002.site4now.net;Initial Catalog=DB_A3E75F_gk55;Persist Security Info=True;User ID=DB_A3E75F_gk55_admin;Password=helloWorld55!";

        private ClientDevicesContext db = new ClientDevicesContext();

        [HttpGet]
        public void AddRegisteredDevice(int clientId, string phone, string sessionId = null)
        {
            //int? retVal = null;
            string updateQuery = string.Format("UPDATE ClientDevices SET DeviceList = cast ( DeviceList as nvarchar(max))  + cast ('; {0}' as nvarchar(max) )where ClientId = {1}",
                phone, clientId);
            string insertQuery = string.Format("INSERT INTO[dbo].[ClientDevices]([ClientId],[DeviceList]) VALUES({0},'{1}');",
                clientId, phone);
            string query = ClientHasRegisteredDevice(clientId) ? updateQuery : insertQuery;
            if (sessionId != null) query += string.Format("; UPDATE Sessions set UnregisteredNumber = null where SessionId='{0}';", sessionId);

            if (!IsRegisteredDevice(clientId, phone))
                /*retVal = */ExecuteSqlServerNonQueryDb(query);

            //return retVal;

        }

        bool ClientHasRegisteredDevice(int clientId)
        {
            string query = string.Format("select * from ClientDevices where clientId = {0}", clientId);
            DataTable dt = GetFilledSqlServerDataTable(query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                    return true;

            return false;
        }

        bool IsRegisteredDevice(int clientId, string phoneNumber)
        {
            string query = string.Format("select DeviceList from ClientDevices where ClientId = {0}", clientId);
            DataTable dt = GetFilledSqlServerDataTable(query); if (dt == null || dt.Rows.Count < 1) return false;

            return (dt.Rows[0][0].ToString().Contains(phoneNumber));
        }

        private DataTable GetFilledSqlServerDataTable(/*SqlConnection conn,*/ string qry)
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

        /*
         *For UPDATE, INSERT, and DELETE statements, the return value is the number of rows 
         *affected by the command. For all other types of statements, the return value is -1.
        */
        private int ExecuteSqlServerNonQueryDb(/*string connectionString,*/ string query)
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

        // GET: ClientDevices
        public ActionResult Index()
        {
            return View(db.ClientDevices.ToList());
        }

        // GET: ClientDevices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientDevice clientDevice = db.ClientDevices.Find(id);
            if (clientDevice == null)
            {
                return HttpNotFound();
            }
            return View(clientDevice);
        }

        // GET: ClientDevices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientDevices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientDeviceId,ClientId,DeviceList")] ClientDevice clientDevice)
        {
            if (ModelState.IsValid)
            {
                db.ClientDevices.Add(clientDevice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clientDevice);
        }

        // GET: ClientDevices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientDevice clientDevice = db.ClientDevices.Find(id);
            if (clientDevice == null)
            {
                return HttpNotFound();
            }
            return View(clientDevice);
        }

        // POST: ClientDevices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientDeviceId,ClientId,DeviceList")] ClientDevice clientDevice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientDevice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientDevice);
        }

        // GET: ClientDevices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientDevice clientDevice = db.ClientDevices.Find(id);
            if (clientDevice == null)
            {
                return HttpNotFound();
            }
            return View(clientDevice);
        }

        // POST: ClientDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientDevice clientDevice = db.ClientDevices.Find(id);
            db.ClientDevices.Remove(clientDevice);
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
}
