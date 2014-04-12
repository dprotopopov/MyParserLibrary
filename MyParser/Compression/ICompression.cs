using System.IO;

namespace MyParser.Compression
{
    public interface ICompression
    {
        void Decompress(Stream input, Stream output);
    }
}