Shader "Custom/Shader_12"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Help
        #pragma target 3.0

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        float4 LightingHelp(SurfaceOutput s, float3 lightDir, float atten)
        {
            //return dot(s.Normal, lightDir);
            float NdotL = dot(s.Normal, lightDir);
            NdotL = saturate(NdotL);

            float4 FinalColor;
            FinalColor.rgb = s.Albedo * NdotL * _LightColor0.rgb * atten;
            FinalColor.a = s.Alpha;

            return FinalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
