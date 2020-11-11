#version 330 core

uniform sampler2D cr_Screen;

uniform vec2 mouseLoc = vec2(0);

vec4 effect(vec4 pixel, vec2 tex_coords, float time) {
    vec4 pixel_at_cursor = texture2D(cr_Screen, mouseLoc);
    
    if (pixel.rgb == vec3(1)) {
        return pixel_at_cursor;
    }
    
    return vec4(
        mouseLoc.xy,
        pixel.ba
    );
}
