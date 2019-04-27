using Evolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectFitness = System.Func<double, double, double, double, double>;

namespace SchrebergaertenAPI
{
    public class SpaceFiller 
    {
        public SegregatedEvolver<RectAttempt> evolver;

        public Random random;
        public double totalWidth, totalHeight;

        public List<Rect> baseRects;
        public long generation = 0;

        public RectFitness rectFitness;
        public bool allowRotation;

        public int genders = 2, crossovers = 1;
        public double mutatability = 0.05f, survivalRate = 0f, failureImmunity = 0.1f, suddenDeath = 0.001f;

        public int evolverGenders = 2, evolverCrossovers = 1;
        public double evolverMutatability = 0.05f, evolverSurvivalRate = 0f, evolverFailureImmunity = 0.1f, evolverSuddenDeath = 0.001f;

        public SpaceFiller(int batchSize, int evolverCount, List<Rect> baseRects, RectFitness rectFitness, bool allowRotation)
        {
            random = new Random();

            this.rectFitness = rectFitness;
            this.allowRotation = allowRotation;

            this.baseRects = baseRects;
            totalWidth = baseRects.Sum(x => x.Width);
            totalHeight = baseRects.Sum(x => x.Height);

            evolver = new SegregatedEvolver<RectAttempt>(evolverCount,
                Enumerable.Repeat((RectAttempt)null, batchSize)
                .Select(x =>
                {
                    RectAttempt rectAttempt = new RectAttempt(
                        baseRects.Select(y =>
                            new Rect(
                                (int)((random.NextDouble() - 0.5f) * totalWidth), 
                                (int)((random.NextDouble() - 0.5f) * totalHeight),
                                y.Width, 
                                y.Height)
                            )
                            .ToList(),
                        rectFitness,
                        allowRotation);
                    return rectAttempt;
                }
            ).ToList())
            {
                genders = genders,
                crossovers = crossovers,
                mutatability = mutatability,
                survivalRate = survivalRate,
                failureImmunity = failureImmunity,
                suddenDeath = suddenDeath,
            };
        }

        public void Generation()
        {
            evolver.genders = genders;
            evolver.crossovers = crossovers;
            evolver.mutatability = mutatability;
            evolver.survivalRate = survivalRate;
            evolver.failureImmunity = failureImmunity;
            evolver.suddenDeath = suddenDeath;

            evolver.evolverGenders = evolverGenders;
            evolver.evolverCrossovers = evolverCrossovers;
            evolver.evolverMutatability = evolverMutatability;
            evolver.evolverSurvivalRate = evolverSurvivalRate;
            evolver.evolverFailureImmunity = evolverFailureImmunity;
            evolver.evolverSuddenDeath = evolverSuddenDeath;
            
            evolver.Generation();

            generation++;
        }
    }
}
