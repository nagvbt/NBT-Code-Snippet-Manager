using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using NagCode.ViewModels;

namespace NagCode.BL
{
    /// <summary>
    /// Author: NBT
    /// DragDropManager for managing Drag and Drop Operations
    /// </summary>
    public class DragDropManager
    {

        private Point _startPoint = new Point(0, 0);
        private FrameworkElement _draggingElement;
        private NagCodeViewModel _nagCodeModel;
        private ListBox _snipList;
        /// <summary>
        /// Drag and Drop:
        /// Called for every mouse move on the snipList
        /// If the mouse moves outside the MainWindow, start the drag
        /// </summary>
        public void MouseMove(object sender, MouseEventArgs e, ListBox snipList, NagCodeViewModel nagCodeModel)
        {
            _snipList = snipList;
            _nagCodeModel = nagCodeModel;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                FrameworkElement currentElement = sender as FrameworkElement;
                if (_draggingElement != currentElement)
                {
                    return;
                }

                // If the mouse moves outside the MainWindow, start the drag.
                if (!(_startPoint.X == 0) || !(_startPoint.Y == 0))
                {

                    Point currentPoint = e.GetPosition(_snipList);
                    if ((Math.Abs(_startPoint.X - currentPoint.X) > 2) && (Math.Abs(_startPoint.Y - currentPoint.Y) > 2))
                    {
                        DataObject dataObject = SetDataInClipboard();

                        if (dataObject == null)
                        {
                            return;
                        }

                        DragDropEffects dropEffect = DragDrop.DoDragDrop(_draggingElement, dataObject, DragDropEffects.Copy | DragDropEffects.Move);
                        if (dropEffect != DragDropEffects.None)
                        {
                            if (_snipList.SelectedIndex < _snipList.Items.Count - 1)
                            {
                                _snipList.SelectedIndex++;
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// When Mouse Down on the ListItem(code Snip) the dragStartPoint and draggingElement created
        /// </summary>
        public void PreviewMouseDown(object sender, MouseButtonEventArgs e, ListBox snipList)
        {
            _startPoint = e.GetPosition(snipList);
            _draggingElement = sender as FrameworkElement;
        }

        /// <summary>
        /// Called for every selection changed on snipList
        /// </summary>
        public void SelectionChanged(ListBox snipList, NagCodeViewModel nagCodeModel)
        {
            _nagCodeModel = nagCodeModel;
            _snipList = snipList;
            SetDataInClipboard();
        }

        private DataObject SetDataInClipboard()
        {
            if (_snipList.SelectedIndex == -1)
            {
                return null;
            }
            object payload = _nagCodeModel.SnipList[_snipList.SelectedIndex].Data;

            DataObject dataObject = new DataObject();
            dataObject.SetData(payload);

            Clipboard.SetDataObject(dataObject);

            return dataObject;
        }


    }
}
