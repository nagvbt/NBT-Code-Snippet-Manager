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

    private DragDropManager _dragDropManager = new DragDropManager();
    public NagCodeViewModel NagCodeModel => DataContext as NagCodeViewModel;
    public ClipboardNotification SnippetLogic { get; set; }

    public NagCodeView()
    {
      InitializeComponent();
      SaveWindowLocationInSettings();
      ListenToClipboard();
      TooltipInitialize();
      InitializeApp();
    }

    private static void TooltipInitialize()
    {
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
      ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
      ToolTipService.BetweenShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
    }

    private void ListenToClipboard()
    {
      ClipboardNotification.ClipboardUpdate += ClipboardUpdate;
    }

    private void InitializeApp()
    {
      NagCodeModel.ReadSnipFileUsingFilepathSetting();
    }

    private void SaveWindowLocationInSettings()
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

    private void ApplicationExit(object sender, ExitEventArgs e)
    {
      NagCodeModel.ExitMethod();
    }

    private void LaunchPresentView()
    {
      NagCodeModel.IsInPresentMode = true;
      NagCodeModel.IsClipboardManager = false;

      var presentWindow = new PresentationView();
      presentWindow.Show();
      presentWindow.Closed += PresentWindowClosedAction;
      Hide();
    }

    public CheckBox TopmostCheckbox
    {
      get
      {
        return MyTopmostCheckBox;
      }
    }

    private void ToggleTopmost(bool isTopmost)
    {
      if (NagCodeModel != null)
      {
        NagCodeModel.IsTopmost = isTopmost;
      }
      Topmost = isTopmost;
    }

    private void OpenEditView()
    {
      EditViewLogic editViewLogic = new EditViewLogic(NagCodeModel, SnipList);
      editViewLogic.OpeningRequest(NagCodeModel.SelectedSnip, false);
    }

    private void MoveSnipUpDown(KeyEventArgs e)
    {
      if (NagCodeModel.IsInPresentMode)
      {
        return;
      }
      if (e.Key == Key.Up)
      {
        NagCodeModel.MoveSnippetUpMethod();       
      }
      else if (e.Key == Key.Down)
      {
        NagCodeModel.MoveSnippetDownMethod();       
      }
      e.Handled = true;
    }


    #region view-handlers

    private void NagCodeView_Closed(object sender, EventArgs e)
    {
      NagCodeModel.ExitMethod();
    }

    private void NagCodeView_LocationChanged(object sender, EventArgs e)
    {
      SaveWindowLocationInSettings();
    }

    private void NagCodeView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      SaveWindowLocationInSettings();
    }

    private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    {
      Left += e.HorizontalChange;
      Top += e.VerticalChange;
    }

    private void PresentButton_Click(object sender, RoutedEventArgs e)
    {
      LaunchPresentView();
    }

    private void PresentLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
      LaunchPresentView();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      NagCodeModel.ExitMethod();
    }

    private void CloseLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
      NagCodeModel.ExitMethod();
    }

    private void PresentWindowClosedAction(object sender, EventArgs e)
    {
      Show();
    }

    private void MyTopmostCheckBox_Checked(object sender, RoutedEventArgs e)
    {
      ToggleTopmost(true);
    }

    private void MyTopmostCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      ToggleTopmost(false);
    }

    private void SnipList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      OpenEditView();
    }

    private void SnipList_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      MoveSnipUpDown(e);
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

    private void MenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
      NagCodeModel.OpenMenuMethod();
    }
    #endregion // ui-handlers


  }
}
