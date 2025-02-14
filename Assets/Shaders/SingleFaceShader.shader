Shader "Custom/SingleFaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 贴图
        _Scale ("Texture Scale", Float) = 1.0 // 贴图缩放
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Scale;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // o.uv = v.uv * _Scale; // 调整 UV 以缩放贴图
                // 仅在 Z+ 面缩放，并确保 UV 居中
                if (abs(v.normal.z - 1) < 0.01)
                {
                    o.uv = (v.uv - 0.5) * _Scale + 0.5;
                }
                else
                {
                    o.uv = v.uv; // 其他面保持默认 UV
                }
                
                // o.normal = mul(v.normal, (float3x3)unity_WorldToObject); // 世界法向量
                o.normal = v.normal; // 直接使用对象空间的法线
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 仅在本地坐标系 Z+ 方向的面上显示贴图
                if (abs(i.normal.z - 1) < 0.01)
                {
                    return tex2D(_MainTex, i.uv);
                }
                return fixed4(1, 1, 1, 1); // 其他面显示白色
            }
            ENDCG
        }
    }
}