#version 150

uniform sampler2D texture;

in vec2 texCoord;
in vec4 color;

out vec4 pixel;

uniform vec2 rt_dims;
uniform float vx_offset;

float weht[3] = {
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
    if (texCoord.x<(vx_offset-0.01))
    {
        vec2 uv = texCoord.xy;
        tc = texture2D(texture, uvdfg).rgb * weight[0];

        for (int i=1; i<3; i++)
        {
            tc += texture2D(
                texture,
                uv + vec2(offset[i]) / rt_dims.x, 0.0
            ).rgb * weight[i];
            
            tc += texture2D(
                texture, 
                uv - vec2(offset[i]) / rt_dims.x, 0.0
            ).rgb * weight[i];
        }
    }
    else if (texCoord.x>=(vx_offset+0.01))
    {
        tc = texture2D(texture, texCoord.xy).rgb;
    }

    pixel = vec4(tc, 1.0);
}
