// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:273,x:35277,y:32654,varname:node_273,prsc:2|emission-6048-OUT,alpha-9039-OUT,voffset-3824-OUT;n:type:ShaderForge.SFN_Tex2d,id:4386,x:34443,y:33110,ptovrint:False,ptlb:zhezhao,ptin:_zhezhao,varname:node_4386,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7902-OUT;n:type:ShaderForge.SFN_Tex2d,id:2519,x:32482,y:32663,ptovrint:False,ptlb:noise1,ptin:_noise1,varname:node_2519,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6534-OUT;n:type:ShaderForge.SFN_Tex2d,id:7339,x:33696,y:32549,ptovrint:False,ptlb:Main,ptin:_Main,varname:node_7339,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-184-OUT;n:type:ShaderForge.SFN_TexCoord,id:1596,x:31862,y:32631,varname:node_1596,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:6534,x:32244,y:32680,varname:node_6534,prsc:2|A-1596-UVOUT,B-5498-OUT;n:type:ShaderForge.SFN_Vector4Property,id:4103,x:31732,y:32778,ptovrint:False,ptlb:n1_v,ptin:_n1_v,varname:node_4103,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Append,id:2841,x:31921,y:32789,varname:node_2841,prsc:2|A-4103-X,B-4103-Y;n:type:ShaderForge.SFN_Multiply,id:5498,x:32090,y:32789,varname:node_5498,prsc:2|A-2841-OUT,B-7546-T;n:type:ShaderForge.SFN_Time,id:7546,x:31911,y:32974,varname:node_7546,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1980,x:32711,y:32725,varname:node_1980,prsc:2|A-2519-A,B-8376-OUT;n:type:ShaderForge.SFN_Slider,id:8376,x:32429,y:32895,ptovrint:False,ptlb:niuqu_1,ptin:_niuqu_1,varname:node_8376,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:1275,x:32699,y:33099,ptovrint:False,ptlb:noise2,ptin:_noise2,varname:node_1275,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2131-OUT;n:type:ShaderForge.SFN_TexCoord,id:1498,x:32040,y:33129,varname:node_1498,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:2131,x:32329,y:33113,varname:node_2131,prsc:2|A-1498-UVOUT,B-4334-OUT;n:type:ShaderForge.SFN_Append,id:8712,x:31963,y:33316,varname:node_8712,prsc:2|A-2037-X,B-2037-Y;n:type:ShaderForge.SFN_Multiply,id:4334,x:32242,y:33360,varname:node_4334,prsc:2|A-8712-OUT,B-7590-T;n:type:ShaderForge.SFN_Time,id:7590,x:31717,y:33487,varname:node_7590,prsc:2;n:type:ShaderForge.SFN_Vector4Property,id:2037,x:31766,y:33316,ptovrint:False,ptlb:n2_v,ptin:_n2_v,varname:node_2037,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Multiply,id:8290,x:32900,y:33099,varname:node_8290,prsc:2|A-1275-A,B-1992-OUT;n:type:ShaderForge.SFN_Slider,id:1992,x:32526,y:33464,ptovrint:False,ptlb:niuqu_2,ptin:_niuqu_2,varname:node_1992,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:7831,x:33080,y:32869,varname:node_7831,prsc:2|A-1980-OUT,B-8290-OUT;n:type:ShaderForge.SFN_TexCoord,id:2652,x:33124,y:32645,varname:node_2652,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:184,x:33459,y:32721,varname:node_184,prsc:2|A-2652-UVOUT,B-7831-OUT,C-3601-OUT;n:type:ShaderForge.SFN_Vector4Property,id:6304,x:34544,y:33313,ptovrint:False,ptlb:node_6304,ptin:_node_6304,varname:node_6304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Multiply,id:3824,x:34862,y:33133,varname:node_3824,prsc:2|A-4386-RGB,B-6304-XYZ;n:type:ShaderForge.SFN_Tex2d,id:8232,x:33696,y:32763,ptovrint:False,ptlb:mask,ptin:_mask,varname:_zhezhao_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6048,x:34521,y:32566,varname:node_6048,prsc:2|A-8652-OUT,B-8450-RGB;n:type:ShaderForge.SFN_Multiply,id:6982,x:34260,y:32768,varname:node_6982,prsc:2|A-7339-A,B-8232-G;n:type:ShaderForge.SFN_Vector4Property,id:151,x:33068,y:33055,ptovrint:False,ptlb:Main_v,ptin:_Main_v,varname:node_151,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Append,id:9619,x:33270,y:33055,varname:node_9619,prsc:2|A-151-X,B-151-Y;n:type:ShaderForge.SFN_Multiply,id:3601,x:33563,y:33069,varname:node_3601,prsc:2|A-9619-OUT,B-6677-T;n:type:ShaderForge.SFN_Time,id:6677,x:33146,y:33248,varname:node_6677,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:3707,x:33834,y:33064,varname:node_3707,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector4Property,id:9526,x:33797,y:33299,ptovrint:False,ptlb:Z_v,ptin:_Z_v,varname:node_9526,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Append,id:762,x:34014,y:33345,varname:node_762,prsc:2|A-9526-X,B-9526-Y;n:type:ShaderForge.SFN_Multiply,id:5939,x:34186,y:33355,varname:node_5939,prsc:2|A-762-OUT,B-6132-T;n:type:ShaderForge.SFN_Time,id:6132,x:34014,y:33524,varname:node_6132,prsc:2;n:type:ShaderForge.SFN_Add,id:7902,x:34232,y:33127,varname:node_7902,prsc:2|A-3707-UVOUT,B-5939-OUT;n:type:ShaderForge.SFN_Color,id:6766,x:33696,y:32343,ptovrint:False,ptlb:color,ptin:_color,varname:node_6766,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:8652,x:34097,y:32491,varname:node_8652,prsc:2|A-6766-RGB,B-7339-RGB;n:type:ShaderForge.SFN_VertexColor,id:8450,x:34250,y:32627,varname:node_8450,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9039,x:34743,y:32806,varname:node_9039,prsc:2|A-8450-A,B-6982-OUT,C-8232-A;proporder:2519-7339-4103-8376-1275-2037-1992-4386-6304-8232-151-9526-6766;pass:END;sub:END;*/

Shader "YB/YB_UVliudong" {
    Properties {
        _noise1 ("noise1", 2D) = "white" {}
        _Main ("Main", 2D) = "white" {}
        _n1_v ("n1_v", Vector) = (0,0,0,0)
        _niuqu_1 ("niuqu_1", Range(0, 1)) = 0
        _noise2 ("noise2", 2D) = "white" {}
        _n2_v ("n2_v", Vector) = (0,0,0,0)
        _niuqu_2 ("niuqu_2", Range(0, 1)) = 0
        _zhezhao ("zhezhao", 2D) = "white" {}
        _node_6304 ("node_6304", Vector) = (0,0,0,0)
        _mask ("mask", 2D) = "white" {}
        _Main_v ("Main_v", Vector) = (0,0,0,0)
        _Z_v ("Z_v", Vector) = (0,0,0,0)
        [HDR]_color ("color", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _zhezhao; uniform float4 _zhezhao_ST;
            uniform sampler2D _noise1; uniform float4 _noise1_ST;
            uniform sampler2D _Main; uniform float4 _Main_ST;
            uniform float4 _n1_v;
            uniform float _niuqu_1;
            uniform sampler2D _noise2; uniform float4 _noise2_ST;
            uniform float4 _n2_v;
            uniform float _niuqu_2;
            uniform float4 _node_6304;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float4 _Main_v;
            uniform float4 _Z_v;
            uniform float4 _color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                float4 node_6132 = _Time + _TimeEditor;
                float2 node_7902 = (o.uv0+(float2(_Z_v.r,_Z_v.g)*node_6132.g));
                float4 _zhezhao_var = tex2Dlod(_zhezhao,float4(TRANSFORM_TEX(node_7902, _zhezhao),0.0,0));
                v.vertex.xyz += (_zhezhao_var.rgb*_node_6304.rgb);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_7546 = _Time + _TimeEditor;
                float2 node_6534 = (i.uv0+(float2(_n1_v.r,_n1_v.g)*node_7546.g));
                float4 _noise1_var = tex2D(_noise1,TRANSFORM_TEX(node_6534, _noise1));
                float4 node_7590 = _Time + _TimeEditor;
                float2 node_2131 = (i.uv0+(float2(_n2_v.r,_n2_v.g)*node_7590.g));
                float4 _noise2_var = tex2D(_noise2,TRANSFORM_TEX(node_2131, _noise2));
                float4 node_6677 = _Time + _TimeEditor;
                float2 node_184 = (i.uv0+((_noise1_var.a*_niuqu_1)*(_noise2_var.a*_niuqu_2))+(float2(_Main_v.r,_Main_v.g)*node_6677.g));
                float4 _Main_var = tex2D(_Main,TRANSFORM_TEX(node_184, _Main));
                float3 emissive = ((_color.rgb*_Main_var.rgb)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                fixed4 finalRGBA = fixed4(finalColor,(i.vertexColor.a*(_Main_var.a*_mask_var.g)*_mask_var.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _zhezhao; uniform float4 _zhezhao_ST;
            uniform float4 _node_6304;
            uniform float4 _Z_v;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_6132 = _Time + _TimeEditor;
                float2 node_7902 = (o.uv0+(float2(_Z_v.r,_Z_v.g)*node_6132.g));
                float4 _zhezhao_var = tex2Dlod(_zhezhao,float4(TRANSFORM_TEX(node_7902, _zhezhao),0.0,0));
                v.vertex.xyz += (_zhezhao_var.rgb*_node_6304.rgb);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
