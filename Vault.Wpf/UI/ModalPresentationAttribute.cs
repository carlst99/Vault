using MvvmCross.Presenters.Attributes;
using System;

namespace Vault.Wpf.UI
{
    public sealed class ModalPresentationAttribute : MvxBasePresentationAttribute
    {
        public Type OwnerWindowType { get; set; }
    }
}
