Shader "Silhouette 2D"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        [HDR]_OutlineColor("OutlineColor", Color) = (0, 1, 0, 1)
        _OutlineThickness("OutlineThickness", Range(0, 5)) = 3
    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Transparent"
                "Queue" = "Overlay"
            }

            Pass
            {
                Name "Sprite Lit"
                Tags
                {
                    "LightMode" = "Universal2D"
                }

            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Off
            ZTest LEqual
            ZWrite Off
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // Keywords
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_SCREENPOSITION
            #pragma multi_compile_instancing
            #define SHADERPASS_SPRITELIT


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _OutlineColor;
            float _OutlineThickness;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_63A5927A_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_DD7DAD64_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_6D12461A_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_2D46CEE7_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_1D8F4203_Sampler_3_Linear_Repeat);

            // Graph Functions

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }

            void Unity_Negate_float2(float2 In, out float2 Out)
            {
                Out = -1 * In;
            }

            void Unity_Clamp_float(float In, float Min, float Max, out float Out)
            {
                Out = clamp(In, Min, Max);
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }

            // Graph Vertex
            // GraphVertex: <None>

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
            };

            struct SurfaceDescription
            {
                float4 Color;
                float4 Mask;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_A2F6709F_Out_0 = _OutlineThickness;
                float _Vector1_41CF0BC9_Out_0 = 0.01;
                float _Multiply_312945B5_Out_2;
                Unity_Multiply_float(_Property_A2F6709F_Out_0, _Vector1_41CF0BC9_Out_0, _Multiply_312945B5_Out_2);
                float4 _Combine_2E79E207_RGBA_4;
                float3 _Combine_2E79E207_RGB_5;
                float2 _Combine_2E79E207_RG_6;
                Unity_Combine_float(_Multiply_312945B5_Out_2, 0, 0, 0, _Combine_2E79E207_RGBA_4, _Combine_2E79E207_RGB_5, _Combine_2E79E207_RG_6);
                float2 _TilingAndOffset_BCDE53_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_2E79E207_RG_6, _TilingAndOffset_BCDE53_Out_3);
                float4 _SampleTexture2D_63A5927A_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_BCDE53_Out_3);
                float _SampleTexture2D_63A5927A_R_4 = _SampleTexture2D_63A5927A_RGBA_0.r;
                float _SampleTexture2D_63A5927A_G_5 = _SampleTexture2D_63A5927A_RGBA_0.g;
                float _SampleTexture2D_63A5927A_B_6 = _SampleTexture2D_63A5927A_RGBA_0.b;
                float _SampleTexture2D_63A5927A_A_7 = _SampleTexture2D_63A5927A_RGBA_0.a;
                float4 _Combine_DBB03CCB_RGBA_4;
                float3 _Combine_DBB03CCB_RGB_5;
                float2 _Combine_DBB03CCB_RG_6;
                Unity_Combine_float(0, _Multiply_312945B5_Out_2, 0, 0, _Combine_DBB03CCB_RGBA_4, _Combine_DBB03CCB_RGB_5, _Combine_DBB03CCB_RG_6);
                float2 _TilingAndOffset_D6565769_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_DBB03CCB_RG_6, _TilingAndOffset_D6565769_Out_3);
                float4 _SampleTexture2D_DD7DAD64_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_D6565769_Out_3);
                float _SampleTexture2D_DD7DAD64_R_4 = _SampleTexture2D_DD7DAD64_RGBA_0.r;
                float _SampleTexture2D_DD7DAD64_G_5 = _SampleTexture2D_DD7DAD64_RGBA_0.g;
                float _SampleTexture2D_DD7DAD64_B_6 = _SampleTexture2D_DD7DAD64_RGBA_0.b;
                float _SampleTexture2D_DD7DAD64_A_7 = _SampleTexture2D_DD7DAD64_RGBA_0.a;
                float _Add_E6BA4D8F_Out_2;
                Unity_Add_float(_SampleTexture2D_63A5927A_A_7, _SampleTexture2D_DD7DAD64_A_7, _Add_E6BA4D8F_Out_2);
                float2 _Negate_FA53ED2B_Out_1;
                Unity_Negate_float2(_Combine_2E79E207_RG_6, _Negate_FA53ED2B_Out_1);
                float2 _TilingAndOffset_E99648DE_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Negate_FA53ED2B_Out_1, _TilingAndOffset_E99648DE_Out_3);
                float4 _SampleTexture2D_6D12461A_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_E99648DE_Out_3);
                float _SampleTexture2D_6D12461A_R_4 = _SampleTexture2D_6D12461A_RGBA_0.r;
                float _SampleTexture2D_6D12461A_G_5 = _SampleTexture2D_6D12461A_RGBA_0.g;
                float _SampleTexture2D_6D12461A_B_6 = _SampleTexture2D_6D12461A_RGBA_0.b;
                float _SampleTexture2D_6D12461A_A_7 = _SampleTexture2D_6D12461A_RGBA_0.a;
                float2 _Negate_4F404D64_Out_1;
                Unity_Negate_float2(_Combine_DBB03CCB_RG_6, _Negate_4F404D64_Out_1);
                float2 _TilingAndOffset_2FFF2A8E_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Negate_4F404D64_Out_1, _TilingAndOffset_2FFF2A8E_Out_3);
                float4 _SampleTexture2D_2D46CEE7_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_2FFF2A8E_Out_3);
                float _SampleTexture2D_2D46CEE7_R_4 = _SampleTexture2D_2D46CEE7_RGBA_0.r;
                float _SampleTexture2D_2D46CEE7_G_5 = _SampleTexture2D_2D46CEE7_RGBA_0.g;
                float _SampleTexture2D_2D46CEE7_B_6 = _SampleTexture2D_2D46CEE7_RGBA_0.b;
                float _SampleTexture2D_2D46CEE7_A_7 = _SampleTexture2D_2D46CEE7_RGBA_0.a;
                float _Add_7EFEC614_Out_2;
                Unity_Add_float(_SampleTexture2D_6D12461A_A_7, _SampleTexture2D_2D46CEE7_A_7, _Add_7EFEC614_Out_2);
                float _Add_C11018C4_Out_2;
                Unity_Add_float(_Add_E6BA4D8F_Out_2, _Add_7EFEC614_Out_2, _Add_C11018C4_Out_2);
                float _Clamp_30BB05D_Out_3;
                Unity_Clamp_float(_Add_C11018C4_Out_2, 0, 1, _Clamp_30BB05D_Out_3);
                float4 _SampleTexture2D_1D8F4203_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_1D8F4203_R_4 = _SampleTexture2D_1D8F4203_RGBA_0.r;
                float _SampleTexture2D_1D8F4203_G_5 = _SampleTexture2D_1D8F4203_RGBA_0.g;
                float _SampleTexture2D_1D8F4203_B_6 = _SampleTexture2D_1D8F4203_RGBA_0.b;
                float _SampleTexture2D_1D8F4203_A_7 = _SampleTexture2D_1D8F4203_RGBA_0.a;
                float _Subtract_DB8622DC_Out_2;
                Unity_Subtract_float(_Clamp_30BB05D_Out_3, _SampleTexture2D_1D8F4203_A_7, _Subtract_DB8622DC_Out_2);
                float4 _Property_AA0454D4_Out_0 = _OutlineColor;
                float4 _Multiply_9E01ACFC_Out_2;
                Unity_Multiply_float((_Subtract_DB8622DC_Out_2.xxxx), _Property_AA0454D4_Out_0, _Multiply_9E01ACFC_Out_2);
                float4 _Add_C2C52A17_Out_2;
                Unity_Add_float4(float4(0, 0, 0, 0), _Multiply_9E01ACFC_Out_2, _Add_C2C52A17_Out_2);
                surface.Color = _Add_C2C52A17_Out_2;
                surface.Mask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
                float4 screenPosition;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.color;
                output.interp02.xyzw = input.screenPosition;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.color = input.interp01.xyzw;
                output.screenPosition = input.interp02.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                output.uv0 = input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "Sprite Normal"
            Tags
            {
                "LightMode" = "NormalsRendering"
            }

                // Render State
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                Cull Off
                ZTest LEqual
                ZWrite Off
                // ColorMask: <None>


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define VARYINGS_NEED_TEXCOORD0
                #pragma multi_compile_instancing
                #define SHADERPASS_SPRITENORMAL


                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
                CBUFFER_END
                TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                SAMPLER(_SampleTexture2D_63A5927A_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_DD7DAD64_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_6D12461A_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_2D46CEE7_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2D_1D8F4203_Sampler_3_Linear_Repeat);

                // Graph Functions

                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }

                void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                {
                    RGBA = float4(R, G, B, A);
                    RGB = float3(R, G, B);
                    RG = float2(R, G);
                }

                void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                {
                    Out = UV * Tiling + Offset;
                }

                void Unity_Add_float(float A, float B, out float Out)
                {
                    Out = A + B;
                }

                void Unity_Negate_float2(float2 In, out float2 Out)
                {
                    Out = -1 * In;
                }

                void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                {
                    Out = clamp(In, Min, Max);
                }

                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }

                void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
                }

                void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A + B;
                }

                // Graph Vertex
                // GraphVertex: <None>

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                    float4 uv0;
                };

                struct SurfaceDescription
                {
                    float4 Color;
                    float3 Normal;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float _Property_A2F6709F_Out_0 = _OutlineThickness;
                    float _Vector1_41CF0BC9_Out_0 = 0.01;
                    float _Multiply_312945B5_Out_2;
                    Unity_Multiply_float(_Property_A2F6709F_Out_0, _Vector1_41CF0BC9_Out_0, _Multiply_312945B5_Out_2);
                    float4 _Combine_2E79E207_RGBA_4;
                    float3 _Combine_2E79E207_RGB_5;
                    float2 _Combine_2E79E207_RG_6;
                    Unity_Combine_float(_Multiply_312945B5_Out_2, 0, 0, 0, _Combine_2E79E207_RGBA_4, _Combine_2E79E207_RGB_5, _Combine_2E79E207_RG_6);
                    float2 _TilingAndOffset_BCDE53_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_2E79E207_RG_6, _TilingAndOffset_BCDE53_Out_3);
                    float4 _SampleTexture2D_63A5927A_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_BCDE53_Out_3);
                    float _SampleTexture2D_63A5927A_R_4 = _SampleTexture2D_63A5927A_RGBA_0.r;
                    float _SampleTexture2D_63A5927A_G_5 = _SampleTexture2D_63A5927A_RGBA_0.g;
                    float _SampleTexture2D_63A5927A_B_6 = _SampleTexture2D_63A5927A_RGBA_0.b;
                    float _SampleTexture2D_63A5927A_A_7 = _SampleTexture2D_63A5927A_RGBA_0.a;
                    float4 _Combine_DBB03CCB_RGBA_4;
                    float3 _Combine_DBB03CCB_RGB_5;
                    float2 _Combine_DBB03CCB_RG_6;
                    Unity_Combine_float(0, _Multiply_312945B5_Out_2, 0, 0, _Combine_DBB03CCB_RGBA_4, _Combine_DBB03CCB_RGB_5, _Combine_DBB03CCB_RG_6);
                    float2 _TilingAndOffset_D6565769_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_DBB03CCB_RG_6, _TilingAndOffset_D6565769_Out_3);
                    float4 _SampleTexture2D_DD7DAD64_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_D6565769_Out_3);
                    float _SampleTexture2D_DD7DAD64_R_4 = _SampleTexture2D_DD7DAD64_RGBA_0.r;
                    float _SampleTexture2D_DD7DAD64_G_5 = _SampleTexture2D_DD7DAD64_RGBA_0.g;
                    float _SampleTexture2D_DD7DAD64_B_6 = _SampleTexture2D_DD7DAD64_RGBA_0.b;
                    float _SampleTexture2D_DD7DAD64_A_7 = _SampleTexture2D_DD7DAD64_RGBA_0.a;
                    float _Add_E6BA4D8F_Out_2;
                    Unity_Add_float(_SampleTexture2D_63A5927A_A_7, _SampleTexture2D_DD7DAD64_A_7, _Add_E6BA4D8F_Out_2);
                    float2 _Negate_FA53ED2B_Out_1;
                    Unity_Negate_float2(_Combine_2E79E207_RG_6, _Negate_FA53ED2B_Out_1);
                    float2 _TilingAndOffset_E99648DE_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Negate_FA53ED2B_Out_1, _TilingAndOffset_E99648DE_Out_3);
                    float4 _SampleTexture2D_6D12461A_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_E99648DE_Out_3);
                    float _SampleTexture2D_6D12461A_R_4 = _SampleTexture2D_6D12461A_RGBA_0.r;
                    float _SampleTexture2D_6D12461A_G_5 = _SampleTexture2D_6D12461A_RGBA_0.g;
                    float _SampleTexture2D_6D12461A_B_6 = _SampleTexture2D_6D12461A_RGBA_0.b;
                    float _SampleTexture2D_6D12461A_A_7 = _SampleTexture2D_6D12461A_RGBA_0.a;
                    float2 _Negate_4F404D64_Out_1;
                    Unity_Negate_float2(_Combine_DBB03CCB_RG_6, _Negate_4F404D64_Out_1);
                    float2 _TilingAndOffset_2FFF2A8E_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Negate_4F404D64_Out_1, _TilingAndOffset_2FFF2A8E_Out_3);
                    float4 _SampleTexture2D_2D46CEE7_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_2FFF2A8E_Out_3);
                    float _SampleTexture2D_2D46CEE7_R_4 = _SampleTexture2D_2D46CEE7_RGBA_0.r;
                    float _SampleTexture2D_2D46CEE7_G_5 = _SampleTexture2D_2D46CEE7_RGBA_0.g;
                    float _SampleTexture2D_2D46CEE7_B_6 = _SampleTexture2D_2D46CEE7_RGBA_0.b;
                    float _SampleTexture2D_2D46CEE7_A_7 = _SampleTexture2D_2D46CEE7_RGBA_0.a;
                    float _Add_7EFEC614_Out_2;
                    Unity_Add_float(_SampleTexture2D_6D12461A_A_7, _SampleTexture2D_2D46CEE7_A_7, _Add_7EFEC614_Out_2);
                    float _Add_C11018C4_Out_2;
                    Unity_Add_float(_Add_E6BA4D8F_Out_2, _Add_7EFEC614_Out_2, _Add_C11018C4_Out_2);
                    float _Clamp_30BB05D_Out_3;
                    Unity_Clamp_float(_Add_C11018C4_Out_2, 0, 1, _Clamp_30BB05D_Out_3);
                    float4 _SampleTexture2D_1D8F4203_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                    float _SampleTexture2D_1D8F4203_R_4 = _SampleTexture2D_1D8F4203_RGBA_0.r;
                    float _SampleTexture2D_1D8F4203_G_5 = _SampleTexture2D_1D8F4203_RGBA_0.g;
                    float _SampleTexture2D_1D8F4203_B_6 = _SampleTexture2D_1D8F4203_RGBA_0.b;
                    float _SampleTexture2D_1D8F4203_A_7 = _SampleTexture2D_1D8F4203_RGBA_0.a;
                    float _Subtract_DB8622DC_Out_2;
                    Unity_Subtract_float(_Clamp_30BB05D_Out_3, _SampleTexture2D_1D8F4203_A_7, _Subtract_DB8622DC_Out_2);
                    float4 _Property_AA0454D4_Out_0 = _OutlineColor;
                    float4 _Multiply_9E01ACFC_Out_2;
                    Unity_Multiply_float((_Subtract_DB8622DC_Out_2.xxxx), _Property_AA0454D4_Out_0, _Multiply_9E01ACFC_Out_2);
                    float4 _Add_C2C52A17_Out_2;
                    Unity_Add_float4(float4(0, 0, 0, 0), _Multiply_9E01ACFC_Out_2, _Add_C2C52A17_Out_2);
                    surface.Color = _Add_C2C52A17_Out_2;
                    surface.Normal = float3 (0, 0, 1);
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 normalWS;
                    float4 tangentWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    float3 interp00 : TEXCOORD0;
                    float4 interp01 : TEXCOORD1;
                    float4 interp02 : TEXCOORD2;
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output = (PackedVaryings)0;
                    output.positionCS = input.positionCS;
                    output.interp00.xyz = input.normalWS;
                    output.interp01.xyzw = input.tangentWS;
                    output.interp02.xyzw = input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output = (Varyings)0;
                    output.positionCS = input.positionCS;
                    output.normalWS = input.interp00.xyz;
                    output.tangentWS = input.interp01.xyzw;
                    output.texCoord0 = input.interp02.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                    output.uv0 = input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"

                ENDHLSL
            }

            Pass
            {
                Name "Sprite Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }

                    // Render State
                    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                    Cull Off
                    ZTest Always
                    ZWrite Off
                    // ColorMask: <None>


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0

                    // Keywords
                    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
                    // GraphKeywords: <None>

                    // Defines
                    #define _SURFACE_TYPE_TRANSPARENT 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define ATTRIBUTES_NEED_COLOR
                    #define VARYINGS_NEED_TEXCOORD0
                    #define VARYINGS_NEED_COLOR
                    #pragma multi_compile_instancing
                    #define SHADERPASS_SPRITEFORWARD


                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float4 _OutlineColor;
                    float _OutlineThickness;
                    CBUFFER_END
                    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                    SAMPLER(_SampleTexture2D_63A5927A_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_DD7DAD64_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_6D12461A_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_2D46CEE7_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_1D8F4203_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    void Unity_Multiply_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }

                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }

                    void Unity_Negate_float2(float2 In, out float2 Out)
                    {
                        Out = -1 * In;
                    }

                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }

                    void Unity_Subtract_float(float A, float B, out float Out)
                    {
                        Out = A - B;
                    }

                    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                    {
                        Out = A + B;
                    }

                    // Graph Vertex
                    // GraphVertex: <None>

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                        float4 uv0;
                    };

                    struct SurfaceDescription
                    {
                        float4 Color;
                        float3 Normal;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float _Property_A2F6709F_Out_0 = _OutlineThickness;
                        float _Vector1_41CF0BC9_Out_0 = 0.01;
                        float _Multiply_312945B5_Out_2;
                        Unity_Multiply_float(_Property_A2F6709F_Out_0, _Vector1_41CF0BC9_Out_0, _Multiply_312945B5_Out_2);
                        float4 _Combine_2E79E207_RGBA_4;
                        float3 _Combine_2E79E207_RGB_5;
                        float2 _Combine_2E79E207_RG_6;
                        Unity_Combine_float(_Multiply_312945B5_Out_2, 0, 0, 0, _Combine_2E79E207_RGBA_4, _Combine_2E79E207_RGB_5, _Combine_2E79E207_RG_6);
                        float2 _TilingAndOffset_BCDE53_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_2E79E207_RG_6, _TilingAndOffset_BCDE53_Out_3);
                        float4 _SampleTexture2D_63A5927A_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_BCDE53_Out_3);
                        float _SampleTexture2D_63A5927A_R_4 = _SampleTexture2D_63A5927A_RGBA_0.r;
                        float _SampleTexture2D_63A5927A_G_5 = _SampleTexture2D_63A5927A_RGBA_0.g;
                        float _SampleTexture2D_63A5927A_B_6 = _SampleTexture2D_63A5927A_RGBA_0.b;
                        float _SampleTexture2D_63A5927A_A_7 = _SampleTexture2D_63A5927A_RGBA_0.a;
                        float4 _Combine_DBB03CCB_RGBA_4;
                        float3 _Combine_DBB03CCB_RGB_5;
                        float2 _Combine_DBB03CCB_RG_6;
                        Unity_Combine_float(0, _Multiply_312945B5_Out_2, 0, 0, _Combine_DBB03CCB_RGBA_4, _Combine_DBB03CCB_RGB_5, _Combine_DBB03CCB_RG_6);
                        float2 _TilingAndOffset_D6565769_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_DBB03CCB_RG_6, _TilingAndOffset_D6565769_Out_3);
                        float4 _SampleTexture2D_DD7DAD64_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_D6565769_Out_3);
                        float _SampleTexture2D_DD7DAD64_R_4 = _SampleTexture2D_DD7DAD64_RGBA_0.r;
                        float _SampleTexture2D_DD7DAD64_G_5 = _SampleTexture2D_DD7DAD64_RGBA_0.g;
                        float _SampleTexture2D_DD7DAD64_B_6 = _SampleTexture2D_DD7DAD64_RGBA_0.b;
                        float _SampleTexture2D_DD7DAD64_A_7 = _SampleTexture2D_DD7DAD64_RGBA_0.a;
                        float _Add_E6BA4D8F_Out_2;
                        Unity_Add_float(_SampleTexture2D_63A5927A_A_7, _SampleTexture2D_DD7DAD64_A_7, _Add_E6BA4D8F_Out_2);
                        float2 _Negate_FA53ED2B_Out_1;
                        Unity_Negate_float2(_Combine_2E79E207_RG_6, _Negate_FA53ED2B_Out_1);
                        float2 _TilingAndOffset_E99648DE_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Negate_FA53ED2B_Out_1, _TilingAndOffset_E99648DE_Out_3);
                        float4 _SampleTexture2D_6D12461A_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_E99648DE_Out_3);
                        float _SampleTexture2D_6D12461A_R_4 = _SampleTexture2D_6D12461A_RGBA_0.r;
                        float _SampleTexture2D_6D12461A_G_5 = _SampleTexture2D_6D12461A_RGBA_0.g;
                        float _SampleTexture2D_6D12461A_B_6 = _SampleTexture2D_6D12461A_RGBA_0.b;
                        float _SampleTexture2D_6D12461A_A_7 = _SampleTexture2D_6D12461A_RGBA_0.a;
                        float2 _Negate_4F404D64_Out_1;
                        Unity_Negate_float2(_Combine_DBB03CCB_RG_6, _Negate_4F404D64_Out_1);
                        float2 _TilingAndOffset_2FFF2A8E_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Negate_4F404D64_Out_1, _TilingAndOffset_2FFF2A8E_Out_3);
                        float4 _SampleTexture2D_2D46CEE7_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _TilingAndOffset_2FFF2A8E_Out_3);
                        float _SampleTexture2D_2D46CEE7_R_4 = _SampleTexture2D_2D46CEE7_RGBA_0.r;
                        float _SampleTexture2D_2D46CEE7_G_5 = _SampleTexture2D_2D46CEE7_RGBA_0.g;
                        float _SampleTexture2D_2D46CEE7_B_6 = _SampleTexture2D_2D46CEE7_RGBA_0.b;
                        float _SampleTexture2D_2D46CEE7_A_7 = _SampleTexture2D_2D46CEE7_RGBA_0.a;
                        float _Add_7EFEC614_Out_2;
                        Unity_Add_float(_SampleTexture2D_6D12461A_A_7, _SampleTexture2D_2D46CEE7_A_7, _Add_7EFEC614_Out_2);
                        float _Add_C11018C4_Out_2;
                        Unity_Add_float(_Add_E6BA4D8F_Out_2, _Add_7EFEC614_Out_2, _Add_C11018C4_Out_2);
                        float _Clamp_30BB05D_Out_3;
                        Unity_Clamp_float(_Add_C11018C4_Out_2, 0, 1, _Clamp_30BB05D_Out_3);
                        float4 _SampleTexture2D_1D8F4203_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                        float _SampleTexture2D_1D8F4203_R_4 = _SampleTexture2D_1D8F4203_RGBA_0.r;
                        float _SampleTexture2D_1D8F4203_G_5 = _SampleTexture2D_1D8F4203_RGBA_0.g;
                        float _SampleTexture2D_1D8F4203_B_6 = _SampleTexture2D_1D8F4203_RGBA_0.b;
                        float _SampleTexture2D_1D8F4203_A_7 = _SampleTexture2D_1D8F4203_RGBA_0.a;
                        float _Subtract_DB8622DC_Out_2;
                        Unity_Subtract_float(_Clamp_30BB05D_Out_3, _SampleTexture2D_1D8F4203_A_7, _Subtract_DB8622DC_Out_2);
                        float4 _Property_AA0454D4_Out_0 = _OutlineColor;
                        float4 _Multiply_9E01ACFC_Out_2;
                        Unity_Multiply_float((_Subtract_DB8622DC_Out_2.xxxx), _Property_AA0454D4_Out_0, _Multiply_9E01ACFC_Out_2);
                        float4 _Add_C2C52A17_Out_2;
                        Unity_Add_float4(float4(0, 0, 0, 0), _Multiply_9E01ACFC_Out_2, _Add_C2C52A17_Out_2);
                        surface.Color = _Add_C2C52A17_Out_2;
                        surface.Normal = float3 (0, 0, 1);
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        float4 uv0 : TEXCOORD0;
                        float4 color : COLOR;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        float4 positionCS : SV_POSITION;
                        float4 texCoord0;
                        float4 color;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        float4 interp00 : TEXCOORD0;
                        float4 interp01 : TEXCOORD1;
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output = (PackedVaryings)0;
                        output.positionCS = input.positionCS;
                        output.interp00.xyzw = input.texCoord0;
                        output.interp01.xyzw = input.color;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output = (Varyings)0;
                        output.positionCS = input.positionCS;
                        output.texCoord0 = input.interp00.xyzw;
                        output.color = input.interp01.xyzw;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                        output.uv0 = input.texCoord0;
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"

                    ENDHLSL
                }

        }
            FallBack "Hidden/Shader Graph/FallbackError"
}
