using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukrainians.Infrastrusture.Data.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
