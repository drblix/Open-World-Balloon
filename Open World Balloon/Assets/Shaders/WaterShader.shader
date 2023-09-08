Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AnimationSpeed("Animation Speed", Range(0, 100)) = 0
        _Amplitude("Amplitude", Range(0, 100)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            //#pragma vertex vert
            #pragma vertex vertexFunc
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #define PI 3.141592653589793

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AnimationSpeed;
            float _Amplitude;

            /*
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            */

            v2f vertexFunc(appdata IN)
            {
                v2f OUT;
                
                IN.vertex.y += sin(_Time.x * _AnimationSpeed + IN.vertex.x) * _Amplitude;
                //IN.vertex.z += cos(_Time.x * _AnimationSpeed + IN.vertex.y) * _Amplitude;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;

                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}
