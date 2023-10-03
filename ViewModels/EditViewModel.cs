using CommunityToolkit.Mvvm.ComponentModel;
using NagCode.Models;

namespace NagCode.ViewModels
{
    /// <summary>
    /// Author: NBT
    /// 
    /// </summary>
    public class EditViewModel : ObservableRecipient
    {
        private Snip _snipToEdit;

        public Snip SnipToEdit
        {
            get => _snipToEdit;
            set
            {
                _snipToEdit = value;
                OnPropertyChanged(nameof(_snipToEdit));
            }
        }
    }
}
