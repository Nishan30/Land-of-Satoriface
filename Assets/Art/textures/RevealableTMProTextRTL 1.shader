Shader "Custom/ScrollingText" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Speed("Scroll Speed", Range(0, 1)) = 0.5
    }
        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
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
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _Speed;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 uv = i.uv;
                    uv.x -= _Time.y * _Speed;
                    return tex2D(_MainTex, uv);
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
