Shader "Custom/PixelateURPShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", Float) = 0.05
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }
            LOD 100

            Pass
            {
                Name "Pixelate"
                ZTest Always Cull Off ZWrite Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _PixelSize;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 pixelatedUV = floor(i.uv / _PixelSize) * _PixelSize;
                    return tex2D(_MainTex, pixelatedUV);
                }
                ENDCG
            }
        }

            Fallback "Unlit"
}
