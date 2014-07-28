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