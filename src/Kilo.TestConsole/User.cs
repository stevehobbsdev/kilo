using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kilo.TestConsole
{
    public class User
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public EmailAddress EmailAddress { get; set; }

        public User()
        {
        }
    }
}
