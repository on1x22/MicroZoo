using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.Infrastructure.Models.Persons;

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
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId
                                                                      && p.Deleted == false);
            if (person == null)
                return default;

            return person;
        }

        public async Task<Person> AddPersonAsync(Person person)
        {
            //var person = person.ToPerson();

            await _dbContext.Persons.AddAsync(person);
            await SaveChangesAsync();

            return person;
        }
        
        public async Task<Person> UpdatePersonAsync(int personId, Person person)
        {
            if (person == null)
                return default;

            var updatedPerson = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId
                                                                             && p.Deleted == false);

            if (person == null)
                return default;

            updatedPerson!.FirstName = person!.FirstName;
            updatedPerson!.LastName = person!.LastName;
            updatedPerson!.Email = person!.Email;
            updatedPerson!.IsManager = person!.IsManager;
            updatedPerson!.ManagerId = person!.ManagerId; 

            await SaveChangesAsync();

            return person;
        }

        public async Task<Person> SoftDeletePersonAsync(/*int personId*/Person person)
        {
            /*var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                return default;

            _dbContext.Persons.Remove(person);*/
            person.Deleted = true;
            _dbContext.Update(person);

            await SaveChangesAsync();

            return person;
        }

        public async Task<List<Person>> GetSubordinatePersonnelAsync(int personId) =>       
            await _dbContext.Persons.Where(p => p.ManagerId == personId && p.Deleted == false)
                .ToListAsync();

        public async Task<bool> CheckPersonIsManager(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId 
                                                                      && p.Deleted == false);
            if (person == null) 
                return false;

            return person.IsManager;
        }

        public async Task<List<Person>> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                             int newManagerId)
        {
            var subordinatePersonnel = await _dbContext.Persons.Where(p => 
            p.ManagerId == currentManagerId && p.Deleted == false).ToListAsync();

            subordinatePersonnel.ForEach(sp => sp.ManagerId = newManagerId);
            
            await SaveChangesAsync();
            
            return subordinatePersonnel;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
