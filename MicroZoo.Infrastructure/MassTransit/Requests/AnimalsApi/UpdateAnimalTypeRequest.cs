using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to update animal type by request receiving from RabbitMq
    /// </summary>
    public class UpdateAnimalTypeRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Animal type Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// New information about animal type that will be updated
        /// </summary>
        public AnimalTypeDto AnimalTypeDto { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="UpdateAnimalTypeRequest"/> class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="animalTypeDto"></param>
        /// <param name="accessToken"></param>
        public UpdateAnimalTypeRequest(int id, AnimalTypeDto animalTypeDto, string accessToken)
             : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AnimalTypeDto = animalTypeDto;
            //AccessToken = accessToken;
        }
    }
}
