namespace Library.Core.ValueObjects;

public sealed record Surname
{
    public Surname(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Surname cannot be empty or whitespace.", nameof(value));

        if (value.Length is < 2 or > 50)
            throw new ArgumentException("Surname must be between 2 and 50 characters long.", nameof(value));

        Value = value;
    }

    public string Value { get; }
    
    public static implicit operator Surname(string value) => value is null ? null : new Surname(value);

    public static implicit operator string(Surname value) => value.Value;

    public override string ToString() => Value;
}