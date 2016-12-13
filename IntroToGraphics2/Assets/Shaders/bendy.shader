Shader "Custom/bendy" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ControlPoint("Control Point", Vector) = (0, 0, 0, 0)
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Front

		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		half4 _ControlPoint;

		struct Input 
		{
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void vert (inout appdata_full v)
		{
			half t = v.texcoord.y;
			half t_one = 1 - t;

			half p = 2 * _ControlPoint.x * t_one * t +
			_ControlPoint.y * t * t;

			//half p = 3 * _ControlPoint.x * t_one * t +
			//3 * _ControlPoint.y * t_one * t * t + 
			//_ControlPoint.z * t * t * t;

			v.vertex.xyz += v.tangent * p;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
