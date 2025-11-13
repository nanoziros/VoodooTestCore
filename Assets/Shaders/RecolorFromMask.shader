Shader "UI/RecolorFromMask"
{
    Properties
    {
        _MainTex ("Main Tex (Beauty)", 2D) = "white" {}
        _MaskTex ("Mask Tex", 2D) = "black" {}

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile __ UNITY_UI_CLIP_RECT UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _MaskTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                OUT.worldPosition = IN.vertex;
                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, IN.uv);
                fixed mask = tex2D(_MaskTex, IN.uv).r;

                fixed3 tintColor = IN.color.rgb;
                fixed3 tinted = baseCol.rgb * tintColor;
                fixed3 finalRGB = lerp(baseCol.rgb, tinted, mask);

                fixed4 col = fixed4(finalRGB, baseCol.a * IN.color.a);
                
                return col;
            }
            ENDCG
        }
    }
}
