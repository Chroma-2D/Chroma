<p align="center"><img src="https://img.vddcore.eu/AnSrjXY.png"></img><br>Chroma Framework</p>

**What? Another one?**  
*Yeah!* ***Another one!*** Chroma is a 2D-focused game development framework. Works just as well for other kinds of 2D applications too. Might as well call it a glorified SDL wrapper for .NET Core 3.1+ if you like.

**Why?**  
Because MonoGame is so slow to add hassle-free shader compilation on Linux that it seemed faster to me to write an entire goddamn game development framework. Thought to myself "might as well learn something along the way...". Besides, I've always wanted to have something like this for fast prototyping.

**Alright. What are its features?**  
Oh I'm glad you asked! Features include, but are not limited to:  
&nbsp; ▐ Cross-platform support - Windows, Linux and macOS!  
&nbsp; ▐ Hassle-free framework bring-up allowing you to prototype fast and without bullshit boilerplate.  
&nbsp; ▐ Supports GLSL shaders on any platform. No shader pre-compilation - plain old .frag and .vert.  
&nbsp; ▐ MIT licensed! Do whatever you want to/with it, I want my name on Chroma, though.  
&nbsp; ▐ Early feedback sessions suggest no documentation needed to understand the API.  
&nbsp; ▐ Allows you to draw primitive shapes out-of-box. I'm looking at you, MonoGame.  
&nbsp; ▐ Err... Is actively developed? At least until I deem it feature-complete.  
&nbsp; ▐ Generally tries to make the gamedev's life easier rather than harder.  
&nbsp; ▐ Drawing inspiration from some of the best frameworks out there.  
&nbsp; ▐ <egoboost\>Very clever native bootloading system.</egoboost\>  
&nbsp; ▐ FreeType-based TTF and BMFont bitmap font format support.  
&nbsp; ▐ Easy-to-understand rendering controls.  
&nbsp; ▐ Robust Xbox Controller support.  
&nbsp; ▐ Flexible audio system.  

**Mobile platform support?**  
Not even considered at the moment. You're free to use [Love2D](https://love2d.org/) though. I really dig its robustness and ease-of-use.

**You got a roadmap or something?**  
Where we're going we don't need a roadmap.  
Just watch the Issues page. Whenever I get an idea for a feature or someone suggests one I either throw it away or add it to project taskboards.

**Are there any examples?**  
Might surprise you, there are some! For example [this guy](https://github.com/Hacktix) made a CHIP-8 emulator called [CHROMA-8](https://github.com/Hacktix/CHROMA-8). That madlad also made an [audio synthesis library](https://github.com/Hacktix/ChromaSynth) and even a [GameBoy emulator](https://github.com/Hacktix/ChromaBoy) - all utilizing Chroma! Such dedication. Wow. 

Alternatively, you can check commit history for some API usage examples as well as a ghetto examples I create on my [personal blog](https://vddcore.eu/chroma-docs).

**How about a quick-start guide?**  
```
dotnet new console -n MyGameLongName
cd MyGameLongName
dotnet add MyGameLongName.csproj package Chroma -v 0.11.0-alpha
dotnet build
```
For sources [see here](https://vddcore.eu/chroma-docs/chroma-creating-an-empty-project).

