using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversePolishCalculator
{
    internal static class Extensions
    {
        internal static Dictionary<string, Func<int[], int>> CalculationStrategies =
            new Dictionary<string, Func<int[], int>>()
            {
                ["+"] = ints => ints.Sum(),
                ["-"] = ints => ints.First() - ints.Second(),
                ["*"] = ints => ints.First() * ints.Second(),
                ["/"] = ints => ints.First() / ints.Second(),
                ["%"] = ints => ints.First() % ints.Second(),
                ["^"] = ints => (int)Math.Pow(ints.First(), ints.Second())
            };
        internal static int Apply(
            this int[] ints,
            string op
        )
        {
            return CalculationStrategies[op](ints);
        }

        internal static int Second(this int[] ints)
        {
            return ints.Skip(1).First();
        }

        internal static int ValidateDoesNotExceedDisplayConstraints(this int value)
        {
            return value.ToString().Length > 8
                ? throw new InvalidOperationException("Calculation output may not exceed 8 characters")
                : value;
        }

        internal static bool IsOperator(this string str)
        {
            return CalculationStrategies.ContainsKey(str);
        }

        internal static string LastCharacter(this string str)
        {
            return string.IsNullOrEmpty(str) 
                ? null 
                : str[str.Length - 1].ToString();
        }

        internal static string EmptyIfNull(this string str)
        {
            return str ?? "";
        }

        internal static int[] ToInts(this string[] strings)
        {
            return strings
                .TryConvertToInts()
                .ValidateHaveTwoInts();
        }

        internal static IEnumerable<int> TryConvertToInts(this string[] strings)
        {
            return strings.Select(s =>
                int.TryParse(s, out var result)
                    ? result
                    : throw new ArgumentException());
        }

        internal static int[] ValidateHaveTwoInts(this IEnumerable<int> ints)
        {
            var result = ints.ToArray();
            return result.Length == 2
                ? result
                : throw new ArgumentException();
        }
    }
}