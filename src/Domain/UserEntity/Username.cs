using SharedKernel;

namespace Domain.UserEntity;

public record Username
{
    public string Value { get; init; }

    private static readonly char[] UnallowedSymbols = @"!@#$%^&*()=+/|\ ".ToCharArray();

    private Username(string value)
    {
        Value = value;
    }

    public static Result<Username> Create(string value)
    {
        if (value.Length is < 4 or > 30)
        {
            return new Error();
        }

        if (value.ToCharArray().Any(x => UnallowedSymbols.Contains(x)))
        {
            return new Error();
        }

        return new Username(value);
    }
}