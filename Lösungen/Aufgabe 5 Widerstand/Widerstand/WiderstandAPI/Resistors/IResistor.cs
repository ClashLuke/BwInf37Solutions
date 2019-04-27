using System;
using System.Collections.Generic;
using System.Drawing;

namespace WiderstandAPI.Resistors
{
    public interface IResistor : ICloneable
    {
        double Value { get; }
        List<IResistor> Resistors { get; set; }
        int Count { get; }

        Bitmap Draw(ResistorStyle style);
    }
}
