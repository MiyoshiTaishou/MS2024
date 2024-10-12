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

            // ���������̕`���L���ɂ���
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
                    // �s�N�Z���T�C�Y�Ɋ�Â���UV���W���ۂ߂�
                    float2 pixelatedUV = floor(i.uv / _PixelSize) * _PixelSize;
                    // �e�N�X�`���̐F�ƃA���t�@���擾
                    fixed4 col = tex2D(_MainTex, pixelatedUV);
                    // �A���t�@�`�����l���i�����x�j�����̂܂ܕێ�
                    return col;
                }
                ENDCG
            }
        }

            // �t�H�[���o�b�N�̐ݒ�i�ȗ������ꂽ�o�[�W�������\�������ۂɓK�p�j
                    Fallback "Diffuse"
}
