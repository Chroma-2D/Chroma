#version 110

vec4 effect(vec4 pixel, vec2 tex_coords, float time) {
    pixel.r *= tex_coords.x * mod(fract(time) * 5.0f, time);
    pixel.g *= tex_coords.y * mod(fract(time) * 5.0f, tex_coords.y);

    return pixel;
}