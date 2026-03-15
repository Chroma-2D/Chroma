namespace Chroma.Hardware;

using System.Globalization;
using System.Text;

public readonly record struct ProductInfo(
    ushort VendorId, 
    ushort ProductId)
{
    public override string ToString()
    {
        var sb = new StringBuilder();
            
        sb.Append(VendorId.ToString("X4", CultureInfo.InvariantCulture));
        sb.Append(':');
        sb.Append(ProductId.ToString("X4", CultureInfo.InvariantCulture));

        return sb.ToString();
    }
}