namespace Chroma.Graphics;

using System.Text;

public sealed class DisplayDpi
{
    public static readonly DisplayDpi None = new(0, 0, 0); 
            
    public float Diagonal { get; }
    public float Horizontal { get; }
    public float Vertical { get; }

    internal DisplayDpi(float d, float h, float v)
    {
        Diagonal = d;
        Horizontal = h;
        Vertical = v;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append("Diagonal DPI: ");
        sb.Append(Diagonal);
        sb.AppendLine();

        sb.Append("Horizontal DPI: ");
        sb.Append(Horizontal);
        sb.AppendLine();

        sb.Append("Vertical DPI: ");
        sb.Append(Vertical);

        return sb.ToString();
    }
}