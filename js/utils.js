this.year = '2002';
this.month = '04';
this.ym = '2002-04'

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
  var mtxt = $("#dbutm option:selected").text();
  var ytxt = $("#dbuty option:selected").text();
  deactivateDate();
  if ((mval > 5) && (yval == 2014)) { return; }
  if ((yval > 2014) || (yval < 2002)) { return; }
  if ((mval < 4) && (yval == 2002)) { return; }
  $("#dlabm").html(mtxt);
  $("#dlaby").html(ytxt);
  this.year = yval;
  this.month = mval;
  this.ym = yval.concat("-").concat(mval);
}

//*** ACTIVATE ANIMATION ***//
function runGif() {
  
}