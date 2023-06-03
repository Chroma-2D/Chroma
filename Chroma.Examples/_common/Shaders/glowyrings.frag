#version 330 core

in float cr_Time;
in vec2 cr_ScreenSize;
uniform sampler2D cr_Texture;

uniform vec2 mouse_pos;

uniform float low_fft;
uniform float mid_fft;
uniform float high_fft;

const float M_TAU = 6.28318;

float dot2( in vec2 v ) { return dot(v,v); }

vec3 pal(in float t, in vec3 a, in vec3 b, in vec3 c, in vec3 d) {
    return a + b * cos(M_TAU * (c*t+d));
}

float sdBox( in vec2 p, in vec2 b )
{
    vec2 d = abs(p)-b;
    return length(max(d,0.0)) + min(max(d.x,d.y),0.0);
}

vec2 rotate(vec2 v, float a) {
    float s = sin(a);
    float c = cos(a);
    mat2 m = mat2(c, -s, s, c);
    return m * v;
}

vec4 effect(in vec4 pixel, in vec2 texcoords) {
    vec2 pixelCoords = cr_ScreenSize * texcoords;
    vec2 uv = (pixelCoords * 2.0 - cr_ScreenSize.xy) / cr_ScreenSize.y;
    vec2 mouse_offset = vec2(
        0.4 * (mouse_pos.x + sin(cr_Time / 1.5) / 2),
        0.4 * (mouse_pos.y + cos(cr_Time / 1.5) / 2)
    );
    
//    uv += mouse_offset;
    uv = rotate(uv, cr_Time / 2 + mid_fft / 2.3);
    
    float d = sdBox(uv, vec2(low_fft / 3.));
    
    vec3 outcolor = pal(
        d + low_fft / 2 * high_fft / 3.58,
        vec3(0.5, 0.5, 0.5),
        vec3(0.5, 0.5, 0.5),
        vec3(1.0, 1.0, 1.0),
        vec3(0, 0.33, 0.66)
    );
    
    d = sin(d * 18. + (low_fft * 3.5) + cr_Time * 2) / 8.;
    d = abs(d / 2);
    d = 0.02 / d;

    outcolor *= d;
    return vec4(outcolor, 1.0);
}