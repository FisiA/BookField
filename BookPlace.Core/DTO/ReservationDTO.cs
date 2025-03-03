namespace BookPlace.Core.DTO
{
    public class ReservationDTO
    {
        public Guid Id { get; set; }
        public string NameAndSurname { get; set; }
        public string Email { get; set; }
        public DateTime ReservationFrom { get; set; }
        public DateTime ReservationTo { get; set; }
    }
}
