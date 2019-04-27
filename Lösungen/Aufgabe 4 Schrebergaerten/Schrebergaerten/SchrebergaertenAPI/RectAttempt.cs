using Evolution;
using System;
using System.Collections.Generic;
using System.Linq;
using RectFitness = System.Func<double, double, double, double, double>;

namespace SchrebergaertenAPI
{
    public class RectAttempt : IEvolvable
    {
        public List<Rect> rects;

        public double Fitness { get; private set; }
        public RectFitness rectFitness;
        public bool allowRotation;

        public RectAttempt(List<Rect> rects, RectFitness rectFitness, bool allowRotation)
        {
            this.rects = rects;
            this.rectFitness = rectFitness;
            this.allowRotation = allowRotation;
        }

        public RectAttempt(RectAttempt toClone) : this(toClone.rects.Select(x => new Rect(x)).ToList(), toClone.rectFitness, toClone.allowRotation)
        { }

        public Rect BoundingBox()
        {
            if (rects.Count == 0) return new Rect();
            double XStart = rects.Min(x => x.XStart);
            double YStart = rects.Min(x => x.YStart);
            double XEnd   = rects.Max(x => x.XEnd);
            double YEnd   = rects.Max(x => x.YEnd);

            return new Rect((XStart + XEnd) / 2f, (YStart + YEnd) / 2f, XEnd - XStart, YEnd - YStart);
        }

        public void CalculateFitness()
        {
            if (CheckCollisions()) Fitness = -1f;
            else
            {
                Rect boundingBox = BoundingBox();

                double CenterDistance =
                    rects.Average(x => Math.Sqrt(
                        Math.Pow(x.XCenter - boundingBox.XCenter, 2) +
                        Math.Pow(x.YCenter - boundingBox.YCenter, 2)
                    ));

                Rect averageBox = new Rect(
                    rects.Average(x => x.XCenter),
                    rects.Average(x => x.YCenter),
                    0,
                    0);

                double WeightedCenterDistance =
                    rects.Average(x => 
                        Math.Sqrt(Math.Pow(x.XCenter - averageBox.XCenter, 2) + Math.Pow(x.YCenter - averageBox.YCenter, 2)));

                double Area = 
                    boundingBox.Area;

                double Density =
                    Fitness = rects.Average(x =>
                        rects.Average(y => 
                            Math.Sqrt(Math.Pow(x.XCenter - y.XCenter, 2) + Math.Pow(x.YCenter - y.YCenter, 2) )));

                Fitness = rectFitness(Density, Area, CenterDistance, WeightedCenterDistance);
            }
        }

        // Nimmt an, dass alle RectAttempts im gleichen Format sind (gleiche Menge an Rechtecken etc.)
        public IEvolvable Crossover(IEnumerable<IEvolvable> other, int crossovers, Random random)
        {
            List<RectAttempt> parents = other.Select(x => (RectAttempt)x).Union(new[] { this }).ToList();

            List<int> crossoverSpots = new List<int>(crossovers);
            int geneCount = rects.Count * 2;
            for (int i = 0; i < crossovers; i++)
            {
                int nextSpot;

                while (crossoverSpots.Contains(nextSpot = random.Next(0, geneCount))) ;

                crossoverSpots.Add(nextSpot);
            }
            crossoverSpots.Sort();

            RectAttempt output = new RectAttempt(this);
            int currentParent = 0;
            for (int i = 0; i < geneCount; i++)
            {
                if (crossoverSpots.Count > 0 && i > crossoverSpots[0])
                {
                    currentParent = (currentParent + 1) % parents.Count;
                    crossoverSpots.RemoveAt(0);
                }

                if (i % 2 == 0)
                {
                    output.rects[i / 2].XCenter = parents[currentParent].rects[i / 2].XCenter;
                }
                else
                {
                    output.rects[i / 2].YCenter = parents[currentParent].rects[i / 2].YCenter;
                }
            }
            return output;
        }

        public void Mutate(double mutatability, Random random)
        {
            rects.ForEach(x =>
            {
                if (random.NextDouble() < mutatability)
                {
                    x.XCenter += random.Next(-2, 3);
                    x.YCenter += random.Next(-2, 3);
                }

                if (allowRotation && random.NextDouble() < mutatability)
                {
                    double buffer = x.Width;
                    x.Width = x.Height;
                    x.Height = buffer;
                }
            });
        }

        public bool CheckCollisions()
        {
            for (int i = 0; i < rects.Count - 1; i++)
            {
                Rect rectA = rects[i];
                for (int j = i + 1; j < rects.Count; j++)
                {
                    Rect rectB = rects[j];

                    if (rectA.Overlap(rectB))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool EqualRects(RectAttempt obj)
        {
            return allowRotation == obj.allowRotation && rects.SequenceEqual(obj.rects);
        }
    }
}
