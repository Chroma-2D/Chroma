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

        private MessageBoxSeverity Severity { get; }
        private List<MessageBoxButton> Buttons { get; }

        private string Title { get; set; }
        private string Message { get; set; }

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

        public MessageBox WithButton(string text, Action<int> action)
        {
            Buttons.Add(
                new MessageBoxButton
                {
                    ID = Buttons.Count, 
                    Text = text,
                    Action = action
                }
            );
            return this;
        }

        public int Show(Window owner = null)
        {
            var buttonData = Buttons.Select(button => new SDL2.SDL_MessageBoxButtonData
            {
                buttonid = button.ID,
                text = button.Text,
                flags = 0
            }).Reverse().ToArray();

            var msgBoxData = new SDL2.SDL_MessageBoxData
            {
                title = Title,
                message = Message,
                numbuttons = buttonData.Length,
                buttons = buttonData,
                colorScheme = null,
                flags = (SDL2.SDL_MessageBoxFlags)Severity,
                window = owner?.Handle ?? IntPtr.Zero
            };

            if (SDL2.SDL_ShowMessageBox(ref msgBoxData, out var result) < 0)
            {
                Log.Error($"Couldn't show message box: {SDL2.SDL_GetError()}");
                return 0x00DEAD00;
            }

            Buttons.First(x => x.ID == result)
                   .Action?.Invoke(result);

            return result;
        }

        public static void Show(MessageBoxSeverity severity, string title, string message, Window owner = null)
        {
            if (SDL2.SDL_ShowSimpleMessageBox(
                (SDL2.SDL_MessageBoxFlags)severity,
                title,
                message,
                owner?.Handle ?? IntPtr.Zero
            ) < 0)
            {
                Log.Error($"Couldn't show message box: {SDL2.SDL_GetError()}");
            }
        }
    }
}