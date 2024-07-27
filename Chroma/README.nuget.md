### Chroma Framework
Chroma is a 2D game development framework focused on quick prototyping.

### Change log
#### Changes in release 0.65.0
- Updates to some diagnostic strings.
- SDL updated to 2.30.5
- Fixed a bug where garbage collected `DisposableResource`objects
  would be disposed outside of the main thread.
- Dropped support for .NET 6, move to .NET 7.
- Added nullability annotations to the API.

#### Changes in release 0.64.0
- `Game.LoadContent()` callback has been refactored to `Game.Initialize(IContentPipeline)` 
- `Game.Content` property has been removed. It has been replaced by `Game.Initialize(IContentPipeline)`.
- `HookPoint.LoadContent` has been renamed to `HookPoint.Initialize`.
- Certain prefix hook methods may now mutate input callback arguments if used with `ref` modifier.
- Fixed a bug where initialization hook point was never invoked.

#### Changes in release 0.63.2
- Linux natives will now work properly with PulseAudio

#### Changes in release 0.63.1
- Linux natives will now initialize properly.

#### Changes in release 0.63.0 [//! Broken on Linux !//]
- Natives are now built automatically using GitHub Actions.
- MacOS support is now ARM64 only. This might change in the future.
- Updated SDL for all platforms to version 2.30.3
- Updated FreeType for all platforms do version 2.13.2
- Added `FreeTypeException` exception type to classify TTF rendering errors.
- Added error checks for FreeType native call failures.

#### Changes in release 0.62.2
- `Window.SaveScreenshot(Stream)` no longer destroys the stream after it's done taking a screenshot.

#### Changes in release 0.62.1
- Actions returning values can now also use non-reference types.

#### Changes in release 0.62.0
- `Dispatcher` was extended to allow execution of tasks from non-async callers.
- Actions scheduled to run on main thread can now return values.

#### Changes in release 0.61.1
- `Game` base class now implements `IDisposable`.
- `Game` base class now exposes `protected void OnDispose()` that is called 
  at the beginning of `Dispose()` method execution.

#### Changes in release 0.61.0
- Added Vector2 and Vector3 constructors for Camera class.
- Added a convenient way to cut a sub-texture out of a larger texture.
- Removed obsolete overloads for `RenderTo`, `WithCamera` and `Batch` RenderContext calls.

#### Changes in release 0.60.1
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

