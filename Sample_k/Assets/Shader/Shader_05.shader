//Time함수 사용
Shader "Custom/Shader_5"
{
    Properties
    {
        _MainTex ("texture1", 2D) = "white" {}
        _OuTime("TimeSpeed", float) = 0
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

        float   _OuTime;
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 OuTime = tex2D (_MainTex, float2(IN.uv_MainTex.x + _Time.y*_OuTime, IN.uv_MainTex.y));
            o.Albedo = OuTime.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
