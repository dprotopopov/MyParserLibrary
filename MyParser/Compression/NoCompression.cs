using System.IO;

namespace MyParser.Compression
{
    public class NoCompression : ICompression
    {
        public void Decompress(Stream input, Stream output)
        {
            input.CopyTo(output);
        }
    }
}