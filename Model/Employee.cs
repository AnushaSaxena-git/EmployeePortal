using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortal.Model
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
         
        public string Email { get; set; }
        public string? Mobile { get; set; }

        public int Age { get; set; }

        public int Salary { get; set; }
        public int Rating { get; set; } 

        public bool? Status { get; set; }
        public string ImagePath { get; set; } // Store the file path or URL

        public ICollection<Hobbies>? Hobbies { get; set; }
    }

}
