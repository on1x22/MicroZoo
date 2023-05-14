using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.PersonsApi.Models;
using System.Reflection.Metadata.Ecma335;
using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _dbContext;

        public PersonRepository(PersonDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Task<Person> CreateNewEmployee(Person employee)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> GetById(int id) => 
            await _dbContext.Persons.FirstOrDefaultAsync(x => x.Id == id);
        

        public Task<List<Person>> GetEmployeesOfManager(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateEmployee(Person employee)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == employee.Id);

            if (person == null) return;

            person.FirstName = employee.FirstName; 
            person.LastName = employee.LastName;
            person.Email = employee.Email;
            person.IsManager = employee.IsManager;
            person.ManagerId = employee.ManagerId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
