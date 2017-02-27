using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

namespace MrFixIt.Controllers
{
    public class JobsController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(db.Jobs.Include(i => i.Worker).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            db.Jobs.Add(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Claim(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        [ActionName("Claim")]
        public IActionResult ClaimJob(int jobId)
        {
            Job job = db.Jobs.Include(j => j.Worker).FirstOrDefault(items => items.JobId == jobId);
            Worker claimingWorker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);

            if (claimingWorker.Avaliable)
            {
                claimingWorker.Avaliable = false;
                job.Worker = claimingWorker;
            }

            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

        
    }
}
