#!/bin/bash
THREADCOUNT=8
CMAKE_GENERATOR="Unix Makefiles"
CMAKE=cmake
CMAKE_FLAGS=-Wno-dev
MAKE=make

 FT_TAG=VER-2-11-0
 HB_TAG=2.9.0
SDL_TAG=release-2.0.16

rm -rf .natives/buildroot
mkdir -p .natives/artifacts && cd .natives
ARTIFACT_PATH=$(pwd)/artifacts

FT_PATH=$(pwd)/freetype
FT_SOPATH=$(pwd)/buildroot/libfreetype.so.6.18.0

HB_PATH=$(pwd)/harfbuzz
HB_ARPATH=$(pwd)/buildroot/libharfbuzz.a

SDL_PATH=$(pwd)/SDL
SDL_SOPATH=$(pwd)/buildroot/libSDL2-2.0.so.0.16.0

SDL_GPU_PATH=$(pwd)/SDL_gpu
SDL_GPU_SOPATH=$(pwd)/buildroot/SDL_gpu/lib/libSDL2_gpu.so

SDL_SOUND_PATH=$(pwd)/SDL_sound
SDL_SOUND_SOPATH=$(pwd)/buildroot/libSDL2_sound.so.2.0.0

SDL_NMIX_PATH=$(pwd)/SDL_nmix
SDL_NMIX_SOPATH=$(pwd)/buildroot/libSDL2_nmix.so.1.1.0

git clone https://github.com/freetype/freetype   $FT_PATH
cd freetype && git checkout $FT_TAG && cd ..

git clone https://github.com/harfbuzz/harfbuzz   $HB_PATH
cd harfbuzz && git checkout $HB_TAG && cd ..

git clone https://github.com/libsdl-org/SDL      $SDL_PATH
cd SDL && git checkout $SDL_TAG && cd ..

git clone https://github.com/Chroma-2D/SDL-gpu   $SDL_GPU_PATH
git clone https://github.com/Chroma-2D/SDL_sound $SDL_SOUND_PATH
git clone https://github.com/Chroma-2D/SDL_nmix  $SDL_NMIX_PATH

mkdir buildroot && cd buildroot
$CMAKE $CMAKE_FLAGS ../harfbuzz/ -G "$CMAKE_GENERATOR"               \
                    -DCMAKE_BUILD_TYPE=Release                       \
                    -DHB_HAVE_FREETYPE=1                             \
                    -DHB_BUILD_SUBSET=0                              \
                    -DFREETYPE_INCLUDE_DIRS=$FT_PATH/include         \
                    && $MAKE -j$THREADCOUNT
mv $HB_ARPATH $ARTIFACT_PATH/libharfbuzz.a && rm -rf *

$CMAKE $CMAKE_FLAGS ../freetype/ -G "$CMAKE_GENERATOR"               \
                    -DCMAKE_BUILD_TYPE=Release                       \
                    -DBUILD_SHARED_LIBS=1                            \
                    -DFT_REQUIRE_HARFBUZZ=1                          \
                    -DHARFBUZZ_INCLUDE_DIRS=$HB_PATH/include         \
                    -DHARFBUZZ_LIBRARY=$ARTIFACT_PATH/libharfbuzz.a  \
                    -DFT_DISABLE_ZLIB=1                              \
                     && $MAKE -j$THREADCOUNT
mv $FT_SOPATH $ARTIFACT_PATH/libfreetype.so && rm -rf *

$CMAKE $CMAKE_FLAGS ../SDL -G "$CMAKE_GENERATOR"                     \
              -DCMAKE_BUILD_TYPE=Release                             \
              && $MAKE -j$THREADCOUNT
mv $SDL_SOPATH $ARTIFACT_PATH/libSDL2.so && rm -rf *

$CMAKE $CMAKE_FLAGS ../SDL_gpu -G "$CMAKE_GENERATOR"                \
                  -DCMAKE_BUILD_TYPE=Release                        \
                  -DDISABLE_OPENGL_1_BASE=1                         \
                  -DDISABLE_OPENGL_1=1                              \
                  -DDISABLE_OPENGL_2=1                              \
                  -DBUILD_STATIC=0                                  \
                  -DBUILD_DEMOS=0                                   \
                  -DSDL2_INCLUDE_DIRS=$SDL_PATH/include             \
                  -DSDL2_INCLUDE_DIR=$SDL_PATH/include              \
                  -DSDL2_LIBRARY=$ARTIFACT_PATH/libSDL2.so          \
                  -DSDL2_LIBRARIES=$ARTIFACT_PATH/libSDL2.so        \
                  && $MAKE -j$THREADCOUNT
mv $SDL_GPU_SOPATH $ARTIFACT_PATH/libSDL2_gpu.so && rm -rf *

$CMAKE $CMAKE_FLAGS ../SDL_sound -G "$CMAKE_GENERATOR"             \
                    -DSDLSOUND_BUILD_STATIC=0                      \
                    -DSDLSOUND_BUILD_SHARED=1                      \
                    -DSDLSOUND_BUILD_TEST=0                        \
                    -DSDL2_INCLUDE_DIRS=$SDL_PATH/include          \
                    -DSDL2_INCLUDE_DIR=$SDL_PATH/include           \
                    -DSDL2_LIBRARY=$ARTIFACT_PATH/libSDL2.so       \
                    -DSDL2_LIBRARIES=$ARTIFACT_PATH/libSDL2.so     \
                    && $MAKE -j$THREADCOUNT
mv $SDL_SOUND_SOPATH $ARTIFACT_PATH/libSDL2_sound.so && rm -rf *

$CMAKE $CMAKE_FLAGS ../SDL_nmix -G "$CMAKE_GENERATOR"                    \
                   -DSDL_SOUND_INCLUDE_DIR=$SDL_SOUND_PATH/src           \
                   -DSDL_SOUND_LIBRARY=$ARTIFACT_PATH/libSDL2_sound.so   \
                   -DSDL_SOUND_LIBRARIES=$ARTIFACT_PATH/libSDL2_sound.so \
                   -DSDL2_INCLUDE_DIR=$SDL_PATH/include                  \
                   -DSDL2_LIBRARY=$ARTIFACT_PATH/libSDL2.so              \
                   -DSDL2_LIBRARIES=$ARTIFACT_PATH/libSDL2.so            \
                   && $MAKE -j$THREADCOUNT
mv $SDL_NMIX_SOPATH $ARTIFACT_PATH/libSDL2_nmix.so && rm -rf *

cd $ARTIFACT_PATH
bzip2 *.so
chmod 644 *.bz2

mv libfreetype.so.bz2 libfreetype.bz2
mv libSDL2.so.bz2 libSDL2.bz2
mv libSDL2_gpu.so.bz2 libSDL2_gpu.bz2
mv libSDL2_sound.so.bz2 libSDL2_sound.bz2
mv libSDL2_nmix.so.bz2 libSDL2_nmix.bz2

echo "Done. Natives:"
find $ARTIFACT_PATH -name "*.bz2"
