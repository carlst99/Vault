using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using MvvmCross.Plugin.Visibility;
using StreamEncryptor.Predefined;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace Vault.Wpf.Views
{
    public class NativeMvxVisibilityValueConverter : MvxNativeValueConverter<MvxVisibilityValueConverter> { }

    public class NativePathToImageConverter : MvxNativeValueConverter<PathToImageConverter> { }

    public class PathToImageConverter : MvxValueConverter<string, BitmapFrame>
    {
        protected override BitmapFrame Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            using (FileStream fs = new FileStream(value, FileMode.Open, FileAccess.Read))
            using (var encryptor = new AesHmacEncryptor("V7GAe5ZRJ4GtxZ3S8jJLCZNQP2SXTyO4"))
            {
                MemoryStream ms = new MemoryStream();
                encryptor.DecryptAsync(fs, ms).Wait();
                return BitmapFrame.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
            }
        }
    }
}