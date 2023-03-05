namespace SuperNotepad.Model.Data; 

public class FileDetails {
    public string? FileName { get; set; }
    public string FileContents { get; set; }

    public FileDetails(string? fileName) {
        FileName = fileName;
    }

    public override string ToString() {
        return $"{nameof(FileName)}: {FileName}, {nameof(FileContents)}: {FileContents}";
    }
}