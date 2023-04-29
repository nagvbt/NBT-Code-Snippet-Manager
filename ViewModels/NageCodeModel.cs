namespace NagCode.ViewModels
{
  using CommunityToolkit.Mvvm.ComponentModel;
  using CommunityToolkit.Mvvm.Input;
  using Newtonsoft.Json;
  using System;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.IO;
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;

  public class NageCodeModel : ObservableRecipient
  {
    private ObservableCollection<Interfaces.ISnip> snippetList = new ObservableCollection<Interfaces.ISnip>();
    private int SnippetCounter = 1;
    private Interfaces.ISnip selectedSnippet;
    private ContextMenu _mainMenu;
    private bool _isTopmost = true;

    public NageCodeModel()
    {
      SnippetList = new ObservableCollection<Interfaces.ISnip>();

      InitializeMainMenu();

      OpenMenu = new RelayCommand(OpenMenuMethod);
      InsertNewSnippet = new RelayCommand<bool>(InsertNewSnippetMethod);
      DeleteSnippet = new RelayCommand(DeleteSnippetMethod);
      MoveSnippetUp = new RelayCommand(MoveSnippetUpMethod);
      MoveSnippetDown = new RelayCommand(MoveSnippetDownMethod);
      InsertSeperator = new RelayCommand(InsertSeperatorMethod);
      Exit = new RelayCommand(ExitMethod);
    }

    public RelayCommand Exit { get; set; }
    public RelayCommand InsertSeperator { get; set; }
    public RelayCommand MoveSnippetDown { get; set; }
    public RelayCommand MoveSnippetUp { get; set; }
    public RelayCommand DeleteSnippet { get; set; }
    public RelayCommand<bool> InsertNewSnippet { get; set; }
    public RelayCommand OpenMenu { get; private set; }
    public bool IsInPresentMode { get; set; }
    public bool IsClipboardManager { get; set; }
    public bool IsDirty { get; set; }

    public bool IsTopmost
    {
      get => _isTopmost;
      set
      {
        _isTopmost = value;
        OnPropertyChanged();
      }
    }

    public Interfaces.ISnip SelectedSnippet
    {
      get => selectedSnippet;
      set
      {
        selectedSnippet = value;

        if (selectedSnippet != null)
        {
          Clipboard.SetText(selectedSnippet.Data);
        }

        OnPropertyChanged();
      }
    }

    public ObservableCollection<Interfaces.ISnip> SnippetList
    {
      get => snippetList;
      set => snippetList = value;
    }

    private void OpenMenuMethod()
    {
      _mainMenu.IsOpen = true;
    }

    private void SelectedSnippetChangedEvent(object sender, PropertyChangedEventArgs e)
    {
      Models.Snip modifiedSnippet = (Models.Snip)sender;
      SelectedSnippet = modifiedSnippet;
    }

    internal void SelectSnippet(Interfaces.ISnip snippetListItem)
    {
      SelectedSnippet = snippetListItem;
      SelectedSnippet.PropertyChanged += SelectedSnippetChangedEvent;
    }

    internal bool ItemWithDataExists(string data)
    {
      return SnippetList.Any(x => x.Data == data);
    }

    internal void SwapSnippets(int indexA, int indexB)
    {
      if (indexB >= 0 && SnippetList.Count > indexB)
      {
        // Remember snippet
        var tmpA = SnippetList[indexA];
        var tmpB = SnippetList[indexB];

        // Swap snippet
        SnippetList[indexA] = tmpB;
        SnippetList[indexB] = tmpA;

        FixIds();

        SelectedSnippet = SnippetList[indexB];
      }
    }

    private void FixIds()
    {
      var counter = 1;
      foreach (var item in SnippetList)
      {
        if (!item.IsSeperator)
        {
          ((Models.Snip)item).Id = counter;
          counter++;
        }
        else
        {
          counter = 1; //start from 1 again after seperator
        }
      }

      SnippetCounter = counter;
    }

    internal void DeleteSnippetMethod()
    {
      if (SelectedSnippet != null)
      {
        IsDirty = true;
        SnippetList.Remove(SelectedSnippet);
        FixIds();
        OnPropertyChanged(nameof(SnippetList));
      }
    }

    internal void Add(string label, string data)
    {
      SnippetList.Add(new Models.Snip(SnippetCounter, label, data));
      SnippetCounter++;
      OnPropertyChanged(nameof(SnippetList));
    }

    internal void InsertSeperatorMethod()
    {
      IsDirty = true;

      SnippetList.Add(new Models.Seperator());
      SnippetCounter = 1;
      OnPropertyChanged(nameof(SnippetList));
    }

    internal void Clear()
    {
      SnippetList.Clear();
      OnPropertyChanged(nameof(SnippetList));
    }

    internal Interfaces.ISnip GetItemByListId(int selectedIndex)
    {
      return SnippetList[selectedIndex];
    }

    internal string SerializeList()
    {
      return JsonConvert.SerializeObject(SnippetList, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple });
    }

    internal void DeserializeList(string fileContent)
    {
      SnippetList = JsonConvert.DeserializeObject<ObservableCollection<Interfaces.ISnip>>(fileContent, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
      OnPropertyChanged(nameof(SnippetList));
    }

    /// <summary>
    /// Initializes the menu.
    /// </summary>
    private void InitializeMainMenu()
    {
      _mainMenu = new ContextMenu();
      _mainMenu.Width = 200;

      var mainMenuItemAlwaysOntop = new MenuItem { Header = "Topmost" };
      mainMenuItemAlwaysOntop.IsCheckable = true;
      mainMenuItemAlwaysOntop.IsChecked = true;
      mainMenuItemAlwaysOntop.Click += ItemAlwaysOnTopClick;
      _mainMenu.Items.Add(mainMenuItemAlwaysOntop);

      var mainMenuItemActAsClipboardManager = new MenuItem { Header = "Clipboard Manager" };
      mainMenuItemActAsClipboardManager.IsCheckable = true;
      mainMenuItemActAsClipboardManager.IsChecked = IsClipboardManager;
      mainMenuItemActAsClipboardManager.Click += ItemActAsClipboardManagerClick;
      _mainMenu.Items.Add(mainMenuItemActAsClipboardManager);

      var itemSave = new MenuItem { Header = "Save" };
      itemSave.Click += new RoutedEventHandler(ItemSaveClick);
      _mainMenu.Items.Add(itemSave);

      var itemLoad = new MenuItem { Header = "Load" };
      itemLoad.Click += new RoutedEventHandler(ItemLoadClick);
      _mainMenu.Items.Add(itemLoad);

      var itemNew = new MenuItem { Header = "New" };
      itemNew.Click += new RoutedEventHandler(ItemNewClick);
      _mainMenu.Items.Add(itemNew);

      var itemExit = new MenuItem { Header = "Exit" };
      itemExit.Click += new RoutedEventHandler(ExitItemClick);
      _mainMenu.Items.Add(itemExit);

      _mainMenu.IsOpen = false;
    }

    void ItemSaveClick(object sender, RoutedEventArgs e)
    {
      Export();
    }

    public void Export()
    {
      // Configure save file dialog box
      Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
      saveDialog.FileName = "NewSnip"; // Default file name
      saveDialog.DefaultExt = ".json"; // Default file extension
      saveDialog.Filter = "JSON documents (.json)|*.json"; // Filter files by extension

      // Show save file dialog box
      Nullable<bool> result = saveDialog.ShowDialog();

      // Process save file dialog box results
      if (result == true)
      {
        // Save document
        string filename = saveDialog.FileName;

        //serialize list as JSON
        string jsonToSave = SerializeList();

        TextWriter writer = new StreamWriter(filename);
        writer.Write(jsonToSave);
        writer.Close();

        IsDirty = false;
      }
    }

    void ItemLoadClick(object sender, RoutedEventArgs e)
    {
      // Configure save file dialog box
      Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
      // Default file name
      openDialog.FileName = "NewSnip";
      openDialog.DefaultExt = ".json";
      openDialog.Filter = "JSON documents (.json)|*.json"; // Filter files by extension

      // Show save file dialog box
      Nullable<bool> result = openDialog.ShowDialog();

      // Process load file dialog box results
      if (result == true)
      {
        string filename = openDialog.FileName;
        SaveSnipFilePath(filename);
        ReadSnipFile(filename);
      }
    }

    private static void SaveSnipFilePath(string filename)
    {
      Properties.Settings.Default.snipFilePath = filename;
      Properties.Settings.Default.Save();
    }

    private void ReadSnipFile(string filename)
    {
      using (TextReader reader = new StreamReader(filename))
      {
        var fileContent = reader.ReadToEnd();
        DeserializeList(fileContent);
        SelectedSnippet = null;
      }
    }

    public void StartApp()
    {
      string filename = Properties.Settings.Default.snipFilePath;
      if (filename != "")
      {
        ReadSnipFile(filename);
      }

    }

    private void ExitItemClick(object sender, RoutedEventArgs e)
    {
      ExitMethod();
    }

    public void ExitMethod()
    {
      if (IsDirty)
      {
        Export();
      }

      Environment.Exit(0);
    }

    void ItemNewClick(object sender, RoutedEventArgs e)
    {
      Clear();
    }

    void ItemActAsClipboardManagerClick(object sender, RoutedEventArgs e)
    {
      var item = sender as MenuItem;
      if (item != null)
      {
        if (IsClipboardManager)
        {
          item.IsChecked = false;
          IsClipboardManager = false;
        }
        else
        {
          item.IsChecked = true;
          IsClipboardManager = true;
        }
      }
    }

    void ItemAlwaysOnTopClick(object sender, RoutedEventArgs e)
    {
      var item = sender as MenuItem;

      IsTopmost = !IsTopmost;
      if (item != null) item.IsChecked = IsTopmost;
    }

    private void AddSnippet(string clipboardString)
    {
      Add(clipboardString, clipboardString);
    }

    public void InsertNewSnippetMethod(bool ignoreIfDuplicate = false)
    {
      IsDirty = true;
      string clipbaordData = null;
      try
      {
        clipbaordData = Clipboard.GetText();
      }
      catch (Exception)
      {
        //this failed at least once...
      }

      if (clipbaordData != null)
      {
        var clipboardString = clipbaordData as String;
        if (clipboardString != null)
        {
          if (!ignoreIfDuplicate || !ItemWithDataExists(clipboardString))
          {
            AddSnippet(clipboardString);
          }
        }
      }
    }

    public void MoveSnippetUpMethod()
    {
      IsDirty = true;

      int selectedIndex = SnippetList.IndexOf(SelectedSnippet);
      if (selectedIndex != -1 && selectedIndex != 0)
      {
        SwapSnippets(selectedIndex, selectedIndex - 1);
      }
    }

    public void MoveSnippetDownMethod()
    {
      IsDirty = true;

      int selectedIndex = SnippetList.IndexOf(SelectedSnippet);
      if (SelectedSnippet != null)
      {
        SwapSnippets(selectedIndex, selectedIndex + 1);
      }
    }
  }
}
