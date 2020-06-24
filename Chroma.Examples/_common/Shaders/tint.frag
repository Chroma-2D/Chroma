#version 130

in vec4 color;
in vec2 texCoord;

uniform vec2 mouseLoc;
uniform sampler2D tex;

out vec4 outColor;

void main(void)
{
    vec4 newColor = texture2D(tex, texCoord);
    newColor.g = mouseLoc.x;
    newColor.r = mouseLoc.y;
    
    outColor = newColor;
}