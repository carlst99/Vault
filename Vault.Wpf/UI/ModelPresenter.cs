using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Vault.Wpf.UI
{
    public class ModalPresenter : MasterDetailPresenter
    {
        public ModalWpfViewPresenter(ContentControl root)
            : base(root)
        {

        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<ModalPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var view = WpfViewLoader.CreateView(request);
                        return ShowModalWindow(view, attribute, request);
                    },
                    (viewModel, attribute) => CloseWindow(viewModel));

            base.RegisterAttributeTypes();
        }

        protected virtual Task<bool> ShowModalWindow(FrameworkElement element, ModalPresentationAttribute attribute, MvxViewModelRequest request)
        {
            Window window;
            if (element is IMvxWindow mvxWindow)
            {
                window = (Window)element;
                mvxWindow.Identifier = element.GetType().Name;
            }
            else if (element is Window normalWindow)
            {
                // Accept normal Window class
                window = normalWindow;
            }
            else
            {
                // Wrap in window
                window = new MvxWindow
                {
                    Identifier = element.GetType().Name
                };
            }
            window.Closed += Window_Closed;
            FrameworkElementsDictionary.Add(window, new Stack<FrameworkElement>());

            if (!(element is Window))
            {
                FrameworkElementsDictionary[window].Push(element);
                window.Content = element;
            }

            ContentControl owner = FrameworkElementsDictionary.Keys.FirstOrDefault(c => c.GetType() == attribute.OwnerWindowType);
            if (owner != default)
                window.Owner = (Window)owner;

            window.ShowDialog();
            return Task.FromResult(true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Closed -= Window_Closed;

            if (FrameworkElementsDictionary.ContainsKey(window))
                FrameworkElementsDictionary.Remove(window);
        }
    }
}
