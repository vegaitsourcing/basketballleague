'use strict';

module.exports = {

	initSlider: function() {
		let $slider = $('.slider');
		if ($slider !== undefined && $slider.length) {
			$slider.slick();
		}
	},

	objectFit: function () {

		if ($('img') !== undefined && $('img').length > 0) {
			// IE object fit
			objectFitImages();
		}
	},

	// equal heights
	equalHeights: function(arrayItems, count) {
		if (arrayItems !== undefined && arrayItems.length > 0) {
			arrayItems.height('');

			let maxH = 0;

			if (count) {
				let arrays = [];
				while (arrayItems.length > 0) {
					arrays.push(arrayItems.splice(0, count));
				}

				for (let i = 0; i < arrays.length; i += 1) {
					let data = arrays[i];
					maxH = 0;
					for (let j = 0; j < data.length; j += 1) {
						let currentH = $(data[j]).outerHeight(true);
						if (currentH > maxH) {
							maxH = currentH;
						}
					}

					for (let k = 0; k < data.length; k += 1) {
						$(data[k]).css('height', maxH);
					}
				}
			} else {
				arrayItems.each(function () {
					let currentH2 = $(this).outerHeight(true);
					if (currentH2 > maxH) {
						maxH = currentH2;
					}
				});

				arrayItems.css('height', maxH);
			}
		}
	}
};
