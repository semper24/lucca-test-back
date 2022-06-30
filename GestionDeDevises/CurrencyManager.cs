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
            // Console.WriteLine(this.FileContent);
        }
        void getFileContent(string filePath)
        {
            // Check if file exist or not
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
            // foreach (CurrencyRate r in Rates) {
            //     Console.WriteLine(r.startCurrency+"-"+r.endCurrency+"-"+r.rate);
            // }
        }
        void findBestConversion()
        {
            List<double> blockList = new List<double>();
            List<CurrencyRate> array1 = new List<CurrencyRate>();
            List<String> array2 = new List<string>();
            var nbOfConversions = 0;
            var currentCurrency = this.ConvInfo.targetCurrency;
            double lastCheck = -1;
            var tmp = "";
            int i = 0;

            array2.Add(currentCurrency);

            // array2.Add(this.ConvInfo.initialCurrency);
            Console.WriteLine("START!!\n");
            while (/*VALUE != 2/* || */(currentCurrency != this.ConvInfo.initialCurrency)) {
                for (; i != this.Rates.Count; i++) {
                    tmp = this.Rates[i].checkAndGet(currentCurrency);
                    // Console.WriteLine("Checking ("+this.Rates[i].startCurrency+"-"+this.Rates[i].endCurrency+")");
                    // Console.WriteLine("TMP = "+tmp+"\n");
                    if ((tmp != null) && (this.Rates[i].rate != lastCheck) && (!blockList.Contains(this.Rates[i].GetHashCode()))) {
                        // Console.WriteLine("Found Curr = ("+tmp+")");
                        nbOfConversions+=1;
                        array1.Add(this.Rates[i]);
                        array2.Add(tmp);
                        lastCheck = this.Rates[i].rate;
                        // Console.WriteLine("Code of the found line={"+lastCheck+"}");
                        currentCurrency = tmp;
                        tmp="";
                        i=-1;
                    }
                    // if (i >= 0) {
                    //     Console.WriteLine("LastCheckCode={"+lastCheck+"} -- ActualCode={"+this.Rates[i].GetHashCode()+"}");
                    //     Console.WriteLine("Is actual in blockList = {"+blockList.Contains(this.Rates[i].GetHashCode()).ToString()+"}\n");
                    // }                    
                }
                // Console.WriteLine("("+currentCurrency+") was not found!");
                nbOfConversions -= 1;
                blockList.Add(lastCheck);
                if (array1.Count > 0)
                    array1.RemoveAt(array1.Count - 1);
                if (array2.Count > 0)
                    array2.RemoveAt(array2.Count - 1);
                if (array2.Count != 0)
                    currentCurrency = array2.Last();
                else {
                    currentCurrency = ConvInfo.targetCurrency;
                    array2.Add(currentCurrency);
                }
                // Console.WriteLine("Array1 size = "+array1.Count);
                // Console.WriteLine("Array2 size = "+array2.Count);
                // Console.WriteLine("Block list size = "+blockList.Count);
                // Console.WriteLine("The curCuren is now = ("+currentCurrency+")");
                i=0;
                // VALUE += 1;
                // break;
            }
            array1.Reverse();
            array2.Reverse();
            double res = ConvInfo.amount;
            int k = 0;
            double inver = 0; 

            foreach (CurrencyRate r in array1) {
                Console.WriteLine(r.startCurrency+"-"+r.endCurrency+"-"+r.rate);
            }
            foreach (string s in array2) {
                Console.WriteLine(s);
            }
            for (int j = 0; j != array1.Count; j++) {
                if ((array1[j].startCurrency == array2[k]) &&
                (array1[j].endCurrency == array2[k+1])) {
                    res = Math.Round((res * array1[j].rate), 4);
                }
                else {
                    inver = Math.Round(1 / array1[j].rate, 4);
                    res = Math.Round((res * inver), 4);
                }
                k++;
            }
            res = Math.Round(res);
        }
        // Attributes
        public string FilePath { get; set; } = default!;
        public string[] FileContent { get; set; } = default!;
        private ConversionInfo ConvInfo { get; set; } = default!;
        private List<CurrencyRate> Rates {get; set; } = new List<CurrencyRate>();
    }
}