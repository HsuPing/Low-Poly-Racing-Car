Shader "Custom/Kart" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SpecColor("Specular Color", Color) = (0,0,0,0)		
		_Shininess("Shininess",Float) = 1.0
		_Gloss("Gloss", Float) = 1.0
	}

	SubShader
	{
		Tags {"RenderType" = "Opaque"}
		
		CGPROGRAM
		#pragma surface surf BlinnPhong

		sampler2D _MainTex;
		half _Shininess;
		float _Gloss;
		
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Gloss;
			o.Specular = _Shininess;
			o.Gloss = _Gloss;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
