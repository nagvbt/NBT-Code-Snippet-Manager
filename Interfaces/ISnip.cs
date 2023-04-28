namespace NagCode.Interfaces
{
  using ICSharpCode.AvalonEdit.Document;
  using Newtonsoft.Json;
  using System;
  using System.ComponentModel;

  public interface ISnip : INotifyPropertyChanged
    {
        string Label { get; }
        string Data { get; }
        bool IsSeperator { get; }
        string ToString();

        Guid UniqueGuid { get; }

        [JsonIgnore]
        TextDocument Document { get; }
    }
}
