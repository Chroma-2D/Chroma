#version 330 core

uniform sampler2D cr_Screen;

in vec3 cr_VertexPosition;
in vec4 cr_VertexColor;
in vec2 cr_TexCoord;

void main(void) {
    gl_FragColor = texture2D(cr_Screen, cr_TexCoord);
}