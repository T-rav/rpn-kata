﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversePolishCalculator
{
    public interface ICalculator
    {
        int Calculate(string input);
    }

    public class Calculator : ICalculator
    {
        public int Calculate(string input)
        {
            var (numbers, op) = SplitInputIntoNumbersAndOperator(input);
            var parts = numbers
                .EmptyIfNull()
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                .ToInts();
            return parts
                     .Sum()
                     .ValidateDoesNotExceedDisplayConstraints();
        }

        private (string numbers, string op) SplitInputIntoNumbersAndOperator(string input)
        {
            var op = GetOperatorFor(input);
            var numbers = RemoveOperatorFrom(input, op);
            return (numbers, op);
        }

        private string RemoveOperatorFrom(string input, string op)
        {
            return input.EmptyIfNull().Replace(op, "");
        }

        private string GetOperatorFor(string input)
        {
            var lastChar = input.EmptyIfNull().LastCharacter();
            return lastChar.IsOperator()
                    ? lastChar
                    : "+";
        }
    }

    internal static class Extensions
    {
        internal static int ValidateDoesNotExceedDisplayConstraints(this int value)
        {
            return value.ToString().Length > 8
                    ? throw new InvalidOperationException("Calculation output may not exceed 8 characters")
                    : value;
        }

        internal static bool IsOperator(this string str)
        {
            return str == "+";
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