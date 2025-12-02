Shader "Custom/VideoCorrection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Gamma ("Gamma Correction", Range(0.1, 3.0)) = 1.0
        _Saturation ("Saturation", Range(0.0, 2.0)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Gamma;
            float _Saturation;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed3 AdjustSaturation(fixed3 color, float saturation)
            {
                float grey = dot(color, fixed3(0.2126, 0.7152, 0.0722));
                return lerp(fixed3(grey, grey, grey), color, saturation);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                
                // Apply Gamma Correction (fixes washed out look)
                col.rgb = pow(col.rgb, _Gamma);

                // Apply Saturation (fixes grayscale look)
                col.rgb = AdjustSaturation(col.rgb, _Saturation);

                return col;
            }
            ENDCG
        }
    }
}
