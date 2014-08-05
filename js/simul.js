//*** GLOBALS ***//
this.darkglobe = false;
this.speed = 1.0;
this.exagg = 1.2;
this.run = 1;
this.drawalpha = 3;
this.timemax = 0;

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

var globeupdate = gui.add(this, 'darkglobe' ).name("Night View");

var sizeupdate = gui.add(this, 'binsize', 0.5, 5.0).name("Binsize (degrees)");
sizeupdate.onChange(function(value){
  binsize = Math.floor(binsize * 2) / 2;
  //binsize = 3.0;
  clearSimul()
});
gui.add(this, 'speed', 1.0, 7.0).name("Simulation Speed");
gui.add(this, 'exagg', 1.0, 2.0).name("Orbit Exaggeration");
gui.add(this, 'drawalpha', 0.0, 7.0).name("Draw Brightness");
gui.add(this, 'timemax').listen();;

var renderer, scene, camera, light, controls;
var starSphere, containerEarth;
var earthMesh, radius;
var segments, ccolor;
var material;
var projector;
var circle, meshOverlay;

var overRenderer;
var points;

var lastbin = new THREE.Vector2(-1, -1);
this.geo30 = new THREE.Geometry();

//*** INITIALIZE THREE.JS ***//
function init() {
  renderer = new THREE.WebGLRenderer({
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
  scene = new THREE.Scene(); //initilize scene
  camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 0.01, 250); //add new camera definition (FOV deg, aspect ratio, near, far)
  camera.position.z = 4; //set z axis camera position

  //*** AMBIENT LIGHT ***//
  light = new THREE.AmbientLight(0x222222);
  scene.add(light);

  //*** DIRECTIONAL LIGHT ***//
  light = new THREE.DirectionalLight(0xffffff, 1);
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
  starSphere = createStarfield();
  scene.add(starSphere);

  //*** EARTH ***//
  //Render Container
  containerEarth = new THREE.Object3D();
  containerEarth.rotateZ(-earthaxistilt * Math.PI/180);
  containerEarth.position.z = 0;
  scene.add(containerEarth);

  //Mesh
  earthMesh = createEarth();
  earthMesh.receiveShadow = true;
  earthMesh.castShadow = true;
  containerEarth.add(earthMesh);

  globeupdate.onChange(function(value){
    //change displaymodes
    if (!value)
    {
      material = new THREE.MeshPhongMaterial({
        map: THREE.ImageUtils.loadTexture('img/earthmap1k.jpg'),
        bumpMap: THREE.ImageUtils.loadTexture('img/earthbump1k.jpg'),
        bumpScale	: 0.05,
      });
      earthMesh.material = material;
      earthMesh.material.needsUpdate = true;
      earthMesh.receiveShadow = true;
      earthMesh.castShadow = true;
    }
    else
    {
      material = new THREE.MeshBasicMaterial({
        map: THREE.ImageUtils.loadTexture('img/world.jpg')
      });
      earthMesh.material = material;
      earthMesh.material.needsUpdate = true;
      earthMesh.receiveShadow = false;
      earthMesh.castShadow = false;
    }
  });

  //*** GRACE ORBIT ***//
  radius   = g_a / earthradius;
  segments = 64;
  ccolor = 0x0044ff;
  material = new THREE.LineBasicMaterial( { color: ccolor } );
  projector = new THREE.Projector();

  //Line of orbit
  geometry = new THREE.CircleGeometry( radius, segments );
  geometry.vertices.shift(); // Remove center vertex
  circle = new THREE.Line(geometry, material);
  circle.rotation.x = -1 * Math.PI / 180;
  circle.castShadow = circle.receiveShadow = false;
  scene.add(circle);

  //UV Drawing Canvas
  meshOverlay = addCanvasOverlay();
  scene.add(meshOverlay);

  //*** DAT.GLOBE POINTS ***//
  points = createPoints();
  scene.add(points);

  /*geometry = new THREE.SphereGeometry( 0.1, 16, 16 );
  material = new THREE.MeshBasicMaterial( {color: 0xffff00} );
  var groundpoint = new THREE.Mesh( geometry, material );
  scene.add( groundpoint );*/

  //*** CAMERA CONTROLS ***//
  //Trackball Controller
  controls = new THREE.TinyTrackballControls(camera, renderer.domElement);
  controls.rotateSpeed = 0.4;
  controls.noZoom = false;
  controls.noPan = true;
  controls.staticMoving = false;
  controls.minDistance = 1.75;
  controls.maxDistance = 8.5;
  controls.dynamicDampingFactor = 0.25;
}

//*** Animation Loop ***//
// shim layer with setTimeout fallback
window.requestAnimFrame = (function(callback){
  return  window.requestAnimationFrame       ||
          window.webkitRequestAnimationFrame ||
          window.mozRequestAnimationFrame    ||
          (function(/* function */ callback, /* DOMElement */ element){
            var vendors = ['webkit', 'moz'];
            for(var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
                window.requestAnimationFrame = window[vendors[x]+'RequestAnimationFrame'];
                window.cancelAnimationFrame =
                  window[vendors[x]+'CancelAnimationFrame'] || window[vendors[x]+'CancelRequestAnimationFrame'];
            }

            if (!window.requestAnimationFrame)
            {
              window.requestAnimationFrame = function(callback, element) {
                    var currTime = new Date().getTime();
                    var timeToCall = Math.max(0, 16 - (currTime - lastTime));
                    var id = window.setTimeout(function() { callback(currTime + timeToCall); },
                      timeToCall);
                    lastTime = currTime + timeToCall;
                    return id;
                };
            }

            if (!window.cancelAnimationFrame)
                window.cancelAnimationFrame = function(id) {
                    clearTimeout(id);
                };
          });
});

//Animation loop
function animate() {
  requestAnimFrame(animate);

  var nowMsec = new Date().getTime();

  //lastTime = lastTime || nowMsec-1000/60;

  var degsec = g_period / 360 ; //s (in simulation) it takes to traverse binsize
  var msframe = 60 * Math.pow(10,(this.speed - deltarealt)); //ms in sim / ms in realtime
  var deltat = nowMsec - lastTime;
  var maxtime = degsec / msframe * 1000; //maximum msec between frames = ms in sim / ms in realtime
  this.timemax = maxtime;

  var deltaMsec = Math.min(maxtime, deltat);

  if (deltat > maxtime)
  { $('#timer').css('color', '#ffa500');
    $('.timetype').css('color', '#ffa500'); }
  else
  { $('#timer').css('color', '#ccc');
    $('.timetype').css('color', '#ccc'); }

  //var deltaMsec = nowMsec - lastTime;
  lastTime = nowMsec;

  render(deltaMsec / 1000, nowMsec / 1000);
}

var colorFn = colorFn || function(x) {
  var c = new THREE.Color();
  if (x==0.0) {
      c.setHSV( ( 0.6 - ( x * 0.5 ) ), 0, 0 );
  } else {
      c.setHSV( ( 0.6 - ( x * 0.5 ) ), 1.0, 1.0 );
  }
  return c;
};

/*var addData = function(data, opts) {
  var lat, lng, size, color, i;


  for (i = 0; i < data.length; i += 3) {
    lat = data[i];
    lng = data[i + 1];
    color = colorFn(data[i+2]);
    size = 0; // data[i + 2]; // CHANGED
    addPoint(lat, lng, size, color, subgeo);
  }
  this._baseGeometry = subgeo;

};*/

function addPoint(lat, lng, size, color, subgeo) {
  var phi = (90 - lat) * Math.PI / 180;
  var theta = (180 - lng) * Math.PI / 180;

  point.position.x = 200 * Math.sin(phi) * Math.cos(theta);
  point.position.y = 200 * Math.cos(phi);
  point.position.z = 200 * Math.sin(phi) * Math.sin(theta);

  point.lookAt(mesh.position);

  point.scale.z = -size;
  point.updateMatrix();
  var i;
  for (i = 0; i < point.geometry.faces.length; i++) {
    point.geometry.faces[i].color = color;
  }

  GeometryUtils.merge(subgeo, point);
}


//*** RENDER LOOP ***//

function render(delta, now) {
  //UPDATE CLOCK
  this.time += delta * 60 * Math.pow(10,(this.speed - deltarealt)) * run;
  var rotsun = this.time / tropicalyear * 2 * Math.PI; //revolution of Earth
  light.position.set(10*Math.cos(-rotsun),0,10*Math.sin(-rotsun)); //rotsun = counterclockwise revolution
  updateTime();

  //ROTATE EARTH
  containerEarth.rotation.y += 1/1440 * 2 * Math.PI * delta * Math.pow(10,(this.speed - deltarealt)) * run * siderealday;

  while (containerEarth.rotation.y >= Math.PI * 2) { containerEarth.rotation.y -= Math.PI * 2; }

  //RENDER ORBIT
  //find location of satellite in 3D space
  var grace = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, g_om, g_t, this.time);

  //coerce to 2D screen coordinates
  var width = window.innerWidth, height = window.innerHeight;
  var widthHalf = width / 2, heightHalf = height / 2;

  //vector = 2d projected vector for orbit
  var vector = new THREE.Vector3(grace.x,  grace.y, grace.z);
  var projector = new THREE.Projector();
  projector.projectVector( vector, camera );

  vector.x = (( vector.x * widthHalf ) + widthHalf);
  vector.y = (-( vector.y * heightHalf ) + heightHalf);
  vector.z = 1;

  //groundtrack point
  var uv = sphere_uv(grace);

  if (this.run == 1)
  {
    var uvadj = new THREE.Vector2(uv.x * 360 - (containerEarth.rotation.y / Math.PI / 180), uv.y * 180 - 90);
    var id = GetGridID(new THREE.Vector2(uvadj.x, uvadj.y));
    if ((id.x != lastbin.x) || (id.y != lastbin.y))
    {
      var loc = GetTopLeft(id);
      var size = GetSize(loc);
      var uvx = uvadj.x / 360 * $('#maincanvas').width() - Math.ceil(containerEarth.rotation.y / Math.PI / 2 * $('#maincanvas').width());
      if (uvx < 0) { uvx+= $('#maincanvas').width(); }
      var uvy = (uvadj.y + 90) / 180 * $('#maincanvas').height();
      var uvr = 2 * this.binsize;

      var ctx = $('#maincanvas')[0].getContext("2d");
      //ctx.globalAlpha = 1;
      //ctx.clearRect(0, 0, $('#maincanvas').width(), $('#maincanvas').height());
      //ctx.fillRect(0,0,1000,400); //1024, 512
      ctx.fillStyle = "#0044ff";
      ctx.globalAlpha = Math.pow(2, this.drawalpha - 7); //0.025;
      if (this.drawalpha == 0) { ctx.globalAlpha = 0; }
      ctx.beginPath();
      //ctx.fillRect(uvx, uvy, size.x / 360 * $('#maincanvas').width(), size.y / 180 * $('#maincanvas').height());
      //(x,y,r,sAngle,eAngle,counterclock)
      ctx.arc(uvx, uvy, uvr, 0, 2 * Math.PI, false);
      ctx.fill();
      //ctx.rotate( -1 * Math.PI / 180
      lastbin = id;
    }
  }

  var canvasTexture = new THREE.Texture($('#maincanvas')[0]);
  canvasTexture.needsUpdate = true;
  meshOverlay.material.map = canvasTexture;
  meshOverlay.material.needsUpdate = true;
  meshOverlay.rotation.y = containerEarth.rotation.y;

  //update display
  $("#satcircle").css('display', 'block');
  $("#satcircle").css('left', (vector.x - 2).toString() + 'px');
  $("#satcircle").css('top', (vector.y - 2).toString() + 'px');
  $("#sattext").css('display', 'block');
  $("#sattext").css('left', (vector.x - 2).toString() + 'px');
  $("#sattext").css('top', (vector.y - 18).toString() + 'px');

  circle.scale.x = circle.scale.y = circle.scale.z = this.exagg;

  //UPDATE CONTROLS
  controls.update();
  renderer.render(scene, camera);
  
  requestAnimationFrame(animate);
}

//*** RUN EVERYTHING ***//
if (!Detector.webgl) {
  Detector.addGetWebGLMessage();
}
else
{
  init();
  var lastTime = new Date().getTime();
  animate();
}
