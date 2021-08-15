using System.Text;

namespace Chroma.Hardware
{
    public readonly struct ProductInfo
    {
        public ushort VendorId { get; }
        public ushort ProductId { get; }

        public ProductInfo(ushort vid, ushort pid)
        {
            VendorId = vid;
            ProductId = pid;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.Append(VendorId);
            sb.Append(":");
            sb.Append(ProductId);

            return sb.ToString();
        }
    }
}