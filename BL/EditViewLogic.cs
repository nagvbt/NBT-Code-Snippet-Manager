namespace NagCode.BL
{
  using NagCode.Interfaces;
  using NagCode.Models;
  using NagCode.ViewModels;
  using NagCode.Views;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Media.Media3D;

  public class EditViewLogic
  {
    public ViewModels.NagCodeModel NagCodeModel;

    private ListBox SnipList;

    private double _presentViewLeft;  // the name field
    private double _presentViewTop;  // the name field
    public double PresentViewLeft    // the Name property
    {
      get => _presentViewLeft;
      set => _presentViewLeft = value;
    }

    public double PresentViewTop    // the Name property
    {
      get => _presentViewTop;
      set => _presentViewTop = value;
    }

    public EditViewLogic(ViewModels.NagCodeModel nagCodeModel, ListBox listSnippets)
    {
      SnipList = listSnippets;
      NagCodeModel = nagCodeModel;
    }

    public EditViewLogic(ViewModels.NagCodeModel nagCodeModel)
    {
      NagCodeModel = nagCodeModel;
    }

    internal void OpeningRequest()
    {

      var editWindow = new Views.EditView(NagCodeModel);

      ShowEditView(editWindow);

      //editWindow.EditViewModel.SnippetToEdit.PropertyChanged += EditWindowChange;

    }

    private static void ShowEditView(EditView editWindow)
    {
      double Top = Properties.Settings.Default.Top;
      double Left = Properties.Settings.Default.Left;
      double Width = Properties.Settings.Default.Width;

      editWindow.WindowStartupLocation = WindowStartupLocation.Manual;
      editWindow.Left = Left + Width;
      // editWindow.Top = Top + (Height - editWindow.Height) / 2
      editWindow.Top = Top;

      editWindow.Show();
    }

    internal void OpeningRequest(ISnip selectedSnippet, bool IsPresentView)
    {
      if (SnipList.SelectedIndex != -1)
      {
        if (!selectedSnippet.IsSeperator)
        {
          var editWindow = new Views.EditView((Snip)NagCodeModel.SelectedSnippet, NagCodeModel);

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
       
          editWindow.EditViewModel.SnippetToEdit.PropertyChanged += EditWindowChange;
        }
      }
    }

    private void EditWindowChange(object sender, PropertyChangedEventArgs e)
    {
      NagCodeModel.SelectedSnippet = (Snip)sender;
      NagCodeModel.IsDirty = true;
    }
  }
}
