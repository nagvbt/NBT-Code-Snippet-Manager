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
    private EditViewLogic _editViewLogic;

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


    private void SetEditViewPosition()
    {
      if (_editViewLogic == null)
      {
        return;
      }
      _editViewLogic.PresentViewLeft = Left;
      _editViewLogic.PresentViewTop = Top;
    }

    private void OpenEditView()
    {
      _editViewLogic = new EditViewLogic(NagCodeModel, SnipList);
      SetEditViewPosition();
      _editViewLogic.OpeningRequest(NagCodeModel.SelectedSnip, true);
    }

    #region view-handlers
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void SnipList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      OpenEditView();
    }
    private void PresentWindowWindow_Closed(object sender, System.EventArgs e)
    {
      NagCodeModel.IsInPresentMode = false;
    }

    private void MoveButton_Click(object sender, RoutedEventArgs e)
    {
      MoveViewLeftOrRightToScreen();
    }

    private void MoveViewLeftOrRightToScreen()
    {
      Left = (Left == 0) ? (SystemParameters.PrimaryScreenWidth - Width) : 0;
      SetEditViewPosition();
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
    #endregion

  }
}
