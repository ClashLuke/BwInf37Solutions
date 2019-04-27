using RandomHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution
{
    public class SegregatedEvolver<T>
        where T : IEvolvable
    {
        public List<Evolver<T>> evolvers;
        public int evolverCount;
        public List<T> Batch => evolvers.SelectMany(x => x.batch).ToList();
        public Random random;

        public int genders = 2, crossovers = 1;
        public double mutatability = 0.05f, survivalRate = 0f, failureImmunity = 0.1f, suddenDeath = 0.001f;

        public int evolverGenders = 2, evolverCrossovers = 1;
        public double evolverMutatability = 0.05f, evolverSurvivalRate = 0f, evolverFailureImmunity = 0.1f, evolverSuddenDeath = 0.001f;

        public SegregatedEvolver(int evolverCount, List<T> batch, Random random = null)
        {
            if (random == null) random = new Random();
            this.random = random;

            this.evolverCount = evolverCount;
            evolvers = new List<Evolver<T>>(evolverCount);
            for (int i = 0; i < evolverCount; i++)
            {
                evolvers.Add(new Evolver<T>(batch, random));
            }
        }

        private int generation = 0;

        public void Generation()
        {
            evolvers.ForEach(x =>
            {
                x.genders = genders;
                x.crossovers = crossovers;
                x.mutatability = mutatability;
                x.survivalRate = survivalRate;
                x.failureImmunity = failureImmunity;
                x.suddenDeath = suddenDeath;
            });
            evolvers.ForEach(x => x.Generation());

            if (generation++ % 10 == 0)
            {
                List<T> batch = Batch;
                batch.ForEach(x => x.CalculateFitness());

                double averageFitness = batch.Average(x => x.Fitness);
                double survivalFitness = evolverSurvivalRate < 0 ? -evolverSurvivalRate * batch.Min(x => x.Fitness) + (1 + evolverSurvivalRate) * averageFitness :
                                                                   evolverSurvivalRate * batch.Max(x => x.Fitness) + (1 - evolverSurvivalRate) * averageFitness;

                evolvers = evolvers
                    .Where(x => (x.batch.Average(y => y.Fitness) > 0 &&
                                 random.NextDouble() < failureImmunity
                               ||
                                (x.batch.Average(y => y.Fitness) >= survivalFitness &&
                                 random.NextDouble() >= suddenDeath)))
                    .ToList();

                if (evolvers.Count < evolverCount)
                {
                    WeightedListRandom<Evolver<T>> genePool = new WeightedListRandom<Evolver<T>>(evolvers, random);

                    while (evolvers.Count < evolverCount)
                    {
                        List<Evolver<T>> parents = genePool.Next(genders, x => x.batch.Average(y => y.Fitness), false).ToList();

                        List<int> crossoverSpots = new List<int>(crossovers);
                        int batchSize = parents[0].batchSize;
                        for (int i = 0; i < evolverCrossovers; i++)
                        {
                            int nextSpot;

                            while (crossoverSpots.Contains(nextSpot = random.Next(0, batchSize))) ;

                            crossoverSpots.Add(nextSpot);
                        }
                        crossoverSpots.Sort();

                        Evolver<T> evolver = new Evolver<T>(new List<T>(batchSize), random);
                        int currentParent = 0;
                        for (int i = 0; i < batchSize; i++)
                        {
                            if (crossoverSpots.Count > 0 && i > crossoverSpots[0])
                            {
                                currentParent = (currentParent + 1) % parents.Count;
                                crossoverSpots.RemoveAt(0);
                            }

                            evolver.batch.Add(parents[currentParent].batch[i]);
                        }
                        evolvers.Add(evolver);
                    }
                }
            }
        }
    }
}
