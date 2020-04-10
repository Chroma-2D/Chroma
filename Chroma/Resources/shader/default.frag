﻿uniform sampler2D texture;

in vec4 color;
in vec2 texCoord;

void main(void)
{
    gl_FragColor = texture2D(texture, texCoord);
}