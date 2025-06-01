namespace Task9.Application.Exceptions;

public static class ClientTripExceptions
{
    public class ClientAlreadyRegisteredException()
        : BaseExceptions.ValidationException("Client is already registered for this trip.");
    
    public class TripNotFoundException()
        : BaseExceptions.NotFoundException("Trip not found.");

    public class TripAlreadyStartedException()
        : BaseExceptions.ValidationException("Registration is not allowed: trip has already started or passed.");
}