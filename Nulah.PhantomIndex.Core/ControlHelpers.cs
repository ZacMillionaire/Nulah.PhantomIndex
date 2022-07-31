using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nulah.PhantomIndex.Core
{
    public static class ControlHelpers
    {
        /// <summary>
        /// Brightens or darkens a given colour. Negative values darken, and <paramref name="brightnessAdjustment"/> must be between -1 and 1
        /// </summary>
        /// <param name="color"></param>
        /// <param name="brightnessAdjustment">Darken or lighten amount, from -1 to 1</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static Color ChangeColorBrightness(Color color, float brightnessAdjustment)
        {
            if (brightnessAdjustment < -1 || brightnessAdjustment > 1)
            {
                throw new NotSupportedException($"{nameof(brightnessAdjustment)} must be between -1 and 1 inclusive");
            }

            // do nothing if the brightness adjustment is 0
            if (brightnessAdjustment == 0)
            {
                return color;
            }

            if (brightnessAdjustment < 0)
            {
                // Darken the colours, add 1 to the brightness to bring it back to a positive final value after adjustment
                brightnessAdjustment = 1 + brightnessAdjustment;

                return Color.FromArgb(color.A,
                    (byte)(color.R * brightnessAdjustment),
                    (byte)(color.G * brightnessAdjustment),
                    (byte)(color.B * brightnessAdjustment)
                );
            }
            else
            {
                // Lighten the colour by getting the distance from full bright (255) the original colour was,
                // multiplying it by the adjustment then adding the original value back
                return Color.FromArgb(color.A,
                    (byte)((255 - color.R) * brightnessAdjustment + color.R),
                    (byte)((255 - color.G) * brightnessAdjustment + color.G),
                    (byte)((255 - color.B) * brightnessAdjustment + color.B)
                );
            }
        }
    }
}
