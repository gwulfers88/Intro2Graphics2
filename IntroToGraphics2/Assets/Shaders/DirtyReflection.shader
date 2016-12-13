Shader "Custom/DirtyReflection" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_CubeMap("Cube Map", CUBE) = ""{}
		_ReflectionMap("Reflection Map", 2D) = "White" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _CubeMap;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap; 
			float3 worldRefl;
			INTERNAL_DATA
		};

		sampler2D _ReflectionMap;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			half r = tex2D(_ReflectionMap, IN.uv_MainTex).r;

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			//o.Emission = texCUBE(_CubeMap, IN.worldReflection).rgb * _Reflection;
			o.Emission = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal)).rgb * r;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

/*
	Make a texture and fill it at some particular values.
	ClickObj->GetHitInfo->GetUV->Transform to Index->Fill with white.
*/