//³ë¸» ¸Ê
Shader "Custom/Shader_6"
{
    Properties
    {
        _MainTex ("Albedo_1(RGB)", 2D) = "white" {}
        _BumpTex ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            float3 n = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));
            o.Albedo = c.rgb;
            o.Normal = n;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
