Shader "Custom/Shader_v2_Standard" {
	Properties {

		_Transition("Transition", Range(0.01,1)) = 0.0

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Glossiness ("Smoothness", range(0,1)) = 1.0
		_GlossinessMap ("Smoothness Alpha", 2D) = "white" {}
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Parallax("Height", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Heightmap (A)", 2D) = "black" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		
		_Red("Red", 2D) = "black" {}
		_Green("Green", 2D) = "black2" {}
		_Blue("Blue", 2D) = "black3" {}

	}
	SubShader {
		Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows Lambert alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _ParallaxMap;
		sampler2D _GlossinessMap;
		sampler2D _Red;
		sampler2D _Green;
		sampler2D _Blue;

		float4 _Color;
		float _Parallax;

		half _Glossiness;
		half _Metallic;
		half _Transition;
		float4 _normalTransition;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_GlossinessMap;
			float3 viewDir;
		};

		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			//get the color from the texture and the corresponding uv points on the color alphas
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			//gets the sample from the alpha textures, has functionality to remap the value based on the transition parameter
			fixed4 daRed = _Transition + (tex2D(_Red, IN.uv_MainTex) - 0) * (1.0 - _Transition) / (1.0 - 0);
			fixed4 daBlue = _Transition + (tex2D(_Blue, IN.uv_MainTex) - 0) * (1.0 - _Transition) / (1.0 - 0);
			fixed4 daGreen = _Transition + (tex2D(_Green, IN.uv_MainTex) - 0) * (1.0 - _Transition) / (1.0 - 0);

			//set the initial color of the object to white
			float sumrgb = (daRed.r + daGreen.g + daBlue.b) / 3.0;
			float4 white = float4(1.0, 1.0, 1.0, 1.0);
			col = (1 - sumrgb) * white + (sumrgb * float4(daRed.r * col.r * _Color.r, daGreen.g * col.g * _Color.g, daBlue.b * col.b * _Color.b, col.a));
			
			//calculate the normal offset for the texture
			half h = tex2D(_ParallaxMap, IN.uv_BumpMap).w;
			float2 offset = ParallaxOffset(h, _Parallax, IN.viewDir);
			IN.uv_MainTex += offset;
			IN.uv_BumpMap += offset;

			//remaps the transition value into a range to be applied to the normal maps
			_normalTransition = 0.9 + (_Transition - 0.1) * (1.0 - 0.9) / (1.0 - 0.1);

			//set albedo from previous color caluclations based on color alphas
			o.Albedo = col.rgb;
			
			//set metalic based on slider
			o.Metallic = _Metallic * _Transition;
			
			//set smoothness from gloss map
			o.Smoothness = _Glossiness * IN.uv_GlossinessMap * _Transition;
			
			//set alpha from color
			o.Alpha = col.a;
			
			//apply normal mapping
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap) * _normalTransition);
		
		}
		ENDCG
	}
	FallBack "Transparent/Cutout/Diffuse"
}
