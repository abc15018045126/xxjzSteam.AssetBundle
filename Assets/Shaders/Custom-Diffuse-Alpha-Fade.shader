Shader "Custom/Diffuse-Alpha-Fade" {
	Properties {
		[HideInInspector] _Color ("Color",color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Opacity ("Alpha Fade", Range(0,1)) = 1
		[Toggle(BLOW)] _UseBlow ("Use Blow?", float) = 0
		_Blow ("Blow", vector) = (0.01,0.01,50,50)
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
	}

	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		LOD 300
		Cull [_Cull]

		CGPROGRAM
		#pragma surface surf Lambert alpha:blend vertex:vert 
		#pragma shader_feature SPLIT
		#pragma shader_feature BLOW

		sampler2D _MainTex;
		half _Opacity;

#if BLOW
		float4 _Blow;
#endif

		struct Input {
			float2 uv_MainTex;
		};

		void vert(inout appdata_full v, out Input i)
		{
			UNITY_INITIALIZE_OUTPUT(Input, i);
#if BLOW
			float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
			float2 offset = TransformViewToProjection(norm.xy);
			v.vertex.xy += sin((_Time.x + offset.xy) * _Blow.zw) * _Blow.xy;
#endif
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a * _Opacity;
		}
		ENDCG
	}

	SubShader {
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
		Cull[_Cull]

		CGPROGRAM
		#pragma surface surf Lambert alpha:blend

		sampler2D _MainTex;
		half _Opacity;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a * _Opacity;
		}
		ENDCG
	}
	
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
