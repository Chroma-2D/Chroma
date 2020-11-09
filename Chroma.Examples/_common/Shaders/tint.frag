#version 110

uniform sampler2D display;

varying vec2 _CR_texCoord;
varying vec4 _CR_vertexColor;

uniform vec2 mouseLoc;

void main(void)
{
    vec4 newColor = texture2D(display, _CR_texCoord);
    
    newColor.g = mouseLoc.x;
    newColor.r = mouseLoc.y;
    
    gl_FragColor = newColor;
}