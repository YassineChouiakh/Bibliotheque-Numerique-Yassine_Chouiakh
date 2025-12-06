using System;

public class ItemIntrouvableException : Exception
{
    public ItemIntrouvableException(string message) : base(message) { }
}