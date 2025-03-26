using BookPlace.Core.DTO.Reservation;

namespace BookPlace.UnitTests.Data
{
    public static class ReservationStaticData
    {
        public static List<ReservationDTO> GetReservations()
        {
            return new List<ReservationDTO>
            {
                new ReservationDTO
                {
                    Id = Guid.Parse("c7bef3c2-9e31-4a95-8a9f-fc8fee7d07a0"),
                    ReservationFrom = new DateTime(2025, 03, 15, 17, 0, 0),
                    ReservationTo = new DateTime(2025, 03, 15, 18, 0, 0),
                    Email = "john.smith@noemail.com",
                    NameAndSurname = "John Smith"
                },
                new ReservationDTO
                {
                    Id = Guid.Parse("857e93da-688a-42d7-bfc4-1cc1da685770"),
                    ReservationFrom = new DateTime(2025, 03, 20, 12, 0, 0),
                    ReservationTo = new DateTime(2025, 03, 20, 13, 0, 0),
                    Email = "jack.alan@noemail.com",
                    NameAndSurname = "Jack Alan"
                },
                new ReservationDTO
                {
                    Id = Guid.Parse("5ee05482-f0a6-44fd-890e-57b0cfb89f46"),
                    ReservationFrom = new DateTime(2025, 03, 21, 13, 0, 0),
                    ReservationTo = new DateTime(2025, 03, 21, 15, 0, 0),
                    Email = "arnold.lee@noemail.com",
                    NameAndSurname = "Arnold Lee"
                }
            };
        }
    }
}
