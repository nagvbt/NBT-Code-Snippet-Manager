using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace NagCode.BL
{
  public class DragDropManager
    {
 
    private Point dragStartPoint = new Point(0, 0);
    private FrameworkElement draggingElement;
    public ViewModels.NagCodeModel NagCodeModel;

    /// <summary>
    /// Drag and Drop:
    /// Called for every mouse move on the ListSnippets
    /// If the mouse moves outside the MainWindow, start the drag
    /// </summary>
    public void MouseMove(object sender, MouseEventArgs e, ListBox ListSnippets, ViewModels.NagCodeModel nagCodeModel)
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

            object payload = nagCodeModel.SnippetList[ListSnippets.SelectedIndex].Data;

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
    public void SelectionChanged(object sender, SelectionChangedEventArgs e, ListBox ListSnippets, ViewModels.NagCodeModel nagCodeModel)
    {
      if (ListSnippets.SelectedIndex == -1)
      {
        return;
      }
      object payload = nagCodeModel.SnippetList[ListSnippets.SelectedIndex].Data;

      DataObject dataObject = new DataObject();
      dataObject.SetData(payload);

      Clipboard.SetDataObject(dataObject);
    }

    /// <summary>
    /// Drag and Drop:
    /// When Mouse Down on the ListItem(code Snippet) the dragStartPoint and draggingElement created
    /// </summary>
    public void PreviewMouseDown(object sender, MouseButtonEventArgs e, ListBox ListSnippets)
    {
      dragStartPoint = e.GetPosition(ListSnippets);
      draggingElement = sender as FrameworkElement;
    }
  }
}
