using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SuperNotepad.Extra;

namespace SuperNotepad.ViewModel;

public class MainViewModel : ViewModel {

    private string _contents = String.Empty;
    private string? _windowTitle;
    private string? _filePath;
    private bool _contentsChanged = false;
    private bool _allowedToChange = true;

    protected override void Init() {
        WindowTitle = "Unnamed";
    }

    private string? FilePath {
        get => _filePath;
        set {
            _filePath = value;
            WindowTitle = value;
        }
    }

    public string? WindowTitle {
        get => _windowTitle;
        set => SetField(ref _windowTitle, value + " - SuperNotepad");
    }

    public string Contents {
        get => _contents;
        set {
            SetTrigger(true);
            SetField(ref _contents, value);
        }
    }

    public bool AllowedToChange {
        get => _allowedToChange;
        set => SetField(ref _allowedToChange, value);
    }

    public ICommand NewFile => new CommandDelegate(parameter => {
        OfferFileSaving();
        Contents = String.Empty;
        FilePath = "Unnamed";
        AllowedToChange = true;
        SetTrigger(false);
    });

    public ICommand OpenFile => new CommandDelegate(parameter => {
        OfferFileSaving();

        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = Config.FILE_FILTER;
        if (dialog.ShowDialog() == true) {
            FilePath = dialog.FileName;
        }
        else return;
        AllowedToChange = true;
        Contents = File.ReadAllText(FilePath);
        SetTrigger(false);
    });

    public ICommand SaveFile => new CommandDelegate(parameter => { SaveContents(); });

    public ICommand CloseFile => new CommandDelegate(parameter => {
        OfferFileSaving();
        Contents = String.Empty;
        FilePath = String.Empty;
        AllowedToChange = false;
        SetTrigger(false);
    });

    private void OfferFileSaving() {
        if (!_contentsChanged)
            return;
        MessageBoxResult result =
            MessageBox.Show("Do you want to save changes?", "Confirm action", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.No)
            return;
        SaveContents();
    }

    private void SaveContents() {
        if (string.IsNullOrEmpty(FilePath)) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = Config.FILE_FILTER;

            if (dialog.ShowDialog() == true) {
                FilePath = dialog.FileName;
            }
            else return;
        }

        File.WriteAllText(FilePath, Contents);
        SetTrigger(false);
    }

    private void SetTrigger(bool contentsChanged) {
        this._contentsChanged = contentsChanged;
        WindowTitle = (this._contentsChanged ? "*" : String.Empty)
                      + (string.IsNullOrEmpty(FilePath) ? "Unnamed" : FilePath);
    }
}