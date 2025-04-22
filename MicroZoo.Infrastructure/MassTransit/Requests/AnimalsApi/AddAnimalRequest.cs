using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to add animal by request receiving from RabbitMq
    /// </summary>
    public class AddAnimalRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Information about animal
        /// </summary>
        public AnimalDto AnimalDto { get; set; }
        
        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="AddAnimalRequest"/> class
        /// </summary>
        /// <param name="animalDto"></param>
        /// <param name="accessToken"></param>
        public AddAnimalRequest(AnimalDto animalDto, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalDto = animalDto;
            //AccessToken = accessToken;
        }
    }
}
