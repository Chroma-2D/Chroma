#version 120

varying vec4 color;
varying vec2 texCoord;

uniform vec2 mouseLoc;
uniform sampler2D tex;

void main(void)
{
    vec4 newColor = texture2D(tex, texCoord);
    newColor.g = mouseLoc.x;
    newColor.r = mouseLoc.y;
    
    gl_FragColor = newColor;
}