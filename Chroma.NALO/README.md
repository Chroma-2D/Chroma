## Chroma NAtive LOader
This is a project aiming to ease development of libraries targeting Chroma Framework
that also depend on native libraries to do the job. This document attempts to explain
what makes up a NALO-compliant library.

### NALO compliance requirements
1. > All your natives **must be compressed with BZ2**.
   
2. > All your natives must be put in your library as embedded resources.  
   > 
   > This can be circumvented by distributing your library with the natives already extracted and  
   > disabling boot-time checksum verification. This is not recommended for both reliability and
   > security reasons.

3. > The directory structure of embedded resources must follow the following scheme:
   > ```
   > MySuperDuperLibraryProjectDirectory
   >   Binaries
   >     linux_64
   >       libnative_1.bz2
   >       libnative_2.bz2
   >     windows_64
   >       native_1.bz2
   >       native_2.bz2
   >     osx_64
   >       libnative_1.bz2
   >       libnative_2.bz2
   > ```
   > 
   > If a platform directory is missing, your project will simply not load on a given platform.  
   > Whether or not you want to support a platform is up to you.
   > 
   > If you need an example of a valid directory structure consider visiting [Chroma.Natives](https://github.com/Chroma-2D/Chroma/tree/master/Chroma.Natives).

4. > Your library must trigger `NativeLoader.LoadNatives()` from a valid [ModuleInitializer](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.moduleinitializerattribute?view=net-6.0). Any attempts to load natives from methods that are not a module initializer will fail.
