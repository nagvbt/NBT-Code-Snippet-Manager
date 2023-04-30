namespace NagCode.Views
{
  using System.Windows;



  public partial class EditView : Window
  {
    public ViewModels.EditViewModel EditViewModel => DataContext as ViewModels.EditViewModel;
    public ViewModels.NagCodeModel _nagCodeModel;
    private bool _isEditing = false;

    /// <summary>
    /// EDIT: Invoked when Edit the Existing Snip
    /// </summary>
    /// <param name="snippetToEdit"></param>
    /// <param name="nagCodeModel"></param>
    public EditView(Models.Snip snippetToEdit, ViewModels.NagCodeModel nagCodeModel)
    {
      InitializeComponent();
      _isEditing = true;
      Title = "Edit Snip";
      txtLabel.Text = snippetToEdit.Label;
      txtData.Text = snippetToEdit.Data;
      _nagCodeModel = nagCodeModel;

      this.EditViewModel.SnippetToEdit = snippetToEdit;
    }

    /// <summary>
    /// ADD: Invoked for Adding New Snip
    /// </summary>
    /// <param name="nagCodeModel"></param>
    public EditView(ViewModels.NagCodeModel nagCodeModel)
    {
      InitializeComponent();
      _isEditing= false;
      Title = "New Snip";
      _nagCodeModel = nagCodeModel;
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {


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
        this.EditViewModel.SnippetToEdit.Label = txtLabel.Text;
        this.EditViewModel.SnippetToEdit.Data= txtData.Text;

        //Notify listbox schanged
       _nagCodeModel.OnPropertyChanged();
      }
      Close();
    }
  }
}
