public class ErrorCurrencyManager : Exception
{
    public ErrorCurrencyManager(string message) : base(message) {}
}

public class ErrorFileReading : ErrorCurrencyManager
{
    public ErrorFileReading() : base("Error: The file was not found.") {}
}

public class ErrorFilePath : ErrorCurrencyManager
{
    public ErrorFilePath() : base("Error: No filepath was given.") {}
}

// Error File Content 
public class ErrorFileContent : ErrorCurrencyManager
{
    public ErrorFileContent(string message) : base(message) {}
}

public class ErrorCurrencies : ErrorFileContent
{
    public ErrorCurrencies() : base("Error: Currencies must be a 3 caracter alpha code.") {}
}
public class ErrorRate : ErrorFileContent
{
    public ErrorRate() : base("Error: Rates must be a rounded number with 4 digits after the '.'") {}
}

public class ErrorNonExistingCurrencies : ErrorFileContent
{
    public ErrorNonExistingCurrencies() : base("Error: Currencies you entered cannot be found.") {}
}