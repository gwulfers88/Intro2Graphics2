Shader "Custom/Water" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Amount ("Amplitude", float) = 1
		_Frequency ("Frequency", float) = 1
		_Offset ("Offset", float) = 0
		_Center ("Center", Vector) = (0, 0, 0, 0)
		_CubeMap("Cube Map", CUBE) = ""{}
		_Reflection("Reflection", Range(0, 1)) = 0
		_BumpMap("Bumpmap", 2D) = "bump" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert //alpha:fade

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

		half _Glossiness;
		half _Metallic;
		half _Amount;
		half _Frequency;
		half _Offset;
		half2 _Center;
		half _Reflection;

		fixed4 _Color;

		void vert(inout appdata_full v)
		{
			half2 uv = v.texcoord.xy;

			uv -= _Center;

			half x = uv.x * uv.x + uv.y * uv.y;

			half a = _Amount * sin(_Frequency * (x - _Offset * _Time.x));
			v.vertex.xyz += v.normal * a;
		}

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			//o.Emission = texCUBE(_CubeMap, IN.worldReflection).rgb * _Reflection;
			o.Emission = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal)).rgb * _Reflection;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
