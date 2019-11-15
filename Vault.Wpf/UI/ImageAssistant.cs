using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vault.Wpf.UI
{
    public static class ImageAssistant
    {
        public static readonly DependencyProperty StretchSource =
            DependencyProperty.RegisterAttached("StretchSource", typeof(ImageSource), typeof(ImageAssistant), new PropertyMetadata(null, OnStretchSourceChanged));

        public static readonly DependencyProperty ActualHeight =
            DependencyProperty.RegisterAttached("ActualHeight", typeof(double), typeof(ImageAssistant), new PropertyMetadata(0d, OnActualHeightChanged));

        private static void OnStretchSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Image i = (Image)d;
            i.Source = (ImageSource)e.NewValue;

            if (i.Source.Height > GetActualHeight(i) && GetActualHeight(i) != 0)
                i.Stretch = Stretch.Uniform;
            else
                i.Stretch = Stretch.None;
        }

        private static void OnActualHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Image i = (Image)d;
            if (i.Source == null)
                return;

            if (i.Source.Height > GetActualHeight(i))
                i.Stretch = Stretch.Uniform;
            else
                i.Stretch = Stretch.None;
        }

        public static void SetStretchSource(UIElement element, ImageSource value)
        {
            element.SetValue(StretchSource, value);
        }

        public static ImageSource GetStretchSource(UIElement element)
        {
            return (ImageSource)element.GetValue(StretchSource);
        }

        public static void SetActualHeight(UIElement element, double value)
        {
            element.SetValue(ActualHeight, value);
        }

        public static double GetActualHeight(UIElement element)
        {
            return (double)element.GetValue(ActualHeight);
        }
    }
}
