using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WhatsNewWPF.LiveSorting
{
    public class ChangeDirectionToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var direction = value as ChangeDirection? ?? ChangeDirection.Neutral;
            if (direction == ChangeDirection.Up)
                return (char)0xE1FE;
            if (direction == ChangeDirection.Down)
                return (char)0xE1FC;
            return (char)0x268A;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ChangeDirectionToForegroundConverter : IValueConverter
    {
        private readonly SolidColorBrush Green = new SolidColorBrush(Color.FromArgb(255, 65, 139, 0));
        private readonly SolidColorBrush Red = new SolidColorBrush(Color.FromArgb(255, 200, 30, 0));
        private readonly SolidColorBrush Blue = new SolidColorBrush(Color.FromArgb(255, 50, 100, 200));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var direction = value as ChangeDirection? ?? ChangeDirection.Neutral;
            if (direction == ChangeDirection.Up)
                return Green;
            if (direction == ChangeDirection.Down)
                return Red;
            return Blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
