#version 420

uniform sampler2D display;

in vec2 _CR_texCoord;
in vec4 _CR_vertexColor;

out vec4 pixel;

uniform vec2 rt_dims;
uniform float vx_offset;

float weight[3] = { 
    0.2270270270f, 
    0.3162162162f, 
    0.0702702703f 
};

float offset[3] = {
    0.0f,
    1.3846153846f,
    3.2307692308f
};

void main()
{   
    vec3 tc = vec3(1.0, 0.0, 0.0);
    if (_CR_texCoord.x<(vx_offset-0.01))
    {
        vec2 uv = _CR_texCoord.xy;
        tc = texture2D(display, uv).rgb * weight[0];

        for (int i=1; i<3; i++)
        {
            tc += texture2D(
                display,
                uv + vec2(offset[i]) / rt_dims.x, 0.0
            ).rgb * weight[i];
            
            tc += texture2D(
                display, 
                uv - vec2(offset[i]) / rt_dims.x, 0.0
            ).rgb * weight[i];
        }
    }
    else if (_CR_texCoord.x>=(vx_offset+0.01))
    {
        tc = texture2D(display, _CR_texCoord.xy).rgb;
    }

    pixel = vec4(tc, 1.0);
}
