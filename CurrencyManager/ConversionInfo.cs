
public struct ConversionInfo
{
    public ConversionInfo()
    {
        this.initialCurrency = "";
        this.targetCurrency = "";
        this.amount = 0;
    }
    public ConversionInfo(string initCur, int amnt, string trgtCur)
    {
        this.initialCurrency = initCur;
        this.targetCurrency = trgtCur;
        this.amount = amnt;
    }
    public string initialCurrency;
    public string targetCurrency;
    public int amount;
}
