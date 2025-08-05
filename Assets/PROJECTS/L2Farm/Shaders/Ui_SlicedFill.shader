Shader "UI/Custom/SlicedFill"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FillAmount ("Fill Amount", Range(0, 1)) = 1.0
        _FillDirection ("Fill Direction", Vector) = (1,0,0,0) // x=dir type, y=axis, z=reverse, w=unused
        _UseRoundedFill ("Use Radial Fill", Float) = 0

        // Slicing properties (set by Unity UI)
        _Border ("Border", Vector) = (0,0,0,0)
        _UVSec ("UV Sec", Vector) = (0,0,0,0)

        // Required for UI Blending
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _CullMode ("Cull Mode", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "RenderPipeline" = "UniversalPipeline"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull [_CullMode]
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Sliced Fill"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 texcoord     : TEXCOORD0;
                float4 color        : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 texcoord     : TEXCOORD0;
                float4 color        : COLOR;
                float2 pixelSize    : TEXCOORD1;
                float4 rect         : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // Textures & Parameters
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _MainTex_TexelSize;
                float4 _Border;
                float4 _MainTex_EditorPreview;
                float _FillAmount;
                float4 _FillDirection;
                float _UseRoundedFill;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);
                output.color = input.color * _Color;

                // Pass pixel size and rect for slicing logic
                float2 pixelSize = 1.0 / _ScreenParams.xy;
                output.pixelSize = _MainTex_TexelSize.xy * pixelSize;
                output.rect = float4(0, 0, 1, 1); // Assume full rect; adjust if needed

                return output;
            }

            // Simulate 9-slice UV clamping based on border (now takes float4)
            float2 Apply9Slice(float2 uv, float4 border)
            {
                // Convert pixel borders to UV space
                float2 minBorder = float2(
                    border.x * _MainTex_TexelSize.z,  // left
                    border.y * _MainTex_TexelSize.w   // bottom
                );
                float2 maxBorder = float2(
                    border.z * _MainTex_TexelSize.z,  // right
                    border.w * _MainTex_TexelSize.w   // top
                );

                float2 slicedUv = uv;

                // X-axis
                if (uv.x < minBorder.x)
                    slicedUv.x = uv.x; // left stretch
                else if (uv.x > 1.0 - maxBorder.x)
                    slicedUv.x = lerp(1.0 - maxBorder.x, 1.0, (uv.x - (1.0 - maxBorder.x)) / maxBorder.x);
                else
                    slicedUv.x = lerp(minBorder.x, 1.0 - maxBorder.x, (uv.x - minBorder.x) / (1.0 - minBorder.x - maxBorder.x));

                // Y-axis
                if (uv.y < minBorder.y)
                    slicedUv.y = uv.y; // bottom stretch
                else if (uv.y > 1.0 - maxBorder.y)
                    slicedUv.y = lerp(1.0 - maxBorder.y, 1.0, (uv.y - (1.0 - maxBorder.y)) / maxBorder.y);
                else
                    slicedUv.y = lerp(minBorder.y, 1.0 - maxBorder.y, (uv.y - minBorder.y) / (1.0 - minBorder.y - maxBorder.y));

                return slicedUv;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;

                // Only apply fill to center region (inside borders)
                float2 borderScale = _Border.xy / _MainTex_TexelSize.zw; // left/bottom
                float2 borderScale2 = _Border.zw / _MainTex_TexelSize.zw; // right/top

                // Check if inside center region (where fill should happen)
                bool inCenterX = uv.x > borderScale.x && uv.x < (1.0 - borderScale2.x);
                bool inCenterY = uv.y > borderScale.y && uv.y < (1.0 - borderScale2.y);

                // Default: show full image
                float2 sampledUv = Apply9Slice(uv, _Border); // Only needs uv and border
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampledUv) * input.color;

                // Fill logic: only affect center
                float fill = 1.0;

                if (inCenterX && inCenterY)
                {
                    float localUvX = (uv.x - borderScale.x) / (1.0 - borderScale.x - borderScale2.x);
                    float localUvY = (uv.y - borderScale.y) / (1.0 - borderScale.y - borderScale2.y);

                    if (_UseRoundedFill > 0.5)
                    {
                        // Radial fill from center
                        float2 center = float2(0.5, 0.5);
                        float dist = length(float2(localUvX, localUvY) - center);
                        float maxDist = length(center);
                        fill = smoothstep(0, maxDist * (1.0 - _FillAmount), dist);
                    }
                    else
                    {
                        // Directional fill
                        float dir = _FillDirection.x; // 0=horizontal, 1=vertical
                        float reverse = _FillDirection.z;

                        float progress = dir > 0.5 ? localUvY : localUvX;
                        if (reverse > 0.5) progress = 1.0 - progress;

                        fill = step(progress, _FillAmount);
                    }
                }
                else
                {
                    // Always show border regions
                    fill = 1.0;
                }

                color.a *= fill;
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "Universal Render Pipeline/2D/Sprite-Lit-Default"
}