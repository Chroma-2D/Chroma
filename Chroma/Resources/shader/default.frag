#version 110

uniform sampler2D display;

varying vec4 _CR_vertexColor;
varying vec2 _CR_texCoord;
varying float _CR_time;

void main(void)
{
    gl_FragColor = texture2D(display, _CR_texCoord);
}
