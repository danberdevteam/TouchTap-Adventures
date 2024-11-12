Shader "Custom/FlatWaterShader"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveHeight ("Wave Height", Float) = 0.1
        _Color ("Water Color", Color) = (0.2, 0.5, 0.8, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _MainTex_ST;
            float _WaveSpeed;
            float _WaveHeight;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float wave = sin((i.uv.y + _Time.y * _WaveSpeed) * 10.0) * _WaveHeight;
                fixed4 col = tex2D(_MainTex, i.uv + float2(wave, 0));
                return col * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
