namespace NagCode.Interfaces
{
    using ICSharpCode.AvalonEdit.Document;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Author: NBT
    /// ISnip
    /// </summary>
    public interface ISnip : INotifyPropertyChanged
    {
        string Name { get; }
        string Data { get; }
        bool IsSeperator { get; }
        string ToString();
        Guid GuId { get; }

        [JsonIgnore]
        TextDocument Document { get; }
    }
}
