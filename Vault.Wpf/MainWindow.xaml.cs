using BespokeFusion;
using Microsoft.WindowsAPICodePack.Dialogs;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Messenger;
using Serilog;
#if RELEASE
using Squirrel;
#endif
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
        private MvxSubscriptionToken _openFileDialogSubToken;
        private MvxSubscriptionToken _saveFileDialogSubToken;
#pragma warning restore IDE0052 // Remove unread private members

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MvxWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _dialogSubToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().SubscribeOnMainThread<DialogMessage>(OnDialogMessage);
            _openFileDialogSubToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().SubscribeOnMainThread<OpenFileDialogMessage>(OnOpenFileDialogMessage);
            _saveFileDialogSubToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().SubscribeOnMainThread<SaveFileDialogMessage>(OnSaveFileDialogMessage);

#if RELEASE
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/carlst99/Vault").ConfigureAwait(false))
                    await mgr.UpdateApp().ConfigureAwait(false);
            } catch (Exception ex)
            {
                Log.Error(ex, "Could not update application");
            }
#endif
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

        private void OnOpenFileDialogMessage(OpenFileDialogMessage message)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog
            {
                Title = message.Title,
                EnsurePathExists = true,
                Multiselect = message.MultiSelect
            };
            foreach (Tuple<string, string> filter in message.Filters)
                cofd.Filters.Add(new CommonFileDialogFilter(filter.Item1, filter.Item2));

            CommonFileDialogResult result = cofd.ShowDialog();
            cofd.Dispose();
            if (result == CommonFileDialogResult.Ok)
            {
                if (message.MultiSelect)
                    message.Callback?.Invoke(true, cofd.FileNames);
                else
                    message.Callback?.Invoke(true, new List<string> { cofd.FileName });
            }
            else
            {
                message.Callback?.Invoke(false, null);
            }
        }

        private void OnSaveFileDialogMessage(SaveFileDialogMessage message)
        {
            CommonSaveFileDialog csfd = new CommonSaveFileDialog
            {
                Title = message.Title,
                EnsurePathExists = true,
                
                DefaultFileName = message.DefaultFileName,
                DefaultExtension = message.DefaultFileExtension,
                EnsureValidNames = true
            };

            CommonFileDialogResult sResult = csfd.ShowDialog();
            csfd.Dispose();
            if (sResult == CommonFileDialogResult.Ok)
                message.Callback?.Invoke(true, csfd.FileName);
            else
                message.Callback?.Invoke(false, null);
        }
    }
}
