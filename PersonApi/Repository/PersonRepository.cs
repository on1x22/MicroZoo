using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.PersonsApi.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _dbContext;

        public PersonRepository(PersonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person> GetPersonAsync(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                return default;

            return person;
        }

        public async Task<Person> AddPersonAsync(PersonDto personDto)
        {
            var person = personDto.ToPerson();

            await _dbContext.Persons.AddAsync(person);
            await SaveChangesAsync();

            return person;
        }
        
        public async Task<Person> UpdatePersonAsync(int personId, PersonDto personDto)
        {
            if (personDto == null)
                return default;

            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
                return default;

            person!.FirstName = personDto!.FirstName;
            person!.LastName = personDto!.LastName;
            person!.Email = personDto!.Email;
            person!.IsManager = personDto!.IsManager;
            person!.ManagerId = personDto!.ManagerId;

            await SaveChangesAsync();

            return person;
        }

        public async Task<Person> DeletePersonAsync(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                return default;

            _dbContext.Persons.Remove(person);
            await SaveChangesAsync();

            return person;
        }

        public async Task<List<Person>> GetSubordinatePersonnelAsync(int personId) =>       
            await _dbContext.Persons.Where(p => p.ManagerId == personId).ToListAsync();

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

        public async Task<bool> CheckPersonIsManager(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null) 
                return false;

            return person.IsManager;
        }

        public async Task<List<Person>> ChangeManagerForSubordinatePersonnel(int currentManagerId, int newManagerId)
        {
            var subordinatePersonnel = await _dbContext.Persons.Where(p => p.ManagerId == currentManagerId).ToListAsync();
            subordinatePersonnel.ForEach(sp => sp.ManagerId = newManagerId);
            
            await SaveChangesAsync();
            
            return subordinatePersonnel;
        }
    }
}
