namespace NagCode.Views
{

  using NagCode.BL;
  using NagCode.ViewModels;
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  public partial class NagCodeView : Window
  {
    public NagCodeModel NagCodeModel => DataContext as NagCodeModel;
    public ClipboardNotification SnippetLogic { get; set; }
    private DragDropManager dragDropManager;


    public NagCodeView()
    {
      InitializeComponent();

      //listen to clipboard
      ClipboardNotification.ClipboardUpdate += ClipboardUpdate;

      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
      ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
      ToolTipService.BetweenShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));

      NagCodeModel.StartApp();
      SaveWindowLocation();
      dragDropManager = new DragDropManager();

    }

    private void SaveWindowLocation()
    {
      Properties.Settings.Default.Top = Top;
      Properties.Settings.Default.Left = Left;
      Properties.Settings.Default.Height = Height;
      Properties.Settings.Default.Width = Width;
      Properties.Settings.Default.Save();
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
      var ewl = new BL.EditViewLogic(NagCodeModel, ListSnippets);
      ewl.OpeningRequest(NagCodeModel.SelectedSnippet, false);
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

    private void ListSnippets_MouseMove(object sender, MouseEventArgs e)
    {
      dragDropManager.MouseMove(sender, e, ListSnippets, NagCodeModel);  
    }

    private void ListSnippets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      dragDropManager.SelectionChanged(sender, e, ListSnippets, NagCodeModel);  
    }

    private void ListSnippets_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      dragDropManager.PreviewMouseDown(sender, e, ListSnippets);
    }

    private void NagCodeViewWindow_Closed(object sender, EventArgs e)
    {
      NagCodeModel.ExitMethod();
    }

    private void NagCodeViewWindow_LocationChanged(object sender, EventArgs e)
    {
      SaveWindowLocation();
    }

    private void NagCodeViewWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      SaveWindowLocation();
    }
  }
}
