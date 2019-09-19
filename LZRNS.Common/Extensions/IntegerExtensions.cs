namespace LZRNS.Common.Extensions
{
    public static class IntegerExtensions
    {
        public static bool IsOdd(this int number)
        {
            return !IsEven(number);
        }

        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }
    }
}