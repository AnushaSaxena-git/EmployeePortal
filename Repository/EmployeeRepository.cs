using EmployeePortal.Data;
using EmployeePortal.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Repository
{
    public class EmployeeRepository
    {
        private readonly AppDbContext db;
        private readonly HobbyRepository hobbyRepository;
        public EmployeeRepository(AppDbContext dbContext, HobbyRepository hobbyRepository)
        {
            this.db = dbContext;
            this.hobbyRepository = hobbyRepository;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {


            return await db.Employees.ToListAsync();
        }
        public async Task SaveEmployee(Employee emp)
        {
            try
            {

                await db.Employees.AddAsync(emp);

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception();
            }
        }
        public async Task UpdateEmployee(int id, Employee emp)
        {
            try
            {
                var employee = await db.Employees.FindAsync(id);
                if (employee == null)
                {

                    throw new Exception("employee not found ");
                }
                employee.Name = emp.Name;
                employee.Email = emp.Email;
                employee.Salary = emp.Salary;
                employee.Status = emp.Status;
                employee.Age = emp.Age;
                employee.Mobile = emp.Mobile;

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw new Exception();
            }

        }
        public async Task DeleteEmplyoee(int id)
        {
            var employee = await db.Employees.FindAsync(id);
            if (id == null)
            {

                throw new Exception("id not found  in the database");
            }
            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

        }
        public async Task<Employee> GetDetailsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID provided.");
            }

            var employee = await db.Employees.FindAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            return employee;
        }
        public async Task AddHobby(Hobbies hobbies, int id)
        {
            try
            {
                var emp = await db.Employees.FindAsync(id);
                if (emp == null)
                    throw new Exception("Employee not found ");
                await db.AddAsync(hobbies);

                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {   throw new Exception("employee was not added due to the internal exception");

            }


        }
    }
}