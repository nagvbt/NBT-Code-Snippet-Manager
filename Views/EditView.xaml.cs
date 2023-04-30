namespace NagCode.Views
{
  using System.Windows;


  public partial class EditView : Window
  {
    public ViewModels.EditViewModel EditViewModel => DataContext as ViewModels.EditViewModel;

    public EditView(Models.Snip snippetToEdit)
    {
      InitializeComponent();

      this.EditViewModel.SnippetToEdit = snippetToEdit;
    }

        private void Window_Closed(object sender, System.EventArgs e)
        {

        }
  }
}
