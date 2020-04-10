#version 130

uniform sampler2D texture;

in vec4 color;
in vec2 texCoord;

uniform float rotation;

void main()
{
    vec4 pixelColor = texture2D(texture, texCoord);
    pixelColor.x *= texCoord.x;
    pixelColor.y *= texCoord.y;
    pixelColor.z *= texCoord.x + texCoord.y + (rotation / 360);

    gl_FragColor = pixelColor;
}