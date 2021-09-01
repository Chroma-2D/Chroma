#version 330 core

in vec3 cr_VertexPosition;
in vec4 cr_VertexColor;
in vec2 cr_TexCoord;

uniform vec4 tri_color;

void main(void) {
    gl_FragColor = tri_color;
}
