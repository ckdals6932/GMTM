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

        //�޸鸸 ������ �մϴ�. (2��° pass���� �ո��� ������ //cull back �߰�) 
        cull front
        
        //zbuffer�� ���̰��� ������� �ʽ��ϴ�. 
        //zbuffer�� ���̰��� 0.0 ~ 1.0���� ����Ѵ�. (ī�޶�� ���� ������ 0.0, ���� �ָ������� 1.0)
        // 
        //zwrite�� off�� �ϸ� zbuffer�� 1�� ��ϵȴ�.
        //���� �ָ� �ִٰ� �Ǵ��Ͽ� �ٸ�������Ʈ�� �������� �ܰ����� �׷����� �ʴ´�.
        //RenderType�� Queue�� Transparent�� �ٲ��ش�.
        zwrite On
            
        CGPROGRAM
        #pragma surface surf NoLight vertex:vert noshadow noambient //�ʿ���� ȯ�汤�� �׸��ڸ� �����ݴϴ�.
        #pragma target 3.0

        float4 _OutLineColor;
        float _OutLineWidth;

        //vertex�� normal �������� Ȯ������ݴϴ�.
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
