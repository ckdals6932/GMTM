//색상변경, 밝기조절
Shader "Custom/Shader_2"
{
    Properties
    {
        _ColorR ("Red", Range(0, 1)) = 1.0
        _ColorG ("Green", Range(0, 1)) = 1.0
        _ColorB ("Blue", Range(0, 1)) = 1.0
        _Brightness("Brightness", Range(-1,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        float _ColorR, _ColorG, _ColorB;
        float _Brightness;
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = float3(_ColorR, _ColorG, _ColorB) + _Brightness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
