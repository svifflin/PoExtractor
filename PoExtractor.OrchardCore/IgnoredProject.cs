namespace PoExtractor.OrchardCore
{
    public class IgnoredProject
    {
        private const string Core = "Core";
        private const string Root = "Orchard.Web";
        private const string Cms = "Modules\\Orchard.";
        private const string Syscache = "Modules\\SysCache";

        public static string[] ToList() => new[] { Core, Cms, Syscache};
    }
}
