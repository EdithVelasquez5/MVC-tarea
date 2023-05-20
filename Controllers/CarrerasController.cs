using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MVC_tarea.Models;
using static System.Net.WebRequestMethods;

namespace MVC_tarea.Controllers
{
    public class CarrerasController : Controller
    {
        private DonBoscoEntities db = new DonBoscoEntities();

        // GET: Carreras
        public ActionResult Index()
        {
            return View(db.Carrera.ToList());
        }

        public ActionResult Carreras()
        {
            return View(db.Carrera.ToList());
        }

        // GET: Carreras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return HttpNotFound();
            }
            return View(carrera);
        }

        // GET: Carreras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carreras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCarrera,Nombre,Descripcion,Materias,Ciclos")] Carrera carrera)
        {
            HttpPostedFileBase http = Request.Files[0];
            WebImage webImage = new WebImage(http.InputStream);
            carrera.Imagen = webImage.GetBytes();

            if (ModelState.IsValid)
            {
                db.Carrera.Add(carrera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carrera);
        }

        // GET: Carreras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return HttpNotFound();
            }
            return View(carrera);
        }

        // POST: Carreras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCarrera,Nombre,Descripcion,Materias,Ciclos,")] Carrera carrera)
        {
            byte[] ImagenActual = null;

            HttpPostedFileBase http = Request.Files[0];
            if (http == null)
            {
                ImagenActual = db.Carrera.SingleOrDefault(t => t.IdCarrera == carrera.IdCarrera).Imagen;
                carrera.Imagen = ImagenActual;
            }
            else
            {
                WebImage webImage = new WebImage(http.InputStream);
                carrera.Imagen = webImage.GetBytes();
            }

            if (ModelState.IsValid)
            {
                db.Entry(carrera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carrera);
        }

        // GET: Carreras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrera carrera = db.Carrera.Find(id);
            if (carrera == null)
            {
                return HttpNotFound();
            }
            return View(carrera);
        }

        // POST: Carreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carrera carrera = db.Carrera.Find(id);
            db.Carrera.Remove(carrera);
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

        public ActionResult getImage(int id)
        {
            Carrera carrerask = db.Carrera.Find(id);
            byte[] byteimagen = carrerask.Imagen;

            MemoryStream memoryStream = new MemoryStream(byteimagen);
            Image image = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpg");

        }
    }
}
