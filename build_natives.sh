#!/bin/bash
case $($(command -v uname) | tr '[:upper:]' '[:lower:]') in
    linux*)
        echo "Building for Linux"
        ./build_natives_linux.sh
        ;;

    darwin*)
        echo "Building for OS X"
        ./build_natives_osx.sh
        ;;

    msys*|mingw*)
        echo "Building for Win64 MinGW"
        ./build_natives_mingw.sh
        ;;

    *)
        echo "Your platform is not supported."
    ;;
esac
