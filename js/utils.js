this.year = '2002';
this.month = '04';
this.ym = '2002-04';
this.time = 0; //time in seconds
this.sidereal = false;

//*** FULL SCREEN ***//
function fullscreen() {
  if ((window.fullScreen) ||
   (window.innerWidth == screen.width && window.innerHeight == screen.height))
  {
    // fullscreen
    if (document.exitFullscreen) {
      document.exitFullscreen();
    } else if (document.webkitExitFullscreen) {
      document.webkitExitFullscreen();
    } else if (document.mozCancelFullScreen) {
      document.mozCancelFullScreen();
    } else if (document.msExitFullscreen) {
      document.msExitFullscreen();
    }
  }
  else
  {
    // NOT fullscreen
    if (document.documentElement.requestFullscreen) {
      document.documentElement.requestFullscreen();
    } else if (document.documentElement.mozRequestFullScreen) {
      document.documentElement.mozRequestFullScreen();
    } else if (document.documentElement.webkitRequestFullscreen) {
      document.documentElement.webkitRequestFullscreen();
    } else if (document.documentElement.msRequestFullscreen) {
      document.documentElement.msRequestFullscreen();
    }
  }
  
  fsupdateicon();
}

function fsupdateicon()
{
  //retest fullscreen and replace icon
  if ((window.fullScreen) ||
   (window.innerWidth == screen.width && window.innerHeight == screen.height))
  {
    document.getElementById('fsbutton').innerHTML = "<span class='mega-octicon octicon-screen-normal fadeicon'></span>";
  }
  else
  {
    document.getElementById('fsbutton').innerHTML = "<span class='mega-octicon octicon-screen-full fadeicon'></span>";
  }
}

//*** RESIZE CANVAS ***//
function resizeCanvas() {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  
  renderer.setSize(window.innerWidth, window.innerHeight);
  
  fsupdateicon();
}
window.addEventListener('resize', resizeCanvas);

//*** CHANGE MONTH/YEAR ***//
function activateDate() {
  $("#dlab").css("display", "none");
  $("#dbut").css("display", "block");
  $("#dbutm").val(this.month);
  $("#dbuty").val(this.year);
}
function deactivateDate() {
  $("#dlab").css("display", "block");
  $("#dbut").css("display", "none");
}
function changeDate() {
  var mval = $("#dbutm").val();
  var yval = $("#dbuty").val();
  if ((mval > 5) && (yval == 2014)) { mval = '05'; $("#dbutm").val('05'); }
  if ((mval < 4) && (yval == 2002)) { mval = '04'; $("#dbutm").val('04'); }
  var mtxt = $("#dbutm option:selected").text();
  var ytxt = $("#dbuty option:selected").text();
  deactivateDate();
  
  $("#dlabm").html(mtxt);
  $("#dlaby").html(ytxt);
  this.year = yval;
  this.month = mval;
  this.ym = yval.concat("-").concat(mval);
}

//*** SIMULATION CONTROLS ***//
function startSimul() {
  if (this.run == 1)
  {
    this.run = 0;
    $("#runbutton").html("<span class='mega-octicon octicon-playback-play fadeicon'>");
  }
  else
  {
    this.run = 1;
    $("#runbutton").html("<span class='mega-octicon octicon-playback-pause fadeicon'>");
  }
}
function resetSimul() {
  if (this.run == 1) { startSimul(); }
  containerEarth.rotation.y = 0;
  clearSimul();
  this.time = 0;
}
function clearSimul() {
  var con = $('#maincanvas')[0].getContext("2d");
  con.globalAlpha = 1;
  con.clearRect(0, 0, $('#maincanvas').width(), $('#maincanvas').height());
  lastbin = [new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1),
             new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1),
             new THREE.Vector2(-1, -1), new THREE.Vector2(-1, -1)];
}
function changeDateFormat()
{
  if (this.sidereal) {
    $('#timesidereal').html('Solar Day (switch)');
    this.sidereal = false;
  }
  else {
    $('#timesidereal').html('Sidereal Day (switch)');
    this.sidereal = true;
  }
}
function updateTime()
{
  var t = this.time;
  var d, h;
  if (this.sidereal)
  {
    var d = Math.floor(t / (86400 * siderealday));
    t -= d * (86400 * siderealday);
  }
  else
  {
    var d = Math.floor(t / 86400);
    t -= d * 86400;
  }
  var h = Math.floor(t / 3600);
  t -= h * 3600;
  var m = Math.floor(t / 60);
  t -= m * 60;
  var s = Math.floor(t);
  var so = s >= 10 ? s.toString() : "0" + s.toString();
  var mo = m >= 10 ? m.toString() : "0" + m.toString();
  var ho = h >= 10 ? h.toString() : "0" + h.toString();
  var dout = d >= 10 ? d.toString() : "0" + d.toString();
  
  var str = "T+ " + dout + ":" +  ho + ":" + mo + ":" + so;
  
  $("#timer").text(str);
}

//*** READING KEYBOARD ***//
document.addEventListener('keydown', onDocumentKeyDown, false);
function onDocumentKeyDown(event) {
  switch (event.keyCode) {
    case 32: //spacebar
      startSimul();
      event.preventDefault(); break;
    case 82: //R key
      resetSimul();
      event.preventDefault(); break;
  }
}

//*** CANVAS SAVING ***//
function saveImage(canvas) {
  download(canvas, "image.png");
}

function download(canvas, filename) {

    /// create an "off-screen" anchor tag
    var lnk = document.createElement('a'),
        e;

    /// the key here is to set the download attribute of the a tag
    lnk.download = filename;

    /// convert canvas content to data-uri for link. When download
    /// attribute is set the content pointed to by link will be
    /// pushed as "download" in HTML5 capable browsers
    lnk.href = canvas.toDataURL();

    /// create a "fake" click-event to trigger the download
    if (document.createEvent) {

        e = document.createEvent("MouseEvents");
        e.initMouseEvent("click", true, true, window,
                         0, 0, 0, 0, 0, false, false, false,
                         false, 0, null);

        lnk.dispatchEvent(e);

    } else if (lnk.fireEvent) {

        lnk.fireEvent("onclick");
    }
}

//*** FILE I/O ***//
/*function createFile() {
  var xmlhttp = new XMLHttpRequest();
  xmlhttp.open("GET","writer.php?t=create",true);
  xmlhttp.send();
}

function saveFile() {
  var xmlhttp = new XMLHttpRequest();
  xmlhttp.open("GET","writer.php?t=save",true);
  xmlhttp.send();
}

function writeData(str) {
  var xmlhttp = new XMLHttpRequest();
  xmlhttp.open("GET","writer.php?t=append&s="+str,true);
  xmlhttp.send();
}*/


//*** ACTIVATE ANIMATION ***//
function runGif() {
  
}
