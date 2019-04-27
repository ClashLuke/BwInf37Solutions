using System;

namespace SchrebergaertenAPI
{
    public class Rect : ICloneable
    {
        public double XCenter, YCenter;
        public double Width, Height;

        public double XStart { get => XCenter - Width / 2f; }
        public double YStart { get => YCenter - Height / 2f; }
        public double XEnd { get => XCenter + Width / 2f;  }
        public double YEnd { get => YCenter + Height / 2f; }

        public double Area => Width * Height;

        public Rect() { }
        public Rect(Rect clone) : this(clone.XCenter, clone.YCenter, clone.Width, clone.Height) { }

        public Rect(double xCenter, double yCenter, double width, double height)
        {
            XCenter = xCenter;
            YCenter = yCenter;
            Width = width;
            Height = height;
        }

        public bool Overlap(Rect other)
        {
            return Overlap(this, other);
        }

        public static bool Overlap(Rect left, Rect right)
        {
            if (left.XEnd <= right.XStart || left.XStart >= right.XEnd) return false;
            if (left.YEnd <= right.YStart || left.YStart >= right.YEnd) return false;
            return true;
        }

        public object Clone()
        {
            return new Rect(this);
        }

        public override bool Equals(object obj)
        {
            return obj is Rect rect &&
                   XCenter == rect.XCenter &&
                   YCenter == rect.YCenter &&
                   Width == rect.Width &&
                   Height == rect.Height;
        }

        public override int GetHashCode()
        {
            var hashCode = -1178209874;
            hashCode = hashCode * -1521134295 + XCenter.GetHashCode();
            hashCode = hashCode * -1521134295 + YCenter.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + XStart.GetHashCode();
            hashCode = hashCode * -1521134295 + YStart.GetHashCode();
            hashCode = hashCode * -1521134295 + XEnd.GetHashCode();
            hashCode = hashCode * -1521134295 + YEnd.GetHashCode();
            hashCode = hashCode * -1521134295 + Area.GetHashCode();
            return hashCode;
        }
    }
}
