using System.ComponentModel.DataAnnotations;
namespace Garadice.Models
{
    public enum EmployeePosition
    {
        [Display(Name = "Accountant")]
        Accountant=0,
        Supervisor,
        Receptionist
    }
}