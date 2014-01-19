
namespace MyParserLibrary
{
    public static class Defaults
    {
        public static ReturnFieldInfo DefaultUrlInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Url",
            ReturnFieldXpathTemplate = @"//a[@href]",
            ReturnFieldResultTemplate = @"{{href}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @".+"
        };

        public static ReturnFieldInfo DefaultEmailInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Email",
            ReturnFieldXpathTemplate = @"//.",
            ReturnFieldResultTemplate = @"{{InnerHtml}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @"\b[a-zAZ0-9]+([\.-][a-zAZ0-9]+)*@([a-zAZ0-9-]+\.)+[a-zA-Z]{2,6}\b"
        };
    }
}
