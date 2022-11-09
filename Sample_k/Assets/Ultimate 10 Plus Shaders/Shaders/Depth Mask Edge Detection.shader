Shader "Ultimate 10+ Shaders/Depth Mask Edge Detection"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Cull Back
        ZTest Always

        Pass
        {
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
                float4 position : POSITION;
                float4 screenPos : TEXCOORD0;
            };

            fixed4 _Color;
            sampler2D _CameraDepthTexture;

            v2f vert(appdata input)
            {
                v2f output;

                output.position = UnityObjectToClipPos(input.vertex);
                output.screenPos = output.position;

                return output;
            }

            half4 pixel;
            half2 uv;
            fixed onePixelW, onePixelH;
            half4 frag(v2f input) : SV_Target
            {    
                uv = input.screenPos.xy / input.screenPos.w;
                uv.x = (uv.x + 1) * .5;
                uv.y = (uv.y + 1) * .5;

                onePixelW = 1.0 / _ScreenParams.x;
                onePixelH = 1.0 / _ScreenParams.y;

                pixel = Linear01Depth(abs(
                        tex2D(_CameraDepthTexture, float2(uv.x - onePixelW, uv.y)) - 
                        tex2D(_CameraDepthTexture, float2(uv.x + onePixelW, uv.y)) + 
                        tex2D(_CameraDepthTexture, float2(uv.x, uv.y + onePixelH)) -
                        tex2D(_CameraDepthTexture, float2(uv.x, uv.y - onePixelH))
                    ));

                return pixel * _Color;
            }    
            ENDCG
        }
    }
}
