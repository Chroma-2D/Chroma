#version 330 core

uniform sampler2D cr_Screen;

in float cr_Time;
in vec2 cr_ScreenSize;

uniform vec2 mouseLoc = vec2(0);

vec4 effect(vec4 pixel, vec2 tex_coords) {
    return vec4(
        pixel.rg * mouseLoc.xy,
        pixel.ba
    );
}