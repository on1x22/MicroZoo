using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to add animal type by request receiving from RabbitMq
    /// </summary>
    public class AddAnimalTypeRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Information about animal type
        /// </summary>
        public AnimalTypeDto AnimalTypeDto { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="AddAnimalTypeRequest"/> class
        /// </summary>
        /// <param name="animalTypeDto"></param>
        /// <param name="accessToken"></param>
        public AddAnimalTypeRequest(AnimalTypeDto animalTypeDto, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalTypeDto = animalTypeDto;
            //AccessToken = accessToken;
        }
    }
}
