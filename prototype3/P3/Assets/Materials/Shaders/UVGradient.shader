Shader "Custom/UVGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct vertData
            {
                float4 position: POSITION;
                float2 uv: TEXCOORD0;
            };

            vertData vert (appdata v)
            {
                vertData o;
                o.position = UnityObjectToClipPos(v.position);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (vertData i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = float4(i.uv, 1, 1);
                return col;
            }
            ENDCG
        }
    }
}
