
namespace MyParserLibrary
{
    public static class Defaults
    {
        public static ReturnFieldInfo DefaultUrlInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Url",
            ReturnFieldXpath = @"//a[@href]",
            ReturnFieldResult = @"{{href}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @".+"
        };

        public static ReturnFieldInfo DefaultEmailInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Email",
            ReturnFieldXpath = @"//.",
            ReturnFieldResult = @"{{InnerHtml}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @"\b[a-zAZ0-9]+([\.-][a-zAZ0-9]+)*@([a-zAZ0-9-]+\.)+[a-zA-Z]{2,6}\b"
        };

        public static ReturnFieldInfo DefaultPhoneInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Phone",
            ReturnFieldXpath = @"//.",
            ReturnFieldResult = @"{{InnerHtml}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @"\b\(?\d{3}\)?-? *\d{3}-? *-?\d{4}\b"
        };

        public static ReturnFieldInfo DefaultPublicationIdInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "PublicationId",
            ReturnFieldXpath = @"/",
            ReturnFieldResult = @"{{PublicationId}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @".+"
        };

        public static ReturnFieldInfo DefaultOtherPageUrlInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "OtherPageUrl",
            ReturnFieldXpath = @"//a[contains(@href,'page')]",
            ReturnFieldResult = @"{{PublicationId}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @".+"
        };
        public static ReturnFieldInfo DefaultOptionlInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Option",
            ReturnFieldXpath = @"//select/option",
            ReturnFieldResult = @"{{value}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @".+"
        };
        public static ReturnFieldInfo DefaultValuelInfo = new ReturnFieldInfo
        {
            ReturnFieldId = "Value",
            ReturnFieldXpath = @"//select/option",
            ReturnFieldResult = @"{{InnerText}}",
            ReturnFieldRegexPattern = @".*",
            ReturnFieldRegexReplacement = @"$&",
            ReturnFieldRegexSelect = @".+"
        };
    }
}
