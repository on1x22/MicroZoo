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

        public Task<Person> CreateNewPerson(Person employee)
        {
            throw new NotImplementedException();
        }

        public Task DeletePerson(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> GetPersonAsync(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(x => x.Id == personId);
            if (person == null)
                return default;

            return person;
        }
        

        public Task<List<Person>> GetEmployeesOfManager(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePerson(Person employee)
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
