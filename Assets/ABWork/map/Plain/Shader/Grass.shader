Shader "Unlit/Grass"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Col("Color",color) = (0,1,0,1)
		_Height("Height",Range(0.1,2)) = 0.2
		_Width("Width",Range(0,2)) = 0.1
		_Density("Density",Range(1,20))=2
		_SwingStrength("Swing Strength",Range(0,1))=0.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" ="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		//ZWrite off
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry  geom
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct gs_input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct g2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float color:COLOR0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Col;
			float _Width;
			float _Height;
			int _Density;
			float _SwingStrength;

			uniform float angle2rad = 3.1415926/180.0;

			gs_input vert (appdata_base v)
			{
				gs_input o = (gs_input)0;
				o.pos = v.vertex;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed2 randPos(fixed2 value){
				fixed2 pos = fixed2(dot(value, fixed2(127.1, 337.1)), dot(value, fixed2(269.5, 183.3)));
				pos = frac(sin(pos) * 43758.5453123);
				return pos;
			}
			fixed rand(float x)
			{
				return frac(sin(x));
			}

			float3 rotate(float3 pos,float angle)
			{
				float rad = radians(angle);
				float x = pos.x;
				float z = pos.z; 
				pos.x = x*cos(rad) - z*sin(rad);
				pos.z = x*sin(rad) + z*cos(rad);
				return pos;
			}
			//offset位置
			void genGrass(float3 pos ,inout TriangleStream<g2f> triStream)
			{

				int split = 2;

				float strength =0.2;
		
				float width = _Width;

				float height = _Height + rand((pos.x*24.2315+pos.z*0.1545))*(_Height*0.75);
				float maxHeight = height*split;

				fixed v = 1.0/split-0.01;
				float layer = v;
				//倾斜
				float3 lean = float3(0,0,1);
				//摆动
				float swing = cos(_Time.y+pos.x+pos.z);
				float2 randXY = (randPos(pos.xz)*0.5-0.5)*swing*_SwingStrength;
				float3 offset = float3(randXY.x,0,randXY.y);
				float randValue =  rand(pos.x+pos.z+(pos.x+pos.z)*0.155124)*0.5-0.5;
				//随机颜色
				float col = 1.0 -rand((pos.x+pos.z)*15615)*0.3;
				//x'=xcosα-ysinα
				//y'=xsinα+ycosα
				float angle = rand((pos.x+pos.z)*5204+height)*360;
	
				for(int i=0;i<split;i++)
				{

					g2f f1 =(g2f)0;
					float bendStrength =pow(pos.y/maxHeight,2);
					lean.z = randValue*bendStrength;
					f1.vertex = UnityObjectToClipPos(rotate(float3(width,0,0)+lean,angle) +pos+offset*bendStrength);
					f1.uv = float2(1,v-layer);
					f1.color = col;
					triStream.Append(f1);

					g2f f2 =(g2f)0;
					bendStrength = pow(pos.y/maxHeight,2);
					lean.z = randValue*bendStrength;
					f2.vertex = UnityObjectToClipPos(rotate(float3(-width,0,0)+lean,angle) +pos+offset*bendStrength);
					f2.uv = float2(0,v-layer);
					f2.color = col;
					triStream.Append(f2);

					g2f f3 =(g2f)0;
					bendStrength = pow((pos.y+height)/maxHeight,2);
					lean.z =  randValue*bendStrength;
					f3.vertex = UnityObjectToClipPos(rotate(float3(width,height,0)+lean,angle) +pos+offset*bendStrength);
					f3.uv = float2(1,v);
					f3.color = col;
					triStream.Append(f3);

					g2f f4 =(g2f)0;
					bendStrength = pow((pos.y+height)/maxHeight,2);
					lean.z = randValue*bendStrength;
					f4.vertex = UnityObjectToClipPos(rotate(float3(-width,height,0)+lean,angle) +pos+offset*bendStrength);
					f4.uv = float2(0,v);
					f4.color = col;
					triStream.Append(f4);

					pos.y+=height;
					v += layer;
				}
				triStream.RestartStrip();
			}
			
            [maxvertexcount(50)]
            void geom(triangle gs_input p[3], inout TriangleStream<g2f> triStream)
            {
				float3 offset = (p[0].pos.xyz + p[1].pos.xyz +p[2].pos.xyz)/3;

				offset.xz += (randPos(offset.xz)*0.5-0.25);

				for(int i =0 ;i<_Density;i++)
					genGrass(offset +float3(rand(offset.x+offset.z+i*0.215)*0.5-0.5,0,rand(offset.x+offset.z+i*0.345)*0.5-0.5),triStream);
            }
			
			fixed4 frag (g2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a-0.1);
				col = col*_Col*i.color;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
