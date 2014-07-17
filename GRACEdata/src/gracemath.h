#ifndef GRACEDATA_MATH
#define GRACEDATA_MATH

#include <stdio.h>
#include <tchar.h>
#include <math.h>

namespace GRACEdata 
{
	class math
	{
	public:
		static float velocity(float deltadistance, float deltatime);
		static float acceleration(float deltavelocity, float deltatime);
		static float acceleration(float initialvelocity, float finalvelocity, float deltadistance);
		static float distance(float deltax, float deltay);
	};
}

#endif