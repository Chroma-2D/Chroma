#version 100

precision highp float;
precision mediump int;

uniform sampler2D display;

varying vec4 _CR_vertexColor;
varying vec2 _CR_texCoord;
varying float _CR_time;
varying vec2 _CR_screenSize;

vec4 effect(in vec4 pixel, in vec2 tex_coords, in vec2 screen_size, in float time);

void main(void)
{
    gl_FragColor = effect(texture2D(display, _CR_texCoord), _CR_texCoord, _CR_screenSize, _CR_time);
}
