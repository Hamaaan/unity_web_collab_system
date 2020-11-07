Shader "Unlit/ColorChange"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

        _Color("Color", Color) = (1,1,1,1)

        [MaterialToggle] _IsNega("Is Nega", Float) = 0

        _Hue("Hue", Float) = 0         //色相
        _Sat("Saturation", Float) = 1  //彩度
        _Val("Value", Float) = 1       //明度
    }

    SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100
            Cull off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;

                    float4 color    : COLOR;

                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;

                    fixed4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                fixed4 _Color;

                Float _IsNega;

                half _Hue, _Sat, _Val;

                

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                half3 shift = half3(_Hue, _Sat, _Val);

                col = col*_Color;

                if (_IsNega == 0)
                {
                    return col;

                }
                else {
                    col.rgb = 1 - col.rgb;
                    return col;
                }
            }
            ENDCG
        }
    }
}
