﻿using Help_Desk_2.DataAccessLayer;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Help_Desk_2.Models;
using Help_Desk_2.Utilities;
using System;
using System.Net;

namespace Help_Desk_2.Controllers
{
    public class FAQsController : Controller
    {
        private HelpDeskContext db = new HelpDeskContext();

        // GET: FAQs
        public ActionResult Index()
        {
            //return View();
            //var FAQs = db.KnowledgeFAQs.Include(k => k.UserProfile);
            return View(db.KnowledgeFAQs.ToList());
        }

        // GET: FAQs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FAQs/New
        public ActionResult New()
        {
            //ViewBag.originatorID = new SelectList(db.UserProfiles, "userID", "loginName");
            ViewBag.mode = 0;
            return View("FAQOne");
        }

        // POST: FAQs/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "type,headerText,description,links")] KnowledgeFAQ faq)
        {
            if (ModelState.IsValid)
            {
                UserData ud = new UserData();
                UserProfile userProfile = ud.getUserProfile();
                
                faq.dateComposed = DateTime.Now;
                faq.originatorID = userProfile.userID;

                faq = db.KnowledgeFAQs.Add(faq);

                /***** Add File ************/
                AllSorts.saveAttachments(faq.ID, db);
                db.SaveChanges();

                if (Request.Form.AllKeys.Contains("btnSave"))
                {
                    return RedirectToAction("Edit/" + faq.ID);
                }
                return RedirectToAction("Index");
            }

            ViewBag.mode = 0;
            ViewBag.newTicket = "1";
            ViewBag.addAttachCode = "1"; //Activate attach code 
            ViewBag.addTinyMCECode = "1"; //Activate tinymce code
            return View("FAQOne",faq);
        }

        // GET: FAQs/Suggest
        public ActionResult Suggest()
        {
            return View();
        }

        // GET: FAQs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeFAQ faq = db.KnowledgeFAQs.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.mode = 1;
            return View("FAQOne",faq);
        }

        // POST: FAQs/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,originatorID,expiryDate,dateComposed,type,headerText,description,links,deleteField")] KnowledgeFAQ faq)
        {
            if (ModelState.IsValid)
            {

                db.Entry(faq).State = EntityState.Modified;

                /***** Add File ************/
                AllSorts.saveAttachments(faq.ID, db, faq.deleteField);
                db.SaveChanges();

                if (Request.Form.AllKeys.Contains("btnSave"))
                {
                    return RedirectToAction("Edit/" + faq.ID);
                }
                return RedirectToAction("Index");
            }

            ViewBag.mode = 1;
            return View("FAQOne", faq);
        }
    

        // GET: FAQs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FAQs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        /************
        

        // GET: KnowledgeFAQs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeFAQ knowledgeFAQ = db.KnowledgeFAQs.Find(id);
            if (knowledgeFAQ == null)
            {
                return HttpNotFound();
            }
            return View(knowledgeFAQ);
        }

        // GET: KnowledgeFAQs/Create
        public ActionResult Create()
        {
            ViewBag.originatorID = new SelectList(db.UserProfiles, "userID", "loginName");
            return View();
        }

        // POST: KnowledgeFAQs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,originatorUsername,originatorFullname,dateComposed,exiryDate,headerText,description,originatorID,links")] KnowledgeFAQ knowledgeFAQ)
        {
            if (ModelState.IsValid)
            {
                db.KnowledgeFAQs.Add(knowledgeFAQ);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.originatorID = new SelectList(db.UserProfiles, "userID", "loginName", knowledgeFAQ.originatorID);
            return View(knowledgeFAQ);
        }

        // GET: KnowledgeFAQs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeFAQ knowledgeFAQ = db.KnowledgeFAQs.Find(id);
            if (knowledgeFAQ == null)
            {
                return HttpNotFound();
            }
            ViewBag.originatorID = new SelectList(db.UserProfiles, "userID", "loginName", knowledgeFAQ.originatorID);
            return View(knowledgeFAQ);
        }

        // POST: KnowledgeFAQs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,originatorUsername,originatorFullname,dateComposed,exiryDate,headerText,description,originatorID,links")] KnowledgeFAQ knowledgeFAQ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knowledgeFAQ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.originatorID = new SelectList(db.UserProfiles, "userID", "loginName", knowledgeFAQ.originatorID);
            return View(knowledgeFAQ);
        }

        // GET: KnowledgeFAQs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeFAQ knowledgeFAQ = db.KnowledgeFAQs.Find(id);
            if (knowledgeFAQ == null)
            {
                return HttpNotFound();
            }
            return View(knowledgeFAQ);
        }

        // POST: KnowledgeFAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KnowledgeFAQ knowledgeFAQ = db.KnowledgeFAQs.Find(id);
            db.KnowledgeFAQs.Remove(knowledgeFAQ);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        *******************/

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
