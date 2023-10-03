namespace NagCode.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using ICSharpCode.AvalonEdit.Document;
    using Interfaces;
    using System;

    /// <summary>
    /// Author: NBT
    /// Seperator is used to insert separator in between snippets
    /// </summary>
    public class Seperator : ObservableRecipient, ISnip
    {
        private readonly string _theSeparator = "---------------------------------------";
        private Guid _guid;
        public Seperator()
        {
            _guid = Guid.NewGuid();
        }

        public string Data
        {
            get => string.Empty;
        }

        public bool IsSeperator
        {
            get => true;
        }

        public string Name
        {
            get => _theSeparator;
        }

        public Guid GuId
        {
            get => _guid;
        }

        public TextDocument Document
        {
            get => null;
        }
    }
}
