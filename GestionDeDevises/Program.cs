using System;
using CurrencyManager;


class Program
{
    static void Main(string[] args)
    {
        var manager = new CurrencyManager.CurrencyManager(args[0]);
        manager.launchConversion();
    }
}
