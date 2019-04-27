using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VollDanebenGUI
{
    public static class VollDaneben
    {
        private static double[] BasicSolver(int[] choices, int availablePoints) // Greedy Solver, Passes = 0
        {
            int[] editedChoices = choices.OrderBy(x => x).ToArray();

            return Enumerable
                .Range(0, availablePoints)
                .Select(x =>
                    Enumerable.Range(x * (int)Math.Floor((double)editedChoices.Length / availablePoints), (int)Math.Ceiling((double)editedChoices.Length / availablePoints))
                    .Average(y => editedChoices[y]))
                .ToArray();
        }

        public static double[] OptimizeSolution(int[] choices, double[] luckyNumbers, int start, int passes, int stride) // Optimizer, Passes incrementer
        {
            if (passes > 0)
            {
                int availablePoints = luckyNumbers.Length;
                double min = choices.Min(), max = choices.Max();
                double[] costs = Enumerable.Repeat(Cost(choices, luckyNumbers), availablePoints).ToArray();

                for (int pass = start; pass < passes + start; pass += stride)
                {
                    double move = (max - min) / pass;

                    for (int i = 0; i < availablePoints; i++)
                    {
                        double prevNumber = luckyNumbers[i];

                        luckyNumbers[i] = Math.Max(min, prevNumber - move);
                        double leftCost = Cost(choices, luckyNumbers);

                        luckyNumbers[i] = Math.Min(max, prevNumber + move);
                        double rightCost = Cost(choices, luckyNumbers);

                        luckyNumbers[i] = prevNumber;

                        if (leftCost < rightCost && leftCost < costs[i])
                        {
                            luckyNumbers[i] = Math.Max(min, prevNumber - move);
                            costs[i] = leftCost;
                        }
                        else if(rightCost < costs[i])
                        {
                            luckyNumbers[i] = Math.Min(max, prevNumber + move);
                            costs[i] = rightCost;
                        }
                    }
                }
            }

            return luckyNumbers.ToArray();
        }

        public static int[] GeneralSolver(int[] choices, int availablePoints, int quality, int start, int stride) // General Solver, Passes = n >= 0
        {
            if (quality < 0) throw new ArgumentOutOfRangeException("Passes cannot be negative");
            double[] luckyNumbers = BasicSolver(choices, availablePoints);
            return OptimizeSolution(choices, luckyNumbers, start, quality, stride).Select(x => (int)x).ToArray();
        }

        public static int Cost(int[] choices, int[] luckyNumbers) => choices.Sum(x => luckyNumbers.Min(y => Math.Abs(y - x)));
        private static double Cost(int[] choices, double[] luckyNumbers) => choices.Sum(x => luckyNumbers.Min(y => Math.Abs(y - x)));
    }
}
