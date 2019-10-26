using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vault.Core.ViewModels
{
    public class HomeViewModel : Base.ViewModelBase, IMasterPresentationViewModel
    {
        #region Fields

        private object _detailView;
        private int _selectedPageIndex;

        #endregion

        #region Commands

        public IMvxCommand OnNavigationRequestedCommand => new MvxCommand<Type>(OnNavigateRequested);

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
        /// Gets or sets the index of the selected navigation item
        /// </summary>
        public int SelectedPageIndex
        {
            get => _selectedPageIndex;
            set => SetProperty(ref _selectedPageIndex, value);
        }

        /// <summary>
        /// Gets a dictionary of navigatable viewmodel types, and their friendly name
        /// </summary>
        public Dictionary<Type, string> NavigationItems { get; }

        #endregion

        public HomeViewModel(IMvxNavigationService navigationService)
            : base(navigationService)
        {
            NavigationItems = new Dictionary<Type, string>()
            {
                { typeof(ImageDisplayViewModel), "Images" },
                { typeof(VideoDisplayViewModel), "Videos" }
            };
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            if (NavigationItems.Count > 0)
            {
                OnNavigateRequested(NavigationItems.Keys.First());
                SelectedPageIndex = 0;
            }
        }

        private void OnNavigateRequested(Type navigationItem)
        {
            NavigationService.Navigate(navigationItem);
        }
    }
}
