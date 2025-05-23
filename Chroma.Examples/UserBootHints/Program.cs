using Chroma.Boot;
using UserBootHints;

BootHints.Set("SDL_RENDER_OPENGL_SHADERS", "1");

using (var game = new GameCore())
    game.Run();