namespace BookPlace.Core.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid Id { get; set; }
        public required string NameAndSurname { get; set; }
        public required string Email { get; set; }
        public required DateTime ReservationFrom { get; set; }
        public required DateTime ReservationTo { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
