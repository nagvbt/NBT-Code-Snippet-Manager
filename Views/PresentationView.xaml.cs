namespace NagCode.Views
{
  using NagCode.BL;
  using NagCode.ViewModels;
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  public partial class PresentationView : Window
  {
    public ViewModels.NagCodeModel NagCodeModel => DataContext as ViewModels.NagCodeModel;

    private DragDropManager dragDropManager;
    private BL.EditViewLogic ewl;

    public PresentationView()
    {
      InitializeComponent();
      SetViewDimentions();
      DisableScrollbars();
      dragDropManager = new DragDropManager();
    }

    private void SetViewDimentions()
    {
      // Position and margin
      int marginTop = 25;
      int marginBottom = 25;
      PresentWindowWindow.Height = SystemParameters.PrimaryScreenHeight - marginTop - marginBottom;
      PresentWindowWindow.Top = marginTop;
      PresentWindowWindow.Left = SystemParameters.PrimaryScreenWidth - PresentWindowWindow.Width;
    }

    private void DisableScrollbars()
    {
      ListSnippets.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
      ListSnippets.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
    }

    private void PresentWindowWindow_Closed(object sender, System.EventArgs e)
    {
      NagCodeModel.IsInPresentMode = false;
    }

    private void MoveButton(object sender, RoutedEventArgs e)
    {
      Left = (Left == 0) ? (SystemParameters.PrimaryScreenWidth - Width) : 0;
      if (ewl!=null)
      {
        ewl.PresentViewLeft = Left;
        ewl.PresentViewTop = Top;
      }

    }

    private void CloseButton(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void ListSnippets_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      ewl = new BL.EditViewLogic(NagCodeModel, ListSnippets);
      ewl.PresentViewLeft = Left;
      ewl.PresentViewTop = Top;
      ewl.OpeningRequest(NagCodeModel.SelectedSnippet, true);
    }

    private void ListSnippets_MouseMove(object sender, MouseEventArgs e)
    {
      dragDropManager.MouseMove(sender, e, ListSnippets, NagCodeModel);
    }

    private void ListSnippets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      dragDropManager.SelectionChanged(sender, e, ListSnippets, NagCodeModel);
    }

    private void ListSnippets_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      dragDropManager.PreviewMouseDown(sender, e, ListSnippets);
    }




  }
}
