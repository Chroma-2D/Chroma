#version 110

attribute vec3 gpu_Vertex;
attribute vec2 gpu_TexCoord;
attribute vec4 gpu_Color;
attribute float gpu_Time;

uniform mat4 gpu_ModelViewProjectionMatrix;

varying vec4 _CR_vertexColor;
varying vec2 _CR_texCoord;
varying float _CR_time;

void main(void)
{
    _CR_vertexColor = vec4(gpu_Color);
    _CR_texCoord = vec2(gpu_TexCoord);
    _CR_time = float(gpu_Time);
    
    gl_Position = gpu_ModelViewProjectionMatrix * vec4(gpu_Vertex, 1.0);
}
