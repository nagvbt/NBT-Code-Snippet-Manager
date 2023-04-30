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
  
  public class NagCodeViewModel : ObservableRecipient
  {
    private ObservableCollection<Interfaces.ISnip> _snipList = new ObservableCollection<Interfaces.ISnip>();
    private int _snipCounter = 1;
    private Interfaces.ISnip _selectedSnip;
    private ContextMenu _mainMenu;
    private bool _isTopmost = true;

    public NagCodeViewModel()
    {
      SnipList = new ObservableCollection<Interfaces.ISnip>();

      InitializeMainMenu();

      OpenMenu = new RelayCommand(OpenMenuMethod);
      InsertNewSnippet = new RelayCommand<bool>(InsertNewSnippetMethod);
      DeleteSnippet = new RelayCommand(DeleteSnippetMethod);
      MoveSnippetUp = new RelayCommand(MoveSnippetUpMethod);
      MoveSnippetDown = new RelayCommand(MoveSnippetDownMethod);
      InsertSeperator = new RelayCommand(InsertSeperatorMethod);
      AddSnip = new RelayCommand(AddSnipMethod);
      Exit = new RelayCommand(ExitMethod);
    }

    public RelayCommand Exit { get; set; }
    public RelayCommand InsertSeperator { get; set; }
    public RelayCommand AddSnip { get; set; }
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

    public Interfaces.ISnip SelectedSnip
    {
      get => _selectedSnip;
      set
      {
        _selectedSnip = value;

        if (_selectedSnip != null)
        {
          Clipboard.SetText(_selectedSnip.Data);
        }

        OnPropertyChanged();
      }
    }

    public ObservableCollection<Interfaces.ISnip> SnipList
    {
      get => _snipList;
      set => _snipList = value;
    }

    private void OpenMenuMethod()
    {
      _mainMenu.IsOpen = true;
    }

    private void SelectedSnippetChangedEvent(object sender, PropertyChangedEventArgs e)
    {
      Models.Snip modifiedSnippet = (Models.Snip)sender;
      SelectedSnip = modifiedSnippet;
    }

    internal void SelectSnippet(Interfaces.ISnip snippetListItem)
    {
      SelectedSnip = snippetListItem;
      SelectedSnip.PropertyChanged += SelectedSnippetChangedEvent;
    }

    internal bool ItemWithDataExists(string data)
    {
      return SnipList.Any(x => x.Data == data);
    }

    internal void SwapSnippets(int indexA, int indexB)
    {
      if (indexB >= 0 && SnipList.Count > indexB)
      {
        // Remember snip
        var tmpA = SnipList[indexA];
        var tmpB = SnipList[indexB];

        // Swap snip
        SnipList[indexA] = tmpB;
        SnipList[indexB] = tmpA;

        FixIds();

        SelectedSnip = SnipList[indexB];
      }
    }

    private void FixIds()
    {
      var counter = 1;
      foreach (var item in SnipList)
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

      _snipCounter = counter;
    }

    internal void DeleteSnippetMethod()
    {
      if (SelectedSnip != null)
      {
        IsDirty = true;
        SnipList.Remove(SelectedSnip);
        FixIds();
        OnPropertyChanged(nameof(SnipList));
      }
    }

    public void OnPropertyChanged()
    {
      OnPropertyChanged(nameof(SnipList));
    }

    internal void Add(string name, string data)
    {
      SnipList.Add(new Models.Snip(_snipCounter, name, data));
      _snipCounter++;
      FixIds();
     OnPropertyChanged(nameof(SnipList));
    }

    internal void InsertSeperatorMethod()
    {
      IsDirty = true;

      SnipList.Add(new Models.Seperator());
      _snipCounter = 1;
      OnPropertyChanged(nameof(SnipList));
    }


    internal void AddSnipMethod()
    {
      var ewl = new BL.EditViewLogic(this);
      ewl.OpeningRequest();
    }

    internal void Clear()
    {
      SnipList.Clear();
      OnPropertyChanged(nameof(SnipList));
    }

    internal Interfaces.ISnip GetItemByListId(int selectedIndex)
    {
      return SnipList[selectedIndex];
    }

    internal string SerializeList()
    {
      return JsonConvert.SerializeObject(SnipList, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple });
    }

    internal void DeserializeList(string fileContent)
    {
      SnipList = JsonConvert.DeserializeObject<ObservableCollection<Interfaces.ISnip>>(fileContent, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
      OnPropertyChanged(nameof(SnipList));
    }

    /// <summary>
    /// Initializes the menu.
    /// </summary>
    private void InitializeMainMenu()
    {
      _mainMenu = new ContextMenu();
      _mainMenu.Width = 200;

      //var mainMenuItemAlwaysOntop = new MenuItem { Header = "Topmost" };
      //mainMenuItemAlwaysOntop.IsCheckable = true;
      //mainMenuItemAlwaysOntop.IsChecked = true;
      //mainMenuItemAlwaysOntop.Click += ItemAlwaysOnTopClick;
      //_mainMenu.Items.Add(mainMenuItemAlwaysOntop);

      var mainMenuItemActAsClipboardManager = new MenuItem { Header = "Clipboard Manager" };
      mainMenuItemActAsClipboardManager.IsCheckable = true;
      mainMenuItemActAsClipboardManager.IsChecked = IsClipboardManager;
      mainMenuItemActAsClipboardManager.Click += ItemActAsClipboardManagerClick;
      _mainMenu.Items.Add(mainMenuItemActAsClipboardManager);

      //var itemSave = new MenuItem { Header = "Save" };
      //itemSave.Click += new RoutedEventHandler(ItemSaveClick);
      //_mainMenu.Items.Add(itemSave);

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

    //void ItemSaveClick(object sender, RoutedEventArgs e)
    //{
    //  Export();
    //}

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

        WriteSnipFile(filename);
        SaveSnipFilePath(filename);
        IsDirty = false;
      }
    }

    private void WriteSnipFile(string filename)
    {
      //serialize list as JSON
      string jsonToSave = SerializeList();

      TextWriter writer = new StreamWriter(filename);
      writer.Write(jsonToSave);
      writer.Close();
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
        SelectedSnip = null;
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

      int selectedIndex = SnipList.IndexOf(SelectedSnip);
      if (selectedIndex != -1 && selectedIndex != 0)
      {
        SwapSnippets(selectedIndex, selectedIndex - 1);
      }
    }

    public void MoveSnippetDownMethod()
    {
      IsDirty = true;

      int selectedIndex = SnipList.IndexOf(SelectedSnip);
      if (SelectedSnip != null)
      {
        SwapSnippets(selectedIndex, selectedIndex + 1);
      }
    }
  }
}
