namespace NagCode.Views
{
  using MahApps.Metro.Controls;
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  public partial class PresentationView : MetroWindow
  {
    public ViewModels.NageCodeModel NageCodeModel => DataContext as ViewModels.NageCodeModel;

    // Drag and Drop ---{
    private Point dragStartPoint = new Point(0, 0);
    private FrameworkElement draggingElement;
    // Drag and Drop ---}

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
      ListSnippets.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
      ListSnippets.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
    }

    private void PresentWindowWindow_Closed(object sender, System.EventArgs e)
    {
      NageCodeModel.IsInPresentMode = false;
    }

    private void MoveButton(object sender, RoutedEventArgs e)
    {
      Left = (Left == 0) ? (SystemParameters.PrimaryScreenWidth - Width) : 0;
    }

    private void CloseButton(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void ListSnippets_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var ewl = new Models.EditViewLogic(NageCodeModel, ListSnippets);
      ewl.OpeningRequest(NageCodeModel.SelectedSnippet);
    }


    /// <summary>
    /// Drag and Drop:
    /// Called for every mouse move on the ListSnippets
    /// If the mouse moves outside the MainWindow, start the drag
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListSnippets_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        e.Handled = true;
        FrameworkElement currentElement = sender as FrameworkElement;
        if (draggingElement != currentElement)
        {
          return;
        }


        // If the mouse moves outside the MainWindow, start the drag.
        if (!(dragStartPoint.X == 0) || !(dragStartPoint.Y == 0))
        {

          Point currentPoint = e.GetPosition(ListSnippets);
          if ((Math.Abs(dragStartPoint.X - currentPoint.X) > 2) && (Math.Abs(dragStartPoint.Y - currentPoint.Y) > 2))
          {

            if (ListSnippets.SelectedIndex == -1)
            {
              return;
            }

            object payload = NageCodeModel.SnippetList[ListSnippets.SelectedIndex].Data;

            DataObject dataObject = new DataObject();
            dataObject.SetData(payload);

            Clipboard.SetDataObject(dataObject);


            DragDropEffects dropEffect = DragDrop.DoDragDrop(draggingElement, dataObject, DragDropEffects.Copy | DragDropEffects.Move);
            if (dropEffect != DragDropEffects.None)
            {
              if (ListSnippets.SelectedIndex < ListSnippets.Items.Count - 1)
              {
                ListSnippets.SelectedIndex++;
              }
            }
          }
        }

      }
    }

    /// <summary>
    /// Drag and Drop:
    /// Called for every selection changed on ListSnippets
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListSnippets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (ListSnippets.SelectedIndex == -1)
      {
        return;
      }
      object payload = NageCodeModel.SnippetList[ListSnippets.SelectedIndex].Data;

      DataObject dataObject = new DataObject();
      dataObject.SetData(payload);

      Clipboard.SetDataObject(dataObject);

    }

    /// <summary>
    /// Drag and Drop:
    /// When Mouse Down on the ListItem(code Snippet) the dragStartPoint and draggingElement created
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListSnippets_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      Size dragSize = new Size(20, 20);
      dragStartPoint = e.GetPosition(ListSnippets);
      draggingElement = sender as FrameworkElement;
    }


  }
}
