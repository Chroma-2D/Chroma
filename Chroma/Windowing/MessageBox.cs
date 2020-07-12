using System;
using System.Collections.Generic;
using System.Linq;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Windowing
{
    public class MessageBox
    {
        private static Log Log { get; } = LogManager.GetForCurrentAssembly();

        public MessageBoxSeverity Severity { get; }
        public List<MessageBoxButton> Buttons { get; }

        public string Title { get; private set; }
        public string Message { get; private set; }

        public MessageBox(MessageBoxSeverity severity)
        {
            Severity = severity;
            Buttons = new List<MessageBoxButton>();

            Title = $"Chroma Framework - {Severity}";
        }

        public MessageBox Titled(string title)
        {
            Title = title;
            return this;
        }

        public MessageBox WithMessage(string message)
        {
            Message = message;
            return this;
        }

        public MessageBox WithButton(int id, string text)
        {
            Buttons.Add(new MessageBoxButton {ID = id, Text = text});
            return this;
        }

        public int Show()
        {
            var buttonData = Buttons.Select(button => new SDL2.SDL_MessageBoxButtonData
            {
                buttonid = button.ID,
                text = button.Text,
                flags = 0
            }).ToArray();

            var msgBoxData = new SDL2.SDL_MessageBoxData
            {
                title = Title,
                message = Message,
                numbuttons = buttonData.Length,
                buttons = buttonData,
                colorScheme = null,
                flags = (SDL2.SDL_MessageBoxFlags)Severity,
                window = IntPtr.Zero
            };

            if (SDL2.SDL_ShowMessageBox(ref msgBoxData, out var result) < 0)
            {
                Log.Error($"Couldn't show message box: {SDL2.SDL_GetError()}");
                return 0x00DEAD00;
            }

            return result;
        }

        public static void Show(MessageBoxSeverity severity, string title, string message)
        {
            if (SDL2.SDL_ShowSimpleMessageBox(
                (SDL2.SDL_MessageBoxFlags)severity,
                title,
                message,
                IntPtr.Zero
            ) < 0)
            {
                Log.Error($"Couldn't show message box: {SDL2.SDL_GetError()}");
            }
        }
    }
}