using MicroZoo.PersonsApi.Models;
using MicroZoo.PersonsApi.Repository;
using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Apis
{
    public class PersonApi : IApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/", () => "Hello PersonApi!");

            app.MapGet("/person/{id}", GetPersonById);

            app.MapPut("/person", UpdatePerson);
        }

        internal static async Task<IResult> GetPersonById(int id, IPersonRepository repository) =>
            await repository.GetPersonById(id) is Person person
            ? Results.Ok(person)
            : Results.NotFound();

        internal static async Task<IResult> UpdatePerson(Person person, IPersonRepository repository)
        {
            await repository.UpdatePerson(person);
            return Results.NoContent();
        }

    }
}
