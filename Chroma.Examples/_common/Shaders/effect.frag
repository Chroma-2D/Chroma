#version 420

vec4 effect(vec4 pixel, vec2 tex_coords, vec2 screen_size, float time) {
    uint x = uint(tex_coords.x * screen_size.x);
    uint y = uint(tex_coords.y * screen_size.y);

    pixel.r *= 1.0 / mod(fract(time) * 5, time);
    pixel.g *= (y/screen_size.y) * mod(fract(time) * 5, y);

    return pixel;
}