#version 130

uniform sampler2D tex;
uniform sampler2D tex2;

in vec4 color;
in vec2 texCoord;

uniform vec2 screenSize;
uniform float scanlineDensity;
uniform float blurDistance;

vec4 blur5(sampler2D image, vec2 uv, vec2 resolution, vec2 direction) {
  vec4 color = vec4(0.0);
  vec2 off1 = vec2(1.3333333333333333) * direction;

  color += texture2D(image, uv) * 0.29411764705882354;
  color += texture2D(image, uv + (off1 / resolution)) * 0.35294117647058826;
  color += texture2D(image, uv - (off1 / resolution)) * 0.35294117647058826;
  return color; 
}

void main()
{
    vec4 finalColor;
    vec2 coords = texCoord.xy;

    /*finalColor = blur5(tex, coords, screenSize, vec2(blurDistance, 0));
    finalColor += blur5(tex, coords, screenSize, vec2(-blurDistance, 0));

    int actualPixelY = int(coords.y * screenSize.y);
    int modulus = int(mod(actualPixelY, scanlineDensity));

    if(modulus == 0 && actualPixelY != 0)
        finalColor /= 1.9f;

    finalColor /= 1.60f;*/

    finalColor = texture2D(tex, coords);
    
    vec4 color = texture2D(tex2, coords);
    if(color.w >= 1)
        finalColor += color;
    
    gl_FragColor = finalColor;
}