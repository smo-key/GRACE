//*** GLOBALS ***//
this.binsize = 3.0;

//*** PROPERTIES ***//
var binslon = function() {
  return Math.ceil(360 / this.binsize) + 1;
}
var binslat = function() {
  return Math.ceil(180 / this.binsize) + 1;
}
var binslatcenter = function() {
  return (binslat - 1) / 2;
}

//*** METHODS ***//
function sign(n) {
  if (n > 0) { return 1; }
  if (n < 0) { return -1; }
  return 0;
}
function GetGrid(p) {
  return new THREE.Vector2(GetGridLoc(p.x), GetGridLoc(p.y));
}
function GetGridLoc(n) {
  return sign(n) * (Math.floor((Math.abs(n) / this.binsize) - 0.5) + 1);
}
function GetCenter(grid) {
  var lon = this.binsize * grid.x;
  var lat = this.binsize * grid.y;
}
