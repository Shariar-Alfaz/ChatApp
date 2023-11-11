using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using static ChatApp.Domain.Entities.Enum.EnumCollection;

namespace ChatApp.Persistence.Membership
{
    public class ApplicationUser : IdentityUser<Guid> 
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Street { get; set; }
        public string Povience { get; set; }
        public string Country { get; set; }
        public  string Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        
        /// User is aggregate root so, DO NOT ADD ANY NAVIGATION PROPERTY
    }
}
