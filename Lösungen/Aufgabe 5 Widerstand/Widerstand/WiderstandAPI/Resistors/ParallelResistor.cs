using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiderstandAPI.Resistors
{
    public class ParallelResistor : IResistor
    {
        public ParallelResistor()
        {
            Resistors = new List<IResistor>();
        }

        public ParallelResistor(params double[] val)
        {
            Resistors = new List<IResistor>(val.Select(x => (SingleResistor)x));
        }

        public ParallelResistor(params IResistor[] val)
        {
            Resistors = new List<IResistor>(val.Select(x => x));
        }

        public List<IResistor> Resistors { get; set; }
        public double Value => 1f / Resistors.Sum(x => 1f / x.Value);

        public int Count => Resistors.Sum(x => x.Count);

        public object Clone() => new ParallelResistor { Resistors = Resistors.Select(x => (IResistor)x.Clone()).ToList() };

        public Bitmap Draw(ResistorStyle style)
        {
            List<Bitmap> resistors = Resistors.Select(x => x.Draw(style)).ToList();

            Bitmap bitmap = new Bitmap(resistors.Max(x => x.Width) + style.WireLength * 2, resistors.Sum(x => x.Height + style.WireLength) - style.WireLength, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(style.BackColor), 0, 0, bitmap.Width, bitmap.Height);

                int sum = 0;

                Pen pen = new Pen(new SolidBrush(style.WireColor), style.WireThickness);

                resistors.ForEach(res =>
                {
                    g.DrawLine(pen, 0, sum + res.Height / 2, bitmap.Width - style.WireLength - style.WireThickness, sum + res.Height / 2);
                    g.DrawImage(res, (bitmap.Width - res.Width) / 2, sum);
                    sum += res.Height + style.WireLength;
                });

                g.DrawLine(pen, 0, resistors[0].Height / 2, 0, sum - style.WireLength - resistors.Last().Height / 2);
                g.DrawLine(pen, bitmap.Width - style.WireLength - style.WireThickness, resistors[0].Height / 2, bitmap.Width - style.WireLength - style.WireThickness, sum - style.WireLength - resistors.Last().Height / 2);
                g.DrawLine(pen, bitmap.Width - style.WireThickness, bitmap.Height / 2, bitmap.Width - style.WireLength - style.WireThickness, sum - style.WireLength - bitmap.Height / 2);
            }

            return bitmap;
        }
    }
}
