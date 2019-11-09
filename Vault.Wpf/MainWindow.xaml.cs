using BespokeFusion;
using Microsoft.WindowsAPICodePack.Dialogs;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Windows;
using Vault.Core.Model.Messages;

namespace Vault.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
#pragma warning disable IDE0052 // Remove unread private members
        private MvxSubscriptionToken _dialogSubToken;
        private MvxSubscriptionToken _fileSubToken;
#pragma warning restore IDE0052 // Remove unread private members

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MvxWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _dialogSubToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().SubscribeOnMainThread<DialogMessage>(OnDialogMessage);
            _fileSubToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().SubscribeOnMainThread<FileMessage>(OnFileMessage);

            using (UpdateManager mgr = new UpdateManager("C:\\Users\\carls\\source\\repos\\Vault\\Releases"))
                await mgr.UpdateApp().ConfigureAwait(false);
        }

        private void OnDialogMessage(DialogMessage message)
        {
            switch (message.MessageType)
            {
                case DialogMessage.DialogMessageType.Error:
                    MaterialMessageBox.ShowError(message.Content);
                    break;
                case DialogMessage.DialogMessageType.Info:
                    MaterialMessageBox.Show(message.Content, message.Title);
                    break;
                case DialogMessage.DialogMessageType.Interaction:
                    MessageBoxResult result = MaterialMessageBox.ShowWithCancel(message.Content, message.Title);
                    if (result == MessageBoxResult.OK)
                        message.Callback.Invoke(DialogMessage.DialogButton.Ok);
                    else
                        message.Callback.Invoke(DialogMessage.DialogButton.Cancel);
                    break;
            }
        }

        private void OnFileMessage(FileMessage message)
        {
            switch (message.Type)
            {
                case FileMessage.DialogType.OpenFile:
                    CommonOpenFileDialog cofd = new CommonOpenFileDialog
                    {
                        Title = message.Title,
                        EnsurePathExists = true,
                        Multiselect = message.MultiSelect
                    };
                    foreach (Tuple<string, string> filter in message.Filters)
                    {
                        cofd.Filters.Add(new CommonFileDialogFilter(filter.Item1, filter.Item2));
                    }

                    CommonFileDialogResult result = cofd.ShowDialog();
                    cofd.Dispose();
                    if (result == CommonFileDialogResult.Ok)
                    {
                        if (message.MultiSelect)
                            message.Callback?.Invoke(FileMessage.DialogResult.Succeeded, cofd.FileNames);
                        else
                            message.Callback?.Invoke(FileMessage.DialogResult.Succeeded, new List<string> { cofd.FileName });
                    }
                    else
                    {
                        message.Callback?.Invoke(FileMessage.DialogResult.Failed, null);
                    }

                    break;
            }
        }
    }
}
