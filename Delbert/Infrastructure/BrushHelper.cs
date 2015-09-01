using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Delbert.Infrastructure
{
    public class BrushHelper
    {
        public static SolidColorBrush GetBrushForHex(string hex)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(hex));
        }
    }
}
