﻿using Library.Core.Exceptions;

namespace Library.Core.ValueObjects;

public sealed record Name
{
    public string Value { get; }

    public Name()
    {
    }    
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is > 300 or < 2)
        {
            throw new InvalidNameException(value);
        }
            
        Value = value;
    }

    public static implicit operator Name(string value) => value is null ? null : new Name(value);
    public static implicit operator string(Name value) => value?.Value;
    public override string ToString() => Value;
}