//*** GLOBALS ***//
this.display = '3D Globe';
this.darkglobe = false;
this.speed = 1.0;
this.exagg = 1.2;
this.run = 1;
this.drawalpha = 3;
this.saveimage = function()
{
  var canvas = document.getElementById("maincanvas");
  saveImage(canvas);
};
this.satcount = 2;
this.satsep = 200;

//*** DATGUI ***//
var gui = new dat.GUI({ autoPlace: false });
var customContainer = document.getElementById('gui-container');
customContainer.appendChild(gui.domElement);

var globeupdate = gui.add(this, 'darkglobe' ).name("Night View");

var sizeupdate = gui.add(this, 'binsize', 0.5, 5.0).name("Binsize (degrees)");
sizeupdate.onChange(function(value){
  binsize = Math.floor(binsize * 2) / 2;
  clearSimul()
});
gui.add(this, 'speed', 1.0, 7.0).name("Simulation Speed");
gui.add(this, 'exagg', 1.0, 2.0).name("Orbit Exaggeration");
gui.add(this, 'drawalpha', 0.0, 7.0).name("Draw Brightness");
gui.add(this, 'saveimage').name("Save Image");

var sats = gui.addFolder("Satellites");
var countupdate = sats.add(this, 'satcount', 1, 5).name("Count");
countupdate.onChange(function(value){
  satcount = Math.floor(satcount);

  if (satcount >= 2) {
    $("#satcircleb").css('display', 'block');
    $("#sattextb").css('display', 'block');
  } else {
    $("#satcircleb").css('display', 'none');
    $("#sattextb").css('display', 'none');
  }
  if (satcount >= 3) {
    $("#satcirclec").css('display', 'block');
    $("#sattextc").css('display', 'block');
  } else {
    $("#satcirclec").css('display', 'none');
    $("#sattextc").css('display', 'none');
  }
  if (satcount >= 4) {
    $("#satcircled").css('display', 'block');
    $("#sattextd").css('display', 'block');
  } else {
    $("#satcircled").css('display', 'none');
    $("#sattextd").css('display', 'none');
  }
  if (satcount >= 5) {
    $("#satcirclee").css('display', 'block');
    $("#sattexte").css('display', 'block');
  } else {
    $("#satcirclee").css('display', 'none');
    $("#sattexte").css('display', 'none');
  }
});
var sepupdate = sats.add(this, 'satsep', 0, 10000).name("Seperation (km)");

var renderer, scene, camera, light, controls;
var starSphere, containerEarth;
var earthMesh, radius;
var segments, ccolor;
var material;
var projector;
var circle, meshOverlay;

var overRenderer;
var points;

var lastbin = [new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1),
               new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1),
               new THREE.Vector2(-1, -1)];
this.geo30 = new THREE.Geometry();

//*** INITIALIZE THREE.JS ***//
function init() {
  //Prepare Maths
  g_findperiod();

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
  ccolor = 0x888888;
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
  var db = this.satsep / g_circ();

  var gracea = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, g_om, g_t, this.time);
  var graceca = gracea.clone();
  var graceb = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, g_om + db, g_t, this.time);
  var gracecb = graceb.clone();
  var gracec = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, g_om + (2 * db), g_t, this.time);
  var gracecc = gracec.clone();
  var graced = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, g_om + (3 * db), g_t, this.time);
  var gracecd = graced.clone();
  var gracee = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, g_om + (4 * db), g_t, this.time);
  var gracece = gracee.clone();

  //coerce to 2D screen coordinates
  var width = window.innerWidth, height = window.innerHeight;
  var widthHalf = width / 2, heightHalf = height / 2;

  var satname = ["a","b","c","d","e","A","B","C","D","E"];
  var satcolor = ["#0044ff", "#0aff00", "#f00", "#ff00eb", "#00ffeb"]
  var maxsat = 5;

  if (this.run == 1)
  {
    //groundtrack point
    for (var i = 0; i < this.satcount; i++) {
      var n;
      if (i == 0) { n = gracea; }
      if (i == 1) { n = graceb; }
      if (i == 2) { n = gracec; }
      if (i == 3) { n = graced; }
      if (i == 4) { n = gracee; }

      var uv = sphere_uv(n);
      var uvadj = new THREE.Vector2(uv.x * 360 - (containerEarth.rotation.y / Math.PI / 180), uv.y * 180 - 90);
      var id = GetGridID(new THREE.Vector2(uvadj.x, uvadj.y));
      if ((id.x != lastbin[i].x) || (id.y != lastbin[i].y))
      {
        var loc = GetTopLeft(id);
        var size = GetSize(loc);
        var uvx = uvadj.x / 360 * $('#maincanvas').width() - Math.ceil(containerEarth.rotation.y / Math.PI / 2 * $('#maincanvas').width());
        if (uvx < 0) { uvx+= $('#maincanvas').width(); }
        var uvy = (uvadj.y + 90) / 180 * $('#maincanvas').height();
        var uvr = 2 * this.binsize;

        var ctx = $('#maincanvas')[0].getContext("2d");
        ctx.fillStyle = satcolor[i];
        ctx.globalAlpha = Math.pow(2, this.drawalpha - 7); //0.025;
        if (this.drawalpha == 0) { ctx.globalAlpha = 0; }
        ctx.beginPath();
        ctx.arc(uvx, uvy, uvr, 0, 2 * Math.PI, false);
        ctx.fill();
        lastbin[i] = id;
      }
    }
  }

  var canvasTexture = new THREE.Texture($('#maincanvas')[0]);
  canvasTexture.needsUpdate = true;
  meshOverlay.material.map = canvasTexture;
  meshOverlay.material.needsUpdate = true;
  meshOverlay.rotation.y = containerEarth.rotation.y;

  //update display
  for (var i = 0; i < this.satcount; i++) {
    var n;
    var s = satname[i];
    if (i == 0) { n = graceca; }
    if (i == 1) { n = gracecb; }
    if (i == 2) { n = gracecc; }
    if (i == 3) { n = gracecd; }
    if (i == 4) { n = gracece; }

    var pa = new THREE.Vector3(n.x, n.y, n.z);
    var projector = new THREE.Projector();
    projector.projectVector( pa, camera );
    pa.x = (( pa.x * widthHalf ) + widthHalf);
    pa.y = (-( pa.y * heightHalf ) + heightHalf);
    pa.z = 1;
    $("#satcircle" + s).css('display', 'block');
    $("#satcircle" + s).css('left', (pa.x - 2).toString() + 'px');
    $("#satcircle" + s).css('top', (pa.y - 2).toString() + 'px');
    $("#sattext" + s).css('display', 'block');
    $("#sattext" + s).css('left', (pa.x - 2).toString() + 'px');
    $("#sattext" + s).css('top', (pa.y - 18).toString() + 'px');
  }

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
