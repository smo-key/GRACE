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

//*** DATGUI ***//
//var datGUI = new dat.GUI;

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
  earthMesh.rotation.y += 1/32 * delta;
});

//Atmosphere (Bright Halo)
var geometry = new THREE.SphereGeometry(0.5, 32, 32);
var material = createAtmosphereMaterial();
material.uniforms.glowColor.value.set(0x00b3ff);
material.uniforms.coeficient.value = 0.8;
material.uniforms.power.value = 2.0;
var mesh = new THREE.Mesh(geometry, material);
mesh.scale.multiplyScalar(1.01);
containerEarth.add(mesh);

//Atmosphere (Outer Halo)
var geometry = new THREE.SphereGeometry(0.5, 32, 32);
var material = createAtmosphereMaterial();
material.uniforms.glowColor.value.set(0x00b3ff);
material.uniforms.coeficient.value = 0.5;
material.uniforms.power.value = 4.0;
var mesh = new THREE.Mesh(geometry, material);
mesh.scale.multiplyScalar(1.15);
containerEarth.add(mesh);

//Clouds
var earthCloud = createEarthCloud()
earthCloud.recieveShadow = true;
earthCloud.castShadow = true;
containerEarth.add(earthCloud);
//Cloud Animation
onRenderFcts.push(function(delta, now){
  earthCloud.rotation.y += 1/8 * delta;
})

//*** RENDER SCENE ***//
onRenderFcts.push(function(){
  renderer.render(scene, camera);
})

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
  })
})