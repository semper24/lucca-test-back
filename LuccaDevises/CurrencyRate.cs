public struct CurrencyRate
{
    public CurrencyRate(string startCur, string endCur, double rate)
    {
        this.startCurrency = startCur;
        this.endCurrency = endCur;
        this.rate = rate;
    }
    public string checkAndGet(string targetCurr)
    {
        if (this.startCurrency == targetCurr)
            return (this.endCurrency);
        else if (this.endCurrency == targetCurr)
            return (this.startCurrency);
        return (null);
    }
    public string startCurrency;
    public string endCurrency;
    public double rate;
}