using System;

namespace ApplicantTracking.Application.Exceptions;

public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}

