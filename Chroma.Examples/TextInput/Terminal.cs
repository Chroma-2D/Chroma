namespace TextInput
{
    public class Terminal
    {
        private VGA _vga;

        private int _x;
        private int _y;

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
            
            _vga.CursorX = _x;
            _vga.CursorY = _y;
        }
    }
}