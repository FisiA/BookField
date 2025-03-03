namespace BookPlace.Core.Domain
{
    public class BaseEntity
    {
        public DateTime CreatedOnDate { get; set; }
        public DateTime ModifiedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreateByUserId { get; set; }
        public Guid? ModifiedByUserId { get; set; }
    }
}
