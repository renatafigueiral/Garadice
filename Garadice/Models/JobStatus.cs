using System.ComponentModel.DataAnnotations;
namespace Garadice.Models
{
    public enum JobStatus
    {
        Assigned =0,
        [Display(Name = "In Progress")]
        InProgress,
        Completed,
        Canceled
    }
}
