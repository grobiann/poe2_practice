Shader "Unlit/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogTex ("Fog of War Texture", 2D) = "black" {}
        _WalkableTex ("Walakble Texture", 2D) = "black" {}
        _Opacity ("Opacity", Range(0, 1)) = 1
        _LandscapeOpacity ("Landscape Opacity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

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
            sampler2D _FogTex;
            sampler2D _WalkableTex;
            float4 _MainTex_ST;
            float _Opacity;
            float _LandscapeOpacity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 mainColor = tex2D(_MainTex, i.uv);
                half4 fogColor = tex2D(_FogTex, i.uv);
                half4 walkableColor = tex2D(_WalkableTex, i.uv);

                if(fogColor.a >= 0.1 && fogColor.a <= 0.5 && walkableColor.r >= 0.9)
                {
                    return half4(0.0, 0.0, 1.0, fogColor.a * _Opacity);
                }

                mainColor.a = mainColor.a * fogColor.a * _Opacity;
                return mainColor;
            }
            ENDCG
        }
    }
}
