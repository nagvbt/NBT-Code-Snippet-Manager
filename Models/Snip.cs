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
    private string _name;
    private string _data;
    private Guid _guid;
    private readonly Regex trimmer = new Regex(@"\s\s+");

    public Snip()
    {
      _guid = Guid.NewGuid();
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

    public string Name
    {
      get => _name;
      set
      {
        _name = trimmer.Replace(value, " ").Replace("\r\n", "");
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
      get => new TextDocument() { Text = string.Format("#{0}\r\n{1}", Name, Data) };
    }

    public bool IsSeperator
    {
      get => false;
    }

    public Guid GuId
    {
      get => _guid;
    }

    public Snip(int id, String name, String data)
    {
      Name = name;
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
