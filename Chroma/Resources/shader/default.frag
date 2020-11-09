#version 100

precision highp float;
precision mediump int;

uniform sampler2D display;

varying vec4 _CR_vertexColor;
varying vec2 _CR_texCoord;
varying float _CR_time;
varying vec2  _CR_screenSize;

void main(void)
{
    gl_FragColor = texture2D(display, _CR_texCoord);
}
