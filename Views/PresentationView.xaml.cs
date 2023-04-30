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
    public ViewModels.NagCodeViewModel NagCodeModel => DataContext as ViewModels.NagCodeViewModel;

    private DragDropManager dragDropManager = new DragDropManager();
    private BL.EditViewLogic ewl;

    public PresentationView()
    {
      InitializeComponent();
      SetViewDimentions();
      DisableScrollbars();
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
      SnipList.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
      SnipList.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
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

    private void SnipList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      ewl = new BL.EditViewLogic(NagCodeModel, SnipList);
      ewl.PresentViewLeft = Left;
      ewl.PresentViewTop = Top;
      ewl.OpeningRequest(NagCodeModel.SelectedSnip, true);
    }

    private void SnipList_MouseMove(object sender, MouseEventArgs e)
    {
      dragDropManager.MouseMove(sender, e, SnipList, NagCodeModel);
    }

    private void SnipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      dragDropManager.SelectionChanged(SnipList, NagCodeModel);
    }

    private void SnipList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      dragDropManager.PreviewMouseDown(sender, e, SnipList);
    }

  }
}
