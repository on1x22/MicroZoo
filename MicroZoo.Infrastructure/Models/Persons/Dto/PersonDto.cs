using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.Models.Persons.Dto
{
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsManager { get; set; }
        public int ManagerId { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                IsManager = IsManager,
                ManagerId = ManagerId
            };
        }
    }
}
