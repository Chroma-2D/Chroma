#version 330 core

uniform sampler2D screen;

varying float cr_Time;
varying vec3 cr_VertexPosition;
varying vec4 cr_VertexColor;
varying vec2 cr_TexCoord;

vec4 effect(in vec4 pixel, in vec2 tex_coords, in float time);

void main(void)
{
    gl_FragColor = effect(
        texture2D(screen, cr_TexCoord),
        cr_TexCoord, 
        cr_Time
    );
}