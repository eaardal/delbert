using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Delbert.Infrastructure.Converters
{
    class IsNotebookSelectedToBackgroundConverter : IValueConverter
    {
        private const string DefaultColourHex = "#BADBE6"; // blå
        private const string HighlightColourHex = "#89C3D6";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected;

            if (bool.TryParse(value.ToString(), out isSelected))
            {
                return isSelected
                    ? BrushHelper.GetBrushForHex(HighlightColourHex)
                    : BrushHelper.GetBrushForHex(DefaultColourHex);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
