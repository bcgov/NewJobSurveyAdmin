using System;

public class DuplicateEmployeeException : Exception
{
    public DuplicateEmployeeException()
    {
    }

    public DuplicateEmployeeException(string message)
        : base(message)
    {
    }

    public DuplicateEmployeeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}