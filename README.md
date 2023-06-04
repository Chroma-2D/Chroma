<p align="center"><img src="https://raw.githubusercontent.com/Chroma-2D/Chroma/master/Chroma/Resources/deficon.png"><br>Chroma Framework</p>

**What? Another one?**  
*Yeah!* ***Another one!*** Chroma is a 2D-focused game development framework. Works just as well for other kinds of 2D applications too. Might as well call it a glorified SDL wrapper for .NET 6 if you like.

**Why?**  
Because MonoGame ~~is~~was so slow to add hassle-free shader compilation on Linux that it seemed faster for me to write an entire goddamn game development framework. Thought to myself "might as well learn something along the way...". Besides, I've always wanted to have something like this for fast prototyping.

**Alright. What are its features?**  
Oh I'm glad you asked! Features include, but are not limited to:  
&nbsp; ▐ Cross-platform support - Windows, Linux and macOS!  
&nbsp; ▐ Hassle-free framework bring-up allowing you to prototype fast and without bullshit boilerplate.  
&nbsp; ▐ Supports GLSL shaders on any platform. No shader pre-compilation - plain old .frag and .vert.  
&nbsp; ▐ MIT licensed! Do whatever you want to/with it, I want my name on Chroma, though.  
&nbsp; ▐ Allows you to draw primitive shapes out-of-box. I'm looking at you, MonoGame.  
&nbsp; ▐ Err... Is actively developed? At least until I deem it feature-complete.  
&nbsp; ▐ Generally tries to make the gamedev's life easier rather than harder.  
&nbsp; ▐ Drawing inspiration from some of the best frameworks out there.  
&nbsp; ▐ <egoboost\>Very clever native bootloading system.</egoboost\>  
&nbsp; ▐ FreeType-based TTF and BMFont bitmap font format support.  
&nbsp; ▐ Integrates well with .NET ecosystem more often than not.  
&nbsp; ▐ Easy-to-understand rendering controls.  
&nbsp; ▐ Support for Xbox One/Elite, Nintendo and PlayStation 4 & 5 (trigger haptics) controllers.  
&nbsp; ▐ Flexible audio input and output system.  

**Mobile platform support?**  
Maybe? Until that happens you're free to use [Love2D](https://love2d.org/). I really dig its robustness and ease-of-use.

**Docs?**
[API reference is available](https://chroma-2d.github.io/apiref), but... [I'll do you one better.](https://chroma.vddcore.eu)

**Is the API stable?**
Mostly (meaning 'sometimes a method or two might change its signature'). While the entire thing is ready for people to make stuff with, I consider this software to still be in beta stage.

**You got a roadmap or something?**  
Where we're going we don't need a roadmap.  
Just watch the Issues page. Whenever I get an idea for a feature or someone suggests one I either throw it away or add it to project taskboards.

**Are there any examples?**  
Might surprise you, there are some! For example [this guy](https://github.com/Hacktix) made a CHIP-8 emulator called [CHROMA-8](https://github.com/Hacktix/CHROMA-8). That madlad also made an [audio synthesis library](https://github.com/Hacktix/ChromaSynth) and even a [GameBoy emulator](https://github.com/Hacktix/ChromaBoy) - all utilizing Chroma! Such dedication. Wow. 

Alternatively, you can check commit history for some API usage examples as well as ghetto examples I create on my [personal website](https://vddcore.eu/chroma-docs). There are [some actual demos here as well](https://github.com/Ciastex/Chroma/tree/master/Chroma.Examples)

**How about a quick-start guide?**  
Since 30.06.2020 there's a possibility of using the official project template.  

It can be installed using the following command.
```
dotnet new -i Chroma.GameTemplate
```

Afterwards you can create new Chroma projects using the `dotnet new` template.
```
dotnet new chromagame -n MyGameLongName
dotnet build
dotnet run
```

Alternatively:  
```
dotnet new console -n MyGameLongName
cd MyGameLongName
dotnet add MyGameLongName.csproj package Chroma
dotnet build
```

