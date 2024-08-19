shader_type canvas_item;

uniform float elapsedTime;
uniform float songDuration;

vec3 HSVtoRGB(float hue, float saturation, float vibrance)
{
	vec3 rgb;
	float chroma = saturation * vibrance;
	float Min = vibrance - chroma;
	float hue2;
	if (hue >= 360.0) hue2 = (hue - 360.0)/60.0;
	else hue2 = hue/60.0;
	if (hue2 >= -1.0 && hue2 <= 1.0)
	{
		if (hue2 < 0.0) rgb = vec3(vibrance,Min, Min - hue2 * chroma);
		else rgb = vec3(vibrance, Min + hue2 * chroma, Min);
	}
	else if (hue2 >= 1.0 && hue2 <= 3.0)
	{
		if (hue2 < 2.0) rgb = vec3(Min - (hue2 - 2.0) * chroma, vibrance, Min);
		else rgb = vec3(Min, vibrance, Min + (hue2 - 2.0) * chroma );
	}
	else
	{
		if (hue2 < 4.0) rgb = vec3(Min, Min - (hue2 - 4.0) * chroma, vibrance);
		else rgb = vec3(Min + (hue2 - 4.0) * chroma, Min, vibrance);
	}
	return rgb;
}

void fragment() 
{
	float hue = 0.0;
	float saturation = 1.0;
	float vibrance = 0.1 + elapsedTime / songDuration;
	if (vibrance > 1.0) vibrance = 1.0;
	hue = UV.x * 255.0;
	hue = float(int(hue) % 255);
	hue = hue * 360.0 / 255.0;
	COLOR = vec4(HSVtoRGB(hue, saturation ,vibrance), texture(TEXTURE, UV).a);
}