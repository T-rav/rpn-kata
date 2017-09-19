using System;

namespace ReversePolishCalculator
{
    public class Calculator : ICalculator
    {
        public int Calculate(string input)
        {
            var (numbers, op) = SplitInputIntoNumbersAndOperator(input);
            var parts = numbers
                .EmptyIfNull()
                .Replace(",", " ")
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                .ToInts();
            return parts
                     .Apply(op)
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
}