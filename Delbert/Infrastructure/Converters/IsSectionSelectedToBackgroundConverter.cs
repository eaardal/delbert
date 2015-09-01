using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Delbert.Infrastructure.Converters
{
    class IsSectionSelectedToBackgroundConverter : IValueConverter
    {
        private const string DefaultColourHex = "#C5E6BA"; // grønn
        private const string HighlightColourHex = "#9CD689";

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