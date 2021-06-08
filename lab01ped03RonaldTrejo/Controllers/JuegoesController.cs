﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using lab01ped03RonaldTrejo.DAL;
using lab01ped03RonaldTrejo.Models;

namespace lab01ped03RonaldTrejo.Controllers
{
    public class JuegoesController : Controller
    {
        private GamesContext db = new GamesContext();

        // GET: Juegoes
        public ActionResult Index()
        {
            var juegos = db.Juegos.Include(j => j.Categoria);
            string order = Request.QueryString["order"];
            var juegosOrdenados = juegos.ToList();
            if (order == "asc")
            {
                @ViewBag.order = "desc";
                juegosOrdenados = juegos.OrderBy(j => j.Precio).ToList();
            }
            else
            {
                @ViewBag.order = "asc";
                juegosOrdenados = juegos.OrderByDescending(j => j.Precio).ToList();
            }
            return View(juegosOrdenados);
        }

        // GET: Juegoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juego juego = db.Juegos.Find(id);
            if (juego == null)
            {
                return HttpNotFound();
            }
            return View(juego);
        }

        // GET: Juegoes/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Cat");
            return View();
        }

        // POST: Juegoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NomJuego,Precio,Existencias,Imagen,CategoriaID")] Juego juego)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    if (file != null)
                    {
                        string date = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), date + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        juego.Imagen = date + Path.GetExtension(file.FileName);
                    }
                }
                finally
                {

                }
                db.Juegos.Add(juego);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Cat", juego.CategoriaID);
            return View(juego);
        }

        // GET: Juegoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juego juego = db.Juegos.Find(id);
            if (juego == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Cat", juego.CategoriaID);
            return View(juego);
        }

        // POST: Juegoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NomJuego,Precio,Existencias,Imagen,CategoriaID")] Juego juego)
        {
            if (ModelState.IsValid)
            {
                db.Entry(juego).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaID = new SelectList(db.Categorias, "ID", "Cat", juego.CategoriaID);
            return View(juego);
        }

        // GET: Juegoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juego juego = db.Juegos.Find(id);
            if (juego == null)
            {
                return HttpNotFound();
            }
            return View(juego);
        }

        // POST: Juegoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Juego juego = db.Juegos.Find(id);
            db.Juegos.Remove(juego);
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
