#version 330 core

in float cr_Time;
uniform sampler2D cr_Texture;

uniform float aspect = 1.0;
uniform float distortion = 1.0;
uniform float radius = 1.2;
uniform float alpha = 1.0;
uniform float crop = 1.0;
uniform vec4 crop_color = vec4(0, 0, 0, 0);
uniform float texshift = 0.0;
uniform int mode = 0;
uniform float z_mul = 1.5;

vec2 distort(vec2 p)
{
    float d = length(p);
    float z = sqrt(distortion + d * d * -distortion);
    
    if (mode == 2)
    {
        z *= z_mul;
    }
    
    float r = atan(d, z) / 3.1415926535;
    float phi = atan(p.y, p.x);
    return vec2(r * cos(phi) * (1.0 / aspect) + 0.5, r * sin(phi) + 0.5);
}

vec4 effect(vec4 pixel, vec2 texcoord) {
    vec2 xy = texcoord * 2 - 1.0;
    xy = vec2(xy.x * aspect, xy.y);
    float d = length(xy);

    if (mode == 1 || mode == 2)
    {
        vec4 texel;
        if (d < radius)
        {            
            xy = distort(xy);

            if (mode == 1)
            xy.x += texshift;

            texel = texture(cr_Texture, xy);
            pixel = texel;

            if (mode == 1)
            pixel.a = alpha;
        }
    }

    if (d > crop)
    {
        return crop_color;
    }

    if (mode == 0)
    {
        return vec4(0, 0, 0, 0.4);
    }

    if (mode == 2)
    {
        pixel *= 0.35;
    }

    return pixel;
}