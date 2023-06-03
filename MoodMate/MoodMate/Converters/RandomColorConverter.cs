using System.Globalization;

namespace MoodMate.Converters;

internal class RandomColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var colors = new string[] { "#ffc961", "#53a2ff", "#ef6355", "#d2ddff", "#d9564f", "#ffad61" };
        return Color.FromArgb(colors[new Random().Next(0, colors.Length)]);
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}