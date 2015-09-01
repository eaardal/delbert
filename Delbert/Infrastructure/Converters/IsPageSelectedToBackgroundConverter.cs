using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Delbert.Infrastructure.Converters
{
    class IsPageSelectedToBackgroundConverter : IValueConverter
    {
        private const string DefaultColourHex = "#DBBAE6"; // lilla
        private const string HighlightColourHex = "#C389D6";

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