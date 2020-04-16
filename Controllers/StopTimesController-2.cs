using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCWebApplication2.Models;

namespace MVCWebApplication2.Controllers
{
    public class StopTimesController : Controller
    {
        private StopTimesContext db = new StopTimesContext();

        // GET: StopTimes
        public ActionResult Index()
        {
            return View(db.StopTimes.ToList());
        }

        // GET: StopTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopTime stopTime = db.StopTimes.Find(id);
            if (stopTime == null)
            {
                return HttpNotFound();
            }
            return View(stopTime);
        }

        // GET: StopTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StopTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StopTimeId,Direction,Destination,StopId,TimeOfStop,Route")] StopTime stopTime)
        {
            if (ModelState.IsValid)
            {
                db.StopTimes.Add(stopTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stopTime);
        }

        // GET: StopTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopTime stopTime = db.StopTimes.Find(id);
            if (stopTime == null)
            {
                return HttpNotFound();
            }
            return View(stopTime);
        }

        // POST: StopTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StopTimeId,Direction,Destination,StopId,TimeOfStop,Route")] StopTime stopTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stopTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stopTime);
        }

        // GET: StopTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StopTime stopTime = db.StopTimes.Find(id);
            if (stopTime == null)
            {
                return HttpNotFound();
            }
            return View(stopTime);
        }

        // POST: StopTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StopTime stopTime = db.StopTimes.Find(id);
            db.StopTimes.Remove(stopTime);
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
