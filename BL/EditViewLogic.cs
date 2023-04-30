namespace NagCode.BL
{
  using NagCode.Interfaces;
  using NagCode.Models;
  using NagCode.ViewModels;
  using NagCode.Views;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;

  public class EditViewLogic
  {
    public NagCodeViewModel NagCodeModel { get; set; }
    public ListBox SnipList { get; set; }
    public double PresentViewLeft { get; set; }
    public double PresentViewTop { get; set; }

    public EditViewLogic(NagCodeViewModel nagCodeModel, ListBox snipList)
    {
      SnipList = snipList;
      NagCodeModel = nagCodeModel;
    }

    public EditViewLogic(NagCodeViewModel nagCodeModel)
    {
      NagCodeModel = nagCodeModel;
    }

    internal void OpeningRequest()
    {
      var editWindow = new EditView(NagCodeModel);
      ShowEditView(editWindow);
    }

    private static void ShowEditView(EditView editWindow)
    {
      double Top = Properties.Settings.Default.Top;
      double Left = Properties.Settings.Default.Left;
      double Width = Properties.Settings.Default.Width;
      double Height = Properties.Settings.Default.Height;

      editWindow.WindowStartupLocation = WindowStartupLocation.Manual;
      editWindow.Left = Left + Width;
      editWindow.Top = Top;
      editWindow.Height = Height;

      editWindow.Show();
    }

    internal void OpeningRequest(ISnip selectedSnip, bool IsPresentView)
    {
      if (SnipList.SelectedIndex != -1)
      {
        if (!selectedSnip.IsSeperator)
        {
          var editWindow = new EditView((Snip)NagCodeModel.SelectedSnip, NagCodeModel);

          if (IsPresentView)
          {
            editWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            editWindow.Left = (PresentViewLeft == 0) ? 50 : (PresentViewLeft - editWindow.Width);
            editWindow.Top = PresentViewTop;
            editWindow.Show();
          }
          else
          {
            ShowEditView(editWindow);
          }

          editWindow.EditViewModel.SnipToEdit.PropertyChanged += EditWindowChange;
        }
      }
    }

    private void EditWindowChange(object sender, PropertyChangedEventArgs e)
    {
      NagCodeModel.SelectedSnip = (Snip)sender;
      NagCodeModel.IsDirty = true;
    }
  }
}
