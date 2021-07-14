using System.IO;

namespace QckMox
{
    public interface IFileProvider
    {
        string GetContent(string filePath);
    }

    public class FileProvider : IFileProvider
    {
        public string GetContent(string filePath)
        {
            if(!File.Exists(filePath)) { return null; }

            string content = null;
            using(var reader = File.OpenText(filePath))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }
    }
}