Shader "Custom/Water"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_DepthFactor("Depth Factor", float) = 1.0
		_WaveSpeed("Wave Speed", float) = 1.0
		_WaveAmp("Wave Amp", float) = 0.2
		_DepthRampTex("Depth Ramp", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_MainTex("Main Texture", 2D) = "white" {}
		_DistortStrength("Distort Strength", float) = 1.0
		_ExtraHeight("Extra Height", float) = 0.0
	}

	SubShader
	{
        Tags
		{ 
			"Queue" = "Transparent"
		}

        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            // Properties
            sampler2D _BackgroundTexture;
            sampler2D _NoiseTex;
            float _DistortStrength;
			float _WaveSpeed;
			float _WaveAmp;

            struct vertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 texCoord : TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                output.pos = UnityObjectToClipPos(input.vertex);
                float4 normal4 = float4(input.normal, 0.0);
				float3 normal = normalize(mul(normal4, unity_WorldToObject).xyz);

                output.grabPos = ComputeGrabScreenPos(output.pos);

                // distort based on bump map
				float noiseSample = tex2Dlod(_NoiseTex, float4(input.texCoord.xy, 0, 0));
				output.grabPos.y += sin(_Time*_WaveSpeed*noiseSample)*_WaveAmp * _DistortStrength;
                output.grabPos.x += cos(_Time*_WaveSpeed*noiseSample)*_WaveAmp * _DistortStrength;

                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                return tex2Dproj(_BackgroundTexture, input.grabPos);
            }
            ENDCG
        }

		Pass
		{
            Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
            #include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag
			
			// Properties
			float4 _Color;
			float4 _EdgeColor;
			float _DepthFactor;
			float _WaveSpeed;
			float _WaveAmp;
			float _ExtraHeight;
			sampler2D _CameraDepthTexture;
			sampler2D _DepthRampTex;
			sampler2D _NoiseTex;
			sampler2D _MainTex;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texCoord : TEXCOORD1;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 texCoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = UnityObjectToClipPos(input.vertex);

				float noiseSample = tex2Dlod(_NoiseTex, float4(input.texCoord.xy, 0, 0));
				output.pos.y += sin(_Time * _WaveSpeed * noiseSample + output.pos.x) * _WaveAmp + _ExtraHeight;
				output.pos.x += cos(_Time * _WaveSpeed * noiseSample + output.pos.z) * _WaveAmp;

				output.screenPos = ComputeScreenPos(output.pos);

				output.texCoord = input.texCoord;

				return output;
			}

			float4 frag(vertexOutput input) : COLOR
			{
				float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
				float depth = LinearEyeDepth(depthSample).r;

				float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));
				float4 foamRamp = float4(tex2D(_DepthRampTex, float2(foamLine, 0.5)).rgb, 1.0);

				float4 albedo = tex2D(_MainTex, input.texCoord.xy);

			    float4 col = _Color * foamRamp * albedo;
                return col;
			}

			ENDCG
		}
	}
}