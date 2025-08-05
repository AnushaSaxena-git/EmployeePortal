using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortal.Model
{
    public class Hobbies
    {
        [Key]
        public int Hobbyid { get; set; }

        [Required]
        public string HobbyName { get; set; } = string.Empty; 

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }
    }
}
