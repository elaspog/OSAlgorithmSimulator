using System.Numerics;

namespace VirtualAddressMapper.Models
{
    public static class LargeStringNumberComparator
    {


        public static int CompareNumbers(string x, string y)
        {
            if (x.Length > y.Length) y = y.PadLeft(x.Length, '0');
            else if (y.Length > x.Length) x = x.PadLeft(y.Length, '0');

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] < y[i]) return -1;
                if (x[i] > y[i]) return 1;
            }
            return 0;
        }

        public static bool areEqual(string x, string y)
        { 
            int result = CompareNumbers(x, y);
            if (result == 0)
                return true;
            return false;
        }

        public static bool isFirstGreater(string x, string y)
        {
            int result = CompareNumbers(x, y);
            if (result > 0)
                return true;
            return false;
        }

        public static bool isFirstGreaterOrEqual(string x, string y)
        {
            int result = CompareNumbers(x, y);
            if (result >= 0)
                return true;
            return false;
        }

        public static string getSum(string x, string y)
        {
            BigInteger xBig = new BigInteger();
            BigInteger yBig = new BigInteger();

            BigInteger.TryParse(x, out xBig);
            BigInteger.TryParse(y, out yBig);

            return BigInteger.Add(xBig, yBig).ToString();
            
        }

    }
}
