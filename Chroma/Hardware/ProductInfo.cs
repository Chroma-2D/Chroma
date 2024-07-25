namespace Chroma.Hardware;

using System.Globalization;
using System.Text;

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
            
        sb.Append(VendorId.ToString("X4", CultureInfo.InvariantCulture));
        sb.Append(':');
        sb.Append(ProductId.ToString("X4", CultureInfo.InvariantCulture));

        return sb.ToString();
    }
}