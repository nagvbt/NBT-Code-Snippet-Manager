namespace NagCode.Models
{
  using NagCode.Interfaces;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;

  public class EditViewLogic
  {
    public ViewModels.NageCodeModel NageCodeModel;

    private ListBox SnipList;

    public EditViewLogic(ViewModels.NageCodeModel nageCodeModel, ListBox listSnippets)
    {
      SnipList = listSnippets;
      NageCodeModel = nageCodeModel;
    }

    internal void OpeningRequest(ISnip selectedSnippet)
    {
      if (SnipList.SelectedIndex != -1)
      {
        if (!selectedSnippet.IsSeperator)
        {
          var editWindow = new Views.EditView((Snip)NageCodeModel.SelectedSnippet);
          editWindow.Show();
          editWindow.EditViewModel.SnippetToEdit.PropertyChanged += EditWindowChange;
        }
      }
    }

    internal void OpeningRequest(ISnip selectedSnippet, double Left, double Top, double Width, double Height)
    {
      if (SnipList.SelectedIndex != -1)
      {
        if (!selectedSnippet.IsSeperator)
        {
          var editWindow = new Views.EditView((Snip)NageCodeModel.SelectedSnippet);

          // Show the EditView Nest to the SnipView
          editWindow.WindowStartupLocation = WindowStartupLocation.Manual;
          editWindow.Left = Left + Width;
          editWindow.Top = Top + (Height - editWindow.Height) / 2;
          editWindow.Show();

          editWindow.EditViewModel.SnippetToEdit.PropertyChanged += EditWindowChange;
        }
      }
    }

    private void EditWindowChange(object sender, PropertyChangedEventArgs e)
    {
      NageCodeModel.SelectedSnippet = (Snip)sender;
      NageCodeModel.IsDirty = true;
    }
  }
}
