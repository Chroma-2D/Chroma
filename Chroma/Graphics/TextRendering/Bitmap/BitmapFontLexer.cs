namespace Chroma.Graphics.TextRendering.Bitmap;

internal class BitmapFontLexer
{
    public string CurrentKey { get; private set; } = string.Empty;
    public string CurrentValue { get; private set; } = string.Empty;

    public string Line { get; }

    public char CurrentChar => Line[Position];
    public bool IsEOL => Position >= Line.Length;

    public int Position { get; private set; }

    public BitmapFontLexer(string line)
    {
        Line = line;
        Next();
    }

    public void Next()
    {
        Advance();

        CurrentKey = string.Empty;
        CurrentValue = string.Empty;

        var addingToKey = true;

        while (!IsEOL && CurrentChar != ' ')
        {
            if (CurrentChar == '"')
            {
                Position++;

                CurrentValue = GetString();
                break;
            }
            else if (CurrentChar == '=')
            {
                addingToKey = false;
                Position++;
            }
            else
            {
                if (addingToKey)
                    CurrentKey += CurrentChar;
                else
                    CurrentValue += CurrentChar;

                Position++;
            }
        }
    }

    private string GetString()
    {
        var str = string.Empty;

        while (CurrentChar != '"')
        {
            if (IsEOL)
                throw new BitmapFontException("Unterminated string.");

            if (CurrentChar == '\\')
            {
                Position++;

                if (IsEOL)
                    throw new BitmapFontException("Broken escape sequence.");

                str += CurrentChar;
                Position++;
            }
            else
            {
                str += CurrentChar;
                Position++;
            }
        }

        Position++;
        return str;
    }

    private void Advance()
    {
        while (char.IsWhiteSpace(CurrentChar))
            Position++;
    }
}