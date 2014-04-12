using System.IO;
using System.IO.Compression;

namespace MyParser.Compression
{
    public class DeflateStream : ICompression
    {
        public void Decompress(Stream input, Stream output)
        {
            using (var decompressionStream = new System.IO.Compression.DeflateStream(input, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(output);
            }
        }
    }
}