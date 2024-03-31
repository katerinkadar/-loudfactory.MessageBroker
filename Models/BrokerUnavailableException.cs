using System;

public class BrokerUnavailableException : Exception
{
    public BrokerUnavailableException() { }

    public BrokerUnavailableException(string message) : base(message) { }

    public BrokerUnavailableException(string message, Exception innerException) : base(message, innerException) { }
}