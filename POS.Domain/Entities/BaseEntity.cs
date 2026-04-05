using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreaedAt { get; set; }= DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy {  get; set; }
        public bool IsDeleted { get; set; }=false;
    }
}
