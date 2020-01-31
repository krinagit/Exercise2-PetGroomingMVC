using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using PetGrooming.Models.ViewModels;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class PetController : Controller
    {
        
        private PetGroomingContext db = new PetGroomingContext();

     
        public ActionResult List()
        {
          
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets);
           
        }

       
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

       
        [HttpPost]
        public ActionResult Add( string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
            SqlParameter[] sqlparams = new SqlParameter[5];

            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);

          
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }


        public ActionResult New()
        {

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

        public ActionResult Update(int id)
        {
           
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid = @id", new SqlParameter("@id",id)).FirstOrDefault();

            string query = "select * from species";
           List<Species> selectedspecies = db.Species.SqlQuery(query).ToList();

            UpdatePet viewmodel = new UpdatePet();
            viewmodel.pet = selectedpet;
            viewmodel.species = selectedspecies;
            return View(viewmodel);

            return View(selectedpet);
        }

        [HttpPost]
        public ActionResult Update(int id, string PetName, string PetColor, double PetWeight, string PetNotes)
        {

            string query = "update pets set PetName=@PetName, Weight=@PetWeight,Color=@PetColor,Notes=@PetNotes where PetID=@id";
            SqlParameter[] parameters = new SqlParameter[5];

            Debug.WriteLine(query);

            parameters[0] = new SqlParameter("@PetName",PetName);
            parameters[1] = new SqlParameter("@PetWeight",PetWeight);
            parameters[2] = new SqlParameter("@PetColor",PetColor);
            parameters[3] = new SqlParameter("@PetNotes",PetNotes);
            parameters[4] = new SqlParameter("@id",id);

           db.Database.ExecuteSqlCommand(query, parameters);
           
            return RedirectToAction("List");
        }
        
            public ActionResult Delete(int id)
        {
            string query = "delete from pets where petid=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
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
