using System.IO;

using DigimonWorld2Tool.Views;

namespace DigimonWorld2Tool.FileEditor
{
    public class BaseFileEditor
    {
        public BaseFileEditor(string filePath)
        {
            BackUpFile(filePath);
        }

        protected void BackUpFile(string filePath)
        {
            if (!File.Exists(string.Concat(filePath, ".bak")))
                File.Copy(filePath, string.Concat(filePath, ".bak"));
            else
                DebugWindow.DebugLogMessages.Add($"backup {filePath}.bak already existed. No new backup was created.");
        }
    }
}
