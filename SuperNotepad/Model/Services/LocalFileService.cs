using System.IO;
using SuperNotepad.Model.Data;

namespace SuperNotepad.Model.Services; 

public class LocalFileService : FileService {
    public override FileDetails? LoadFile(string fileName) {
        if (!File.Exists(fileName))
            return null;
        FileDetails result = new FileDetails(fileName);
        result.FileContents = File.ReadAllText(fileName);
        return result;
    }

    public override void SaveFile(FileDetails fileDetails) {
        File.WriteAllText(fileDetails.FileName, fileDetails.FileContents);
    }
}