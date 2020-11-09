#version 100

precision highp float;
precision highp int;

attribute vec3 gpu_Vertex;
attribute vec2 gpu_TexCoord;
attribute vec4 gpu_Color;
attribute float gpu_Time;
attribute float gpu_ScreenWidth;
attribute float gpu_ScreenHeight;

uniform mat4 gpu_ModelViewProjectionMatrix;

varying vec4 _CR_vertexColor;
varying vec2 _CR_texCoord;
varying vec2 _CR_screenSize;
varying float _CR_time;

void main(void)
{
    _CR_vertexColor = vec4(gpu_Color);
    _CR_texCoord = vec2(gpu_TexCoord);
    _CR_time = float(gpu_Time);
    _CR_screenSize = vec2(gpu_ScreenWidth, gpu_ScreenHeight);
    
    gl_Position = gpu_ModelViewProjectionMatrix * vec4(gpu_Vertex, 1.0);
}
