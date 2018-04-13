Shader "Custom/Shader_v2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Glossiness ("Reflective Map", 2D) = "bump" {}
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Parallax("Height", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Heightmap (A)", 2D) = "black" {}
		
		_Transition ("Transition", Range(0.01,1)) = 0.0
		
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

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;

			fixed3 diff : COLOR0;
			fixed3 ambient : COLOR1;
		};

		v2f vert(appdata_base v)
		{
			v2f o;

			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;

			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half n1 = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
			o.diff = n1 * _LightColor0.rgb;
			o.ambient = ShadeSH9(half4(worldNormal, 1));

			return o;
		}

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
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 daRed = tex2D(_Red, IN.uv_MainTex);
			fixed4 daBlue = tex2D(_Blue, IN.uv_MainTex);
			fixed4 daGreen = tex2D(_Green, IN.uv_MainTex);

			float sumrgb = (daRed.r + daGreen.g + daBlue.b) / 3.0;
			float4 white = float4(1.0, 1.0, 1.0, 1.0);
			col = (1 - sumrgb) * white + (sumrgb * float4(daRed.r * col.r, daGreen.g * col.g, daBlue.b * col.b, col.a));
			
			half h = tex2D(_ParallaxMap, IN.uv_BumpMap).w;
			float2 offset = ParallaxOffset(h, _Parallax, IN.viewDir);
			IN.uv_MainTex += offset;
			IN.uv_BumpMap += offset;

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = col.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic * _Transition;
			o.Smoothness = _Glossiness * _Transition;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) * _Transition;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
