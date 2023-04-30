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

    private DragDropManager _dragDropManager = new DragDropManager();
    private BL.EditViewLogic _editViewLogic;

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
      if (_editViewLogic!=null)
      {
        _editViewLogic.PresentViewLeft = Left;
        _editViewLogic.PresentViewTop = Top;
      }

    }

    private void CloseButton(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void SnipList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      _editViewLogic = new BL.EditViewLogic(NagCodeModel, SnipList);
      _editViewLogic.PresentViewLeft = Left;
      _editViewLogic.PresentViewTop = Top;
      _editViewLogic.OpeningRequest(NagCodeModel.SelectedSnip, true);
    }

    private void SnipList_MouseMove(object sender, MouseEventArgs e)
    {
      _dragDropManager.MouseMove(sender, e, SnipList, NagCodeModel);
    }

    private void SnipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _dragDropManager.SelectionChanged(SnipList, NagCodeModel);
    }

    private void SnipList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      _dragDropManager.PreviewMouseDown(sender, e, SnipList);
    }

  }
}
