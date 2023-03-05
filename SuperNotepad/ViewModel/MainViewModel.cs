using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SuperNotepad.Extra;
using SuperNotepad.Model.Data;
using SuperNotepad.Model.Services;

namespace SuperNotepad.ViewModel;

public class MainViewModel : ViewModel {
    private readonly FileService _service = new LocalFileService();

    private FileDetails? Details {
        get => _details;
        set {
            if (SetField(ref _details, value)) {
                OnPropertyChanged(nameof(WindowTitle));
                OnPropertyChanged(nameof(Contents));
                OnPropertyChanged(nameof(FilePath));
            }
        }
    }

    private bool _contentsChanged = false;
    private bool _allowedToChange = true;
    private FileDetails? _details;

    protected override void Init() {
        _details = _service.EmptyFile();
    }

    private string? FilePath {
        get => Details?.FileName;
        set {
            if (Details != null)
                Details.FileName = value;
            WindowTitle = value;
            OnPropertyChanged();
        }
    }

    public string? WindowTitle {
        get =>  (_contentsChanged ? "*" : String.Empty) + (string.IsNullOrEmpty(Details?.FileName) ? "Unnamed" : Details?.FileName) + " - Super Notepad";
        set => OnPropertyChanged();
    }

    public string Contents {
        get => Details?.FileContents ?? String.Empty;
        set {
            OnPropertyChanged();
            if (Details == null) return;
            SetTrigger(true);
            Details.FileContents = value;
        }
    }

    public bool AllowedToChange {
        get => _allowedToChange;
        set => SetField(ref _allowedToChange, value);
    }

    public ICommand NewFile => new CommandDelegate(parameter => {
        OfferFileSaving();
        Details = _service.EmptyFile();
        AllowedToChange = true;
        SetTrigger(false);
    });

    public ICommand OpenFile => new CommandDelegate(parameter => {
        OfferFileSaving();

        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = Config.FILE_FILTER;
        if (dialog.ShowDialog() != true)
            return;
        AllowedToChange = true;
        Details = _service.LoadFile(dialog.FileName);
        SetTrigger(false);
    });

    public ICommand SaveFile => new CommandDelegate(parameter => { SaveContents(); });

    public ICommand CloseFile => new CommandDelegate(parameter => {
        OfferFileSaving();
        Details = null;
        Contents = String.Empty;
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
                SetTrigger(false);
            }
            else return;
        }
        if (Details != null)
            _service.SaveFile(Details);
    }

    private void SetTrigger(bool contentsChanged) {
        this._contentsChanged = contentsChanged;
        OnPropertyChanged(nameof(WindowTitle));
    }
}