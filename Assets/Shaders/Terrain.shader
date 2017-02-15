Shader "Custom/Terrain" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

		_Tex1  ("Low Altitude",   2D) = "white" {}
		_Tex2  ("Medium Altitude",   2D) = "white" {}
		_Tex3  ("High Altitude",   2D) = "white" {}
		_Tex4  ("Cliffs",   2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Tex1;
		sampler2D _Tex2;
		sampler2D _Tex3;
		sampler2D _Tex4;
		sampler2D _Splat;

		fixed _MaxHeight = 0.9526195;

		struct Input {
			float2 uv_Tex1;
			float2 uv_Tex2;
			float2 uv_Tex3;
			float2 uv_Tex4;
			float2 uv_Splat;
			float3 worldPos;
			float3 worldNormal;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		const float PI = 3.14159;

		void surf (Input IN, inout SurfaceOutputStandard o) {

		    fixed4 splat = tex2D(_Splat, IN.uv_Splat);

		    float magnitude = max(splat.r + splat.g + splat.b, 0.001);

		    splat.r = splat.r / magnitude;
		    splat.g = splat.g / magnitude;
		    splat.b = splat.b / magnitude;

			// Albedo comes from a texture tinted by color
			fixed4 cL = tex2D (_Tex1, IN.uv_Tex1);  //low altitude
			fixed4 cM = tex2D (_Tex2, IN.uv_Tex2);  //medium altitude
			fixed4 cH = tex2D (_Tex3, IN.uv_Tex3);  //high altitude
			fixed4 cS = tex2D (_Tex4, IN.uv_Tex4);  //steep slope

			//height cutoffs
			float low = _MaxHeight * 0.5;
			float med = _MaxHeight * 0.85;

			//calculating low-medium height values
			float l1 = min(IN.worldPos.y / low, 1.0);
			fixed4 c_low = lerp(cL, cM, l1);
			//calculating meding-high height values
			float l2 = max((IN.worldPos.y - low) / (med - low), 0.0);
			fixed4 c_height = lerp(c_low, cH, l2);
			//calculating slope of the surface
			//dot the normal and up to get the compliment of the steepness angle
			float l3 = dot( fixed3(0, 1, 0), IN.worldNormal);

			l3 = 1 - cos(PI/2 - acos(l3));

			fixed4 c = lerp(c_height, cS, l3);


			o.Albedo = c.rgb;


			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
