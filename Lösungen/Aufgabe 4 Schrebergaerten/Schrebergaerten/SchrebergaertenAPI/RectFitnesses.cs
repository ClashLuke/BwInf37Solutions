using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectFitness = System.Func<double, double, double, double, double>;

namespace SchrebergaertenAPI
{
    public static class RectFitnesses // kann weggelassen werden, außer Exponents
    {
        public static RectFitness Density = (Density, Area, CenterDistance, WeightedCenterDistance) => 
            1000000f / Density;

        public static RectFitness Area = (Density, Area, CenterDistance, WeightedCenterDistance) => 
            1000000f / Area;

        public static RectFitness CenterDistance = (Density, Area, CenterDistance, WeightedCenterDistance) => 
            CenterDistance;

        public static RectFitness WeightedCenterDistance = (Density, Area, CenterDistance, WeightedCenterDistance) => 
            WeightedCenterDistance;

        public static RectFitness Exponents(double DensityExponent, double AreaExponent, double CenterDistanceExponent, double WeightedCenterDistanceExponent) =>
            (Density, Area, CenterDistance, WeightedCenterDistance) =>
        {
            return (1000000f / Math.Pow(Density, DensityExponent) / Math.Pow(Area, AreaExponent) / Math.Pow(CenterDistance, CenterDistanceExponent) / Math.Pow(WeightedCenterDistance, WeightedCenterDistanceExponent)); 
        };
    }
}
