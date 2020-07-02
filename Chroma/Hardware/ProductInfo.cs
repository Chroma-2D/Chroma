namespace Chroma.Hardware
{
    public struct ProductInfo
    {
        public ushort VendorId { get; }
        public ushort ProductId { get; }

        public ProductInfo(ushort vid, ushort pid)
        {
            VendorId = vid;
            ProductId = pid;
        }
    }
}