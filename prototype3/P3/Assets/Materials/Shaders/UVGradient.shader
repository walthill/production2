Shader "Custom/UVGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LowColor ("Color1", Color) = (0,0,0,1)
        _HighColor ("Color2", Color) = (1,1,1,1)
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        //LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _LowColor;
            float4 _HighColor;
            struct appdata
            {
                float4 position : POSITION;
                float3 normal : Normal;
                float2 uv : TEXCOORD0;
            };
            struct vertData
            {
                float4 position: POSITION;
                float3 normal : Texcoord1;
                float2 uv: TEXCOORD0;
            };

            vertData vert (appdata v)
            {
                vertData o;
                o.position = UnityObjectToClipPos(v.position);
                o.uv = v.uv;
                o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0));
                o.normal = normalize(o.normal);
                return o;
            }

            fixed4 frag (vertData i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = lerp(_HighColor, _LowColor, i.uv.x);
                
                float3 normal = (i.normal);
                col = float4(normal * 0.5 + 0.5, 1);
                col = float4(col.xyz, 1);
                return col;
            }
            ENDCG
        }
    }
}
