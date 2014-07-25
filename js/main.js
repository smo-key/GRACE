var gl; //Global variable for WebGL context
var canvas; //WebGL canvas for drawing

// START EVERYTHING
function start() {
  init();
}

function resizeCanvas() {
  var width = $(window).width();
  var height = $(window).height();
  if (canvas.width != width ||
      canvas.height != height) {
    canvas.width = width;
    canvas.height = height;
  }
}

// INITIALIZE DRAWING
function init() {
  canvas = document.getElementById("glcanvas");
  gl = initWebGL(canvas);
  
  //Only continue if WebGL is working
  if (!gl) { return; }
  
  gl.clearColor(0.0, 0.0, 0.0, 1.0); //clear screen
  gl.enable(gl.DEPTH_TEST); //enable depth testing
  gl.depthFunc(gl.LEQUAL); //near things obscure far things
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT); //clear screen and depth buffer
  
  //set viewport to maximum canvas width
  gl.viewport(0, 0, canvas.width, canvas.height);
}

function initWebGL(canvas) {
  gl = null;
  
  try {
    //try to get the standard context.  If fails, fallback to experimental context
    gl = canvas.getContext("webgl") || canvas.getContext("experimental-webgl");
  }
  catch(e) {}
  
  //if getting GL context fails, give up now
  if (!gl) {
    alert("Unable to initialize WebGL. Your browser may not support it.");
    gl = null;
  }
  
  resizeCanvas();
  return gl;
}

window.addEventListener('resize', resizeCanvas);