﻿using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using MvvmCross.Plugin.Visibility;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
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
            Stream imageStream = null;
            Task.Run(() => imageStream = Mvx.IoCProvider.Resolve<IMediaLoaderService>().LoadImageAsync(value).Result).Wait();
            return BitmapFrame.Create(imageStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
        }
    }

    public class ObjectEqualsMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
                return false;

            object zero = values[0];
            foreach (object element in values)
            {
                if (zero?.Equals(element) != true)
                    return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}