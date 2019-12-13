using System.Text.RegularExpressions;

namespace PoExtractor.Core
{
    /// <summary>
    /// Represents a location of the localizable file in the solution
    /// </summary>
    public class LocalizableFile<T>
    {
        /// <summary>
        /// Gets or sets the path source file
        /// </summary>
        public string SourceFile { get; }
        public string ProjectFilePath { get; }
        public T Translations { get; set; }

        public LocalizableFile(string sourceFile)
        {
            this.SourceFile = sourceFile;
        }
        
        public LocalizableFile(string sourceFile, string projetFilePath)
        {
            this.SourceFile = sourceFile;
            this.ProjectFilePath = projetFilePath;
        }
        
        /// <summary>
        /// Gets the culture of localizable file
        /// </summary>
        public string GetLanguage()
        {
            return Regex.Match(this.SourceFile, @"(?![\\.])[a-z]{2}-[A-Z]{2}(?!1)").Value;
        }

        /// <summary>
        /// Gets the platform of localizable file if specified
        /// </summary>
        public string Platform()
        {
            var code = Regex.Match(this.SourceFile, @"(?>\/)(\w{3})(?>\/)").Value;
            return code ?? "Default";
        }
    }
}