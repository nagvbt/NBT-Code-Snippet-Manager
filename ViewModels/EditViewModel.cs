using CommunityToolkit.Mvvm.ComponentModel;
using NagCode.Models;

namespace NagCode.ViewModels
{
  public class EditViewModel : ObservableRecipient
  {
    private Snip _snipToEdit;

    public Snip SnipToEdit
    {
      get => _snipToEdit;
      set
      {
        _snipToEdit = value;
        OnPropertyChanged();
      }
    }
  }
}
