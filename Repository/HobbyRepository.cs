using EmployeePortal.Data;
using EmployeePortal.DTO;
using EmployeePortal.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Repository
{
    public class HobbyRepository
    {
         private readonly AppDbContext _appDbContext;

        public HobbyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ;   
        }

        public async Task<List<HobbyDto>> GetHobbyByEmployee(int employeeId, List<string>? hobbyNames = null)
        {
            var employeeExists = await _appDbContext.Employees
                .AnyAsync(e => e.Id == employeeId);

            if (!employeeExists)
                return new List<HobbyDto>();

            var query = _appDbContext.Hobbies
                .Where(h => h.EmployeeId == employeeId);

            if (hobbyNames != null && hobbyNames.Any())
            {
                query = query.Where(h => hobbyNames.Contains(h.HobbyName));
            }

            return await query.Select(h => new HobbyDto
            {
                Hobbyid = h.Hobbyid,
                HobbyName = h.HobbyName
            }).ToListAsync();
        }



        public List<Hobbies> getHobby()
        {
            try
            {
                return _appDbContext.Hobbies.ToList();


            }catch(Exception e)
            {

                throw new Exception("not found");
            }


        }

        public async Task AddHobby(Hobbies hobbies)
        {
            try
            {
                var emp = await _appDbContext.Employees.FindAsync(hobbies.EmployeeId);
                if (emp == null)
                    throw new Exception("Employee not found");

                await _appDbContext.Hobbies.AddAsync(hobbies);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Hobby was not added due to an internal exception", ex);
            }
        }
        public async Task UpdateHobby(Hobbies hobbies)
        {
            try
            {
                var existingHobby = await _appDbContext.Hobbies.FindAsync(hobbies.Hobbyid);
                if (existingHobby == null)
                    throw new Exception($"Hobby with ID {hobbies.Hobbyid} not found");

                var employeeExists = await _appDbContext.Employees.AnyAsync(e => e.Id == hobbies.EmployeeId);
                if (!employeeExists)
                    throw new Exception($"Employee with ID {hobbies.EmployeeId} does not exist");

                existingHobby.HobbyName = hobbies.HobbyName;
                existingHobby.EmployeeId = hobbies.EmployeeId;

                _appDbContext.Hobbies.Update(existingHobby);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Hobby update failed due to internal error", ex);
            }
        }
         public async Task DeleteHobby(int id)
        {

            var hobby = await _appDbContext.Hobbies.FindAsync(id);
            if (hobby == null)
                throw new Exception("Hobby not found");
            _appDbContext.Hobbies.Remove(hobby);

            await _appDbContext.SaveChangesAsync(); 



        }
        public async Task AddMultipleHobbies(List<Hobbies> hobbies)
        {
            _appDbContext.Hobbies.AddRange(hobbies);
            await _appDbContext.SaveChangesAsync();
        }




    }
}
