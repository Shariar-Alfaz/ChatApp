using ChatApp.Infrastructure.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain
{
    public class Inbox : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
