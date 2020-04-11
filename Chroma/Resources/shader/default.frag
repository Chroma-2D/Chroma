#version 130

uniform sampler2D tex;

in vec4 color;
in vec2 texCoord;

out vec4 fragColor;

void main(void)
{
    fragColor = texture2D(tex, texCoord);
}