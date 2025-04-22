using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.DbContexts;
using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Repository
{
    /// <summary>
    /// Provides processing of persons requests to database
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="PersonRepository"/> class 
        /// </summary>
        /// <param name="dbContext"></param>
        public PersonRepository(PersonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronous returns information about specified person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<Person> GetPersonAsync(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId
                                                                      && p.Deleted == false);
            if (person == null)
                return default!;

            return person;
        }

        /// <summary>
        /// Asynchronous adds new person to database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<Person> AddPersonAsync(Person person)
        {
            //var person = person.ToPerson();

            await _dbContext.Persons.AddAsync(person);
            await SaveChangesAsync();

            return person;
        }

        /// <summary>
        /// Asynchronous updates information about specified person in database
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<Person> UpdatePersonAsync(int personId, Person person)
        {
            if (person == null)
                return default!;

            var updatedPerson = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId
                                                                             && p.Deleted == false);

            if (person == null)
                return default!;

            updatedPerson!.FirstName = person!.FirstName;
            updatedPerson!.LastName = person!.LastName;
            updatedPerson!.Email = person!.Email;
            updatedPerson!.IsManager = person!.IsManager;
            updatedPerson!.ManagerId = person!.ManagerId; 

            await SaveChangesAsync();

            return person;
        }

        /// <summary>
        /// Asynchronous deletes person from database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Asynchronous returns information about subordinate personnel 
        /// of specified person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<List<Person>> GetSubordinatePersonnelAsync(int personId) =>       
            await _dbContext.Persons.Where(p => p.ManagerId == personId && p.Deleted == false)
                .ToListAsync();

        /// <summary>
        /// Asynchronous checks whether specified person in database or not
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<bool> CheckPersonIsManager(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId 
                                                                      && p.Deleted == false);
            if (person == null) 
                return false;

            return person.IsManager;
        }

        /// <summary>
        /// Asynchronous changes manager id for personnel in database
        /// </summary>
        /// <param name="currentManagerId"></param>
        /// <param name="newManagerId"></param>
        /// <returns></returns>
        public async Task<List<Person>> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                             int newManagerId)
        {
            var subordinatePersonnel = await _dbContext.Persons.Where(p => 
            p.ManagerId == currentManagerId && p.Deleted == false).ToListAsync();

            subordinatePersonnel.ForEach(sp => sp.ManagerId = newManagerId);
            
            await SaveChangesAsync();
            
            return subordinatePersonnel;
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns></returns>
        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
