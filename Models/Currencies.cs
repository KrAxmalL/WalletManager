using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public static class Currencies
    {

        public enum Currency : byte
        {
            Dollar = 1,
            Euro = 2,
            Gryvnya = 3
        }

        private static Dictionary<Currency, decimal> ConversionCoefficients = new Dictionary<Currency, decimal>()
        {
            { Currency.Dollar, 27},
            { Currency.Euro, 30},
            { Currency.Gryvnya, 1}
        };

        public static readonly List<Currency> listToShow = new List<Currency>()
        {
            Currency.Dollar,
            Currency.Euro,
            Currency.Gryvnya
        };

        public static decimal Convert(Currency from, Currency to, decimal amount)
        {
            if (from == to)
            {
                return amount;
            }
            else
            {
                decimal coefFrom = ConversionCoefficients[from];
                decimal coefTo = ConversionCoefficients[to];
                return (amount * (coefFrom / coefTo));
            }
        }

        public static String currencyToString(Currency currency)
        {
            switch (currency)
            {
                case Currency.Dollar: return "Dollar";

                case Currency.Euro: return "Euro";

                case Currency.Gryvnya: return "Hryvnya";

                default: return "Unknown";
            }
        }

        public static String currencyToSymbol(Currency currency)
        {
            switch (currency)
            {
                case Currency.Dollar: return "\u0024"; //dollar sign

                case Currency.Euro: return "\u20AC"; //euro sign

                case Currency.Gryvnya: return "\u20B4"; //hryvnya sign

                default: return "Unknown";
            }
        }
    }
}
