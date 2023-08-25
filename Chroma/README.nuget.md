### Chroma Framework
Chroma is a 2D game development framework focused on quick prototyping.

### Changes in release 0.60.1
- Added extension methods overloading `Rumble` and `RumbleTriggers` for `ControllerDriver` allowing use of float values 
  in range 0.0-1.0.
- Updated SDL_gpu to draw pixel-perfect lines and rectangles, thanks to 
  [Avahe Kellenberger](https://github.com/avahe-kellenberger).
- Linux native dependencies will now properly target the built-in SDL library instead of the system one.
- Added `RenderContext` overloads for `RenderTo`, `WithCamera` and `Batch` aiming to reduce closure allocation on call.
  The old overloads are now obsolete and will be removed in release 0.61.

### Usage
Simply reference this package and use the following template to get started:   
`GameCore.cs` 
```csharp
using System.Numerics;
using Chroma;
using Chroma.Graphics;

namespace MyGame
{
  public class GameCore : Game
  {
    public GameCore()
      : base(new(false, false))
    {
    }

    protected override void Draw(RenderContext context)
      => context.DrawString("Hello, world!", new Vector2(16, 16));
  }
}
```

`Program.cs`
```csharp
using System;

namespace MyGame
{
  static class Program
  {
    static void Main(string[] args)
      => new GameCore.Run();
  }
}
```

Examples of how to use Chroma's can be found [in the example repository](https://github.com/Chroma-2D/Chroma/tree/master/Chroma.Examples).

