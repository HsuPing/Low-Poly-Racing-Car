// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ArrowEmission"
{
	Properties
    {
        _MainTex("main color", color) = (1,1,1,1) 
        [HDR]_RimColor("rim color", Color) = (0.7426471, 0.07644895, 0.07644895, 1)
        _RimPower("rim power", range(1, 10)) = 2
    }

    SubShader
        {
            
            Pass
            {
                
                CGPROGRAM

#pragma vertex vert

#pragma fragment frag
#include"UnityCG.cginc"
                
                struct VertexInput
                {
                    float4 vertex:POSITION;
                    float3 normal:NORMAL;
                    float4 texcoord:TEXCOORD0;
                };
                
                struct VertexOutput
                {
                    float4 vertex:POSITION;
                    float4 uv:TEXCOORD0;
                    float4 NdotV:COLOR;
                };


                float4 _MainTex;
                float4 _RimColor;
                float _RimPower;

               
                VertexOutput vert(VertexInput v)
                {
                    VertexOutput o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    float3 V = WorldSpaceViewDir(v.vertex);
                        V = mul(unity_WorldToObject, V);
                    o.NdotV.x = dot(v.normal, normalize(V));
                    return o;
                }

                
                float4 frag(VertexOutput IN) :COLOR
                {
                    float4 c = float4(_MainTex);
                    c.rgb += (1 - IN.NdotV.x)* _RimPower * _RimColor.rgb;
                    return c;
                }
                   
                    ENDCG
            }
        }
        FallBack "Diffuse"

}