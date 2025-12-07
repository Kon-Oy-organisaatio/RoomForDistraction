Shader "Custom/ProceduralSky"
{
  Properties
  {
    _SkyTint("Sky Tint", Color) = (0.53,0.75,0.92,1)
    _GroundTint("Ground Tint", Color) = (0.32,0.28,0.25,1)
    _AtmosphereThickness("Atmosphere Thickness", Range(0.1,8)) = 1.5
    _Exposure("Exposure", Range(0,8)) = 1.0

    _SunColor("Sun Color", Color) = (1,0.97,0.85,1)
    _SunSize("Sun Size (rad deg)", Range(0.001,0.2)) = 0.04
    _SunConvergence("Sun Convergence", Range(1,32)) = 8

    _CubeTex("Overlay Cubemap", Cube) = "" {}
    _CubemapBlend("Cubemap Blend", Range(0,1)) = 0.5

    _GroundStartHeight("Ground Start Height", Range(-1,1)) = -0.2
    _GroundStartInvert("Invert Ground Y for Ground Start", Range(0,1)) = 0
    _GroundCoverage("Ground Coverage (bottom-up)", Range(0,1)) = 0.5
    _GroundBlendWidth("Ground Blend Width", Range(0,1)) = 0.2

    [Header(Clouds)]
    _CloudNoise("Cloud Noise", 2D) = "black" {}
    _CloudColor("Cloud Color", Color) = (1,1,1,1)
    _CloudScale("Cloud Scale", Range(0.00001, 2.0)) = 0.2
    _CloudSpeed("Cloud Speed", Range(0.0, 1.0)) = 0.05
    _CloudCutoff("Cloud Cutoff", Range(0.0, 1.0)) = 0.5
    _CloudFuzziness("Cloud Fuzziness", Range(0.0, 1.0)) = 0.1
    _CloudSunThrough("Sun Through Clouds", Range(0.0, 1.0)) = 0.2

    [Header(Ground Fog)]
    _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
    _FogHeight("Fog Height", Range(-1.0, 1.0)) = 0.0
    _FogDensity("Fog Density", Range(0.01, 10.0)) = 1.0
    _BuildingFogIntensity("Building Fog Intensity", Range(0.0, 1.0)) = 0.0
  }
  SubShader
  {
    Tags { "Queue" = "Background" "RenderType" = "Background" }
    Cull Off
    ZWrite Off
    Lighting Off

    Pass
    {
      HLSLPROGRAM
      #pragma vertex Vert
      #pragma fragment Frag
      #include "UnityCG.cginc"

      struct appv { float4 vertex : POSITION; };
      struct v2f { float4 pos : SV_POSITION; float3 dir : TEXCOORD0; };

      // Properties (matched to Properties block)
      float4 _SkyTint;
      float4 _GroundTint;
      float _AtmosphereThickness;
      float _Exposure;

      float4 _SunColor;
      float _SunSize;
      float _SunConvergence;

      // which dir.y value maps to start of ground tint (in [-1..1])
      float _GroundStartHeight;
      // 0 = normal, 1 = invert dir.y when computing ground mapping
      float _GroundStartInvert;
      float _GroundCoverage;
      float _GroundBlendWidth;

      sampler2D _CloudNoise;
      float4 _CloudColor;
      float _CloudScale;
      float _CloudSpeed;
      float _CloudCutoff;
      float _CloudFuzziness;
      float _CloudSunThrough;

      float4 _FogColor;
      float _FogHeight;
      float _FogDensity;
      float _BuildingFogIntensity;

      samplerCUBE _CubeTex;
      float _CubemapBlend;

      // Globals set by script: world-space sun direction (normalized)
      float3 _SunDirection; // expected to point TOWARDS the sun (i.e. sunDir)
      
      v2f Vert(appv v)
      {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        // build direction from vertex position for skybox cube-trick
        // normalize view dir in world space using inverse view matrix
        float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
        // for skybox, direction = normalize(worldPos.xyz)
        o.dir = normalize(worldPos.xyz);
        return o;
      }

      // soft-sun disk function (angle in radians)
      static float SunDiskFactor(float cosAngle, float sunSize, float convergence)
      {
        // cosAngle = dot(dir, sunDir)
        // convert sunSize parameter to cosine threshold
        float ang = acos(saturate(cosAngle));
        // soft disk: smoothstep with control by convergence
        float edge = sunSize * convergence;
        float f = saturate(1.0 - smoothstep(sunSize, edge, ang));
        return f;
      }

      // simple tonemap
      static float3 ExposureTone(float3 c, float exposure)
      {
        // filmic-ish simple
        return 1.0 - exp(-c * exposure);
      }

      float3 ProceduralSky(float3 dir)
      {
        float realDirY = lerp(dir.y, -dir.y, saturate(_GroundStartInvert));
        float groundStart = clamp(_GroundStartHeight, -0.999, 0.999);
        float groundEnd = lerp(groundStart, 1.0, saturate(_GroundCoverage));
        float upRaw = (realDirY - groundStart) /
                      max(0.0001, groundEnd - groundStart);
        upRaw = saturate(upRaw);
        float bw = saturate(_GroundBlendWidth);
        float exponent = lerp(0.35, 8.0, 1.0 - bw);
        float up = saturate(pow(upRaw, exponent));
        float g = pow(saturate(up), 1.0 / max(0.0001, _AtmosphereThickness));
        float3 baseSky = lerp(_GroundTint.rgb, _SkyTint.rgb, g);

        // --- Clouds ---
        float cloudMix = 0;
        // Planar mapping for clouds (render only in the sky hemisphere)
        if (realDirY > 0.01)
        {
            // Project view direction to a plane: UV = xz / y
            float2 cloudUV = (dir.xz / realDirY) * _CloudScale;
            cloudUV += _Time.x * _CloudSpeed; // Animate with time
            
            float noise = tex2D(_CloudNoise, cloudUV).r;
            cloudMix = smoothstep(_CloudCutoff, _CloudCutoff + max(0.001, _CloudFuzziness), noise);
            
            // Fade out clouds near the horizon to avoid artifacts
            cloudMix *= saturate((realDirY - 0.01) * 5.0);
        }

        float3 sunDir = normalize(_SunDirection);
        float cosA = dot(dir, sunDir);
        float sunFactor = SunDiskFactor(cosA, _SunSize, _SunConvergence);
        
        // Apply clouds to base sky
        float3 skyWithClouds = lerp(baseSky, _CloudColor.rgb, cloudMix * _CloudColor.a);

        // Tone map the sky/clouds background
        float3 toned = ExposureTone(skyWithClouds, _Exposure);

        // Add sun on top (HDR) to avoid muting by tone mapper
        // Occlude sun by clouds
        float sunOcclusion = cloudMix * _CloudColor.a * (1.0 - _CloudSunThrough);
        float sunVis = saturate(1.0 - sunOcclusion);
        // Apply exposure to sun but skip the compression curve so it can exceed 1.0
        toned += _SunColor.rgb * sunFactor * sunVis * _Exposure;

        return toned;
      }

      float4 Frag(v2f i) : SV_Target
      {
        float3 dir = normalize(i.dir);

        // generate procedural sky color
        float3 proc = ProceduralSky(dir);

        // sample cubemap overlay
        #if UNITY_UV_STARTS_AT_TOP
        float3 cubeDir = float3(dir.x, dir.y, dir.z);
        #else
        float3 cubeDir = dir;
        #endif
        float3 env = texCUBE(_CubeTex, cubeDir).rgb;

        // Smooth alpha from cubemap to avoid hard, aliased seams at cube-face edges.
        // Use the brightest channel to detect content and a small fade width.
        float envMax = max(env.r, max(env.g, env.b));
        const float alphaThreshold = 0.05;     // Increased to remove black spots/fringes
        const float alphaFadeWidth = 0.02;      // smooth area to avoid aliasing
        float envAlpha = smoothstep(alphaThreshold,
                                    alphaThreshold + alphaFadeWidth,
                                    envMax);

        // Respect the user blend slider so the cubemap can be globally faded.
        envAlpha *= saturate(_CubemapBlend);

        // Calculate Ground Fog
        // Fog factor increases as we go down towards and below _FogHeight
        float realDirY = lerp(dir.y, -dir.y, saturate(_GroundStartInvert));
        float groundFog = saturate((_FogHeight - realDirY) * _FogDensity);
        
        // Building fog: combines ground fog with a generic intensity for the buildings
        float buildingFog = saturate(groundFog + _BuildingFogIntensity);
        
        // Mix fog into the cubemap color (buildings)
        env = lerp(env, _FogColor.rgb, buildingFog);
        
        // Composite: where cubemap is present use it, otherwise show procedural sky.
        float3 finalCol = lerp(proc, env, envAlpha);

        // Apply ground fog on top of the final composition (affects both sky and cubemap)
        // We use groundFog here, not buildingFog, so the sky isn't washed out by the generic building fog.
        finalCol = lerp(finalCol, _FogColor.rgb, groundFog * _FogColor.a);

        // Return alpha indicating cubemap opacity (smoothed).
        return float4(finalCol, envAlpha);
      }
      ENDHLSL
    }
  }
  Fallback Off
}
