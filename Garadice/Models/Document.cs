using System;
using System.ComponentModel.DataAnnotations;

namespace Garadice.Models
{
    public class Document
    {

        public int DocumentID { get; set; }


        [StringLength(255)]
        [Display(Name="Document Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [StringLength(100)]
        [Display(Name = "File Content Type")]
        public string ContentType { get; set; }

        [StringLength(100)]
        [Display(Name = "File Extension")]
        public string Extension { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Content { get; set; }
        [Display(Name = "Download Name")]
        public string Path { get; set; }

        [Display(Name = "Document Type")]
        public DocumentType DocumentType { get; set; }


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

        [Display(Name = "Company")]

        public int CompanyID { get; set; }

        public virtual Company Company { get; set; }

    }
}