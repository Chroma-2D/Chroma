### Chroma Framework
Chroma is a 2D game development framework focused on quick prototyping.

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

