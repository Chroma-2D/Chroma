#version 330 core

layout (location = 0) in vec3 gpu_Vertex;
layout (location = 1) in vec2 gpu_TexCoord;
layout (location = 2) in vec4 gpu_Color;
layout (location = 3) in float gpu_Time;

uniform mat4 gpu_ModelViewProjectionMatrix;

out vec3 cr_VertexPosition;
out vec4 cr_VertexColor;
out vec2 cr_TexCoord;
out float cr_Time;

void main(void)
{
    cr_Time = float(gpu_Time);
    cr_VertexPosition = vec3(gpu_Vertex);
    cr_VertexColor = vec4(gpu_Color);
    cr_TexCoord = vec2(gpu_TexCoord);

    gl_Position = gpu_ModelViewProjectionMatrix * vec4(gpu_Vertex, 1.0);
}
