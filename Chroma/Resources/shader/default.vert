#version 330 core

in vec3 gpu_VertexPosition;
in vec4 gpu_VertexColor;
in vec2 gpu_TexCoord;
in vec2 gpu_ScreenSize;
in float gpu_Time;

uniform mat4 gpu_ModelViewProjection;

out vec3 cr_VertexPosition;
out vec4 cr_VertexColor;
out vec2 cr_TexCoord;
out vec2 cr_ScreenSize;
out float cr_Time;

void main(void)
{
    cr_VertexPosition = vec3(gpu_VertexPosition);
    cr_VertexColor = vec4(gpu_VertexColor);
    cr_TexCoord = vec2(gpu_TexCoord);
    cr_ScreenSize = vec2(gpu_ScreenSize);
    cr_Time = float(gpu_Time);

    gl_Position = gpu_ModelViewProjection * vec4(
        gpu_VertexPosition,
        1.0
    );
}