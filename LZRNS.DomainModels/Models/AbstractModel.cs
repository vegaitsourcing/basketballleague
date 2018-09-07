using System;
using System.ComponentModel.DataAnnotations;

namespace LZRNS.DomainModel.Models
{
    public abstract class AbstractModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}