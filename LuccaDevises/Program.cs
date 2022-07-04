static class Program
{
    static void Main(string[] args)
    {
        try
        {
            CurrencyManager.CurrencyManager manager;
            try {
                manager = new CurrencyManager.CurrencyManager(args[0]);
            } catch(IndexOutOfRangeException) { throw (new ErrorFilePath()); }
            manager.launchConversion();
        }
        catch (ErrorCurrencyManager e)
        {
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine(e);
        }
    }
}
