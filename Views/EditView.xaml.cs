namespace NagCode.Views
{
  using System.Windows;

  public partial class EditView : Window
  {
    public ViewModels.EditViewModel EditViewModel => DataContext as ViewModels.EditViewModel;
    public ViewModels.NagCodeViewModel _nagCodeModel;
    private bool _isEditing = false;

    /// <summary>
    /// EDIT: Invoked when Edit the Existing Snip
    /// </summary>
    /// <param name="snipToEdit"></param>
    /// <param name="nagCodeModel"></param>
    public EditView(Models.Snip snipToEdit, ViewModels.NagCodeViewModel nagCodeModel)
    {
      Title = "Edit Snip";
      _isEditing = true;

      InitializeComponent();       
    
      txtLabel.Text = snipToEdit.Name;
      txtData.Text = snipToEdit.Data;
      _nagCodeModel = nagCodeModel;

      this.EditViewModel.SnipToEdit = snipToEdit;
    }

    /// <summary>
    /// ADD: Invoked for Adding New Snip
    /// </summary>
    /// <param name="nagCodeModel"></param>
    public EditView(ViewModels.NagCodeViewModel nagCodeModel)
    {
      InitializeComponent();
      _isEditing= false;
      Title = "New Snip";
      _nagCodeModel = nagCodeModel;
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {
      Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void Submit_Click(object sender, RoutedEventArgs e)
    {
      if (!_isEditing)
      {
        // Add new snip
        _nagCodeModel.Add(txtLabel.Text, txtData.Text);
        _nagCodeModel.IsDirty = true;
      }
      else
      {
        // Edit new snip
        this.EditViewModel.SnipToEdit.Name = txtLabel.Text;
        this.EditViewModel.SnipToEdit.Data= txtData.Text;

        //Notify listbox schanged
       _nagCodeModel.OnPropertyChanged();
      }
      Close();
    }
  }
}
