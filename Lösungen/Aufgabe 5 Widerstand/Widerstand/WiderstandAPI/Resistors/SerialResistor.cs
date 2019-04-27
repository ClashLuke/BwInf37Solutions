using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiderstandAPI.Resistors
{
    public class SerialResistor : IResistor
    {
        public SerialResistor()
        {
            Resistors = new List<IResistor>();
        }

        public SerialResistor(params double[] val)
        {
            Resistors = new List<IResistor>(val.Select(x => (SingleResistor)x));
        }

        public SerialResistor(params IResistor[] val)
        {
            Resistors = val.ToList();
        }

        public List<IResistor> Resistors { get; set; }
        public double Value  => Resistors.Sum(x => x.Value);

        public int Count => Resistors.Sum(x => x.Count);

        public object Clone() => new SerialResistor { Resistors = Resistors.Select(x => (IResistor)x.Clone()).ToList() };

        public Bitmap Draw(ResistorStyle style)
        {
            List<Bitmap> resistors = Resistors.Select(x => x.Draw(style)).ToList();

            Bitmap bitmap = new Bitmap(resistors.Sum(x => x.Width), resistors.Max(x => x.Height), PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(style.BackColor), 0, 0, bitmap.Width, bitmap.Height);

                int sum = 0;

                resistors.ForEach(res =>
                {
                    g.DrawImage(res, sum, (bitmap.Height - res.Height) / 2);
                    sum += res.Width;
                });
            }

            return bitmap;
        }
    }
}
