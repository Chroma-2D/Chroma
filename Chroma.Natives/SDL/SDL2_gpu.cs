using System;
using System.Runtime.InteropServices;
using Chroma.Natives.Interop;
using static Chroma.Natives.SDL.SDL2;

namespace Chroma.Natives.SDL
{
    public static class SDL_gpu
    {
        #region SDL2# Variables

        private const string NativeLibraryName = "SDL2_gpu";

        #endregion

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int GPU_LogCallback(GPU_LogLevelEnum log_level, string format, IntPtr va_list);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate /*GPU_Renderer* */ IntPtr GPU_CreateRendererCallback(GPU_RendererID request);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GPU_FreeRendererCallback(IntPtr renderer); /* GPU_Renderer* */
        #endregion

        #region Version Identification
        public const int SDL_GPU_VERSION_MAJOR = 0;
        public const int SDL_GPU_VERSION_MINOR = 11;
        public const int SDL_GPU_VERSION_PATCH = 0;
        #endregion

        #region Enums
        public enum GPU_RendererEnum : uint
        {
            GPU_RENDERER_UNKNOWN = 0,
            GPU_RENDERER_OPENGL_1_BASE = 1,
            GPU_RENDERER_OPENGL_1 = 2,
            GPU_RENDERER_OPENGL_2 = 3,
            GPU_RENDERER_OPENGL_3 = 4,
            GPU_RENDERER_OPENGL_4 = 5,
            GPU_RENDERER_GLES_1 = 11,
            GPU_RENDERER_GLES_2 = 12,
            GPU_RENDERER_GLES_3 = 13,
            GPU_RENDERER_D3D9 = 21,
            GPU_RENDERER_D3D10 = 22,
            GPU_RENDERER_D3D11 = 23
        }

        public enum GPU_ComparisonEnum
        {
            GPU_NEVER = 0x0200,
            GPU_LESS = 0x0201,
            GPU_EQUAL = 0x0202,
            GPU_LEQUAL = 0x0203,
            GPU_GREATER = 0x0204,
            GPU_NOTEQUAL = 0x0205,
            GPU_GEQUAL = 0x0206,
            GPU_ALWAYS = 0x0207
        }

        public enum GPU_BlendFuncEnum
        {
            GPU_FUNC_ZERO = 0,
            GPU_FUNC_ONE = 1,
            GPU_FUNC_SRC_COLOR = 0x0300,
            GPU_FUNC_DST_COLOR = 0x0306,
            GPU_FUNC_ONE_MINUS_SRC = 0x0301,
            GPU_FUNC_ONE_MINUS_DST = 0x0307,
            GPU_FUNC_SRC_ALPHA = 0x0302,
            GPU_FUNC_DST_ALPHA = 0x0304,
            GPU_FUNC_ONE_MINUS_SRC_ALPHA = 0x0303,
            GPU_FUNC_ONE_MINUS_DST_ALPHA = 0x0305
        }

        public enum GPU_BlendEqEnum
        {
            GPU_EQ_ADD = 0x8006,
            GPU_EQ_SUBTRACT = 0x800A,
            GPU_EQ_REVERSE_SUBTRACT = 0x800B
        }

        public enum GPU_BlendPresetEnum
        {
            GPU_BLEND_NORMAL = 0,
            GPU_BLEND_PREMULTIPLIED_ALPHA = 1,
            GPU_BLEND_MULTIPLY = 2,
            GPU_BLEND_ADD = 3,
            GPU_BLEND_SUBTRACT = 4,
            GPU_BLEND_MOD_ALPHA = 5,
            GPU_BLEND_SET_ALPHA = 6,
            GPU_BLEND_SET = 7,
            GPU_BLEND_NORMAL_KEEP_ALPHA = 8,
            GPU_BLEND_NORMAL_ADD_ALPHA = 9,
            GPU_BLEND_NORMAL_FACTOR_ALPHA = 10
        }

        public enum GPU_FilterEnum
        {
            GPU_FILTER_NEAREST = 0,
            GPU_FILTER_LINEAR = 1,
            GPU_FILTER_LINEAR_MIPMAP = 2
        }

        public enum GPU_SnapEnum
        {
            GPU_SNAP_NONE = 0,
            GPU_SNAP_POSITION = 1,
            GPU_SNAP_DIMENSIONS = 2,
            GPU_SNAP_POSITION_AND_DIMENSIONS = 3
        }

        public enum GPU_WrapEnum
        {
            GPU_WRAP_NONE = 0,
            GPU_WRAP_REPEAT = 1,
            GPU_WRAP_MIRRORED = 2
        }

        public enum GPU_FormatEnum
        {
            GPU_FORMAT_LUMINANCE = 1,
            GPU_FORMAT_LUMINANCE_ALPHA = 2,
            GPU_FORMAT_RGB = 3,
            GPU_FORMAT_RGBA = 4,
            GPU_FORMAT_ALPHA = 5,
            GPU_FORMAT_RG = 6,
            GPU_FORMAT_YCbCr422 = 7,
            GPU_FORMAT_YCbCr420P = 8,
            GPU_FORMAT_BGR = 9,
            GPU_FORMAT_BGRA = 10,
            GPU_FORMAT_ABGR = 11
        }

        public enum GPU_FileFormatEnum
        {
            GPU_FILE_AUTO = 0,
            GPU_FILE_PNG,
            GPU_FILE_BMP,
            GPU_FILE_TGA
        }

        [Flags]
        public enum GPU_FeatureEnum : uint
        {
            GPU_FEATURE_NON_POWER_OF_TWO = 0x1,
            GPU_FEATURE_RENDER_TARGETS = 0x2,
            GPU_FEATURE_BLEND_EQUATIONS = 0x4,
            GPU_FEATURE_BLEND_FUNC_SEPARATE = 0x8,
            GPU_FEATURE_BLEND_EQUATIONS_SEPARATE = 0x10,
            GPU_FEATURE_GL_BGR = 0x20,
            GPU_FEATURE_GL_BGRA = 0x40,
            GPU_FEATURE_GL_ABGR = 0x80,
            GPU_FEATURE_VERTEX_SHADER = 0x100,
            GPU_FEATURE_FRAGMENT_SHADER = 0x200,
            GPU_FEATURE_PIXEL_SHADER = 0x200,
            GPU_FEATURE_GEOMETRY_SHADER = 0x400,
            GPU_FEATURE_WRAP_REPEAT_MIRRORED = 0x800,
            GPU_FEATURE_CORE_FRAMEBUFFER_OBJECTS = 0x1000,

            GPU_FEATURE_ALL_BASE = GPU_FEATURE_RENDER_TARGETS,
            GPU_FEATURE_ALL_BLEND_PRESETS = (GPU_FEATURE_BLEND_EQUATIONS | GPU_FEATURE_BLEND_FUNC_SEPARATE),
            GPU_FEATURE_ALL_GL_FORMATS = (GPU_FEATURE_GL_BGR | GPU_FEATURE_GL_BGRA | GPU_FEATURE_GL_ABGR),
            GPU_FEATURE_BASIC_SHADERS = (GPU_FEATURE_FRAGMENT_SHADER | GPU_FEATURE_VERTEX_SHADER),
            GPU_FEATURE_ALL_SHADERS = (GPU_FEATURE_FRAGMENT_SHADER | GPU_FEATURE_VERTEX_SHADER | GPU_FEATURE_GEOMETRY_SHADER)
        }

        [Flags]
        public enum GPU_InitFlagEnum : uint
        {
            GPU_INIT_DEFAULT = 0x0,
            GPU_INIT_ENABLE_VSYNC = 0x1,
            GPU_INIT_DISABLE_VSYNC = 0x2,
            GPU_INIT_DISABLE_DOUBLE_BUFFER = 0x4,
            GPU_INIT_DISABLE_AUTO_VIRTUAL_RESOLUTION = 0x8,
            GPU_INIT_REQUEST_COMPATIBILITY_PROFILE = 0x10,
            GPU_INIT_USE_ROW_BY_ROW_TEXTURE_UPLOAD_FALLBACK = 0x20,
            GPU_INIT_USE_COPY_TEXTURE_UPLOAD_FALLBACK = 0x40
        }

        public enum GPU_PrimitiveEnum
        {
            GPU_POINTS = 0x0,
            GPU_LINES = 0x1,
            GPU_LINE_LOOP = 0x2,
            GPU_LINE_STRIP = 0x3,
            GPU_TRIANGLES = 0x4,
            GPU_TRIANGLE_STRIP = 0x5,
            GPU_TRIANGLE_FAN = 0x6
        }

        [Flags]
        public enum GPU_BatchFlagEnum : uint
        {
            GPU_BATCH_XY = 0x1,
            GPU_BATCH_XYZ = 0x2,
            GPU_BATCH_ST = 0x4,
            GPU_BATCH_RGB = 0x8,
            GPU_BATCH_RGBA = 0x10,
            GPU_BATCH_RGB8 = 0x20,
            GPU_BATCH_RGBA8 = 0x40,

            GPU_BATCH_XY_ST = (GPU_BATCH_XY | GPU_BATCH_ST),
            GPU_BATCH_XYZ_ST = (GPU_BATCH_XYZ | GPU_BATCH_ST),
            GPU_BATCH_XY_RGB = (GPU_BATCH_XY | GPU_BATCH_RGB),
            GPU_BATCH_XYZ_RGB = (GPU_BATCH_XYZ | GPU_BATCH_RGB),
            GPU_BATCH_XY_RGBA = (GPU_BATCH_XY | GPU_BATCH_RGBA),
            GPU_BATCH_XYZ_RGBA = (GPU_BATCH_XYZ | GPU_BATCH_RGBA),
            GPU_BATCH_XY_ST_RGBA = (GPU_BATCH_XY | GPU_BATCH_ST | GPU_BATCH_RGBA),
            GPU_BATCH_XYZ_ST_RGBA = (GPU_BATCH_XYZ | GPU_BATCH_ST | GPU_BATCH_RGBA),
            GPU_BATCH_XY_RGB8 = (GPU_BATCH_XY | GPU_BATCH_RGB8),
            GPU_BATCH_XYZ_RGB8 = (GPU_BATCH_XYZ | GPU_BATCH_RGB8),
            GPU_BATCH_XY_RGBA8 = (GPU_BATCH_XY | GPU_BATCH_RGBA8),
            GPU_BATCH_XYZ_RGBA8 = (GPU_BATCH_XYZ | GPU_BATCH_RGBA8),
            GPU_BATCH_XY_ST_RGBA8 = (GPU_BATCH_XY | GPU_BATCH_ST | GPU_BATCH_RGBA8),
            GPU_BATCH_XYZ_ST_RGBA8 = (GPU_BATCH_XYZ | GPU_BATCH_ST | GPU_BATCH_RGBA8)
        }

        public enum GPU_FlipEnum
        {
            GPU_FLIP_NONE = 0x0,
            GPU_FLIP_HORIZONTAL = 0x1,
            GPU_FLIP_VERTICAL = 0x2
        }

        public enum GPU_TypeEnum
        {
            GPU_TYPE_BYTE = 0x1400,
            GPU_TYPE_UNSIGNED_BYTE = 0x1401,
            GPU_TYPE_SHORT = 0x1402,
            GPU_TYPE_UNSIGNED_SHORT = 0x1403,
            GPU_TYPE_INT = 0x1404,
            GPU_TYPE_UNSIGNED_INT = 0x1405,
            GPU_TYPE_FLOAT = 0x1406,
            GPU_TYPE_DOUBLE = 0x140A
        }

        public enum GPU_ShaderEnum
        {
            GPU_VERTEX_SHADER = 0,
            GPU_FRAGMENT_SHADER = 1,
            GPU_PIXEL_SHADER = 1,
            GPU_GEOMETRY_SHADER = 2
        }

        public enum GPU_ShaderLanguageEnum
        {
            GPU_LANGUAGE_NONE = 0,
            GPU_LANGUAGE_ARB_ASSEMBLY = 1,
            GPU_LANGUAGE_GLSL = 2,
            GPU_LANGUAGE_GLSLES = 3,
            GPU_LANGUAGE_HLSL = 4,
            GPU_LANGUAGE_CG = 5
        }

        public enum GPU_ErrorEnum
        {
            GPU_ERROR_NONE = 0,
            GPU_ERROR_BACKEND_ERROR = 1,
            GPU_ERROR_DATA_ERROR = 2,
            GPU_ERROR_USER_ERROR = 3,
            GPU_ERROR_UNSUPPORTED_FUNCTION = 4,
            GPU_ERROR_NULL_ARGUMENT = 5,
            GPU_ERROR_FILE_NOT_FOUND = 6
        }

        public enum GPU_DebugLevelEnum
        {
            GPU_DEBUG_LEVEL_0 = 0,
            GPU_DEBUG_LEVEL_1 = 1,
            GPU_DEBUG_LEVEL_2 = 2,
            GPU_DEBUG_LEVEL_3 = 3,
            GPU_DEBUG_LEVEL_MAX = 3
        }

        public enum GPU_LogLevelEnum
        {
            GPU_LOG_INFO = 0,
            GPU_LOG_WARNING,
            GPU_LOG_ERROR
        }
        #endregion

        #region Structs
        public struct GPU_Rect
        {
            public float x, y;
            public float w, h;
        }

        public struct GPU_RendererID
        {
            public AnsiString name;
            public GPU_RendererEnum renderer;
            public int major_version;
            public int minor_version;
        }

        public struct GPU_BlendMode
        {
            public GPU_BlendFuncEnum source_color;
            public GPU_BlendFuncEnum dest_color;
            public GPU_BlendFuncEnum source_alpha;
            public GPU_BlendFuncEnum dest_alpha;

            public GPU_BlendEqEnum color_equation;
            public GPU_BlendEqEnum alpha_equation;
        }

        public struct GPU_Image
        {
            public IntPtr renderer; /* GPU_Renderer* */
            public IntPtr context_target; /* GPU_Target* */
            public IntPtr target; /* GPU_Target* */
            public ushort w, h;
            public bool using_virtual_resolution;
            public GPU_FormatEnum format;
            public int num_layers;
            public int bytes_per_pixel;
            public ushort base_w, base_h;  // Original image dimensions
            public ushort texture_w, texture_h;  // Underlying texture dimensions
            public bool has_mipmaps;

            public float anchor_x; // Normalized coords for the point at which the image is blitted.  Default is (0.5, 0.5), that is, the image is drawn centered.
            public float anchor_y; // These are interpreted according to GPU_SetCoordinateMode() and range from (0.0 - 1.0) normally.

            public SDL_Color color;
            public bool use_blending;
            public GPU_BlendMode blend_mode;
            public GPU_FilterEnum filter_mode;
            public GPU_SnapEnum snap_mode;
            public GPU_WrapEnum wrap_mode_x;
            public GPU_WrapEnum wrap_mode_y;

            public IntPtr data;
            public int refcount;
            public IntPtr is_alias;
        }

        public struct GPU_Camera
        {
            public float x, y, z;
            public float angle;
            public float zoom_x, zoom_y;
            public float z_near, z_far;  // z clipping planes
            public bool use_centered_origin;  // move rotation/scaling origin to the center of the camera's view
        }

        public struct GPU_ShaderBlock
        {
            // Attributes
            public int position_loc;
            public int texcoord_loc;
            public int color_loc;
            // Uniforms
            public int modelViewProjection_loc;
        }

        public struct GPU_MatrixStack
        {
            public uint storage_size;
            public uint size;
            public IntPtr matrix; /* float** */
        }

        public struct GPU_Context
        {
            /* SDL_GLcontext*/
            public IntPtr context;
            public bool failed;

            /*! SDL window ID */
            public uint windowID;

            /*! Actual window dimensions */
            public int window_w;
            public int window_h;

            /*! Drawable region dimensions */
            public int drawable_w;
            public int drawable_h;

            /*! Window dimensions for restoring windowed mode after GPU_SetFullscreen(1,1). */
            public int stored_window_w;
            public int stored_window_h;

            /*! Last target used */
            public IntPtr active_target;

            /*! Internal state */
            public uint current_shader_program;
            public uint default_textured_shader_program;
            public uint default_untextured_shader_program;

            public GPU_ShaderBlock current_shader_block;
            public GPU_ShaderBlock default_textured_shader_block;
            public GPU_ShaderBlock default_untextured_shader_block;

            public bool shapes_use_blending;
            public GPU_BlendMode shapes_blend_mode;
            public float line_thickness;
            public bool use_texturing;

            public int refcount;
            public IntPtr data;
        }

        public struct GPU_Target
        {
            public IntPtr renderer; /* GPU_Renderer* */
            public IntPtr context_target; /* GPU_Target* */
            public IntPtr image; /* GPU_Image* */
            public IntPtr data; /* void* */
            public ushort w, h;
            public bool using_virtual_resolution;
            public ushort base_w, base_h;  // The true dimensions of the underlying image or window
            public bool use_clip_rect;
            public GPU_Rect clip_rect;
            public bool use_color;
            public SDL_Color color;

            public GPU_Rect viewport;

            /*! Perspective and object viewing transforms. */
            public int matrix_mode;
            public GPU_MatrixStack projection_matrix;
            public GPU_MatrixStack view_matrix;
            public GPU_MatrixStack model_matrix;

            public GPU_Camera camera;
            public bool use_camera;


            public bool use_depth_test;
            public bool use_depth_write;
            public GPU_ComparisonEnum depth_function;

            /*! Renderer context data.  NULL if the target does not represent a window or rendering context. */
            public IntPtr context; /* GPU_Context* */
            public int refcount;
            public bool is_alias;
        }

        public struct GPU_AttributeFormat
        {
            public bool is_per_sprite;  // Per-sprite values are expanded to 4 vertices
            public int num_elems_per_value;
            public GPU_TypeEnum type;  // GPU_TYPE_FLOAT, GPU_TYPE_INT, GPU_TYPE_UNSIGNED_INT, etc.
            public bool normalize;
            public int stride_bytes;  // Number of bytes between two vertex specifications
            public int offset_bytes;  // Number of bytes to skip at the beginning of 'values'
        }

        public struct GPU_Attribute
        {
            public int location;
            public IntPtr values; /* void* */  // Expect 4 values for each sprite
            public GPU_AttributeFormat format;
        }

        public struct GPU_AttributeSource
        {
            public bool enabled;
            public int num_values;
            public IntPtr next_value; /* void* */

            // Automatic storage format
            public int per_vertex_storage_stride_bytes;
            public int per_vertex_storage_offset_bytes;
            public int per_vertex_storage_size;  // Over 0 means that the per-vertex storage has been automatically allocated
            public IntPtr per_vertex_storage; /* void* */  // Could point to the attribute's values or to allocated storage
            public GPU_Attribute attribute;
        }

        public struct GPU_ErrorObject
        {
            public AnsiString function;
            public GPU_ErrorEnum error;
            public AnsiString details;
        }

        public struct GPU_Renderer
        {
            /*! Struct identifier of the renderer. */
            public GPU_RendererID id;
            public GPU_RendererID requested_id;
            public SDL_WindowFlags SDL_init_flags;
            public GPU_InitFlagEnum GPU_init_flags;

            public GPU_ShaderLanguageEnum shader_language;
            public int min_shader_version;
            public int max_shader_version;
            public GPU_FeatureEnum enabled_features;

            /*! Current display target */
            public IntPtr current_context_target; /* GPU_Target* */

            /*! 0 for inverted, 1 for mathematical */
            public bool coordinate_mode;

            /*! Default is (0.5, 0.5) - images draw centered. */
            public float default_image_anchor_x;
            public float default_image_anchor_y;

            public IntPtr impl; /* GPU_RendererImpl* */
        }
        #endregion

        #region Initialization
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_version GPU_GetCompiledVersion();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_version GPU_GetLinkedVersion();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetInitWindow(uint windowID);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GPU_GetInitWindow();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetPreInitFlags(GPU_InitFlagEnum GPU_flags);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_InitFlagEnum GPU_GetPreInitFlags();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetRequiredFeatures(GPU_FeatureEnum features);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_FeatureEnum GPU_GetRequiredFeatures();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetDefaultRendererOrder(
            ref int order_size,
            [Out]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)]
            GPU_RendererID[] order
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetRendererOrder(
            ref int order_size,
            [Out]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)]
            GPU_RendererID[] order
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetRendererOrder(
            ref int order_size,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct)]
            GPU_RendererID[] order
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_Init(ushort w, ushort h, SDL_WindowFlags SDL_flags);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_InitRenderer(GPU_RendererEnum renderer_enum, ushort w, ushort h, SDL_WindowFlags SDL_flags);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_InitRendererByID(GPU_RendererID renderer_request, ushort w, ushort h, SDL_WindowFlags SDL_flags);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_IsFeatureEnabled(GPU_FeatureEnum feature);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_CloseCurrentRenderer();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Quit();
        #endregion

        #region Debugging, logging and error handling
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetDebugLevel(GPU_DebugLevelEnum level);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_DebugLevelEnum GPU_GetDebugLevel();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_LogInfo(string format, __arglist);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_LogWarning(string format, __arglist);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_LogError(string format, __arglist);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetLogCallback(GPU_LogCallback callback);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_PushErrorCode(string function, GPU_ErrorEnum error, string details, __arglist);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_ErrorObject GPU_PopErrorCode();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string GPU_GetErrorString(GPU_ErrorEnum error);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetErrorQueueMax(uint max);
        #endregion

        #region Renderer Setup
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_RendererID GPU_MakeRendererID(string name, GPU_RendererEnum renderer, int major_version, int minor_version);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_RendererID GPU_GetRendererID(GPU_RendererEnum renderer);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GPU_GetNumRegisteredRenderers();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetRegisteredRendererList(
            [Out]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct)]
            GPU_RendererID[] renderers_array
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_RegisterRenderer(GPU_RendererID id, GPU_CreateRendererCallback create_renderer, GPU_FreeRendererCallback free_renderer);
        #endregion

        #region Renderer Controls
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_RendererEnum GPU_ReserveNextRendererEnum();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GPU_GetNumActiveRenderers();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetActiveRendererList(
            [Out]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct)]
            GPU_RendererID[] renderers_array
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Renderer* */ IntPtr GPU_GetCurrentRenderer();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetCurrentRenderer(GPU_RendererID id);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Renderer* */ IntPtr GPU_GetRenderer(GPU_RendererID id);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FreeRenderer(IntPtr renderer);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ResetRendererState();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetCoordinateMode(bool use_math_coords);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_GetCoordinateMode();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetDefaultAnchor(float anchor_x, float anchor_y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetDefaultAnchor(out float anchor_x, out float anchor_y);
        #endregion

        #region Context Controls
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_GetContextTarget();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_GetWindowTarget(uint windowID);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_CreateTargetFromWindow(uint windowID);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_MakeCurrent(ref GPU_Target target, uint windowID);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SetWindowResolution(ushort w, ushort h);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SetFullscreen(bool enable_fullscreen, bool use_desktop_resolution);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_GetFullscreen();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_GetActiveTarget();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SetActiveTarget(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SetShapeBlending(bool enable);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_BlendMode GPU_GetBlendModeFromPreset(GPU_BlendPresetEnum preset);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetShapeBlendFunction(GPU_BlendFuncEnum source_color, GPU_BlendFuncEnum dest_color, GPU_BlendFuncEnum source_alpha, GPU_BlendFuncEnum dest_alpha);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetShapeBlendEquation(GPU_BlendEqEnum color_equation, GPU_BlendEqEnum alpha_equation);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetShapeBlendMode(GPU_BlendPresetEnum mode);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float GPU_SetLineThickness(float thickness);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float GPU_GetLineThickness();
        #endregion

        #region Target Controls
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_CreateAliasTarget(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_LoadTarget(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Target* */ IntPtr GPU_GetTarget(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FreeTarget(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetVirtualResolution(IntPtr target, ushort w, ushort h);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetVirtualResolution(IntPtr target, out ushort w, out ushort h);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetVirtualCoords(IntPtr target, out float x, out float y, float displayX, float displayY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UnsetVirtualResolution(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_Rect GPU_MakeRect(float x, float y, float w, float h);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Color GPU_MakerColor(byte r, byte g, byte b, byte a);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetViewport(IntPtr target, GPU_Rect viewport);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UnsetViewport(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_Camera GPU_GetDefaultCamera();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_Camera GPU_GetCamera(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_Camera GPU_SetCamera(IntPtr target, ref GPU_Camera camera);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_EnableCamera(IntPtr target, bool use_camera);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_IsCameraEnabled(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_AddDepthBuffer(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetDepthTest(IntPtr target, bool enable);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetDepthWrite(IntPtr target, bool enable);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetDepthFunction(IntPtr target, GPU_ComparisonEnum compare_operation);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Color GPU_GetPixel(IntPtr target, short x, short y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_Rect GPU_SetClipRect(IntPtr target, GPU_Rect rect);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_Rect GPU_SetClip(IntPtr target, short x, short y, ushort w, ushort h);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UnsetClip(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_IntersectRect(GPU_Rect A, GPU_Rect B, out GPU_Rect result);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_IntersectClipRect(IntPtr target, GPU_Rect B, out GPU_Rect result);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetTargetColor(IntPtr target, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetTargetRGB(IntPtr target, byte r, byte g, byte b);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetTargetRGBA(IntPtr target, byte r, byte g, byte b, byte a);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UnsetTargetColor(IntPtr target);
        #endregion

        #region Surface Controls
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_LoadSurface(string fileName);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_LoadSurface_RW(IntPtr rwops, bool free_rwops);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SaveSurface(IntPtr surface, string filename, GPU_FileFormatEnum format);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SaveSurface_RW(IntPtr surface, IntPtr rwops, bool free_rwops, GPU_FileFormatEnum format);
        #endregion

        #region Image Controls
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Image* */ IntPtr GPU_CreateImage(ushort w, ushort h, GPU_FormatEnum format);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Image* */ IntPtr GPU_CreateImageUsingTexture(IntPtr handle, bool take_ownership);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Image* */ IntPtr GPU_LoadImage(string fileName);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Image* */ IntPtr GPU_LoadImage_RW(IntPtr rwops, bool free_rwops);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern /* GPU_Image* */ IntPtr GPU_CreateAliasImage(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FreeImage(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetImageVirtualResolution(IntPtr image, ushort w, ushort h);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UnsetImageVirtualResolution(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UpdateImage(IntPtr image, ref GPU_Rect image_rect, IntPtr surface, ref GPU_Rect surface_rect);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UpdateImageBytes(IntPtr image, ref GPU_Rect image_rect, byte[] bytes, int bytes_per_row);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_ReplaceImage(IntPtr image, IntPtr surface, ref GPU_Rect surface_rect);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SaveImage(IntPtr image, string fileName, GPU_FileFormatEnum format);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SaveImage_RW(IntPtr image, IntPtr rwops, bool free_rwops, GPU_FileFormatEnum format);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GenerateMipmaps(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetColor(IntPtr image, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetRGB(IntPtr image, byte r, byte g, byte b);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetRGBA(IntPtr image, byte r, byte g, byte b, byte a);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_UnsetColor(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_GetBlending(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetBlending(IntPtr image, bool enable);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetBlendFunction(IntPtr image, GPU_BlendFuncEnum source_color, GPU_BlendFuncEnum dest_color, GPU_BlendFuncEnum source_alpha, GPU_BlendFuncEnum dest_alpha);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetBlendEquation(IntPtr image, GPU_BlendEqEnum color_equation, GPU_BlendEqEnum alpha_equation);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetBlendMode(IntPtr image, GPU_BlendPresetEnum mode);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetImageFilter(IntPtr image, GPU_FilterEnum filter);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetAnchor(IntPtr image, float anchor_x, float anchor_y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_GetAnchor(IntPtr image, out float anchor_x, out float anchor_y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_SnapEnum GPU_GetSnapMode(IntPtr image);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetSnapMode(IntPtr image, GPU_SnapEnum mode);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetWrapMode(IntPtr image, GPU_WrapEnum wrap_mode_x, GPU_WrapEnum wrap_mode_y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_GetTextureHandle(IntPtr image);
        #endregion

        #region Conversions
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_CopyImageFromSurface(IntPtr surface);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_CopyImageFromTarget(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_CopySurfaceFromTarget(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_CopySurfaceFromImage(IntPtr image);
        #endregion

        #region Matrix
        // TODO: Matrix and vector operations
        #endregion

        #region Rendering
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Clear(IntPtr target);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ClearColor(IntPtr target, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ClearRGB(IntPtr target, byte r, byte g, byte b);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ClearRGBA(IntPtr target, byte r, byte g, byte b, byte a);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Blit(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Blit(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRotate(IntPtr image, ref GPU_Rect src_rect, float x, float y, float degrees);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRotate(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y, float degrees);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitScale(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y, float scaleX, float scaleY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitScale(IntPtr image, IntPtr src_rect, float x, IntPtr target, float y, float scaleX, float scaleY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransform(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y, float degrees, float scaleX, float scaleY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransform(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y, float degrees, float scaleX, float scaleY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransformX(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y, float pivot_x, float pivot_y, float degrees, float scaleX, float scaleY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransformX(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y, float pivot_x, float pivot_y, float degrees, float scaleX, float scaleY);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRect(IntPtr image, ref GPU_Rect src_rect, IntPtr target, ref GPU_Rect dest_rect);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRectX(IntPtr image, ref GPU_Rect src_rect, IntPtr target, ref GPU_Rect dest_rect, float degrees, float pivot_x, float pivot_y, GPU_FlipEnum flip_direction);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_TriangleBatch(
            IntPtr image,
            IntPtr target,
            ushort num_vertices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
            float[] values,
            uint num_indices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)]
            ushort[] indices,
            GPU_BatchFlagEnum flags
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_TriangleBatchX(
            IntPtr image,
            IntPtr target,
            ushort num_vertices,
            IntPtr values,
            uint num_indices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)]
            ushort[] indices,
            GPU_BatchFlagEnum flags
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_PrimitiveBatch(
            IntPtr image,
            IntPtr target,
            GPU_PrimitiveEnum primitive_type,
            ushort num_vertices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
            float[] values,
            uint num_indices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)]
            ushort[] indices,
            GPU_BatchFlagEnum flags
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_PrimitiveBatchV(
            IntPtr image,
            IntPtr target,
            GPU_PrimitiveEnum primitive_type,
            ushort num_vertices,
            IntPtr values,
            uint num_indices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)]
            ushort[] indices,
            GPU_BatchFlagEnum flags
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FlushBlitBuffer();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Flip(IntPtr target);
        #endregion

        #region Shapes
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Pixel(IntPtr target, float x, float y, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Line(IntPtr target, float x1, float y1, float x2, float y2, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Arc(IntPtr target, float x, float y, float radius, float start_angle, float end_angle, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ArcFilled(IntPtr target, float x, float y, float radius, float start_angle, float end_angle, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Circle(IntPtr target, float x, float y, float radius, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_CircleFilled(IntPtr target, float x, float y, float radius, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Ellipse(IntPtr target, float x, float y, float rx, float ry, float degrees, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_EllipseFilled(IntPtr target, float x, float y, float rx, float ry, float degrees, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Sector(IntPtr target, float x, float y, float inner_radius, float outer_radius, float start_angle, float end_angle, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SectorFilled(IntPtr target, float x, float y, float inner_radius, float outer_radius, float start_angle, float end_angle, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Tri(IntPtr target, float x1, float y1, float x2, float y2, float x3, float y3, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_TriFilled(IntPtr target, float x1, float y1, float x2, float y2, float x3, float y3, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Rectangle(IntPtr target, float x1, float y1, float x2, float y2, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_RectangleFilled(IntPtr target, float x1, float y1, float x2, float y2, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_RectangleRound(IntPtr target, float x1, float y1, float x2, float y2, float radius, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_RectangleRoundFilled(IntPtr target, float x1, float y1, float x2, float y2, float radius, SDL_Color color);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Polygon(
            IntPtr target,
            uint num_vertices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
            float[] vertices,
            SDL_Color color
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_PolygonFilled(
            IntPtr target,
            uint num_vertices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
            float[] vertices,
            SDL_Color color
        );

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Polyline(
            IntPtr target, 
            uint num_vertices,
            [In]
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4)]
            float[] vertices, 
            SDL_Color color, 
            bool close_loop
        );
        #endregion
    }
}