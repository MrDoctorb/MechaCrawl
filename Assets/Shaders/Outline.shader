Shader "Sprites/Outline2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        
        Blend One OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            fixed4 _Color;
            float4 _MainTex_TexelSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                 //   o.vertex = UnityPixelSnap(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 spriteCol = tex2D(_MainTex, i.uv) * i.color;

                //Only try to draw an outline if the pixel is invisible
                if (spriteCol.a == 0) 
                {
                    // Get the neighbouring four pixels.
                    fixed4 n = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y));
                    fixed4 s = tex2D(_MainTex, i.uv + fixed2(0, -_MainTex_TexelSize.y));
                    fixed4 e = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0));
                    fixed4 w = tex2D(_MainTex, i.uv + fixed2(-_MainTex_TexelSize.x, 0));

                    // If one of the neighbouring pixels is invisible, and one is not, we render an outline.
                    if ((n.a == 0 || s.a  == 0 || e.a == 0 || w.a == 0) &&
                        (n.a != 0 || s.a  != 0 || e.a != 0 || w.a != 0)) 
                    {
                        spriteCol.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
                    }
                }

                spriteCol.rgb *= spriteCol.a;

                return spriteCol;
            }
            ENDCG
        }
    }
}
