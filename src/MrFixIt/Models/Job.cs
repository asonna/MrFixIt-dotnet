using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MrFixIt.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public bool Pending { get; set; }
        public virtual Worker Worker { get; set; }


        public void MarkCompleted()
        {
            Completed = true;
            Pending = false;
        }

        public void MarkPending()
        {
            Completed = false;
            Pending = true;
        }
        public void MarkInactive()
        {
            Completed = false;
            Pending = false;
        }

        public Worker FindWorker(string UserName)
        {
            Worker thisWorker = new MrFixItContext().Workers.FirstOrDefault(i => i.UserName == UserName);
            return thisWorker;
        }
    }
}
