using System.Drawing;

namespace Lab3_Misyuro.Kirill_Logical
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program md = new Program();
            //Check for correctness of the entered number of operators which should be >1
            uint amountOperands = 0;
            bool isNumber;
            do
            {
                Console.Write("Enter the number of operators: ");
                isNumber = uint.TryParse(Console.ReadLine(), out amountOperands) && (amountOperands > 1);
            }
            while (!isNumber);

            Console.WriteLine();


            //Check the entered formula which corresponds to the number of operators
            bool isGoodFormula;
            string formula;
            string[] formulaSplit = { };

            do
            {
                Console.WriteLine("Enter operation with {0} operators: ", amountOperands);
                formula = Console.ReadLine();
                if (!String.IsNullOrEmpty(formula))
                {
                    formulaSplit = formula.Split(" ");
                    if (formulaSplit.Length == 2 * amountOperands - 1)
                    {
                        isGoodFormula = true;
                        for (int i = 1; i < formulaSplit.Length; i += 2)
                        {
                            if (formulaSplit[i] == "&" || formulaSplit[i] == "&&" || formulaSplit[i] == "|" || formulaSplit[i] == "||" || formulaSplit[i] == "^")
                            {
                                isGoodFormula = true;
                            }
                            else { isGoodFormula = false; break; }
                        }
                    }
                    else isGoodFormula = false;
                }

                else isGoodFormula = false;
            }
            while (!isGoodFormula);

            string[] operators = md.GetArrayOperators(formulaSplit);
            string[] operands = md.GetArrayOperands(formulaSplit);

            bool[,] tableTruth = new bool[(int)Math.Pow(2, amountOperands), amountOperands];
            bool[] result = new bool[tableTruth.GetLength(0)];

            //Filling in the truth table
            for (int i = 0; i < tableTruth.GetLength(0); i++)
            {
                for (int j = tableTruth.GetLength(1) - 1; j >= 0; j--)
                {
                    tableTruth[i, j] = md.GetBit(i, j);
                }
            }

            //Operations are currently handled from left to right. Logical operations have execution precedence.
            //Logical AND operator &
            //Logical exclusive OR operator ^
            //Logical OR operator |
            //Conditional logical AND operator &&
            //Conditional logical OR operator ||
            //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators

            //TODO: Solve the problem priority of operator

            //Truth table solution
            for (int i = 0; i < tableTruth.GetLength(0); i++)
            {
                result[i] = tableTruth[i, 0];
            }
            for (int i = 0; i < tableTruth.GetLength(0); i++)
            {
                for (int j = 0; j <= operators.Length - 1; j++)
                {
                    result[i] = md.SolveBool(result[i], operators[j], tableTruth[i, j + 1]);
                }
            }

            //Output truth table
            for (int i = 0; i < tableTruth.GetLength(0); i++)
            {
                for (int j = (int)amountOperands - 1; j >= 0; j--)
                {
                    Console.Write("{0,4}", md.ConvertBoolToInt(tableTruth[i, j]));
                }
                Console.Write("{0,4}", md.ConvertBoolToInt(result[i]));
                Console.WriteLine();
            }
        }

        string[] GetArrayOperators(string[] str)
        {
            string[] result = new string[(str.Length - 1) / 2];
            int count = 0;
            for (int i = 1; i < str.GetLength(0); i += 2)
            {
                result[count] = str[i];
                count++;
            }
            return result;
        }

        string[] GetArrayOperands(string[] str)
        {
            string[] result = new string[(str.Length + 1) / 2];
            int count = 0;
            for (int i = 0; i < str.GetLength(0); i += 2)
            {
                result[count] = str[i];
                count++;
            }
            return result;
        }

        int ConvertBoolToInt(bool a)
        {
            return a ? 1 : 0;
        }

        bool GetBit(int value, int bit)
        {
            int result = value & (1 << bit);
            if (result == 0) return false;
            else return true;
        }

        bool SolveBool(bool a, string opr, bool b)
        {
            switch (opr)
            {
                case "&":
                    return a & b;
                case "|":
                    return a | b;
                case "&&":
                    return a && b;
                case "||":
                    return a || b;
                case "^":
                    return a ^ b;
            }
            return false;
        }



    }
}