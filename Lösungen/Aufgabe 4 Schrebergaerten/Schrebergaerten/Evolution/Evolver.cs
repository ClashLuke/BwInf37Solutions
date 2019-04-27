using RandomHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution
{
    public class Evolver<T>
        where T : IEvolvable
    {
        public Random random;
        public List<T> batch;
        public int batchSize;
        public int genders = 2, crossovers = 1;
        public double mutatability = 0.05f, survivalRate = 0f, failureImmunity = 0.1f, suddenDeath = 0.001f;

        public Evolver(List<T> batch, Random random = null)
        {
            this.batch = batch;
            batchSize = Math.Max(batch.Count, batch.Capacity);

            this.random = random ?? new Random();
        }
        
        public void Generation()
        {
            batch.ForEach(evolvable => evolvable.Mutate(mutatability, random));
            batch.ForEach(evolvable => evolvable.CalculateFitness());
            
            double survivalFitness = survivalRate < 0 ? -survivalRate * batch.Min(x => x.Fitness) + (1 + survivalRate) * batch.Average(x => x.Fitness) :
                                                        survivalRate * batch.Max(x => x.Fitness) + (1 - survivalRate) * batch.Average(x => x.Fitness);

            batch = batch
                .Where(x => x.Fitness > 0 && random.NextDouble() < failureImmunity
                            || (x.Fitness >= survivalFitness && random.NextDouble() >= suddenDeath))
                .ToList();

            if (batch.Count < batchSize)
            {
                WeightedListRandom<T> genePool = new WeightedListRandom<T>(batch, random);

                while (batch.Count < batchSize)
                {
                    List<IEvolvable> parents = genePool.Next(genders, x => x.Fitness, false).Select(x => (IEvolvable)x).ToList();

                    batch.Add((T)parents[0].Crossover(parents.Skip(1), crossovers, random));
                }
            }
        }
    }
}
