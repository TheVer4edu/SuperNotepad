using SuperNotepad.Model.Data;

namespace SuperNotepad.Model.Services; 

public abstract class FileService {
    public abstract FileDetails? LoadFile(string fileName);

    public virtual FileDetails? EmptyFile() {
        return new FileDetails(null);
    }
    public abstract void SaveFile(FileDetails fileDetails);
}