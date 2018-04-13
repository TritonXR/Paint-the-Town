Shader "Custom/Shader_v2" {
	Properties {

		_Transition("Transition", Range(0.01,1)) = 0.0

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpFactor("Normal Intensity", Range(0.1,5.0)) = 1.0
		_Glossiness ("Reflective Map", 2D) = "bump" {}
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Parallax("Height", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Heightmap (A)", 2D) = "black" {}
		
		
		_Red("Red", 2D) = "black" {}
		_Green("Green", 2D) = "black2" {}
		_Blue("Blue", 2D) = "black3" {}

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
		sampler2D _ParallaxMap;
		sampler2D _Red;
		sampler2D _Green;
		sampler2D _Blue;

		float4 _Color;
		float _Parallax;

		half _Glossiness;
		half _Metallic;
		half _Transition;
		float4 _BumpFactor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			//get the color from the texture and the corresponding uv points on the color alphas
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 daRed = tex2D(_Red, IN.uv_MainTex);
			fixed4 daBlue = tex2D(_Blue, IN.uv_MainTex);
			fixed4 daGreen = tex2D(_Green, IN.uv_MainTex);

			//set the initial color of the object to white
			float sumrgb = (daRed.r + daGreen.g + daBlue.b) / 3.0;
			float4 white = float4(1.0, 1.0, 1.0, 1.0);
			col = (1 - sumrgb) * white + (sumrgb * float4(daRed.r * col.r, daGreen.g * col.g, daBlue.b * col.b, col.a));
			
			//calculate the normal offset for the texture
			half h = tex2D(_ParallaxMap, IN.uv_BumpMap).w;
			float2 offset = ParallaxOffset(h, _Parallax, IN.viewDir);
			IN.uv_MainTex += offset;
			IN.uv_BumpMap += offset * _BumpFactor;

			//set albedo from previous color caluclations based on color alphas
			o.Albedo = col.rgb;
			//set metalic based on slider
			o.Metallic = _Metallic * _Transition;
			//set smoothness from gloss map
			o.Smoothness = _Glossiness * _Transition;
			//set alpha from color
			o.Alpha = col.a;
			//apply normal mapping
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) * _Transition;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
