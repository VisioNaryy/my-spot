namespace MySpot.Domain.Data.Exceptions;

public sealed class ParkingSpotAlreadyReservedException : BaseException
{
    public string ParkingSpotName { get; }
    public DateTime Date { get; }

    public ParkingSpotAlreadyReservedException(string parkingSpotName, DateTime date) 
        : base($"Parking spot with name {parkingSpotName} is already reserved for date: {date}")
    {
        ParkingSpotName = parkingSpotName;
        Date = date;
    }
}