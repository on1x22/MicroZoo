using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.PersonsApi.Models;
using System.Reflection.Metadata.Ecma335;

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

            person.firstName = employee.firstName; 
            person.lastName = employee.lastName;
            person.email = employee.email;
            person.isManager = employee.isManager;
            person.managerId = employee.managerId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
