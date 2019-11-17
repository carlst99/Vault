using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vault.Core.ViewModels
{
    public class HubViewModel : Base.ViewModelBase, IMasterPresentationViewModel
    {
        #region Fields

        private object _detailView;
        private Tuple<Type, string> _selectedPage;

        #endregion

        #region Commands

        public IMvxCommand OnNavigationRequestedCommand => new MvxCommand(OnNavigateRequested);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the detail view of this master view
        /// </summary>
        public object DetailView
        {
            get => _detailView;
            set => SetProperty(ref _detailView, value);
        }

        /// <summary>
        /// Gets a dictionary of navigatable viewmodel types, and their friendly name
        /// </summary>
        public List<Tuple<Type, string>> NavigationItems { get; }

        /// <summary>
        /// Gets or sets the selected page
        /// </summary>
        public Tuple<Type, string> SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
        }

        #endregion

        public HubViewModel(IMvxNavigationService navigationService, IMvxMessenger messenger)
            : base(navigationService, messenger)
        {
            NavigationItems = new List<Tuple<Type, string>>()
            {
                new Tuple<Type, string>(typeof(ImageDisplayViewModel), "Images"),
                new Tuple<Type, string>(typeof(VideoDisplayViewModel), "Videos")
            };
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            if (NavigationItems.Count > 0)
            {
                NavigationService.Navigate(NavigationItems[0].Item1);
                SelectedPage = NavigationItems[0];
            }
        }

        private void OnNavigateRequested()
        {
            NavigationService.Navigate(SelectedPage.Item1);
        }
    }
}
