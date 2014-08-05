//*** GLOBALS ***//
this.binsize = 1.0;

//*** PROPERTIES ***//
function binslon() {
  return Math.ceil(360 / this.binsize);
}
function binslat() {
  return Math.ceil(180 / this.binsize) + 1;
}
function binslatcenter() {
  return (binslat() - 1) / 2;
}

//*** BASIC MATH ***//
function sign(n) {
  if (n > 0) { return 1; }
  if (n < 0) { return -1; }
  return 0;
}
function coerce(n, min, max) {
  return Math.max(min, Math.min(max, n));
}

//*** GRID METHODS ***//
//Get ID of grid containing point p
function GetGridID(p) {
  var a = sign(p.x) * (Math.floor((Math.abs(p.x) / this.binsize)));
  var b = (sign(p.y) * (Math.floor((Math.abs(p.y) / this.binsize) - 0.5) + 1)) + binslatcenter();
  return new THREE.Vector2(a, b);
}

//Get left-middle coordinate of grid with certain ID
function GetGridLoc(id) {
  var x = coerce(Math.floor(this.binsize * id.x), 0, 360);
  var y = coerce(Math.floor(this.binsize * (id.y - binslatcenter())), -90, 90);
  return new THREE.Vector2(x, y);
}

function GetTopLeft(id) {
  var x = coerce(this.binsize * id.x, 0, 360);
  var y = coerce(this.binsize * (id.y - binslatcenter()), -90, 90);
  return new THREE.Vector2(x, y);
}

//Get size of the grid with a TOP-LEFT coordinate of loc
function GetSize(loc)
{
  var w = this.binsize;// - Math.abs(loc.x - coerce(loc.x, 0, 360));
  var h = this.binsize;// - Math.abs(loc.y - coerce(loc.y, -90, 90));
  return new THREE.Vector2(w, h);
}
