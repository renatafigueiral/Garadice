using System;
using System.ComponentModel.DataAnnotations;

namespace Garadice.Models
{
    public class Contact {

        public int ContactID { get; set; }

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

        [Display(Name = "Full Name")]
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
        [DataType(DataType.Text)]
        [StringLength(50)]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [StringLength(300)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "More Info")]
        public string Description { get; set; }

        private DateTime? createdDate;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
    }
}