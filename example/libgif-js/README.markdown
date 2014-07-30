# Overview

Forked from the excelent jsgif project (https://github.com/shachaf/jsgif), which was implemented as a bookmarklet to manipulate animated gifs (http://slbkbs.org/jsgif).

This is an attempt to pull out the gif parsing and playing logic, seperate it from the bookmarklet, and publish it as a library that you can use in your project.

As an added bonus, you can make gifs "rubbable" so that scrubbing with your mouse (or rubbing with your finger on a touch device) cause the gif to move back and forth.

# Example

Please see example.html for, you know, and example. This will demonstrate how to use basic play controls for a gif, and also a rubbable one.

Please note: this example must be loaded via a webserver, not directly from disk. I.e. http://localhost/libgif-js/example.html NOT file:///libgif-js/example.html. See the same-domain origin caveat at the bottom of this document for more information.

For a hosted example, check out this post on BuzzFeed.com (http://www.buzzfeed.com/yacomink/rubbable-gifs)

# Technical Details

Of note to the developer, libjs.gif contains a class SuperGif, which can be used to manipulate animated gifs.

## Class: SuperGif

### Example usage:

		<img src="./example1_preview.gif" rel:animated_src="./example1.gif" width="360" height="360" rel:auto_play="1" rel:rubbable="1" />

		<script type="text/javascript">
			$$('img').each(function (img_tag) {
				if (/.*\.gif/.test(img_tag.src)) {
					var rub = new SuperGif({ gif: img_tag } );
					rub.load(function(){
						console.log('oh hey, now the gif is loaded');
					});
				}
			});
		</script>

### Image tag attributes:

* **rel:animated_src** -	If this url is specified, it's loaded into the player instead of src.
					This allows a preview frame to be shown until animated gif data is streamed into the canvas

* **rel:auto_play** -		Defaults to 1 if not specified. If set to zero, a call to the play() method is needed

* **rel:rubbable** -		Defaults to 0 if not specified. If set to 1, the gif will be a canvas with handlers to handle rubbing.

### Constructor options

* **gif**		-		Required. The DOM element of an img tag.
* **auto\_play** -			Optional. Same as the rel:auto_play attribute above, this arg overrides the img tag info.
* **max\_width** -			Optional. Scale images over max\_width down to max_width. Helpful with mobile.
* **rubbable** -			Optional. Make it rubbable.

### Instance methods

#### loading
* **load( callback )** -	Loads the gif into a canvas element and then calls callback if one is passed

#### play controls
* **play** -				Start playing the gif
* **pause** -				Stop playing the gif
* **move_to(i)** -		Move to frame i of the gif
* **move_relative(i)** -	Move i frames ahead (or behind if i < 0)

#### getters
* **get_canvas** - The canvas element that the gif is playing in. Handy for assigning event handlers to.
* **get_playing** - Whether or not the gif is currently playing
* **get_loading** - Whether or not the gif has finished loading/parsing
* **get\_auto_play** - Whether or not the gif is set to play automatically
* **get_length** - The number of frames in the gif
* **get\_current_frame** - The index of the currently displayed frame of the gif

## Caveat: same-domain origin

The gif has to be on the same domain (and port and protocol) as the page you're loading.

The library works by parsing gif image data in js, extracting individual frames, and rendering them on a canvas element. There is no way to get the raw image data from a normal image load, so this library does an XHR request for the image and forces the MIME-type to "text/plain". Consequently, using this library is subject to all the same cross-domain restrictions as any other XHR request.
