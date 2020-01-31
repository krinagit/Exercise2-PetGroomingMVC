using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
          public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult List()
        {
        
            List<Species> myspecies = db.Species.SqlQuery("Select * from species").ToList();

            return View(myspecies);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string SpeciesName)
       
        {
            string query = "Insert into species (Name) values (@SpeciesName)";
            SqlParameter param = new SqlParameter("@SpeciesName",SpeciesName);

            db.Database.ExecuteSqlCommand(query, param);
    

            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            string query = "select * from species where SpeciesID=@id";
            SqlParameter param = new SqlParameter("@id", id);

            Species selectedspecies = db.Species.SqlQuery(query, param).FirstOrDefault();
            return View(selectedspecies);
        }

        [HttpPost]
        public ActionResult Update(int id,string SpeciesName)
        {
            string query = "update species set Name=@SpeciesName where SpeciesID=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
        public ActionResult Show(int id)
        {
            string query = "select * from species where SpeciesID =@id";
            SqlParameter param = new SqlParameter("@id", id);

            Species selectedspecies = db.Species.SqlQuery(query, param).First();
            return View(selectedspecies);
        }
        public ActionResult Delete(int id)
        {

            string query = "delete from species where SpeciesID =@id";
            SqlParameter param = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, param);

            return RedirectToAction("List");
        }

         
          
    }
}