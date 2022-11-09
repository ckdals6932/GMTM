//UV °öÇÏ±â
Shader "Custom/Shader_3"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _U ("U", float) = 1
        _V ("V", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        float _U, _V;
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex * float2(_U, _V));
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
