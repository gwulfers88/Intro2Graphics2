Shader "Custom/burn"
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Thresh1("Threshold 1", Range(0, 1)) = 0.0
		_Thresh2("Threshold 2", Range(0, 1)) = 0.0
		_SaturateTex("Saturation Pattern", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SaturateTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Thresh1;
		half _Thresh2;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 d = tex2D(_SaturateTex, IN.uv_MainTex);

			c.a = (d.r < _Thresh1 ? 0 : 1);
			c *= (d.r < _Thresh2 ? _Color : fixed4(1, 1, 1, 1));

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
