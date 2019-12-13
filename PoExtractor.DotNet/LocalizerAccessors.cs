namespace PoExtractor.DotNet
{
    public static class LocalizerAccessors
    {
        public static readonly string DefaultLocalizerIdentifier = "T";
        public static readonly string VerbatimLocalizerIdentifierRazor = "@T";

        public static readonly string StringLocalizerIdentifier = "S";

        public static readonly string HtmlLocalizerIdentifier = "H";

        public static string[] LocalizerIdentifiers = { DefaultLocalizerIdentifier, VerbatimLocalizerIdentifierRazor, StringLocalizerIdentifier, HtmlLocalizerIdentifier };
    }
}
