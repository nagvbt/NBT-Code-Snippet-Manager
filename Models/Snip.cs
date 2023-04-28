namespace NagCode.Models
{
  using CommunityToolkit.Mvvm.ComponentModel;
  using ICSharpCode.AvalonEdit.Document;
  using Interfaces;
  using System;
  using System.Text.RegularExpressions;

  public class Snip : ObservableRecipient, ISnip
  {
    private int _id = 0;
    private string _label;
    private string _data;
    private Guid _uniqueGuid;
    private readonly Regex trimmer = new Regex(@"\s\s+");

    public Snip()
    {
      _uniqueGuid = Guid.NewGuid();
    }

    public int Id
    {
      get => _id;
      set
      {
        _id = value;
        OnPropertyChanged();
      }
    }

    public string Label
    {
      get => _label;
      set
      {
        _label = trimmer.Replace(value, " ").Replace("\r\n", "");
        CallOnPropertyChanged();
      }
    }

    public string Data
    {
      get => _data;
      set
      {
        _data = value;
        CallOnPropertyChanged();
      }
    }

    public TextDocument Document
    {
      get => new TextDocument() { Text = string.Format("#{0}\r\n{1}", Label, Data) };
    }

    public bool IsSeperator
    {
      get => false;
    }

    public Guid UniqueGuid
    {
      get => _uniqueGuid;
    }

    public Snip(int id, String label, String data)
    {
      Label = label;
      Data = data;
      Id = id;
    }

    private void CallOnPropertyChanged()
    {
      OnPropertyChanged();
      OnPropertyChanged(nameof(Document));
    }
  }
}
