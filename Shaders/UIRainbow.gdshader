shader_type canvas_item;

uniform bool isActive;

vec3 HSVtoRGB(float hue, float saturation, float vibrance)
{
	int hi = int(hue / 60.0) % 6;
	float f = float(hue / 60.0 - float(int(hue / 60.0)));
	vibrance *= 255.0;
	float v = vibrance;
	float p = vibrance * (1.0 - saturation);
	float q = vibrance * (1.0 - f * saturation);
	float t = vibrance * (1.0 - (1.0 - f) * saturation);
	if (hi == 0) 
	{
		return vec3(v / 255.0, t / 255.0 ,p / 255.0);
	}
	else if (hi == 1) 
	{
		return vec3(q / 255.0, v / 255.0 ,p / 255.0);
	}
	else if (hi == 2) 
	{
		return vec3(p / 255.0, v / 255.0 ,t / 255.0);
	}
	else if (hi == 3) 
	{
		return vec3(p / 255.0, q / 255.0 ,v / 255.0);
	}
	else if (hi == 4) 
	{
		return vec3(t / 255.0, p / 255.0 ,v / 255.0);
	}
	else
	{
		return vec3(v / 255.0, p / 255.0 ,q / 255.0);
	}
}

void fragment() 
{
	if(isActive && texture(TEXTURE, UV).r > 0.0 && texture(TEXTURE, UV).g > 0.0 && texture(TEXTURE, UV).b > 0.0)
	{
		float hue = 0.0;
		float saturation = 1.0;
		float vibrance = 1.0;
		hue = UV.x * 360.0 + TIME * 50.0;
		hue = float(int(hue) % 360);
		COLOR = vec4(HSVtoRGB(hue, saturation ,vibrance), texture(TEXTURE, UV).a);
	}
	else 
	{
		COLOR = texture(TEXTURE, UV);
	}
}
