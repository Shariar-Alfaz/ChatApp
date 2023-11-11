using Microsoft.AspNetCore.Identity;

namespace ChatApp.Persistence.Membership
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
            : base() { }

        public ApplicationRole(string roleName)
            : base(roleName) { }
    }
}
