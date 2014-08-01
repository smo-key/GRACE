//*** GLOBALS ***//
this.display = '3D Globe';
this.binsize = 3.0;
this.speed = 0.0;
this.run = 1;

var deltarealt = 1.778; //set zero to realtime (1 delta = 1 second)
var stellarday = 86164.1 / 86400; //seconds in a stellar day / solar day
var tropicalyear = 31556926.08; //seconds in one revolution around the Earth
var earthaxistilt = 23.4; //in degrees, difference from true north and celestial north
var earthradius = 6378.1; //in kilometers
var PI_HALF = Math.PI / 2; //tau

//GRACE Orbital Parameters
var g_a = 6871.0; //Semi-major axis in kilometers, also radius since e is about zero
var g_e = 0.000; //eccentricity
var g_i = 89.000; //inclination in degrees, 89deg is prograde


//*** DATGUI ***//
var gui = new dat.GUI({ autoPlace: false });
var customContainer = document.getElementById('gui-container');
customContainer.appendChild(gui.domElement);

var displayupdate = gui.add(this, 'display', [ '3D Globe', '2D Map'  ] ).name("Display");
displayupdate.onChange(function(value){
  //change displaymodes
  
});   
var sizeupdate = gui.add(this, 'binsize', 1.0, 5.0).name("Binsize (degrees)");
sizeupdate.onChange(function(value){
  binsize = Math.floor(binsize * 2) / 2;
});
var speedupdate = gui.add(this, 'speed', 0.0, 6.0).name("Simulation Speed");

//*** INITIALIZE THREE.JS ***//
var renderer = new THREE.WebGLRenderer({
  antialias: true
});
//Maximize renderer to window
renderer.setSize(window.innerWidth, window.innerHeight);
//Activate the three.js DOM render element
document.body.appendChild(renderer.domElement);
//Allow shadow mapping
renderer.shadowMapEnabled = true;

//*** CREATE SCENE AND CAMERA ***//
var onRenderFcts=[]; //rendering stack
var scene = new THREE.Scene(); //initilize scene
var camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 0.01, 100); //add new camera definition (FOV deg, aspect ratio, near, far)
camera.position.z = 2.5; //set z axis camera position

//*** TIMER ***//
onRenderFcts.push(function(delta, now){
  //one Earth minute per delta
  this.time += delta * 60 * Math.pow(10,(this.speed - deltarealt)) * run;
  var rotsun = this.time / tropicalyear * 2 * Math.PI; //revolution of Earth
  light.position.set(10*Math.cos(-rotsun),0,10*Math.sin(-rotsun)); //rotsun = counterclockwise revolution
  updateTime();
});

//*** AMBIENT LIGHT ***//
var light = new THREE.AmbientLight(0x222222);
scene.add(light);

//*** DIRECTIONAL LIGHT ***//
var light = new THREE.DirectionalLight(0xffffff, 1);
light.position.set(10,0,10);
scene.add(light);

light.castShadow = true;
light.shadowCameraNear = 0.01;
light.shadowCameraFar = 15;
light.shadowCameraFov = 45;

light.shadowCameraLeft = -1;
light.shadowCameraRight = 1;
light.shadowCameraTop = 1;
light.shadowCameraBottom = -1;

light.shadowBias = 0.001;
light.shadowDarkness = 0.2;

light.shadowMapWidth = 1024;
light.shadowMapHeight = 1024;

//*** ADD STARFIELD ***//
var starSphere = createStarfield();
scene.add(starSphere);

//*** EARTH ***//
//Render Container
var containerEarth = new THREE.Object3D();
containerEarth.rotateZ(-earthaxistilt * Math.PI/180);
containerEarth.position.z = 0;
scene.add(containerEarth);

//Mesh
var earthMesh = createEarth();
earthMesh.receiveShadow = true;
earthMesh.castShadow = true;
containerEarth.add(earthMesh);
//Animate Mesh
onRenderFcts.push(function(delta, now){
  //one Earth minute per delta
  containerEarth.rotation.y += 1/1440 * 2 * Math.PI * delta * Math.pow(10,(this.speed - deltarealt)) * run * stellarday;
});

//Clouds
/*var earthCloud = createEarthCloud();
earthCloud.recieveShadow = true;
earthCloud.castShadow = true;
containerEarth.add(earthCloud);
//Cloud Animation
onRenderFcts.push(function(delta, now){
  earthCloud.rotation.y += 1/8 * delta * Math.pow(10,(this.speed - 1)) * run;
});*/

//*** GRACE ORBIT ***//
var radius   = g_a / earthradius;
var segments = 64;
var material = new THREE.LineBasicMaterial( { color: 0x0044ff } ),
geometry = new THREE.CircleGeometry( radius, segments );

geometry.vertices.shift(); // Remove center vertex
scene.add( new THREE.Line( geometry, material ) );



//*** CAMERA CONTROLS ***//
/*var mouse = {x:0, y:0};
document.addEventListener('mousemove', function(event){
  mouse.x = (event.clientX / window.innerWidth) - 0.5;
  mouse.y = (event.clientY / window.innerHeight) - 0.5;
}, false);
//Animate Camera Position (Speed * 3)
onRenderFcts.push(function(delta, now) {
  camera.position.x += (mouse.x * 5 - camera.position.x) * (delta * 5);
  camera.position.y += (mouse.y * 5 - camera.position.y) * (delta * 5);
  camera.lookAt(scene.position);
});*/

var curZoomSpeed = 0;
var zoomSpeed = 0.002;
var overRenderer; //is the mouse on the actual control?

var mouse = { x: 0, y: 0 }, mouseOnDown = { x: 0, y: 0 };
var rotation = { x: 0, y: 0 },
    target = { x: Math.PI*3/2, y: Math.PI / 6.0 },
    targetOnDown = { x: 0, y: 0 };

var distance = 3, distanceTarget = 3;

window.addEventListener('mousedown', onMouseDown, false);
window.addEventListener('mousewheel', onMouseWheel, false);

function onMouseDown(event) {
  event.preventDefault();

  window.addEventListener('mousemove', onMouseMove, false);
  window.addEventListener('mouseup', onMouseUp, false);
  window.addEventListener('mouseout', onMouseOut, false);

  mouseOnDown.x = - event.clientX;
  mouseOnDown.y = event.clientY;

  targetOnDown.x = target.x;
  targetOnDown.y = target.y;

  //window.style.cursor = 'move';
}

function onMouseMove(event) {
  mouse.x = - event.clientX;
  mouse.y = event.clientY;

  var zoomDamp = distance/1000;

  target.x = targetOnDown.x + (mouse.x - mouseOnDown.x) * 0.005 * zoomDamp;
  target.y = targetOnDown.y + (mouse.y - mouseOnDown.y) * 0.005 * zoomDamp;

  target.y = target.y > PI_HALF ? PI_HALF : target.y;
  target.y = target.y < - PI_HALF ? - PI_HALF : target.y;
}

function onMouseUp(event) {
  window.removeEventListener('mousemove', onMouseMove, false);
  window.removeEventListener('mouseup', onMouseUp, false);
  window.removeEventListener('mouseout', onMouseOut, false);
  
  //window.style.cursor = 'auto';
}

function onMouseOut(event) {
  window.removeEventListener('mousemove', onMouseMove, false);
  window.removeEventListener('mouseup', onMouseUp, false);
  window.removeEventListener('mouseout', onMouseOut, false);
}

function onMouseWheel(event) {
  event.preventDefault();
  if (overRenderer) {
    zoom(event.wheelDeltaY * 0.3);
  }
  return false;
}

window.addEventListener('mouseover', function() {
  overRenderer = true;
}, false);

window.addEventListener('mouseout', function() {
  overRenderer = false;
}, false);

function zoom(delta) {
  distanceTarget -= delta;
  distanceTarget = distanceTarget > 5 ? 5 : distanceTarget;
  distanceTarget = distanceTarget < 1.1 ? 1.1 : distanceTarget;
}

onRenderFcts.push(function(delta, now){
  zoom(curZoomSpeed);
  distance += (distanceTarget - distance) * 0.3;

  camera.position.x = distance * Math.sin(rotation.x) * Math.cos(rotation.y);
  camera.position.y = distance * Math.sin(rotation.y);
  camera.position.z = distance * Math.cos(rotation.x) * Math.cos(rotation.y);
});

//*** RENDER SCENE ***//
onRenderFcts.push(function(){
  renderer.render(scene, camera);
});

//*** LOOP ***//
var lastTimeMsec = null;
requestAnimationFrame(function animate(nowMsec){
  //keep looping
  requestAnimationFrame(animate);
  
  //measure time delta (wait 200 msec)
  lastTimeMsec = lastTimeMsec || nowMsec-1000/60;
  var deltaMsec = Math.min(200, nowMsec - lastTimeMsec);
  lastTimeMsec = nowMsec;
  
  //call each update function
  onRenderFcts.forEach(function(onRenderFct){
    onRenderFct(deltaMsec/1000, nowMsec/1000);
  });
});