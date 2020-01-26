using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Dto
{
    public class CustomerDTO
    {
        /// <summary>
        /// The customer's unique id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The customer's first name
        /// </summary>
        /// <example>Philip</example>
        public string FirstName { get; set; }

        /// <summary>
        /// The customer's last name
        /// </summary>
        /// <example>Huynh</example>
        public string LastName { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
