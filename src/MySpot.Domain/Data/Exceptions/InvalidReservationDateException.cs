namespace MySpot.Domain.Data.Exceptions;

public sealed class InvalidReservationDateException : BaseException
{
    public DateTime Date { get; }

    public InvalidReservationDateException(DateTime date) 
        : base($"Reservation date: {date:d} is invalid.")
    {
        Date = date;
    }
}