#nullable disable
namespace CurrencyManager
{
    public class CurrencyManager
    {
        public CurrencyManager() { this.FilePath = "NULL"; }
        public CurrencyManager(string filePath) { this.FilePath = filePath; }
        public void launchConversion()
        {
            this.getFileContent(this.FilePath);
            this.parseFileContent();
            this.checkFileContent();
            this.findConversion();
        }
        private void getFileContent(string filePath)
        {
            try
            {
                this.FileContent = System.IO.File.ReadAllLines(filePath);
            }
            catch (Exception) { throw (new ErrorFileReading()); }
        }
        private void parseFileContent()
        {
            try
            {
                string[] values = this.FileContent[0].Split(';');
                this.ConvInfo = new ConversionInfo(values[0], int.Parse(values[1]), values[2]);
                int nbLines = int.Parse(this.FileContent[1]);

                for (int i = 2; i != (nbLines + 2); i++)
                {
                    values = this.FileContent[i].Split(';');
                    Rates.Add(new CurrencyRate(values[0], values[1], double.Parse(values[2])));
                }
            }
            catch (Exception) { throw (new ErrorFileContent("Error: Problem encountered in the file.")); }
        }
        private bool checkCurrencies(CurrencyRate rate, string curr)
        {
            if ((rate.startCurrency == curr) || (rate.endCurrency == curr))
                return (true);
            return (false);
        }
        private void checkFileContent()
        {
            int initialCurrencyCheck = 0;
            int targetCurrencyCheck = 0;

            if ((ConvInfo.initialCurrency.Length != 3 || ConvInfo.targetCurrency.Length != 3))
                throw (new ErrorCurrencies());
            foreach (CurrencyRate rate in Rates)
            {
                if (checkCurrencies(rate, this.ConvInfo.initialCurrency))
                    initialCurrencyCheck++;
                if (checkCurrencies(rate, this.ConvInfo.targetCurrency))
                    targetCurrencyCheck++;
                if ((rate.startCurrency.Length != 3 || rate.endCurrency.Length != 3))
                    throw (new ErrorCurrencies());
                try
                {
                    if ((rate.rate.ToString().Split('.')[1].Length != 4))
                    {
                        throw (new ErrorRate());
                    }
                }
                catch (IndexOutOfRangeException) { throw (new ErrorRate()); };
            }
            if (initialCurrencyCheck == 0 || targetCurrencyCheck == 0)
                throw (new ErrorNonExistingCurrencies());
        }
        private void findConversion()
        {
            BestConversionFinder finder = new BestConversionFinder();

            Console.WriteLine(finder.findBestConversion(this.ConvInfo, this.Rates));
        }
        // Attributes
        private  string FilePath { get; set; } = default!;
        private  string[] FileContent { get; set; } = default!;
        private ConversionInfo ConvInfo { get; set; } = default!;
        private List<CurrencyRate> Rates { get; set; } = new List<CurrencyRate>();
    }
}