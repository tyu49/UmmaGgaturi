Shader "Custom/Shader_NoiseCellShading"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _ShadowColor("Shadow Color", Color) = (0,0,0,1)
        _ShadowThreshold("Shadow Threshold", Range(0, 1)) = 0.5
        _ShadowSoftness("Shadow Softness", Range(0, 1)) = 0.5
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
                half4 _ShadowColor;
                half _ShadowThreshold;
                half _ShadowSoftness;
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
                half3 normalWS = normalize(IN.normalWS);
                
                Light MainLight = GetMainLight();
                half3 LightWS = normalize(MainLight.direction);
                
                half DotValue = saturate(dot(normalWS, LightWS)); // 노말벡터와 빛 각도를 내적해서 표면이 빛 방향을 얼마나 바라보고 있는지 계산하고 saturate로 0~1로 제한
                
                half halfSoftness = _ShadowSoftness * 0.5;

                half shadowMin = _ShadowThreshold - halfSoftness;
                half shadowMax = _ShadowThreshold + halfSoftness;
                
                //half DotValueStep = step(_ShadowThreshold, DotValue); // 그림자 임계값만큼 그림자 부여
                half DotValueStep = smoothstep(shadowMin,shadowMax,DotValue); //부드러운 그림자 임계값
                
                half4 LerpValue = lerp(_ShadowColor,_BaseColor,DotValueStep); //러프를 통해 그림자 임계값을 기준으로 그림자랑 베이스컬러 적용 
                
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv); 
                return LerpValue * baseMap;
                
                //return half4(DotValueStep, DotValueStep, DotValueStep, 1);
                
                //return half4(LightDir,1);
                
                //return half4(normalColor,1);
                //return half4(IN.normalWS,1); // -1 ~ 1 값
                //return half4(IN.normalWS.z,IN.normalWS.z,IN.normalWS.z,1);
                //return half4(IN.normalWS.y,IN.normalWS.y,IN.normalWS.y,1); 
                //return half4(IN.normalWS.x,IN.normalWS.x,IN.normalWS.x,1);
            }
            ENDHLSL
        }
    }
}
