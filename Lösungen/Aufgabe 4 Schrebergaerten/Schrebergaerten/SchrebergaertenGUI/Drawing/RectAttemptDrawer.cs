using SchrebergaertenAPI;
using System.Drawing;
using System.Drawing.Imaging;

namespace SchrebergaertenGUI.Drawing
{
    public static class RectAttemptDrawer
    {
        public static Bitmap DrawRectAttempt(this RectAttempt rectAttempt, RectAttemptStyle style)
        {
            Rect boundingBox = rectAttempt.BoundingBox();

            int fac = 3;
            Bitmap bitmap = new Bitmap((int)(boundingBox.Width * fac) + 1, (int)(boundingBox.Height) * fac + 1, PixelFormat.Format32bppArgb);
            
            double offsetX = boundingBox.XStart;
            double offsetY = boundingBox.YStart;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                foreach (Rect rect in rectAttempt.rects)
                {
                    g.FillRectangle(new SolidBrush(style.StrokeColor), 
                        (int)(rect.XStart - offsetX) * fac, (int)(rect.YStart - offsetY) * fac, 
                        (int)rect.Width * fac, (int)rect.Height * fac);

                    g.FillRectangle(new SolidBrush(style.FillColor), 
                        (int)(rect.XStart - offsetX) * fac + style.StrokeThickness, (int)(rect.YStart - offsetY) * fac + style.StrokeThickness, 
                        (int)rect.Width * fac - 2 * style.StrokeThickness, (int)rect.Height * fac - 2 * style.StrokeThickness);
                }
            }

            return bitmap;
        }
    }
}
