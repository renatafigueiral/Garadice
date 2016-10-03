using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Garadice.Models
{
    public class Employee
    {
        [Display(Name = "Employee")]
        public int EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Employee Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }

        }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Position")]
        [EnumDataType(typeof(EmployeePosition))]
        public EmployeePosition Position { get; set; }

        [StringLength(300)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "More Info")]
        public string Description { get; set; }


        //[Display(Name = "Maximum Work Hours Monthly")]
        //[Range(0,288)]
        //public int? MaxHoursMonthly
        //{
        //    get { return MaxHoursMonthly ?? 120; }
        //    set { MaxHoursMonthly = value; }
        //}


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }
        private DateTime? createdDate;

        public virtual ICollection<Job> Jobs { get; set; }


    }
}