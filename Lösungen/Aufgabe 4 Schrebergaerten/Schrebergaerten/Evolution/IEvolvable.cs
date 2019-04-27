using System;
using System.Collections.Generic;

namespace Evolution
{
    public interface IEvolvable
    {
        IEvolvable Crossover(IEnumerable<IEvolvable> other, int crossovers, Random random);
        void Mutate(double mutatability, Random random);

        void CalculateFitness(); // Optimierung, nur einmal berechnen
        double Fitness { get; }
    }
}
