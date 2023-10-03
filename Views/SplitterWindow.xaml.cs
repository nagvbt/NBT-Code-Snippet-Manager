using NagCode.BL;
using NagCode.Models;
using NagCode.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NagCode.Views
{
    //public class ExtendedTreeView : TreeView
    //{
    //    public ExtendedTreeView()
    //        : base()
    //    {
    //        this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(___ICH);
    //    }

    //    void ___ICH(object sender, RoutedPropertyChangedEventArgs<object> e)
    //    {
    //        if (SelectedItem != null)
    //        {
    //            SetValue(SelectedItem_Property, SelectedItem);
    //        }
    //    }

    //    public object SelectedItem_
    //    {
    //        get { return (object)GetValue(SelectedItem_Property); }
    //        set { SetValue(SelectedItem_Property, value); }
    //    }
    //    public static readonly DependencyProperty SelectedItem_Property = DependencyProperty.Register("SelectedItem_", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));
    //}

    /// <summary>
    /// Interaction logic for SplitterWindow.xaml
    /// </summary>
    /// 
    public partial class SplitterWindow : Window
    {
        private NagCodeViewModel _nagCodeViewModel => DataContext as NagCodeViewModel;
        private EditViewModel _editViewModel => DataContext as EditViewModel;
        private DragDropManager _dragDropManager = new DragDropManager();
        public SplitterWindow()
        {
            InitializeComponent();
            InitializeApp();
        }

        private void InitializeApp()
        {
            _nagCodeViewModel.ReadSnipFileUsingFilepathSetting();

        }

        //private void SnipList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    // OpenEditView();
        //    SetEditViewModelAndTextBox((Snip)_nagCodeViewModel.SelectedSnip);
        //}

        private void SnipList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MoveSnipUpDown(e);
        }

        private void MoveSnipUpDown(KeyEventArgs e)
        {
            if (_nagCodeViewModel.IsInPresentMode)
            {
                return;
            }
            if (e.Key == Key.Up)
            {
                _nagCodeViewModel.MoveSnippetUpMethod();
            }
            else if (e.Key == Key.Down)
            {
                _nagCodeViewModel.MoveSnippetDownMethod();
            }
            e.Handled = true;
        }
        private void SnipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_nagCodeViewModel != null && _nagCodeViewModel.SelectedSnip != null)
            {
                var x = _nagCodeViewModel.SelectedSnip.GetType();
                if (x.Name != "Seperator")
                {
                    SetEditViewModelAndTextBox((Snip)_nagCodeViewModel.SelectedSnip);
                }
            }
            _dragDropManager.SelectionChanged(SnipList, _nagCodeViewModel);

        }

        private void SnipList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if(_nagCodeViewModel!=null)
            //SetEditViewModelAndTextBox((Snip)_nagCodeViewModel.SelectedSnip);
            _dragDropManager.PreviewMouseDown(sender, e, SnipList);
        }

        private void SnipList_MouseMove(object sender, MouseEventArgs e)
        {
            _dragDropManager.MouseMove(sender, e, SnipList, _nagCodeViewModel);
        }

        private void SetEditViewModelAndTextBox(Snip snipToEdit)
        {
            txtData.Text = snipToEdit.Data;
            //  _editViewModel.SnipToEdit = snipToEdit;
        }


        private void SnipList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // OpenEditView();
            SetEditViewModelAndTextBox((Snip)_nagCodeViewModel.SelectedSnip);

        }
    }
}
