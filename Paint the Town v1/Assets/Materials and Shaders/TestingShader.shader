Shader "Custom/TestingShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_Red ("Red", Range(0,1)) = 0.0
		//_Green ("Green", Range(0,1)) = 0.0
		//_Blue ("Blue", Range(0,1)) = 0.0

		_Red("Red", 2D) = "black" {}
		_Green("Green", 2D) = "black2" {}
		_Blue("Blue", 2D) = "black3" {}
	}
	SubShader
	{

		Pass
		{
		    Tags{"lighMode" = "ForwardBase"}
            
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;

				SHADOW_COORDS(1)
				fixed3 diff : COLOR0;
				fixed3 ambient : COLOR1;

			};

			v2f vert (appdata_base v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half n1 = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = n1 * _LightColor0.rgb;
				o.ambient = ShadeSH9(half4(worldNormal, 1));
				TRANSFER_SHADOW(o);

				//TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			
			sampler2D _MainTex;

			//float _Red;
			//float _Green;
			//float _Blue;

			sampler2D _Red;
			sampler2D _Green;
			sampler2D _Blue;


			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 daRed = tex2D(_Red, i.uv);
				fixed4 daBlue = tex2D(_Blue, i.uv);
				fixed4 daGreen = tex2D(_Green, i.uv);

				//float sumrgb = (_Red + _Green + _Blue) / 3.0;
				//float4 white = float4(1.0, 1.0, 1.0, 1.0);
				//col = (1 - sumrgb) * white + (sumrgb * float4(_Red * col.r, _Green * col.g, _Blue * col.b, col.a));

				float sumrgb = (daRed.r + daGreen.g + daBlue.b) / 3.0;
				float4 white = float4(1.0, 1.0, 1.0, 1.0);
				col = (1 - sumrgb) * white + (sumrgb * float4(daRed.r * col.r,  daGreen.g * col.g, daBlue.b * col.b, col.a));

				//shadow stuffs
				fixed shadow = SHADOW_ATTENUATION(i);
				fixed lighting = i.diff * shadow + i.ambient;
				col.rgb *= lighting+0.5f;


				return col;
			}
			ENDCG
		}


		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 2.0

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma skip_variants SHADOWS_SOFT
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
	}
	//FALLBACK "VertexLit"
}
