namespace MyParser.Library
{
    public static class Defaults
    {
        public static ReturnFieldInfos ReturnFieldInfos = new ReturnFieldInfos
        {
            new ReturnFieldInfo
            {
                ReturnFieldId = "Url",
                ReturnFieldXpathTemplate = @"//a[@href]",
                ReturnFieldResultTemplate = @"{{href}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @".+"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "Email",
                ReturnFieldXpathTemplate = @"/",
                ReturnFieldResultTemplate = @"{{InnerHtml}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @"\b[a-zA-Z0-9]+([\.-][a-zA-Z0-9]+)*@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}\b"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "Phone",
                ReturnFieldXpathTemplate = @"/",
                ReturnFieldResultTemplate = @"{{InnerHtml}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @"\b\(?\d{3}\)?-? *\d{3}-? *-?\d{4}\b"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "PublicationId",
                ReturnFieldXpathTemplate = @"/",
                ReturnFieldResultTemplate = @"{{InnerHtml}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @"\d+"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "PublicationLink",
                ReturnFieldXpathTemplate = @"//a[@href]",
                ReturnFieldResultTemplate = @"{{href}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @".+"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "PublicationDate",
                ReturnFieldXpathTemplate = @"/",
                ReturnFieldResultTemplate = @"{{InnerHtml}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern =
                    @"\d{1,2}(?<dot>[\.\/-])\d{1,2}\k{dot}\d{2}(\d{2})?(\s+\d{1,2}:\d{1,2}(:\d{1,2})?)?"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "OtherPageUrl",
                ReturnFieldXpathTemplate = @"//a[contains(@href,'page')]",
                ReturnFieldResultTemplate = @"{{href}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @".+"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "Option",
                ReturnFieldXpathTemplate = @"//select/option",
                ReturnFieldResultTemplate = @"{{value}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @".+"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "Value",
                ReturnFieldXpathTemplate = @"//select/option",
                ReturnFieldResultTemplate = @"{{InnerText}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexMatchPattern = @".+"
            },
        };
    }
}