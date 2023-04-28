using CommunityToolkit.Mvvm.ComponentModel;
using NagCode.Models;

namespace NagCode.ViewModels
{
  public class EditViewModel : ObservableRecipient
  {
    private Snip _snippetToEdit;

    public Snip SnippetToEdit
    {
      get => _snippetToEdit;
      set
      {
        _snippetToEdit = value;
        OnPropertyChanged();
      }
    }
  }
}
