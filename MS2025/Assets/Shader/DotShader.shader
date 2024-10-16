Shader "Custom/DotShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", Float) = 0.05
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            // 透明部分の描画を有効にする
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

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

                sampler2D _MainTex;
                float _PixelSize;
                float4 _MainTex_ST;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // ピクセルサイズに基づいてUV座標を丸める
                    float2 pixelatedUV = floor(i.uv / _PixelSize) * _PixelSize;
                    // テクスチャの色とアルファを取得
                    fixed4 col = tex2D(_MainTex, pixelatedUV);
                    // アルファチャンネル（透明度）をそのまま保持
                    return col;
                }
                ENDCG
            }
        }

            // フォールバックの設定（簡略化されたバージョンが表示される際に適用）
                    Fallback "Diffuse"
}
