this.cloudopacity = 0.0;
this.dataopacity = 1;

function createEarth() {
	var geometry	= new THREE.SphereGeometry(1, 32, 32)
	var material	= new THREE.MeshPhongMaterial({
		map		: THREE.ImageUtils.loadTexture('img/earthmap1k.jpg'),
		bumpMap		: THREE.ImageUtils.loadTexture('img/earthbump1k.jpg'),
		bumpScale	: 0.05,
	})
	var mesh	= new THREE.Mesh(geometry, material)
	return mesh;
}

function createPoints() {
    points = new THREE.Mesh(this._baseGeometry, new THREE.MeshBasicMaterial({
            color: 0xffffff,
            vertexColors: THREE.FaceColors,
            morphTargets: false
    }));
    points.dynamic = true;
    return points;
}

//var canvasTexture;
function addCanvasOverlay() {
    var spGeo = new THREE.SphereGeometry(1.02,32,32);
    var canvasTexture = new THREE.Texture($('#maincanvas')[0]);
    canvasTexture.needsUpdate = true;

    var material = new THREE.MeshBasicMaterial({
        map : canvasTexture,
        transparent : true,
        opacity: 1
    });

    var mesh = new THREE.Mesh(spGeo,material);
    mesh.dynamic = true;
    mesh.rotation.x = -2 * Math.PI / 180;

    return mesh;
}

function createEarthCloud(){
	// create destination canvas
	var canvasResult	= document.createElement('canvas')
	canvasResult.width	= 1024
	canvasResult.height	= 512
	var contextResult	= canvasResult.getContext('2d')		

	// load earthcloudmap
	var imageMap	= new Image();
	imageMap.addEventListener("load", function() {
		
		// create dataMap ImageData for earthcloudmap1
		var canvasMap	= document.createElement('canvas')
		canvasMap.width	= imageMap.width
		canvasMap.height= imageMap.height
		var contextMap	= canvasMap.getContext('2d')
		contextMap.drawImage(imageMap, 0, 0)
		var dataMap	= contextMap.getImageData(0, 0, canvasMap.width, canvasMap.height)

		// load earthcloudmaptrans
		var imageTrans	= new Image();
		imageTrans.addEventListener("load", function(){
			// create dataTrans ImageData for earthcloudmaptrans
			var canvasTrans		= document.createElement('canvas')
			canvasTrans.width	= imageTrans.width
			canvasTrans.height	= imageTrans.height
			var contextTrans	= canvasTrans.getContext('2d')
			contextTrans.drawImage(imageTrans, 0, 0)
			var dataTrans		= contextTrans.getImageData(0, 0, canvasTrans.width, canvasTrans.height)
			// merge dataMap + dataTrans into dataResult
			var dataResult		= contextMap.createImageData(canvasMap.width, canvasMap.height)
			for(var y = 0, offset = 0; y < imageMap.height; y++){
				for(var x = 0; x < imageMap.width; x++, offset += 4){
					dataResult.data[offset+0]	= dataMap.data[offset+0]
					dataResult.data[offset+1]	= dataMap.data[offset+1]
					dataResult.data[offset+2]	= dataMap.data[offset+2]
					dataResult.data[offset+3]	= 255 - dataTrans.data[offset+0]
				}
			}
			// update texture with result
			contextResult.putImageData(dataResult,0,0)	
			material.map.needsUpdate = true;
		})
		imageTrans.src	= 'img/earthcloudmaptrans.jpg';
	}, false);
	imageMap.src	= 'img/earthcloudmap.jpg';

	var geometry	= new THREE.SphereGeometry(1.02, 32, 32)
	var material	= new THREE.MeshPhongMaterial({
		map		: new THREE.Texture(canvasResult),
		side		: THREE.DoubleSide,
		transparent	: true,
		opacity		: this.cloudopacity,
	})
	var mesh	= new THREE.Mesh(geometry, material)
	return mesh;
}

function createStarfield(){
	var texture	= THREE.ImageUtils.loadTexture('img/starfield.png')
	var material	= new THREE.MeshBasicMaterial({
		map	: texture,
		side	: THREE.BackSide
	})
	var geometry	= new THREE.SphereGeometry(225, 32, 32)
	var mesh	= new THREE.Mesh(geometry, material)
	return mesh	
}
