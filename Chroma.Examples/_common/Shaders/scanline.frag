// https://raw.githubusercontent.com/wessles/GLSL-CRT/master/shader.frag
//

#version 120

uniform float CRT_CURVE_AMNTx; // curve amount on x
uniform float CRT_CURVE_AMNTy; // curve amount on y
#define CRT_CASE_BORDR 0.0125
#define SCAN_LINE_MULT 1250.0

varying vec4 color;
varying vec2 texCoord;

uniform sampler2D tex;

void main() {
    vec2 tc = vec2(texCoord.x, texCoord.y);

    // Distance from the center
    float dx = abs(0.5-tc.x);
    float dy = abs(0.5-tc.y);

    // Square it to smooth the edges
    dx *= dx;
    dy *= dy;

    tc.x -= 0.5;
    tc.x *= 1.0 + (dy * CRT_CURVE_AMNTx);
    tc.x += 0.5;

    tc.y -= 0.5;
    tc.y *= 1.0 + (dx * CRT_CURVE_AMNTy);
    tc.y += 0.5;

    // Get texel, and add in scanline if need be
    vec4 cta = texture2D(tex, vec2(tc.x, tc.y));

    cta.rgb += sin(tc.y * SCAN_LINE_MULT) * 0.02;

    // Cutoff
    if(tc.y > 1.0 || tc.x < 0.0 || tc.x > 1.0 || tc.y < 0.0)
    cta = vec4(0.0);

    // Apply
    gl_FragColor = cta * color;
}
