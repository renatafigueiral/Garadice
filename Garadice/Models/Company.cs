using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Garadice.Models
{
    public class Company
    {
        [Display(Name="Company")]
        public int CompanyID { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(IE)?[0-9][0-9A-Z\+\*][0-9]{5}[A-Z]{0,2}",ErrorMessage = "VAT Invalid Format")]
        [Display(Name = "VAT No")]
        public string VatNumber { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{6}", ErrorMessage = "CRO Invalid Format")]
        [Display(Name = "CRO No")]
        public string CRO { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Company Phone")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Company Email")]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Website")]
        public string SiteUrl { get; set; }

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

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}