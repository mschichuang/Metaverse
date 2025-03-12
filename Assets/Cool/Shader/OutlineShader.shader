Shader "Custom/OutlineGlowShader"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 1, 0, 1) // 描邊顏色
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.01 // 描邊寬度
        _EmissionStrength ("Emission Strength", Range(1, 10)) = 3 // 發光強度
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        Pass
        {
            Name "OUTLINE"
            Cull Front // 只渲染物件背面，避免覆蓋原本的材質
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            fixed4 _OutlineColor;
            float _EmissionStrength;

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                v.vertex.xyz += norm * _OutlineWidth; // 稍微放大模型，產生外框
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 計算發光顏色
                fixed3 emission = _OutlineColor.rgb * _EmissionStrength;
                return fixed4(emission, 1); // 設置發光顏色
            }
            ENDCG
        }
    }
}
