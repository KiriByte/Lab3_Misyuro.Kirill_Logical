using System;
using System.Globalization;
using System.Text;

namespace Lab3_Misyuro.Kirill_Logical
{
    internal class Program
    {
        static void Main()
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
                for (int j = 0; j < tableTruth.GetLength(1); j++)
                {
                    tableTruth[i, tableTruth.GetLength(1) - 1 - j] = md.GetBit(i, j);
                }
            }

            bool[,] tableTruthCopy = tableTruth;
            string formulaCopy = formula;

            result = md.Test(formulaCopy, tableTruthCopy);


            //Output truth table

            for (int i = 0; i < operands.Length; i++)
            {
                Console.Write("{0,-4}", operands[i]);
            }
            Console.WriteLine("{0,-4}", formula);
            for (int i = 0; i < tableTruth.GetLength(0); i++)
            {
                for (int j = 0; j < (int)amountOperands; j++)
                {
                    Console.Write("{0,-4}", md.ConvertBoolToInt(tableTruth[i, j]));

                }
                Console.Write("{0,-4}", md.ConvertBoolToInt(result[i]));
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
                default: return false;
            }
        }


        bool[] Test(string formula, bool[,] tableTruth)
        {
            ////Logical AND operator &
            ////Logical exclusive OR operator ^
            ////Logical OR operator |
            ////Conditional logical AND operator &&
            ////Conditional logical OR operator ||
            bool[] result = new bool[tableTruth.GetLength(0)];
            int index;
            bool[,] tableTruthCopy = tableTruth;
            string[] formulaSplit = formula.Split(' ');
            do
            {
                index = Array.IndexOf(formulaSplit, "&");

                if (index > 0)
                {

                    for (int i = 0; i < tableTruthCopy.GetLength(0); i++)
                    {
                        result[i] = SolveBool(tableTruthCopy[i, (index - 1) / 2], formulaSplit[index], tableTruthCopy[i, (index + 1) / 2]);
                    }
                    ShortFormula(formulaSplit, index);
                };
            }
            while (index > 0);
            do
            {
                index = Array.IndexOf(formulaSplit, "^");
                if (index > 0)
                {

                    for (int i = 0; i < tableTruthCopy.GetLength(0); i++)
                    {
                        result[i] = SolveBool(tableTruthCopy[i, (index - 1) / 2], formulaSplit[index], tableTruthCopy[i, (index + 1) / 2]);
                    }
                    ShortFormula(formulaSplit, index);
                };
            }
            while (index > 0);
            do
            {
                index = Array.IndexOf(formulaSplit, "|");
                if (index > 0)
                {

                    for (int i = 0; i < tableTruthCopy.GetLength(0); i++)
                    {
                        result[i] = SolveBool(tableTruthCopy[i, (index - 1) / 2], formulaSplit[index], tableTruthCopy[i, (index + 1) / 2]);

                    }
                    ShortFormula(formulaSplit, index);
                };
            }
            while (index > 0);
            do
            {
                index = Array.IndexOf(formulaSplit, "&&");
                if (index > 0)
                {

                    for (int i = 0; i < tableTruthCopy.GetLength(0); i++)
                    {
                        result[i] = SolveBool(tableTruthCopy[i, (index - 1) / 2], formulaSplit[index], tableTruthCopy[i, (index + 1) / 2]);

                    }
                    ShortFormula(formulaSplit, index);
                };
            }
            while (index > 0);
            do
            {
                index = Array.IndexOf(formulaSplit, "||");
                if (index > 0)
                {

                    for (int i = 0; i < tableTruthCopy.GetLength(0); i++)
                    {
                        result[i] = SolveBool(tableTruthCopy[i, (index - 1) / 2], formulaSplit[index], tableTruthCopy[i, (index + 1) / 2]);


                    }

                    ShortFormula(formulaSplit, index);
                };
            }
            while (index > 0);

            return result;
        }


        void ShortFormula(string[] array, int index)
        {
            array[index - 1] = string.Join("", array[index - 1], array[index + 1]);
            array[index] = "";
            array[index + 1] = "";


            Array.Copy(array, index + 2, array, index, array.Length - 2 - index);


            array[array.Length - 1] = "";
            array[array.Length - 2] = "";
            Console.WriteLine();

        }

        void ShortTable(bool[,] table, int index, bool result)
        {
            bool[,] array = table;
            array[0, index - 1] = result;
            Array.Copy(array, index + 2, array, index, array.Length - 2 - index); ;
            array[0, array.Length - 1] = false;
            array[0, array.Length - 2] = false;

        }

    }
}