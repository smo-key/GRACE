//*** GLOBALS ***//
this.display = '3D Globe';
this.binsize = 2.0;
this.speed = 1.0;
this.run = false;

//*** DATGUI ***//
var gui = new dat.GUI({ autoPlace: false });
var customContainer = document.getElementById('gui-container');
customContainer.appendChild(gui.domElement);

var displayupdate = gui.add(this, 'display', [ '3D Globe', '2D Map'  ] ).name("Display");
displayupdate.onChange(function(value){
  //change displaymodes
  
});
var sizeupdate = gui.add(this, 'binsize', 0.5, 5.0).name("Binsize (degrees)");
sizeupdate.onChange(function(value){
  binsize = Math.floor(binsize * 2) / 2;
});
var speedupdate = gui.add(this, 'speed', 0.5, 3.5).name("Simulation Speed");

//** LOAD GLOBE **//
/*if(!Detector.webgl){
  Detector.addGetWebGLMessage();
} else {
  var container = document.getElementById('container');
  var globe = new DAT.Globe(container);
  globe.clearData();
  globe.createPoints();
  globe.animate();
  
}*/

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
camera.position.z = 1; //set z axis camera position

//*** AMBIENT LIGHT ***//
var light = new THREE.AmbientLight(0x222222);
scene.add(light);

//*** DIRECTIONAL LIGHT ***//
var light = new THREE.DirectionalLight(0xffffff, 1);
light.position.set(5,5,5);
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
containerEarth.rotateZ(-23.4 * Math.PI/180);
containerEarth.position.z = 0;
scene.add(containerEarth);

//Mesh
var earthMesh = createEarth();
earthMesh.receiveShadow = true;
earthMesh.castShadow = true;
containerEarth.add(earthMesh);

//Animate Mesh
onRenderFcts.push(function(delta, now){
  earthMesh.rotation.y += 1/32 * delta * Math.pow(10,(this.speed - 1));
});

//Clouds
/*var earthCloud = createEarthCloud();
earthCloud.recieveShadow = true;
earthCloud.castShadow = true;
containerEarth.add(earthCloud);
//Cloud Animation
onRenderFcts.push(function(delta, now){
  earthCloud.rotation.y += 1/8 * delta * this.speed;
});*/

//*** CAMERA CONTROLS ***//
var mouse = {x:0, y:0};
document.addEventListener('mousemove', function(event){
  mouse.x = (event.clientX / window.innerWidth) - 0.5;
  mouse.y = (event.clientY / window.innerHeight) - 0.5;
}, false);
//Animate Camera Position (Speed * 3)
onRenderFcts.push(function(delta, now) {
  camera.position.x += (mouse.x * 5 - camera.position.x) * (delta * 5);
  camera.position.y += (mouse.y * 5 - camera.position.y) * (delta * 5);
  camera.lookAt(scene.position);
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