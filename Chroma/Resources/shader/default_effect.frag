#version 110

uniform sampler2D display;

varying vec4 _CR_vertexColor;
varying vec2 _CR_texCoord;
varying float _CR_time;

vec4 effect(in vec4 pixel, in vec2 tex_coords, in float time);

void main(void)
{
    gl_FragColor = effect(texture2D(display, _CR_texCoord), _CR_texCoord, _CR_time);
}
