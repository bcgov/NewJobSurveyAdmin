using System;

public class NoLdapInfoException : Exception
{
    public NoLdapInfoException()
    {
    }

    public NoLdapInfoException(string message)
        : base(message)
    {
    }

    public NoLdapInfoException(string message, Exception inner)
        : base(message, inner)
    {
    }
}