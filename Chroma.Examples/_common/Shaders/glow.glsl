#version 130

varying vec4 color;
varying vec2 texCoord;

uniform sampler2D tex;

uniform float glow_size = .5;
uniform vec3 glow_colour = vec3(.5f, 0, 0);
uniform float glow_intensity = 1;
uniform float glow_threshold = .5;

void main() {
    pixel = texture2D(tex, texCoord);
    
    if (pixel.a <= glow_threshold) {
        ivec2 size = textureSize(tex, 0);

        float uv_x = texCoord.x * size.x;
        float uv_y = texCoord.y * size.y;

        float sum = 0.0;
        for (int n = 0; n < 9; ++n) {
            uv_y = (texCoord.y * size.y) + (glow_size * float(n - 4.5));
            float h_sum = 0.0;
            h_sum += texelFetch(tex, ivec2(uv_x - (4.0 * glow_size), uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x - (3.0 * glow_size), uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x - (2.0 * glow_size), uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x - glow_size, uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x, uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x + glow_size, uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x + (2.0 * glow_size), uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x + (3.0 * glow_size), uv_y), 0).a;
            h_sum += texelFetch(tex, ivec2(uv_x + (4.0 * glow_size), uv_y), 0).a;
            sum += h_sum / 9.0;
        }

        gl_FragColor = vec4(glow_colour, (sum / 9.0) * glow_intensity);
    }
}