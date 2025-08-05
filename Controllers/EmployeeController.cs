using EmployeePortal.Data;
using EmployeePortal.DTO;
using EmployeePortal.Model;
using EmployeePortal.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace EmployeePortal.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        private readonly EmployeeRepository emp;
        private readonly HobbyRepository hobbyRepository;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public EmployeeController(EmployeeRepository employeeRepository, HobbyRepository hobbyRepository, IConfiguration config, AppDbContext context, IWebHostEnvironment env)
        {
            this.emp = employeeRepository;
            this.hobbyRepository = hobbyRepository;
            _config = config;
            _context = context;
            _env = env;



        }

        [HttpGet("EmployeeList")]
        public async Task<IActionResult> EmployeeList()
        {

            var allEmployees = await emp.GetAllEmployees();
            return Ok(allEmployees);


        }
        [HttpPost("AddHobbiesToEmployee")]
        public async Task<IActionResult> AddHobbiesToEmployee([FromBody] HobbbyDtoAdd dto)
        {
            try
            {
                var employee = await emp.GetDetailsAsync(dto.EmployeeId); 
                if (employee == null)
                    return NotFound("Employee not found");  

                foreach (var hobbyName in dto.Hobbies)
                {
                    var hobby = new Hobbies
                    {
                        EmployeeId = dto.EmployeeId,
                        HobbyName = hobbyName
                    };
                    await hobbyRepository.AddHobby(hobby);
                }

                return Ok(new { message = "Hobbies added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //[HttpPost("AddEmployee")]
        //public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto empDto, IFormFile file)
        //{
        //    try
        //    {
        //        var employee = new Employee
        //        {
        //            Name = empDto.Name,
        //            Email = empDto.Email,
        //            Mobile = empDto.Mobile,
        //            Age = empDto.Age,
        //            Salary = empDto.Salary,
        //            Status = empDto.Status
        //        };

        //        await emp.SaveEmployee(employee);
        //        return Ok(employee);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}


        [HttpPost("AddEmployee")]
[Consumes("multipart/form-data")]
public async Task<IActionResult> AddEmployee([FromForm] EmployeeDto empDto, IFormFile imageFile)
{
    try
    {
        string filePath = null;

        if (imageFile != null && imageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            var fileName = Path.GetFileName(imageFile.FileName);
            filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
        }

        var employee = new Employee
        {
            Name = empDto.Name,
            Email = empDto.Email,
            Mobile = empDto.Mobile,
            Age = empDto.Age,
            Salary = empDto.Salary,
            Status = empDto.Status,
            ImagePath = filePath // store path to DB
        };

        await emp.SaveEmployee(employee);
        return Ok(employee);
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}

        //[HttpPost("updateEmployee/{id}")]
        //public async Task<ActionResult> updateEmployee(int id, [FromBody] Employee vm)
        //{
        //    await emp.UpdateEmployee(id, vm);
        //    return Ok(vm);
        //}
        [HttpPost("updateEmployee/{id}")]
        public async Task<ActionResult> updateEmployee(int id, [FromBody] Employee vm)
        {
            try
            {
                await emp.UpdateEmployee(id, vm);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("deleteEmployee/{id}")]

        public async Task<ActionResult> deleteEmployee(int id)
        {
            await emp.DeleteEmplyoee(id);
            return Ok();

        }
        [HttpGet("detailsEmployee/{id}")]
        public async Task<IActionResult> detailsEmployee(int id)
            {
            var emp1 = await emp.GetDetailsAsync(id);
            return Ok(emp1);
        }

        [HttpGet("GetHobbyOfEmployee/{id}")]
        public async Task<IActionResult> GetHobbyOfEmployee(int id)
        {
            var employee = await emp.GetDetailsAsync(id);
            if (employee == null)
                return NotFound("Employee not found");

            var hobbies = await hobbyRepository.GetHobbyByEmployee(id);

            var employeeWithHobbies = new EmployeeWithHobbiesDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Mobile = employee.Mobile,
                Age = employee.Age,
                Salary = employee.Salary,
                Status = employee.Status,
                Hobbies = hobbies 
            };

            return Ok(employeeWithHobbies);
        }


        [HttpPut("UpdateHobby/{id}")]
        public async Task<IActionResult> UpdateHobby( [FromBody] UpdateHobbyDto dto)

        {
            try
            {
                var hobby = new Hobbies
                {
                    Hobbyid = dto.Hobbyid,
                    EmployeeId = dto.EmployeeId,
                    HobbyName = dto.HobbyName
                };

                await hobbyRepository.UpdateHobby(hobby);

                return Ok(new { message = "Hobby updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }




        [HttpGet("GetHobby")]
        public async Task<IActionResult> hobbieslist()
        {

            var hobby = hobbyRepository.getHobby();
            return Ok(hobby);
        }
        [HttpDelete("RemoveHobby/{id}")]
        public async Task<IActionResult> RemoveHobby(int id)
        {
            try
            {
                await hobbyRepository.DeleteHobby(id); 
                return Ok(new { message = "Hobby deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultipleHobbies([FromBody] List<HobbyDto> hobbies)
        {
            if (hobbies == null || hobbies.Count == 0)
            {
                return BadRequest("Hobby list is empty.");
            }

            try
            {
                var hobbyEntities = hobbies.Select(h => new Hobbies
                {
                    HobbyName = h.HobbyName,
                    EmployeeId = h.EmployeeId
                }).ToList();

                await hobbyRepository.AddMultipleHobbies(hobbyEntities);
                return Ok(new { message = "Hobbies added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
            
        {
            var user = _context.users.FirstOrDefault(u => u.Username == loginDto.Username && u.Password == loginDto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = new JwtService(_config).GenerateToken(user.Username);
            return Ok(new { token });
        }
        [HttpPut("{id}/rating")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] int rating)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            employee.Rating = rating;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("qr/{id}")]
        public async Task<IActionResult> GenerateQrCode(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            string qrData = $"Name: {employee.Name}\nDesignation: {employee.Status}\nEmail: {employee.Email}";

            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new PngByteQRCode(qrCodeData))
            {
                var qrCodeBytes = qrCode.GetGraphic(20);
                return File(qrCodeBytes, "image/png");
            }
        }
        [HttpPost("upload-image/{id}")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            var filePath = Path.Combine(uploads, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            employee.ImagePath = $"/uploads/{file.FileName}";
            await _context.SaveChangesAsync();

            return Ok(employee);
        }


    }
}
