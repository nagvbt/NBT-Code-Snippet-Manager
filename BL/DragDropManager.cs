using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using NagCode.ViewModels;

namespace NagCode.BL
{
  public class DragDropManager
    {
 
    private Point _dragStartPoint = new Point(0, 0);
    private FrameworkElement _draggingElement;
    public NagCodeViewModel NagCodeModel;

    /// <summary>
    /// Drag and Drop:
    /// Called for every mouse move on the snipList
    /// If the mouse moves outside the MainWindow, start the drag
    /// </summary>
    public void MouseMove(object sender, MouseEventArgs e, ListBox snipList, NagCodeViewModel nagCodeModel)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        e.Handled = true;
        FrameworkElement currentElement = sender as FrameworkElement;
        if (_draggingElement != currentElement)
        {
          return;
        }

        // If the mouse moves outside the MainWindow, start the drag.
        if (!(_dragStartPoint.X == 0) || !(_dragStartPoint.Y == 0))
        {

          Point currentPoint = e.GetPosition(snipList);
          if ((Math.Abs(_dragStartPoint.X - currentPoint.X) > 2) && (Math.Abs(_dragStartPoint.Y - currentPoint.Y) > 2))
          {

            if (snipList.SelectedIndex == -1)
            {
              return;
            }

            object payload = nagCodeModel.SnipList[snipList.SelectedIndex].Data;

            DataObject dataObject = new DataObject();
            dataObject.SetData(payload);

            Clipboard.SetDataObject(dataObject);

            DragDropEffects dropEffect = DragDrop.DoDragDrop(_draggingElement, dataObject, DragDropEffects.Copy | DragDropEffects.Move);
            if (dropEffect != DragDropEffects.None)
            {
              if (snipList.SelectedIndex < snipList.Items.Count - 1)
              {
                snipList.SelectedIndex++;
              }
            }
          }
        }

      }
    }

    /// <summary>
    /// Drag and Drop:
    /// Called for every selection changed on snipList
    /// </summary>
    public void SelectionChanged(ListBox snipList, NagCodeViewModel nagCodeModel)
    {
      if (snipList.SelectedIndex == -1)
      {
        return;
      }
      object payload = nagCodeModel.SnipList[snipList.SelectedIndex].Data;

      DataObject dataObject = new DataObject();
      dataObject.SetData(payload);

      Clipboard.SetDataObject(dataObject);
    }

    /// <summary>
    /// Drag and Drop:
    /// When Mouse Down on the ListItem(code Snip) the dragStartPoint and draggingElement created
    /// </summary>
    public void PreviewMouseDown(object sender, MouseButtonEventArgs e, ListBox snipList)
    {
      _dragStartPoint = e.GetPosition(snipList);
      _draggingElement = sender as FrameworkElement;
    }
  }
}
