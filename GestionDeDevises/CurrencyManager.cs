#nullable disable

using System;

namespace CurrencyManager
{
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
    public struct CurrencyRate
    {
        public CurrencyRate(string startCur, string endCur, double rate)
        {
            this.startCurrency = startCur;
            this.endCurrency = endCur;
            this.rate = rate;
        }
        public string checkAndGet(string targetCurr) {
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
    public class CurrencyManager
    {
        // Default Ctor
        public CurrencyManager() { this.FilePath = "NULL"; }
        // Ctor
        public CurrencyManager(string filePath) { this.FilePath = filePath; }
        // Methods
        public void launchConversion()
        {
            this.getFileContent(this.FilePath);
            this.parseFileContent();
            this.findBestConversion();
        }
        void getFileContent(string filePath)
        {
            this.FileContent = System.IO.File.ReadAllLines(filePath);
        }
        void parseFileContent()
        {
            string[] values = this.FileContent[0].Split(';');
            this.ConvInfo = new ConversionInfo(values[0], int.Parse(values[1]), values[2]);

            for (int i = 2; i != this.FileContent.Length; i++) {
                values = this.FileContent[i].Split(';');
                Rates.Add(new CurrencyRate(values[0], values[1], double.Parse(values[2])));
            }
        }
        void findBestConversion()
        {
            List<double> blockList = new List<double>();
            List<CurrencyRate> orderedRates = new List<CurrencyRate>();
            List<String> currenciesOrdered = new List<string>();
            var nbOfConversions = 0;
            var currentCurrency = this.ConvInfo.targetCurrency;
            double lastCheck = -1;
            var tmp = "";
            int i = 0;

            currenciesOrdered.Add(currentCurrency);
            while ((currentCurrency != this.ConvInfo.initialCurrency)) {
                for (; i != this.Rates.Count; i++) {
                    tmp = this.Rates[i].checkAndGet(currentCurrency);
                    if ((tmp != null) && (this.Rates[i].rate != lastCheck) && (!blockList.Contains(this.Rates[i].GetHashCode()))) {
                        nbOfConversions+=1;
                        orderedRates.Add(this.Rates[i]);
                        currenciesOrdered.Add(tmp);
                        lastCheck = this.Rates[i].rate;
                        currentCurrency = tmp;
                        tmp="";
                        i=-1;
                    }
                    if (currentCurrency == this.ConvInfo.initialCurrency)
                        break;
                }
                if (currentCurrency == this.ConvInfo.initialCurrency)
                    break;
                nbOfConversions -= 1;
                blockList.Add(lastCheck);
                if (orderedRates.Count > 0)
                    orderedRates.RemoveAt(orderedRates.Count - 1);
                if (currenciesOrdered.Count > 0)
                    currenciesOrdered.RemoveAt(currenciesOrdered.Count - 1);
                if (currenciesOrdered.Count != 0)
                    currentCurrency = currenciesOrdered.Last();
                else {
                    currentCurrency = ConvInfo.targetCurrency;
                    currenciesOrdered.Add(currentCurrency);
                }
                i=0;
            }
            compileCurrencyConversion(orderedRates, currenciesOrdered);
        }
        void compileCurrencyConversion(List<CurrencyRate> orderedRates, List<string> currenciesOrdered) {
            double res = ConvInfo.amount;
            int k = 0;

            orderedRates.Reverse();
            currenciesOrdered.Reverse();

            for (int j = 0; j != orderedRates.Count; j++) {
                if ((orderedRates[j].startCurrency == currenciesOrdered[k]) &&
                (orderedRates[j].endCurrency == currenciesOrdered[k+1]))
                    res = Math.Round((res * orderedRates[j].rate), 4);
                else 
                    res = Math.Round((res * (Math.Round(1 / orderedRates[j].rate, 4))), 4);

                k++;
            }
            Console.WriteLine(res);
        }
        // Attributes
        public string FilePath { get; set; } = default!;
        public string[] FileContent { get; set; } = default!;
        private ConversionInfo ConvInfo { get; set; } = default!;
        private List<CurrencyRate> Rates {get; set; } = new List<CurrencyRate>();
    }
}