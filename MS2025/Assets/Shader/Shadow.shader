Shader "Custom/SpriteShadow"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel Snap", Float) = 0
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
    }

        SubShader
        {
            Tags
            {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }

            Pass
            {
                Name "ForwardLit"
                Tags { "LightMode" = "UniversalForward" }

                Blend One OneMinusSrcAlpha
                Cull Off
                ZWrite Off

                HLSLINCLUDE
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadow.hlsl"
                #include "UnitySprites.cginc"

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _Color;
                float _Cutoff;

                Varyings Vertex(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                    OUT.uv = IN.uv;
                    OUT.color = IN.color * _Color;
                    return OUT;
                }

                half4 Fragment(Varyings IN) : SV_Target
                {
                    float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                    texColor *= IN.color;

                    clip(texColor.a - _Cutoff);  // Alpha Cutoff処理
                    return texColor;
                }

                ENDHLSL

                    // 必要なステートメントを設定
                    // 指定したシェーダーコードをURP向けに定義
                    // LightingやPixelSnapなどの設定をここで行う
                }
        }
            Fallback Off
}
