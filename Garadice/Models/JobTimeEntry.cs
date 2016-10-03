using System;
using System.ComponentModel.DataAnnotations;

namespace Garadice.Models
{
    public class JobTimeEntry
    {

        public int JobTimeEntryID { get; set; }

        [Display(Name = "More Info")]
        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date: dd/MM/yyyy)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime Start { get; set; }


        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        [Display(Name = "Start Time hh:mm")]
        public TimeSpan StartTime { get; set; }


        //[Display(Name = "Number of Hours")]
        //[DataType(DataType.Duration)]
        //public Decimal Duration { get; set; }

        //[DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:hh:mm}")]
        //[Display(Name = "Number of Hours hh:mm")]

        //public TimeSpan? NumberOfHours
        //{
        //    get { return NumberOfHours ?? TimeSpan.Parse("00:00"); }
        //    set { NumberOfHours = value; }

        //}

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        [Display(Name = "Number of Hours hh:mm")]
        public TimeSpan NumberOfHours { get; set; }


        //[Display(Name = "End Time dd/MM/yyyy hh:mm")]
        //[DataType(DataType.DateTime)]
        ////[DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}")]
        //public DateTime? End { get; set; }

        public int JobID { get; set; }
        public virtual Job Job { get; set; }

    }
}