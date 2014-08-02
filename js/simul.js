//*** GLOBALS ***//
this.display = '3D Globe';
this.binsize = 3.0;
this.speed = 1.0;
this.exagg = 1.2;
this.run = 1;

var deltarealt = 2.778; //set zero to realtime (1 delta = 1 second)
var siderealday = 86164.1 / 86400; //seconds in a sidereal day / solar day
var tropicalyear = 31556926.08; //seconds in one revolution around the Earth
var earthaxistilt = 23.4; //in degrees, difference from true north and celestial north
var earthradius = 6378.1; //in kilometers
var PI_HALF = Math.PI / 2; //tau

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
gui.add(this, 'speed', 1.0, 7.0).name("Simulation Speed");
gui.add(this, 'exagg', 1.0, 2.0).name("Orbit Exaggeration");

//*** INITIALIZE THREE.JS ***//
var renderer = new THREE.WebGLRenderer({
  antialias: true
});
//Maximize renderer to window
renderer.setSize(window.innerWidth, window.innerHeight);
//Activate the three.js DOM render element
renderer.setClearColor(0x000000, 1.0);
document.body.appendChild(renderer.domElement);
//Allow shadow mapping
renderer.shadowMapEnabled = true;

//*** CREATE SCENE AND CAMERA ***//
var onRenderFcts=[]; //rendering stack
var scene = new THREE.Scene(); //initilize scene
var camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 0.01, 800); //add new camera definition (FOV deg, aspect ratio, near, far)
camera.position.z = 4; //set z axis camera position

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
  containerEarth.rotation.y += 1/1440 * 2 * Math.PI * delta * Math.pow(10,(this.speed - deltarealt)) * run * siderealday;
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
var ccolor = 0x0044ff;
var material = new THREE.LineBasicMaterial( { color: ccolor } );
var projector = new THREE.Projector();

//Line of orbit
geometry = new THREE.CircleGeometry( radius, segments );
geometry.vertices.shift(); // Remove center vertex
var circle = new THREE.Line(geometry, material);
//circle.rotation.x = -1 * Math.PI / 180;
circle.castShadow = circle.receiveShadow = false;
scene.add(circle);

onRenderFcts.push(function(){
  //find location of satellite in 3D space
  var grace = orbit_circle(g_a / earthradius * this.exagg, 90, g_period, g_om, g_t, this.time);

  //coerce to 2D screen coordinates
  var width = window.innerWidth, height = window.innerHeight;
  var widthHalf = width / 2, heightHalf = height / 2;

  var vector = new THREE.Vector3(grace.x,
                                 grace.y, grace.z);
  var projector = new THREE.Projector();
  projector.projectVector( vector, camera );

  vector.x = (( vector.x * widthHalf ) + widthHalf);
  vector.y = (-( vector.y * heightHalf ) + heightHalf);
  vector.z = 1;

  //update display
  $("#satcircle").css('display', 'block');
  $("#satcircle").css('left', (vector.x - 2).toString() + 'px');
  $("#satcircle").css('top', (vector.y - 2).toString() + 'px');
  $("#sattext").css('display', 'block');
  $("#sattext").css('left', vector.x.toString() + 'px');
  $("#sattext").css('top', (vector.y - 18).toString() + 'px');

  circle.scale.x = circle.scale.y = circle.scale.z = this.exagg;

});

/*geometry = new THREE.CircleGeometry( radius, segments );
geometry.vertices.shift(); // Remove center vertex
geometry.rotation.z = 1.0 * Math.PI/180;

scene.add( new THREE.Line( geometry, material ) );*/


//*** CAMERA CONTROLS ***//
//Trackball Controller
var controls = new THREE.TrackballControls(camera, renderer.domElement);
controls.rotateSpeed = 0.4;
controls.noZoom = false;
controls.noPan = true;
controls.staticMoving = false;
controls.minDistance = 1.75;
controls.maxDistance = 8.5;
controls.dynamicDampingFactor = 0.25;

//*** RENDER SCENE ***//
onRenderFcts.push(function(){
  controls.update();
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
