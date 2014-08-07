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
this.satcount = 1;
this.satsep = 500; //km
this.pairsep = 30; //degrees
this.multiorb = false;
this.endday = 0;

//*** DATGUI ***//
var gui = new dat.GUI({ autoPlace: false });
var customContainer = document.getElementById('gui-container');
customContainer.appendChild(gui.domElement);
var draws = gui.addFolder("Drawing");

var globeupdate = draws.add(this, 'darkglobe' ).name("Night View");

var sizeupdate = draws.add(this, 'binsize', 0.5, 5.0).name("Binsize (degrees)");
sizeupdate.onChange(function(value){
  binsize = Math.floor(binsize * 2) / 2;
  clearSimul();
});
draws.add(this, 'speed', 1.0, 7.0).name("Simulation Speed");
draws.add(this, 'exagg', 1.0, 2.0).name("Orbit Exaggeration");
draws.add(this, 'drawalpha', 0.0, 7.0).name("Draw Brightness");
var enddaygui = draws.add(this, 'endday').name("Stop on Day");
draws.add(this, 'saveimage').name("Save Image");

var sats = gui.addFolder("Satellites");
var countupdate = sats.add(this, 'satcount', 0.5, 3).name("Pairs");
countupdate.onChange(function(value){
  satcount = Math.floor(satcount);
  if (satcount == 0) {  satcount = 0.5; }

  if (satcount >= 1) {
    $("#satcircleb").css('display', 'block');
    $("#sattextb").css('display', 'block');
  } else {
    $("#satcircleb").css('display', 'none');
    $("#sattextb").css('display', 'none');
  }
  if (satcount >= 2) {
    $("#satcirclec").css('display', 'block');
    $("#sattextc").css('display', 'block');
    $("#satcircled").css('display', 'block');
    $("#sattextd").css('display', 'block');
  } else {
    $("#satcirclec").css('display', 'none');
    $("#sattextc").css('display', 'none');
    $("#satcircled").css('display', 'none');
    $("#sattextd").css('display', 'none');
  }
  if (satcount >= 3) {
    $("#satcirclee").css('display', 'block');
    $("#sattexte").css('display', 'block');
    $("#satcirclef").css('display', 'block');
    $("#sattextf").css('display', 'block');
  } else {
    $("#satcirclee").css('display', 'none');
    $("#sattexte").css('display', 'none');
    $("#satcirclef").css('display', 'none');
    $("#sattextf").css('display', 'none');
  }

  updateOrbitStat();
});

sats.add(this, 'satsep', 0, 10000).name("Separation (km)");

this.orb1Om = 0;
this.orb2Om = 90;
this.orb3Om = 60;
this.orb1om = 0;
this.orb2om = 30;
this.orb3om = 60;
this.ocount = 1;

var orbs = gui.addFolder("Orbits");
var multchange = orbs.add(this, 'multiorb').name("Multiple Orbits");
this.o1 = orbs.add(this, 'orb1Om', 0, 360).name("Orbit 1 &Omega;");
this.O1 = orbs.add(this, 'orb1om', 0, 360).name("Orbit 1 &omega;");
this.opair = orbs.add(this, 'pairsep', 0, 360).name("Pair Distance");

multchange.onChange(function(value){
  updateOrbitStat();
});

function updateOrbitStat() {
  this.ocount = 1;
  if (this.opair !== undefined) { this.opair = orbs.remove(this.opair); }
  if (this.o2 !== undefined) { this.o2 = orbs.remove(this.o2); }
  if (this.O2 !== undefined) { this.O2 = orbs.remove(this.O2); }
  if (this.o3 !== undefined) { this.o3 = orbs.remove(this.o3); }
  if (this.O3 !== undefined) { this.O3 = orbs.remove(this.O3); }

  radius   = g_a / earthradius;
  segments = 64;
  ccolor = 0x888888;
  geometry = new THREE.CircleGeometry( radius, segments );
  geometry.vertices.shift(); // Remove center vertex

  if (this.satcount >= 2 && this.multiorb) {
    this.o2 = orbs.add(this, 'orb2Om', 0, 360).name("Orbit 2 &Omega;");
    this.O2 = orbs.add(this, 'orb2om', 0, 360).name("Orbit 2 &omega;");
    this.ocount = 2;
    ob2.visible = true;
  } else {
    ob2.visible = false;
    if (this.satcount > 0.5) { this.opair = orbs.add(this, 'pairsep', 0, 360).name("Pair Distance"); }
  }
  if (this.satcount >= 3 && this.multiorb) {
    this.o3 = orbs.add(this, 'orb3Om', 0, 360).name("Orbit 3 &Omega;");
    this.O3 = orbs.add(this, 'orb3om', 0, 360).name("Orbit 3 &omega;");
    this.ocount = 3;
    ob3.visible = true;
  } else {
    ob3.visible = false;
  }
}


var renderer, scene, camera, light, controls;
var starSphere, containerEarth;
var earthMesh, radius;
var segments, ccolor;
var material;
var projector;
var ob1, ob2, ob3, meshOverlay;

var overRenderer;
var points;

var lastbin = [new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1),
               new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1),
               new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1)];

//*** INITIALIZE THREE.JS ***//

function init() {
  //Prepare Maths
  g_findperiod();
  //createFile();
  //writeData("Hello World!");
  //saveFile();

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
  ob1 = new THREE.Line(geometry, material);
  ob1.castShadow = ob1.receiveShadow = false;
  ob1.rotation.x = -1 * Math.PI / 180;
  scene.add(ob1);

  geometry = new THREE.CircleGeometry( radius, segments );
  geometry.vertices.shift(); // Remove center vertex
  ob2 = new THREE.Line(geometry, material);
  ob2.castShadow = ob2.receiveShadow = false;
  ob2.rotation.x = -1 * Math.PI / 180;
  ob2.visible = false;
  scene.add(ob2);

  geometry = new THREE.CircleGeometry( radius, segments );
  geometry.vertices.shift(); // Remove center vertex
  ob3 = new THREE.Line(geometry, material);
  ob3.castShadow = ob3.receiveShadow = false;
  ob3.rotation.x = -1 * Math.PI / 180;
  ob3.visible = false;
  scene.add(ob3);

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
  var maxtime = degsec / msframe * 1000 * this.binsize; //maximum msec between frames = ms in sim / ms in realtime
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

  var day = this.sidereal ? 24 * siderealday : 24;
  if ((this.time / 3600 / day >= this.endday) && (this.run == 1) && (this.endday > 0))
  {
    startSimul();
    this.endday = 0;
    enddaygui.needsUpdate = true;
  }
  updateTime();

  //ROTATE EARTH AND ORBITS
  containerEarth.rotation.y += 1/1440 * 2 * Math.PI * delta * Math.pow(10,(this.speed - deltarealt)) * run * siderealday;
  ob1.rotation.y = this.orb1Om * Math.PI / 180;
  if (this.ocount >= 2) { ob2.rotation.y = this.orb2Om * Math.PI / 180; }
  if (this.ocount >= 3) { ob3.rotation.y = this.orb3Om * Math.PI / 180; }

  while (containerEarth.rotation.y >= Math.PI * 2) { containerEarth.rotation.y -= Math.PI * 2; }

  //RENDER ORBIT
  //find location of satellite in 3D space
  var db = this.satsep / g_circ();
  var psep = this.pairsep / 360;
  if (this.multiorb) { psep = 0; }
  var om1 = this.orb1om / 360;
  var om2 = this.orb2om / 360;
  var om3 = this.orb3om / 360;

  var gracea = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1, -this.orb1Om, g_t, this.time);
  var graceca = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1, this.orb1Om, g_t, this.time);
  var graceb = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + db, -this.orb1Om, g_t, this.time);
  var gracecb = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + db, this.orb1Om, g_t, this.time);
  if (this.ocount >= 2)
  {
    var gracec = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om2 + psep, -this.orb2Om, g_t, this.time);
    var gracecc = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om2 + psep, this.orb2Om, g_t, this.time);
    var graced = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om2 + db + psep, -this.orb2Om, g_t, this.time);
    var gracecd = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om2 + db + psep, this.orb2Om, g_t, this.time);
    var gracee = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om3 + (2*psep), -this.orb3Om, g_t, this.time);
    var gracece = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om3 + (2*psep), this.orb3Om, g_t, this.time);
    var gracef = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om3 + db + (2*psep), -this.orb3Om, g_t, this.time);
    var gracecf = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om3 + db + (2*psep), this.orb3Om, g_t, this.time);
  }
  else
  {
    var gracec = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + psep, -this.orb1Om, g_t, this.time);
    var gracecc = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + psep, this.orb1Om, g_t, this.time);
    var graced = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + db + psep, -this.orb1Om, g_t, this.time);
    var gracecd = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + db + psep, this.orb1Om, g_t, this.time);
    var gracee = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + (2*psep), -this.orb1Om, g_t, this.time);
    var gracece = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + (2*psep), this.orb1Om, g_t, this.time);
    var gracef = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + db + (2*psep), -this.orb1Om, g_t, this.time);
    var gracecf = orbit_circle(g_a / earthradius * this.exagg, 89, g_period, om1 + db + (2*psep), this.orb1Om, g_t, this.time);
  }

  //coerce to 2D screen coordinates
  var width = window.innerWidth, height = window.innerHeight;
  var widthHalf = width / 2, heightHalf = height / 2;

  var satname = ["a","b","c","d","e","f"];
  var satcolor = ["#0044ff", "#0aff00", "#f00", "#ff00eb", "#00ffeb", "#f5ff00"]

  if (this.run == 1)
  {
    //groundtrack point
    for (var i = 0; i < this.satcount * 2; i++) {
      var n;
      if (i == 0) { n = gracea; }
      if (i == 1) { n = graceb; }
      if (i == 2) { n = gracec; }
      if (i == 3) { n = graced; }
      if (i == 4) { n = gracee; }
      if (i == 5) { n = gracef; }

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
  for (var i = 0; i < this.satcount * 2; i++) {
    var n;
    var s = satname[i];
    if (i == 0) { n = graceca; }
    if (i == 1) { n = gracecb; }
    if (i == 2) { n = gracecc; }
    if (i == 3) { n = gracecd; }
    if (i == 4) { n = gracece; }
    if (i == 5) { n = gracecf; }

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

  ob1.scale.x = ob1.scale.y = ob1.scale.z = this.exagg;
  if (this.ocount >= 2) { ob2.scale.x = ob2.scale.y = ob2.scale.z = this.exagg; }
  if (this.ocount >= 3) { ob3.scale.x = ob3.scale.y = ob3.scale.z = this.exagg; }

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
