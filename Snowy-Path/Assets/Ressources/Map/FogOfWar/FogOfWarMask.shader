Shader "Custom/FogOfWarMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BlurPower("BlurPower", float) = 0.002
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            half4 _Color;
            float _BlurPower;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv_MainTex = TRANSFORM_TEX(IN.uv_MainTex, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                half4 baseColor1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv_MainTex + float2(-_BlurPower, 0));
                half4 baseColor2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv_MainTex + float2(0, -_BlurPower));
                half4 baseColor3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv_MainTex + float2(_BlurPower, 0));
                half4 baseColor4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv_MainTex + float2(0, _BlurPower));
                half4 baseColor = 0.25 * (baseColor1 + baseColor2 + baseColor3 + baseColor4);
                half4 color = half4(baseColor.rgb, _Color.a - baseColor.g);
                return color;
            }

            ENDHLSL
        }
    }
}
