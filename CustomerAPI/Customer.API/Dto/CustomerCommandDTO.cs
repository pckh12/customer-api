using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Dto
{
    public class CustomerCommandDTO
    {
        /// <summary>
        /// The customer's first name
        /// </summary>
        /// <example>Philip</example>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The customer's last name
        /// </summary>
        /// <example>Huynh</example>
        [Required]
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
