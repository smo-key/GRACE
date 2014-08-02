//GRACE Orbital Parameters
var g_a = 6871.0; //Semi-major axis in kilometers, also radius since e is about zero
var g_e = 0.000; //eccentricity
var g_i = 89.000; //inclination in degrees, 89deg is prograde, from plane of reference up
var g_Om = 0; //longitude of ascending node, from zero degrees to ascending node
var g_om = 0; //argument of periapsis, from plane of reference to periapsis
var g_t = 0; //time of periapsis passage
var g_period = 1800; //seconds per revolution

//need r, v (perifocal system)

//circular orbit, no eccentricity
//a: Semi major axis (m)
//i: Inclination angle (deg)
//p: Period (t)
//om: Argument of periapsis (deg)
//T: Time of periapsis passage (t)
//t: Current time
function orbit_circle(a, i, p, om, T, t)
{
  //calculate on 2D plane
  var deg = ((t - T) / p + om) * 2 * Math.PI; //time difference / period = location
  //var x = a * Math.cos(deg);
  //var y = a * Math.sin(deg);

  //calculate to 2D/3D difference
  var orbit = sphere_cartes(a, i, deg);
  return orbit;
}

//r = radius
//theta = polar angle, or inclination (in
//phi = azimuth, from x counterclockwise

function sphere_cartes(r, theta, phi)
{
  var x = r * Math.sin(theta) * Math.cos(phi);
  var y = r * Math.sin(theta) * Math.sin(phi);
  var z = r * Math.cos(theta);
  return new THREE.Vector3(x, y, z);
}

//a: Semi major axis (m)
//e: Eccentricity
//i: Inclination angle (rad)
//Om: Right ascension (rad)
//om: Argument of perigee (rad)
//M0: Mean anomaly at t=0 (rad)
//t: Time vector for orbit computation
function orbit_eci(a, e, i, Om, om, M0, t)
{
  var G = 3.986005*Math.pow(10, 14); //earth universal gravitational constant (m^3/s^2)
  var N = Math.sqrt(G/(a*a*a)); //mean orbit angular speed

}
