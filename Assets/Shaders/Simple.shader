Shader "iPhone/Simple"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_LightAmount ("Light Amount", Range(0,1)) = 0.5
		_RimColor ("Rim Color", Color) = (0,0,0,0)
		_RimPower ("Rim Power", Range(0.5,15.0)) = 3.0
    }

    SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert halfasview approxview noforwardadd nolightmap nodirlightmap exclude_path:prepass 
		#pragma target 3.0

		struct Input {
          float2 uv_MainTex;
          float3 viewDir;
		};

		sampler2D _MainTex;
		half _LightAmount;
		fixed4 _RimColor;
		half _RimPower;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed3 diff = tex2D(_MainTex, IN.uv_MainTex).rgb;
			o.Albedo = diff * _LightAmount;
			half rimP = pow(1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)), _RimPower) * _RimColor.a;
			fixed3 rim = rimP * _RimColor.rgb;
			o.Emission = diff * (1 - _LightAmount) + rim;
		}
		ENDCG
    }

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		CGPROGRAM
		#pragma surface surf Lambert nolightmap nodirlightmap exclude_path:prepass
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		sampler2D _MainTex;
		fixed4 _RimColor;
		half _RimPower;

		void surf(Input IN, inout SurfaceOutput o) {

			fixed3 diff = tex2D(_MainTex, IN.uv_MainTex).rgb;
			half rimP = pow(1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)), _RimPower) * _RimColor.a;
			fixed3 rim = rimP * _RimColor.rgb;
			o.Emission = diff + rim;
		}
		ENDCG
	}

    Fallback "Mobile/VertexLit"	
}

