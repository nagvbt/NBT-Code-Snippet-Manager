namespace NagCode.Views
{
  using System.Windows;



  public partial class EditView : Window
  {
    public ViewModels.EditViewModel EditViewModel => DataContext as ViewModels.EditViewModel;
    public ViewModels.NagCodeModel _nagCodeModel;

    public EditView(Models.Snip snippetToEdit)
    {
      InitializeComponent();

      this.EditViewModel.SnippetToEdit = snippetToEdit;
    }

    public EditView(ViewModels.NagCodeModel nagCodeModel)
    {
      InitializeComponent();

      _nagCodeModel = nagCodeModel;
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {
      if (_nagCodeModel != null)
      {
        _nagCodeModel.Add(txtLabel.Text, txtData.Text);
        _nagCodeModel.IsDirty= true;
      }

    }
  }
}
