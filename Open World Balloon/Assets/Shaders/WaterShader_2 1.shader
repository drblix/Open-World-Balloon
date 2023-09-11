// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Water_2" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _DepthRampTex ("Depth Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", float) = 0.0
        _WaveAmp ("Wave Amplitude", float) = 0.0
        _DepthFactor ("Depth Factor", float) = 0.0
    }

    SubShader {
    
        Tags { 
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
        }
        LOD 200

        CGPROGRAM
        #include "UnityCG.cginc"

        #pragma surface surf Lambert
        //#pragma vertex vert
		//#pragma fragment frag

        sampler2D _MainTex;
        sampler2D _CameraDepthTexture;
        sampler2D _DepthRampTex;
        fixed4 _Color;
        float _WaveSpeed;
        float _WaveAmp;
        float _DepthFactor;

        struct Input {
            float2 uv_MainTex;
        };

        struct vertexInput {
			float4 vertex : POSITION;
			float4 texCoord : TEXCOORD1;
		};

		struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 texCoord : TEXCOORD0;
			float4 screenPos : TEXCOORD1;
		};

		vertexOutput vert(vertexInput input) {
			vertexOutput output;

			output.pos = UnityObjectToClipPos(input.vertex);

			//float noiseSample = tex2Dlod(_NoiseTex, float4(input.texCoord.xy, 0, 0));
			output.pos.y += sin(_Time * _WaveSpeed/* * noiseSample*/ + output.pos.x) * _WaveAmp/* + _ExtraHeight*/;
			output.pos.x += cos(_Time * _WaveSpeed/* * noiseSample*/ + output.pos.z) * _WaveAmp;

			output.screenPos = ComputeScreenPos(output.pos);

			output.texCoord = input.texCoord;

			return output;
		}

		float4 frag(vertexOutput input) : COLOR {
			float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
			float depth = LinearEyeDepth(depthSample).r;

			float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));
			float4 foamRamp = float4(tex2D(_DepthRampTex, float2(foamLine, 0.5)).rgb, 1.0);

			float4 albedo = tex2D(_MainTex, input.texCoord.xy);

			float4 col = _Color * foamRamp * albedo;
            return col;
		}

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        
        ENDCG
    }

    Fallback "Legacy Shaders/VertexLit"
}