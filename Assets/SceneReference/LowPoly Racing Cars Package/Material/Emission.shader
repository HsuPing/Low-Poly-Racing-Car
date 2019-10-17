// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Emission"
{
	Properties
    {
        _MainTex("main tex", 2D) = "gray"{} 
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


                sampler2D _MainTex;
                float4 _RimColor;
                float _RimPower;

               
                VertexOutput vert(VertexInput v)
                {
                    VertexOutput o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    float3 V = WorldSpaceViewDir(v.vertex);
                        V = mul(unity_WorldToObject, V);//视方向从世界到模型坐标系的转换
                    o.NdotV.x = dot(v.normal, normalize(V));//必须在同一坐标系才能正确做点乘运算
                    return o;
                }

                
                float4 frag(VertexOutput IN) :COLOR
                {
                    float4 c = tex2D(_MainTex, IN.uv);
                    //用视方向和法线方向做点乘，越边缘的地方，法线和视方向越接近90度，点乘越接近0.
                    //用（1- 上面点乘的结果）*颜色，来反映边缘颜色情况
                    c.rgb += (1 - IN.NdotV.x)* _RimPower * _RimColor.rgb;
                    return c;
                }
                   
                    ENDCG
            }
        }
        FallBack "Diffuse"

}