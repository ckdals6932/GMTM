Shader "Custom/Shader_9"
{
     Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutLineColor("OutLine Color", Color) = (0,0,0,0)
        _OutLineWidth("OutLine Width", Range(0.001, 0.01)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 200

        //뒷면만 렌더링 합니다. (2번째 pass에서 앞면을 렌더링 //cull back 추가) 
        cull front
        
        //zbuffer에 깊이값을 기록하지 않습니다. 
        //zbuffer는 깊이값을 0.0 ~ 1.0으로 기록한다. (카메라와 가장 가까우면 0.0, 가장 멀리있으면 1.0)
        // 
        //zwrite를 off로 하면 zbuffer에 1로 기록된다.
        //가장 멀리 있다고 판단하여 다른오브젝트에 가려지면 외곽선이 그려지지 않는다.
        //RenderType과 Queue를 Transparent로 바꿔준다.
        zwrite On
            
        CGPROGRAM
        #pragma surface surf NoLight vertex:vert noshadow noambient //필요없는 환경광과 그림자를 없애줍니다.
        #pragma target 3.0

        float4 _OutLineColor;
        float _OutLineWidth;

        //vertex를 normal 방향으로 확장시켜줍니다.
        void vert(inout appdata_full v) 
        {
            v.vertex.xyz += v.normal.xyz * _OutLineWidth;
        }

        struct Input
        {
            float4 color;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
        }

        float4 LightingNoLight(SurfaceOutput s, float3 lightDir, float atten) 
        {
            return float4(_OutLineColor.rgb, 1);
        }
        ENDCG

        cull back
        zwrite on
        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
