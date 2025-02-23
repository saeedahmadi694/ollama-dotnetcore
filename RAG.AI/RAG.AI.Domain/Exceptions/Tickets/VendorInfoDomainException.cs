namespace RAG.AI.Domain.Exceptions.Tickets;

public class VendorInfoDomainException : Exception
{

    public VendorInfoDomainException() : base($"VendorId and VendorTitle did not enter")
    { }
}




