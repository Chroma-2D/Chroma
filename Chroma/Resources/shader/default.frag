#version 120

uniform sampler2D tex;

varying vec4 color;
varying vec2 texCoord;

void main(void)
{
    gl_FragColor = texture2D(tex, texCoord);
}