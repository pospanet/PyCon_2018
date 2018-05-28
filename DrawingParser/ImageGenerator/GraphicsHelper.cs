using System;
using System.Drawing;

namespace ImageGenerator
{
    internal static class BresenhamGraphicsHelper
    {
        internal static void PlotLine(Bitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                {
                    PlotLineLow(bmp, x1, y1, x0, y0, color);
                }
                else
                {
                    PlotLineLow(bmp, x0, y0, x1, y1, color);
                }
            }
            else
            {
                if (y0 > y1)
                {
                    PlotLineHigh(bmp, x1, y1, x0, y0, color);
                }
                else
                {
                    PlotLineHigh(bmp, x0, y0, x1, y1, color);
                }
            }
        }

        private static void PlotLineLow(Bitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int d = 2 * dy - dx;
            int y = y0;

            for (int x = x0; x < x1; x = x + (dx > 0 ? 1 : -1))
            {
                bmp.SetPixel(x, y, color);
                if (d > 0)
                {
                    y = y + yi;
                    d = d - 2 * dx;
                }

                d = d + 2 * dy;
            }
        }

        private static void PlotLineHigh(Bitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int d = 2 * dx - dy;
            int x = x0;

            for (int y = y0; y < y1; y = y + (dy > 0 ? 1 : -1))
            {
                bmp.SetPixel(x, y, color);
                if (d > 0)
                {
                    x = x + xi;
                    d = d - 2 * dy;
                }

                d = d + 2 * dx;
            }
        }
    }

    internal static class XiaolinWuGraphicHelper
    {
        internal static void DrawLine(Bitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            if (x0 == x1 && y0 == y1)
            {
                bmp.SetPixel(x0, y0, Color.White);
                return;
            }
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                swap(ref x0, ref y0);
                swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                swap(ref x0, ref x1);
                swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = y1 - y0;
            int gradient = dy / dx;

            if (dx == 0)
            {
                gradient = 1;
            }

            int xend = round(x0);
            int yend = y0 + gradient * (xend - x0);
            decimal xgap = rfpart((decimal) (x0 + 0.5));
            int xpxl1 = xend;
            int ypxl1 = ipart(yend);

            if (steep)
            {
                plot(bmp, ypxl1, xpxl1, rfpart(yend) * xgap);
                plot(bmp, ypxl1 + 1, xpxl1, fpart(yend) * xgap);
            }
            else
            {
                plot(bmp, xpxl1, ypxl1, rfpart(yend) * xgap);
                plot(bmp, xpxl1, ypxl1 + 1, fpart(yend) * xgap);
            }

            int intery = yend + gradient;

            // handle second endpoint
            xend = round(x1);
            yend = y1 + gradient * (xend - x1);
            xgap = fpart((decimal) (x1 + 0.5));
            int xpxl2 = xend;
            int ypxl2 = ipart(yend);

            if (steep)
            {
                plot(bmp, ypxl2, xpxl2, rfpart(yend) * xgap);
                plot(bmp, ypxl2 + 1, xpxl2, fpart(yend) * xgap);
            }
            else
            {
                plot(bmp, xpxl2, ypxl2, rfpart(yend) * xgap);
                plot(bmp, xpxl2, ypxl2 + 1, fpart(yend) * xgap);
            }

            // main loop
            if (steep)
            {
                for (int x = xpxl1 + 1; x < xpxl2 - 1; x++)
                {
                    plot(bmp, ipart(intery), x, rfpart(intery));
                    plot(bmp, ipart(intery) + 1, x, fpart(intery));
                    intery = intery + gradient;
                }
            }
            else
            {
                for (int x = xpxl1 + 1; x < xpxl2 - 1; x++)
                {
                    plot(bmp, x, ipart(intery), rfpart(intery));
                    plot(bmp, x, ipart(intery) + 1, fpart(intery));
                    intery = intery + gradient;
                }
            }
        }

        private static void swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }

        private static void plot(Bitmap bmp, int x, int y, decimal c)
        {
            int intensity = (int) Math.Round(255 * c);
            intensity = intensity > 255 ? 255 : intensity;
            Color color = Color.FromArgb(intensity, intensity, intensity);
            bmp.SetPixel(x, y, color);
        }

        private static int ipart(decimal x)
        {
            return (int) Math.Floor(x);
        }

        private static int round(decimal x)
        {
            return (int) Math.Round(x);
        }

        private static decimal fpart(decimal x)
        {
            return x - Math.Floor(x);
        }

        private static decimal rfpart(decimal x)
        {
            return 1 - fpart(x);
        }
    }
}