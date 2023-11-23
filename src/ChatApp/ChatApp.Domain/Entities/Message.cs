using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Infrastructure.Entity.Base;

namespace ChatApp.Domain.Entities
{
    public class Message : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public required string Text { get; set; }
        public DateTime SendingTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
