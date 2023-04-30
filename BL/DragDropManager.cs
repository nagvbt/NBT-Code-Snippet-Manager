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
    public ViewModels.NagCodeViewModel NagCodeModel;

    /// <summary>
    /// Drag and Drop:
    /// Called for every mouse move on the snipList
    /// If the mouse moves outside the MainWindow, start the drag
    /// </summary>
    public void MouseMove(object sender, MouseEventArgs e, ListBox snipList, ViewModels.NagCodeViewModel nagCodeModel)
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

          Point currentPoint = e.GetPosition(snipList);
          if ((Math.Abs(dragStartPoint.X - currentPoint.X) > 2) && (Math.Abs(dragStartPoint.Y - currentPoint.Y) > 2))
          {

            if (snipList.SelectedIndex == -1)
            {
              return;
            }

            object payload = nagCodeModel.SnipList[snipList.SelectedIndex].Data;

            DataObject dataObject = new DataObject();
            dataObject.SetData(payload);

            Clipboard.SetDataObject(dataObject);

            DragDropEffects dropEffect = DragDrop.DoDragDrop(draggingElement, dataObject, DragDropEffects.Copy | DragDropEffects.Move);
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
    public void SelectionChanged(ListBox snipList, ViewModels.NagCodeViewModel nagCodeModel)
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
    /// When Mouse Down on the ListItem(code Snippet) the dragStartPoint and draggingElement created
    /// </summary>
    public void PreviewMouseDown(object sender, MouseButtonEventArgs e, ListBox snipList)
    {
      dragStartPoint = e.GetPosition(snipList);
      draggingElement = sender as FrameworkElement;
    }
  }
}
