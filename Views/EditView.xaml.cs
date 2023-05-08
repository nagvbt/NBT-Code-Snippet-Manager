namespace NagCode.Views
{
  using NagCode.Models;
  using NagCode.ViewModels;
  using System.Windows;

  public partial class EditView : Window
  {
    public EditViewModel EditViewModel => DataContext as EditViewModel;
    public NagCodeViewModel _nagCodeModel;
    private bool _isEditing = false;

    /// <summary>
    /// EDIT: Invoked when Edit the Existing Snip
    /// </summary>
    /// <param name="snipToEdit"></param>
    /// <param name="nagCodeModel"></param>
    public EditView(Snip snipToEdit, NagCodeViewModel nagCodeModel)
    {

      InitializeComponent();
      InitEditOrNewView(nagCodeModel, true);
      SetEditViewModelAndTextBox(snipToEdit);
    }

    /// <summary>
    /// ADD: Invoked for Adding New Snip
    /// </summary>
    /// <param name="nagCodeModel"></param>
    public EditView(NagCodeViewModel nagCodeModel)
    {
      InitializeComponent();
      InitEditOrNewView(nagCodeModel, false);
    }

    private void SetEditViewModelAndTextBox(Snip snipToEdit)
    {
      txtName.Text = snipToEdit.Name;
      txtData.Text = snipToEdit.Data;
      EditViewModel.SnipToEdit = snipToEdit;
    }

    private void InitEditOrNewView(NagCodeViewModel nagCodeModel, bool isEditing)
    {
      _isEditing = isEditing;
      _nagCodeModel = nagCodeModel;

      if (isEditing)
      {
        Title = "Edit Snip";
      }
      else
      {
        Title = "New Snip";
      }
    }

    private void EditOrAddSnip()
    {
      if (_isEditing)
      {
        // Edit new snip
        EditViewModel.SnipToEdit.Name = txtName.Text;
        EditViewModel.SnipToEdit.Data = txtData.Text;
      }
      else
      {
        // Add new snip
        _nagCodeModel.Add(txtName.Text, txtData.Text);

      }
    }
    private void ValidateTextBoxes()
    {
      if (!(txtName.Text.Trim().Length > 0) && !(txtData.Text.Trim().Length > 0))
      {
        MessageBox.Show("Name and Data: Cannot be empty", "Error");
        txtName.Focus();
        return;
      }

      if (!(txtName.Text.Trim().Length > 0))
      {
        MessageBox.Show("Name: Cannot be empty", "Error");
        txtName.Focus();
        return;
      }

      if (!(txtData.Text.Trim().Length > 0))
      {
        MessageBox.Show("Data: Cannot be empty", "Error");
        txtData.Focus();
        return;
      }
    }

    #region view-handlers
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
      ValidateTextBoxes();
      EditOrAddSnip();
      Close();
    }
    #endregion

  }
}
