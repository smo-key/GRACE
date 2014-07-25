var createEarth	= function(){
	var geometry	= new THREE.SphereGeometry(0.5, 32, 32)
	var material	= new THREE.MeshPhongMaterial({
		map		: THREE.ImageUtils.loadTexture('../img/earthmap1k.jpg'),
		bumpMap		: THREE.ImageUtils.loadTexture('../img/earthbump1k.jpg'),
		bumpScale	: 0.05,
		specularMap	: THREE.ImageUtils.loadTexture('../img/earthspec1k.jpg'),
		specular	: new THREE.Color('grey'),
	})
	var mesh	= new THREE.Mesh(geometry, material)
	return mesh	
}

var createEarthCloud	= function(){
	// create destination canvas
	var canvasResult	= document.createElement('canvas')
	canvasResult.width	= 1024
	canvasResult.height	= 512
	var contextResult	= canvasResult.getContext('2d')		

	// load earthcloudmap
	var imageMap	= new Image();
	imageMap.addEventListener("load", function() {
		
		// create dataMap ImageData for earthcloudmap
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
		imageTrans.src	= '../img/earthcloudmaptrans.jpg';
	}, false);
	imageMap.src	= '../img/earthcloudmap.jpg';

	var geometry	= new THREE.SphereGeometry(0.51, 32, 32)
	var material	= new THREE.MeshPhongMaterial({
		map		: new THREE.Texture(canvasResult),
		side		: THREE.DoubleSide,
		transparent	: true,
		opacity		: 0.8,
	})
	var mesh	= new THREE.Mesh(geometry, material)
	return mesh	
}

var createStarfield	= function(){
	var texture	= THREE.ImageUtils.loadTexture('../img/starfield.png')
	var material	= new THREE.MeshBasicMaterial({
		map	: texture,
		side	: THREE.BackSide
	})
	var geometry	= new THREE.SphereGeometry(90, 32, 32)
	var mesh	= new THREE.Mesh(geometry, material)
	return mesh	
}