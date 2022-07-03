public struct BestConversionFinder
{
    public BestConversionFinder() { }
    private void init(string targetCurrency)
    {
        this.currentCurrency = targetCurrency;
        this.currenciesOrdered.Add(targetCurrency);
    }
    private double compileCurrencyConversion(ConversionInfo convInfo)
    {
        double res = convInfo.amount;
        int k = 0;

        orderedRates.Reverse();
        currenciesOrdered.Reverse();

        for (int j = 0; j != orderedRates.Count; j++)
        {
            if ((orderedRates[j].startCurrency == currenciesOrdered[k]) &&
            (orderedRates[j].endCurrency == currenciesOrdered[k + 1]))
                res = Math.Round((res * orderedRates[j].rate), 4);
            else
                res = Math.Round((res * (Math.Round(1 / orderedRates[j].rate, 4))), 4);
            k++;
        }
        return (Math.Round(res));
    }
    private bool checkIfCurrencyFound(string tmpCurr, CurrencyRate currRate)
    {
        if ((tmpCurr != null) && (currRate.rate != lastCheck) && (!this.blockList.Contains(currRate.GetHashCode())))
        {
            this.orderedRates.Add(currRate);
            this.currenciesOrdered.Add(tmpCurr);
            this.lastCheck = currRate.rate;
            this.currentCurrency = tmpCurr;
            return (true);
        }
        return (false);
    }
    private void currencyNotFound(ConversionInfo convInfo)
    {
        blockList.Add(lastCheck);
        if (this.orderedRates.Count > 0)
            this.orderedRates.RemoveAt(this.orderedRates.Count - 1);
        if (this.currenciesOrdered.Count > 0)
            this.currenciesOrdered.RemoveAt(this.currenciesOrdered.Count - 1);
        if (this.currenciesOrdered.Count != 0)
            this.currentCurrency = this.currenciesOrdered.Last();
        else
        {
            this.currentCurrency = convInfo.targetCurrency;
            this.currenciesOrdered.Add(this.currentCurrency);
        }
    }
    public double findBestConversion(ConversionInfo convInfo, List<CurrencyRate> rates)
    {
        int i = 0;
        string tmp = "";

        init(convInfo.targetCurrency);
        while ((currentCurrency != convInfo.initialCurrency))
        {
            for (; i != rates.Count; i++)
            {
                tmp = rates[i].checkAndGet(currentCurrency);
                if (checkIfCurrencyFound(tmp, rates[i])) {
                    tmp = "";
                    i = -1;
                }
                if (currentCurrency == convInfo.initialCurrency)
                    break;
            }
            if (currentCurrency != convInfo.initialCurrency) {
                currencyNotFound(convInfo);
                i = 0;
            }
            else
                break;
        }
        return (compileCurrencyConversion(convInfo));
    }
    public List<double> blockList = new List<double>();
    public List<CurrencyRate> orderedRates = new List<CurrencyRate>();
    public List<String> currenciesOrdered = new List<string>();
    public string currentCurrency = "";
    public double lastCheck = -1;
}