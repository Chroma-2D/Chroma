#version 330 core

layout (location = 0) out vec4 _FragColor;

uniform sampler2D cr_Screen;

in vec3 cr_VertexPosition;
in vec4 cr_VertexColor;
in vec2 cr_TexCoord;

void main(void) {
    _FragColor = texture2D(cr_Screen, cr_TexCoord);
}