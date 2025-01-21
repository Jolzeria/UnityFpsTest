Shader "Custom/BulletMarksShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "QUEUE"="Overlay" "IGNOREPROJECTOR"="true" "RenderType"="Transparent"
        }
        LOD 100

        // 启用透明渲染
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Standard

        // 使用 Shader Model 3.0，以实现更好的光照效果
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;  // 添加 _Color 以支持颜色混合

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _Color.rgb; // 颜色与纹理颜色相乘
            o.Alpha = c.a * _Color.a; // 支持透明度调整
        }
        ENDCG
    }
    FallBack "Diffuse"
}