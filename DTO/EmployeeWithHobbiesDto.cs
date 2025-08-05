namespace EmployeePortal.DTO
{
    public class EmployeeWithHobbiesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public bool? Status { get; set; }
        public List<HobbyDto> Hobbies { get; set; } = new List<HobbyDto>();



    }
}
