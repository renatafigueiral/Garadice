using System.ComponentModel.DataAnnotations;
namespace Garadice.Models
{
    public enum JobType
    {
        [Display(Name = "Anti Money Laundry")]
        AML = 0,
        Tax,
        Payroll,
        Other
    }
}