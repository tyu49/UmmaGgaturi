Shader "Custom/Normal_"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes //매쉬에서 정보 가져오는 데이터 구조임 포지션이나 노말같은거
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half3 normalOS : NORMAL; //정밀도가 float보다 낮은 16비트 연산자(float는 32비트), 정밀도 낮은 대신 성능 챙김. Normal 같은 값에 자주 사용
            };

            struct Varyings //버텍스쉐이더가 출력한 값을 프래그먼트쉐이더까지 전달하는 데이터 구조임
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half3 normalWS : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS); //버텍스쉐이더(매쉬)에서 받아온 노말값은 오브젝트스페이스라서 월드스페로 바꿔줌
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                half3 normalColor = half3(IN.normalWS.x,IN.normalWS.y,IN.normalWS.z) / 2 + 0.5;
                
                Light MainLight = GetMainLight();
                half3 LightDir = MainLight.direction / 2 + 0.5;
                
                //return half4(LightDir,1);
                
                return half4(normalColor,1);
                //return half4(IN.normalWS,1); // -1 ~ 1 값
                //return half4(IN.normalWS.z,IN.normalWS.z,IN.normalWS.z,1);
                //return half4(IN.normalWS.y,IN.normalWS.y,IN.normalWS.y,1); 
                //return half4(IN.normalWS.x,IN.normalWS.x,IN.normalWS.x,1);
            }
            ENDHLSL
        }
    }
}
