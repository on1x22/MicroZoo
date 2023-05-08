using MicroZoo.PersonsApi.Models;
using MicroZoo.PersonsApi.Repository;

namespace MicroZoo.PersonsApi.Apis
{
    public class PersonApi : IApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/", () => "Hello PersonApi!");

            app.MapGet("/person/{id}", GetById);

            app.MapPut("/person", UpdateEmployee);
        }

        private async Task<IResult> GetById(int id, IPersonRepository repository) =>
            await repository.GetById(id) is Person person
            ? Results.Ok(person)
            : Results.NotFound();

        private async Task<IResult> UpdateEmployee(Person person, IPersonRepository repository)
        {
            await repository.UpdateEmployee(person);
            return Results.NoContent();
        }

    }
}
