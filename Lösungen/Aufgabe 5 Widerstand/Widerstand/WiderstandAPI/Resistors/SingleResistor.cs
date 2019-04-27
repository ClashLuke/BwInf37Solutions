using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiderstandAPI.Resistors
{
    public class SingleResistor : IResistor
    {
        public SingleResistor() { }

        public SingleResistor(double val)
        {
            Value = val;
        }

        public double Value { get; set; }
        public List<IResistor> Resistors
        {
            get => throw new Exception("Resistors don't have Resistors");
            set => throw new Exception("Resistors don't have Resistors");
        }

        public int Count => 1;
        public int Width => 1;
        public int Height => 1;

        public static implicit operator SingleResistor(double val) => new SingleResistor(val);

        public object Clone() => new SingleResistor { Value = Value };

        public Bitmap Draw(ResistorStyle style)
        {
            string value = Value.ToString(CultureInfo.InvariantCulture) + "Ω";
            Bitmap bitmap = new Bitmap(style.ResistorWidth, style.ResistorHeight, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(style.BackColor), 0, 0, bitmap.Width, bitmap.Height);

                g.FillRectangle(new SolidBrush(style.ResistorColor), 0, 0, bitmap.Width - style.WireLength, bitmap.Height);
                g.DrawString(value, new Font(style.Font, bitmap.Height / 2 - 3), new SolidBrush(style.TextColor), (bitmap.Width - style.WireLength) / 2, bitmap.Height / 2, new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                });
                g.DrawLine(new Pen(new SolidBrush(style.WireColor), style.WireThickness), bitmap.Width - style.WireLength, bitmap.Height / 2, bitmap.Width, bitmap.Height / 2);
            }

            return bitmap;
        }
    }
}
