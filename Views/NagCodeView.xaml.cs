namespace NagCode.Views
{

  using NagCode.BL.SnipLogic;
  using NagCode.ViewModels;
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  public partial class NagCodeView : Window
  {
    public NageCodeModel NagCodeModel => DataContext as NageCodeModel;
    public ClipboardNotification SnippetLogic { get; set; }

    // Drag and Drop ---{
    private Point dragStartPoint = new Point(0, 0);
    private FrameworkElement draggingElement;
    // Drag and Drop ---}

    public NagCodeView()
    {
      InitializeComponent();

      //listen to clipboard
      ClipboardNotification.ClipboardUpdate += ClipboardUpdate;

      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
      ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
      ToolTipService.BetweenShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));

      NagCodeModel.StartApp();
    }

    private void ClipboardUpdate(object sender, EventArgs e)
    {
      if (NagCodeModel.IsClipboardManager)
      {
        NagCodeModel.InsertNewSnippetMethod(true);
      }
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);

      Application.Current.Exit += ApplicationExit;
    }

    void ApplicationExit(object sender, ExitEventArgs e)
    {
      NagCodeModel.ExitMethod();
    }

    private void PresentButton_Click(object sender, RoutedEventArgs e)
    {
      NagCodeModel.IsInPresentMode = true;
      NagCodeModel.IsClipboardManager = false;

      var presentWindow = new PresentationView();
      presentWindow.Show();
      presentWindow.Closed += PresentWindowClosedAction;
      Hide();
    }

    private void PresentWindowClosedAction(object sender, EventArgs e)
    {
      Show();
    }

    private void ListSnippets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var ewl = new Models.EditViewLogic(NagCodeModel, ListSnippets);
      ewl.OpeningRequest(NagCodeModel.SelectedSnippet, Left, Top, Width, Height);
    }

    private void ListSnippets_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (!NagCodeModel.IsInPresentMode)
      {
        if (e.Key == Key.Up)
        {
          NagCodeModel.MoveSnippetUpMethod();
          e.Handled = true;
        }
        else if (e.Key == Key.Down)
        {
          NagCodeModel.MoveSnippetDownMethod();
          e.Handled = true;
        }
      }
    }

    public CheckBox TopmostCheckbox
    {
      get
      {
        return MyTopmostCheckBox;
      }
    }
    private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    {
      Left += e.HorizontalChange;
      Top += e.VerticalChange;
    }

    private void MyTopmostCheckBox_Checked(object sender, RoutedEventArgs e)
    {
      NagCodeModel.IsTopmost = true;
      this.Topmost = true;
    }

    private void MyTopmostCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      NagCodeModel.IsTopmost = false;
      this.Topmost = false;
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

            object payload = NagCodeModel.SnippetList[ListSnippets.SelectedIndex].Data;

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
      object payload = NagCodeModel.SnippetList[ListSnippets.SelectedIndex].Data;

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
    private void ListSnippets_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      Size dragSize = new Size(20, 20);
      dragStartPoint = e.GetPosition(ListSnippets);
      draggingElement = sender as FrameworkElement;
    }

    private void NagCodeViewWindow_Closed(object sender, EventArgs e)
    {
      NagCodeModel.ExitMethod();
    }
  }
}
