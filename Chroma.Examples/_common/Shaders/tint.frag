#version 330 core

in float cr_Time;
in vec2 cr_ScreenSize;

uniform sampler2D cr_Texture;
uniform sampler2D overlay;

uniform bool show_edge = true;
uniform bool show_overlay = true;
uniform bool show_tweak = true;

uniform vec2 mouse_loc = vec2(0);
uniform vec4 border_color = vec4(0);

vec4 effect(vec4 pixel, vec2 tex_coords) {
  vec4 ret = vec4(pixel);

  if (show_overlay) {    
    vec2 ov_pos = vec2(
      (tex_coords.x + 0.2) + 0.3 * sin(cr_Time),
      (tex_coords.y + 0.2) + 0.4 * -cos(cr_Time)
    );
    
    vec4 ov_px = texture(overlay, ov_pos);
    
    if (ret.rgb == vec3(0)) {
      ret.rg += (ov_px.br / 3) * mouse_loc.yx;
      ret *= (ov_px + 0.2);
    }
  }
    
  if (show_tweak) {
    ret.rg *= mouse_loc.xy;
  }
  
  if (show_edge) {
    if (pixel.a == 0) {
      vec4 left = texture(cr_Texture, vec2(tex_coords.x - 0.01, tex_coords.y));
      vec4 right = texture(cr_Texture, vec2(tex_coords.x + 0.01, tex_coords.y));
      vec4 top = texture(cr_Texture, vec2(tex_coords.x, tex_coords.y - 0.01));
      vec4 bottom = texture(cr_Texture, vec2(tex_coords.x, tex_coords.y + 0.01));
      
      if (left.a > 0 && right.a < 1.0)
        return border_color;
      
      if (right.a > 0 && left.a < 1.0)
        return border_color;
      
      if (bottom.a > 0 && top.a < 1.0)
        return border_color;
      
      if (top.a > 0 && bottom.a < 1.0)
        return border_color;
    }
  }

  return ret;
}
