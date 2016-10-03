using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Garadice.Models
{
    public class Job
    {
        [Display(Name ="Job")]
        public int JobID { get; set; }


        [Required]
        [Display(Name = "Job Type")]
        [EnumDataType(typeof(JobType))]
        public JobType JobType { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        [DataType(DataType.Text)]
        [StringLength(255)]
        public string JobName { get; set; }

        [Display(Name = "More Info")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string Description { get; set; }

        [Display(Name ="Employee")]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        [Display(Name ="Company")]
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }


       
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        [Display(Name = "Estimated Duration hh:mm")]

        public TimeSpan? EstimatedDuration { get; set; }
        //{
        //    get
        //    {
        //        if (EstimatedDuration == null)
        //            return new TimeSpan(0, 0, 0);
        //        else
        //            return EstimatedDuration;
        //    }
        //    set { EstimatedDuration = value; }

        //}

        [Required]
        [Display(Name = "Job Status")]
        [EnumDataType(typeof(JobStatus))]
        public JobStatus JobStatus { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }
        private DateTime? createdDate;



        public ICollection<JobTrack> JobTrack;
    }
}