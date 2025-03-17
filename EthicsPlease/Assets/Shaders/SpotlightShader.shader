Shader "Custom/SpotlightShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _SpotlightCenter ("Spotlight Center", Vector) = (0.5, 0.5, 0, 0)
        _SpotlightRadius ("Spotlight Radius", Float) = 0.2
        _Darkness ("Darkness", Float) = 0.5
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float2 _SpotlightCenter;
            float _SpotlightRadius;
            float _Darkness;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                float distanceToCenter = distance(uv, _SpotlightCenter);
                float spotlight = smoothstep(_SpotlightRadius, _SpotlightRadius - 0.05, distanceToCenter);
                fixed4 color = tex2D(_MainTex, uv);
                color.rgb *= lerp(_Darkness, 1.0, spotlight);
                color.a = 1.0;
                return color;
            }
            ENDCG
        }
    }
}