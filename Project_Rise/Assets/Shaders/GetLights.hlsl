#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _SHADOWS_SOFT

void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out half DistanceAtten, out half ShadowAtten) {
#if SHADERGRAPH_PREVIEW
	Direction = float3(0.5, 0.5, 0);
	Color = 1;
	DistanceAtten = 1;
	ShadowAtten = 1;
#else
	#if SHADOWS_SCREEN
		half4 clipPos = TransformWOrldToHClip(WorldPos);
		half4 shadowCoord = ComputeScreenPos(clipPos);
	#else
		half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	#endif
	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAtten = mainLight.distanceAttenuation;
	ShadowAtten = mainLight.shadowAttenuation;
#endif

}

void AdditionalLight_float(float3 WorldPos, int Index, out float3 Direction, out float3 Color, out float DistanceAtten, out float ShadowAtten)
{
	Direction = normalize(float3(0.5f, 0.5f, 0.25f));
	Color = float3(0.0f, 0.0f, 0.0f);
	DistanceAtten = 0.0f;
	ShadowAtten = 0.0f;
#ifndef SHADERGRAPH_PREVIEW
	int pixelLightCount = GetAdditionalLightsCount();
	if (Index < pixelLightCount)
	{
		Light light = GetAdditionalLight(Index, WorldPos);

		Direction = light.direction;
		Color = light.color;
		DistanceAtten = light.distanceAttenuation;
		ShadowAtten = light.shadowAttenuation;
	}
#endif
}

#endif
