﻿using System;
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
            Job job = db.Jobs.Include(items => items.Worker).FirstOrDefault(items => items.JobId == jobId);
            Worker claimingWorker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);

            if (claimingWorker.Available)
            {
                claimingWorker.Available = false;
                job.Worker = claimingWorker;
            }

            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

        [HttpPost]
        public IActionResult StatusChange(int jobId, string changeValue)
        {
            List<Job> workerJobs = db.Jobs.Include(items => items.Worker).Where(items => items.JobId == jobId).ToList();

            if (changeValue == "pending")
            {
                foreach (Job job in workerJobs)
                {
                    if (job.JobId == jobId && job.Completed == false)
                    {
                        job.MarkPending();
                        job.Worker.Available = false;
                        db.Entry(job).State = EntityState.Modified;
                        db.Entry(job.Worker).State = EntityState.Modified;
                    }
                    else if (job.Completed == false)
                    {
                        job.MarkInactive();
                        db.Entry(job).State = EntityState.Modified;
                    }
                }
            }
            else if (changeValue == "complete")
            {
                foreach (Job job in workerJobs)
                {
                    if (job.JobId == jobId)
                    {
                        job.MarkCompleted();
                        job.Worker.Available = true;
                        db.Entry(job).State = EntityState.Modified;
                        db.Entry(job.Worker).State = EntityState.Modified;
                    }
                }
            }
            db.SaveChangesAsync();

            return View(workerJobs);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Job job = db.Jobs.FirstOrDefault(j => j.JobId == id);
            db.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
