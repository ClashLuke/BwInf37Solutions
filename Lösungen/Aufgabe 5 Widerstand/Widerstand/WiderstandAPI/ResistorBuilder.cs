using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiderstandAPI.Resistors;

namespace WiderstandAPI
{
    public static class ResistorBuilder
    {
        public static List<IResistor> BuildResistor(double targetResistance, int k, List<double> resistors, Action<List<IResistor>, IResistor> callback = null)
        {
            return BuildResistor(targetResistance, null, null, ref k, resistors, callback);
        }
        private static List<IResistor> BuildResistor(double targetResistance, IResistor prev, List<IResistor> all, ref int k, List<double> resistors, Action<List<IResistor>, IResistor> callback = null)
        {
            if (all == null) all = new List<IResistor>();
            if (prev == null) prev = new SerialResistor();

            if (prev.Count >= k)
            {
                if (prev.Count == k) all.AddResistor(prev, targetResistance, callback);
                return all;
            }

            IResistor resistor = (IResistor)prev.Clone();

            double bestResult = Math.Min(all.Count == 0 ? double.PositiveInfinity 
                                                        : all.Min(x => Math.Abs(x.Value - targetResistance)), Math.Abs(prev.Value - targetResistance));

            List<IResistor> content = new List<IResistor>();
            void RecurseResistors(IResistor res)
            {
                if (!(res is SingleResistor))
                {
                    content.Add(res);
                    res.Resistors.ForEach(RecurseResistors);
                }
            }

            RecurseResistors(resistor);
            content = content.Distinct().ToList();

            double RecurseOption(IResistor subResistor, IResistor newResistor, ref int remainingK)
            {
                subResistor.Resistors.Add(newResistor);
                Cleanup(ref newResistor);
                double diff = Math.Abs(resistor.Value - targetResistance);
                if (!all.Contains(newResistor))
                {
                    if (diff == 0)
                    {
                        all.AddResistor((IResistor)resistor.Clone(), targetResistance, callback);
                        remainingK = Math.Min(resistor.Count, remainingK);
                    }
                    else if (diff < bestResult)
                    {
                        List<IResistor> results = BuildResistor(targetResistance, resistor, all, ref remainingK, new List<double>(resistors), callback);

                        if (results.Count == 0) all.AddResistor((IResistor)resistor.Clone(), targetResistance, callback);
                        else all.AddResistors(results, targetResistance, callback);

                        all = all.Distinct().ToList();
                    }
                }
                subResistor.Resistors.RemoveAt(subResistor.Resistors.Count - 1);
                return diff;
            }

            foreach (IResistor subResistor in content)
            {
                for (int i = 0; i < resistors.Count; i++)
                {
                    double resistance0 = resistors[i];
                    resistors.RemoveAt(i);

                    if (RecurseOption(subResistor, new SingleResistor(resistance0), ref k) == 0) goto Exit;

                    for (int j = 0; j < resistors.Count; j++)
                    {
                        double resistance1 = resistors[j];
                        resistors.RemoveAt(j);

                        if (RecurseOption(subResistor, new SerialResistor(resistance0, resistance1), ref k) == 0) goto Exit;
                        if (RecurseOption(subResistor, new ParallelResistor(resistance0, resistance1), ref k) == 0) goto Exit;

                        resistors.Add(resistance1);
                    }

                    resistors.Add(resistance0);
                }
            }

            Exit:

            return all;
        }

        public static void AddResistor(this List<IResistor> list, IResistor resistor, double targetResistance, Action<List<IResistor>, IResistor> callback)
        {
            if (list.Contains(resistor)) return;
            int i = 0;
            while (i < list.Count && Math.Abs(list[i].Value - targetResistance) < Math.Abs(resistor.Value - targetResistance)) i++;
            list.Insert(i, resistor);
            if (callback != null && (resistor is SingleResistor || resistor.Resistors.Count > 0)) callback(list, resistor);
        }

        public static void AddResistors(this List<IResistor> list, List<IResistor> resistors, double targetResistance, Action<List<IResistor>, IResistor> callback)
        {
            resistors = new List<IResistor>(resistors);
            for (int i = 0; i < resistors.Count; i++)
            {
                list.AddResistor(resistors[i], targetResistance, callback);
            }
        }

        public static void Cleanup(ref IResistor resistor)
        {
            if (!(resistor is SingleResistor))
            {
                if (resistor.Resistors.Count == 0)
                {
                    throw new Exception("Invalid resistor");
                }
                else if (resistor.Resistors.Count == 1)
                {
                    IResistor internalResistor = resistor.Resistors[0];
                    Cleanup(ref internalResistor);
                    resistor = internalResistor;
                }
                else
                {
                    resistor.Resistors.ForEach(x => Cleanup(ref x));
                    resistor.Resistors = resistor.Resistors.OrderBy(x => x.Value).ToList();
                }
            }
        }
    }
}
