namespace Wiki.Models
{
    public class Dictionaries
    {
        public Dictionaries(string dictionaryPath, string dictionaryFileName)
        {
            DictionaryPath = dictionaryPath;
            DictionaryFileName = ReplaceFileName(dictionaryFileName);
            DictionaryString = string.Format("{0}\\{1}", DictionaryPath, DictionaryFileName);
        }

        public string DictionaryPath { get; set; }
        public string DictionaryFileName { get; set; }
        public string DictionaryString { get; set; }

        private string ReplaceFileName()
        {
            return DictionaryFileName
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
        private string ReplaceFileName(string text)
        {
            return text
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
    }
}