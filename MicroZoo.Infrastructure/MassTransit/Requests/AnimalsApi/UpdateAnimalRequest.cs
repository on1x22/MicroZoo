using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to update animal by request receiving from RabbitMq
    /// </summary>
    public class UpdateAnimalRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Animal's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// New information about animal that will be updated
        /// </summary>
        public AnimalDto AnimalDto { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="UpdateAnimalRequest"/> class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="animalDto"></param>
        /// <param name="accessToken"></param>
        public UpdateAnimalRequest(int id, AnimalDto animalDto, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AnimalDto = animalDto;
            //AccessToken = accessToken;
        }
    }
}
