using System;
using System.Diagnostics;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace MyParser.Compression
{
    public class GZipStream : ICompression
    {
        public void Decompress(Stream input, Stream output)
        {
            using (var decompressionStream = new GZipInputStream(input))
            {
                try
                {
                    decompressionStream.CopyTo(output);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    throw;
                }
            }
        }
    }
}