namespace TextInput;

using System;
using Chroma.Graphics;
using Chroma.Input;

public class Terminal
{
    private VGA _vga;

    private int _x;
    private int _y;

    private string _input;
        
    public Action<string> InputAccepted { get; set; }

    public Terminal(VGA vga)
    {
        _vga = vga;
    }

    public void Reset()
    {
        _x = 0;
        _y = 0;

        _vga.Reset();
    }

    public void WriteLine(string text)
    {
        Write($"{text}\n");
    }

    public void Write(string text)
    {
        for (var i = 0; i < text.Length; i++)
            PutChar(text[i]);
    }

    public void PutChar(char c)
    {
        if (c == '\n')
        {
            _x = 0;
            _y++;
        }
        else if (c == '\r')
        {
            _x = 0;
        }
        else if (c == '\b')
        {
            if ((_x == 0 && _y == 0) || _input.Length == 0)
                return;

            _input = _input[0..(_input.Length - 1)];
            _x--;

            if (_x < 0)
            {
                _x = 0;

                if (_y - 1 >= 0)
                    _y--;
            }

            _vga.SetCharAt(_x, _y, ' ');
        }
        else
        {
            _vga.SetCharAt(_x++, _y, c);
        }

        if (_x >= _vga.TotalCols)
        {
            _x = 0;
            _y++;
        }

        if (_y >= _vga.TotalRows)
        {
            _x = 0;
            _y--;

            _vga.ScrollUp();
        }
    }

    public void Update(float delta)
    {
        _vga.CursorX = _x;
        _vga.CursorY = _y;

        _vga.Update(delta);
    }

    public void Draw(RenderContext context)
    {
        _vga.Draw(context);
    }

    public void AcceptInput()
    {
        InputAccepted?.Invoke(_input);
        _input = string.Empty;
    }

    public void TextInput(TextInputEventArgs e)
    {
        _input += e.Text;
        Write(e.Text);
    }
}