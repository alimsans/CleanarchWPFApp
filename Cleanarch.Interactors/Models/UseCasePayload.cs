using System;

namespace Cleanarch.DomainLayer.Models
{
    public class UseCasePayload
    {
        public Guid RequestId { get; set; }
        public bool IsUrgent { get; set; }
    }
}
