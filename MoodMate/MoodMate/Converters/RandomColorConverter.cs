using System.Globalization;

namespace MoodMate.Converters;

internal class RandomColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var colors = new string[] { "#2C3963", "#e2d9c5", "#8C913F" };
        Random random = new Random();
        return Color.FromArgb(colors[random.Next(0, colors.Length)]);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}