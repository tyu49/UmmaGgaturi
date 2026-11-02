Shader "Custom/CartoonShader"
{
   Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _OutLineSize("OutLine Size", Float) = 0.05
        _DarkThreshold("Dark Threshold", Float) = 0.1
        _LightThreshold("Light Threshold", Float) = 0.4
        _MidColor("Mid Color",Color) = (0.5,0.5,0.5,1)
        _DarkColor("Dark Color",Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _OutLineSize;
                float _DarkThreshold;
                float _LightThreshold;
                half4 _DarkColor;
                half4 _MidColor;
            CBUFFER_END
        ENDHLSL
        
        Pass
        {
            Name "ToonForward"
            Tags { "LightMode" = "UniversalForward" }

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
                half3 LightWS = normalize(MainLight.direction);
                
                half3 normalWS = normalize(IN.normalWS);
                
                half DotValue = saturate(dot(normalWS, LightWS));
                    
                half LightStep = step(_LightThreshold,DotValue);
                half DarkStep = step(_DarkThreshold,DotValue);
                
                half4 LerpStep1 = lerp(_MidColor,_BaseColor,LightStep);
                half4 LerpStep2 = lerp(_DarkColor,LerpStep1,DarkStep);
                //return half4(LightDir,1);
                
                return LerpStep2;
            }
            ENDHLSL 
        }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" } //URP가 오브젝트를 렌더링할 때 추가적인 Pass를 그릴 때 사용하는 LightMode
            Cull Front //뒷면으로 보이게
            
            HLSLPROGRAM
            #pragma vertex OutlineVert
            #pragma fragment OutlineFrag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION; //매쉬의 오브젝트스페이스 포지션
                half3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;  
            };
            
            Varyings OutlineVert(Attributes IN)
            {
                Varyings OUT;
                
                float3 expandDir = normalize(IN.positionOS.xyz);
                float3 expandedPositionOS = IN.positionOS.xyz + expandDir * _OutLineSize;
                
                OUT.positionHCS = TransformObjectToHClip(expandedPositionOS.xyz); //매쉬의 오브젝트스페이스 포지션을 동차 클립 공간의 정점 위치로 변환함.
                
                
                return OUT; //출력해서 GPU로 보냄. 이후 레스터라이징 후에 프레그먼트쉐이더로 넘겨짐
            }

            half4 OutlineFrag(Varyings IN) : SV_Target //여기서 레스터라이징한거 받아서 SV_Target에 검은색 리턴함
            {
                return half4(0, 0, 0, 1); 
            }
            ENDHLSL
        }
    }
}
