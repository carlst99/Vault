using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using MvvmCross.Plugin.Visibility;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using Vault.Core.Converters;
using Vault.Core.Services;

namespace Vault.Wpf.Views
{
    public class NativeMvxVisibilityValueConverter : MvxNativeValueConverter<MvxVisibilityValueConverter> { }

    public class NativePathToImageValueConverter : MvxNativeValueConverter<PathToImageValueConverter> { }

    public class NativeBoolInverterValueConverter : MvxNativeValueConverter<BoolInverterValueConverter> { }

    public class PathToImageValueConverter : MvxValueConverter<string, BitmapFrame>
    {
        protected override BitmapFrame Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            using (FileStream fs = new FileStream(value, FileMode.Open, FileAccess.Read))
            {
                MemoryStream ms = new MemoryStream();
                EncryptorAssistant.GetEncryptor().DecryptAsync(fs, ms).Wait();
                return BitmapFrame.Create(ms, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
            }
        }
    }
}