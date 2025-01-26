using MicroZoo.PersonsApi.Repository;
using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Apis
{
    /// <summary>
    /// Obsilet class
    /// </summary>
    [Obsolete]
    public class PersonsApi : IApi
    {
        /// <summary>
        /// Registers all apis
        /// </summary>
        /// <param name="app"></param>
        public void Register(WebApplication app)
        {
            //app.MapGet("/", () => "Hello PersonApi!");

            app.MapGet("/person/{id}", GetPersonById)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapPut("/person", UpdatePersonApi)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });
        }

        internal static async Task<IResult> GetPersonById(int id, IPersonRepository repository) =>
            await repository.GetPersonAsync(id) is Person person
            ? Results.Ok(person)
            : Results.NotFound();

        internal static /*async Task<*/IResult/*>*/ UpdatePersonApi(Person person, IPersonRepository repository)
        {
            //await repository.UpdatePersonAsync(person);
            return Results.NoContent();
        }

    }
}
