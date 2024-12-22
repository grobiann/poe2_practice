Shader "Unlit/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogTex ("Fog of War Texture", 2D) = "black" {}
        _MovableTex ("Movable Texture", 2D) = "black" {}
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
            sampler2D _MovableTex;
            float4 _MainTex_ST;

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
                half4 movableColor = tex2D(_MovableTex, i.uv);

                if(fogColor.a > 0.0 && fogColor.a < 1.0 && movableColor.r > 0)
                {
                    // gausian distribution
                    // (e^(-(2x-1)^2)-0.37)*1.59
                    // https://www.wolframalpha.com/input?i=%28e%5E%28-%282x-1%29%5E2%29-0.37%29*1.59
                    half fogAlpha = (exp(-pow(2*fogColor.a-1, 2))-0.37)*1.59;
                    half4 fog = half4(0.0, 0.0, 1.0, fogAlpha*0.9);
                    mainColor *= (1 - fogColor.a* 2);
                    return mainColor + fog ;
                }

                mainColor.a = mainColor.a * (1 - fogColor.a);
                return mainColor;
            }
            ENDCG
        }
    }
}


                /*
                Shader "Unlit/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogTex ("Fog of War Texture", 2D) = "black" {}
        
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

                */