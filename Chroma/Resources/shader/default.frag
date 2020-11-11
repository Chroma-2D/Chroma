#version 330 core

uniform sampler2D screen;

in float cr_Time;
in vec3 cr_VertexPosition;
in vec4 cr_VertexColor;
in vec2 cr_TexCoord;

void main(void)
{
    gl_FragColor = texture2D(screen, cr_TexCoord);
}