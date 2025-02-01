namespace MicroZoo.Infrastructure.Models.Persons.Dto
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about a person
    /// </summary>
    public class PersonDto
    {
        /// <summary>
        /// First name of person
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of person
        /// </summary>
        public string? LastName { get; set; }
        
        /// <summary>
        /// Email address
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// Is person manager or ordinary worker
        /// </summary>
        public bool IsManager { get; set; }

        /// <summary>
        /// Id of the manager to whom the worker reports
        /// </summary>
        public int ManagerId { get; set; }

        /// <summary>
        /// Convert instane of <see cref="PersonDto"/> class to the instance 
        /// of the <see cref="Person"/> class
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person
            {
                FirstName = FirstName!,
                LastName = LastName!,
                Email = Email!,
                IsManager = IsManager,
                ManagerId = ManagerId
            };
        }
    }
}
