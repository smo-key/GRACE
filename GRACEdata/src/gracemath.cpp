#include "gracemath.h"

namespace GRACEdata
{
	float math::velocity(float deltadistance, float deltatime) { return deltadistance / deltatime; }
	float math::acceleration(float deltavelocity, float deltatime) { return deltavelocity / deltatime; }
	float math::acceleration(float initialvelocity, float finalvelocity, float deltadistance)
	{
		return 0;
	}
	float math::distance(float deltax, float deltay)
	{
		return sqrtf((deltax * deltax) + (deltay * deltay));
	}
}