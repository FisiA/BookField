using Microsoft.AspNetCore.Identity;

namespace BookPlace.Core.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime ModifiedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreateByUserId { get; set; }
        public Guid? ModifiedByUserId { get; set; }
    }
}
