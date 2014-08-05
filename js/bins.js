//*** GLOBALS ***//
this.binsize = 3.0;

//*** PROPERTIES ***//
var binslon = function() {
  return Math.ceil(360 / this.binsize);
}
var binslat = function() {
  return Math.ceil(180 / this.binsize) + 1;
}
var binslatcenter = function() {
  return (binslat - 1) / 2;
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
  var a = sign(n) * (Math.floor((Math.abs(p.x) / this.binsize)));
  var b = (sign(n) * (Math.floor((Math.abs(p.y) / this.binsize) - 0.5) + 1)) + binslatcenter;
  return new THREE.Vector2(a, b);
}

//Get left-middle coordinate of grid with certain ID
function GetGridLoc(id) {
  var x = coerce(Math.Floor(this.gridsize * id.x), 0, 360);
  var y = coerce(Math.Floor(this.gridsize * (id.y - binslatcenter)), -90, 90);
  return new THREE.Vector2(x, y);
}

function GetTopLeft(id) {
  var x = coerce(Math.Floor(this.gridsize * id.x), 0, 360);
  var y = coerce(Math.Floor(this.gridsize * (id.y - binslatcenter) - (this.gridsize / 2)), -90, 90);
  return new THREE.Vector2(x, y);
}

//Get size of the grid with a TOP-LEFT coordinate of loc
function GetSize(loc)
{
  var w = this.gridsize - Math.abs(loc.x - coerce(loc.x, 0, 360));
  var h = this.gridsize - Math.abs(loc.y - coerce(loc.y, -90, 90));
  return new THREE.Vector2(w, h);
}
