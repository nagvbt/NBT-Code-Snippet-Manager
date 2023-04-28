namespace NagCode.Models
{
  using CommunityToolkit.Mvvm.ComponentModel;
  using ICSharpCode.AvalonEdit.Document;
  using Interfaces;
  using System;

  public class Seperator : ObservableRecipient, ISnip
  {
    private readonly string _theSeparator = " ---------------------------- ";
    private Guid _uniqueGuid;
    public Seperator()
    {
      _uniqueGuid = Guid.NewGuid();
    }

    public string Data
    {
      get => string.Empty;
    }

    public bool IsSeperator
    {
      get => true;
    }

    public string Label
    {
      get => _theSeparator;
    }

    public Guid UniqueGuid
    {
      get => _uniqueGuid;
    }

    public TextDocument Document
    {
      get => null;
    }
  }
}
