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

	mobileNav: function() {
		$('.header .open-menu').on('click', function() {
			var $this = $(this);
			$this.toggleClass('active');
			$('.header .main-nav').slideToggle('normal', function () {
			});
		});
	},

	popup: function() {

		var topScroll = 0;
		var isScrollDisabled = false;

		// popups
		// open popup on link/button click
		$('.js-open-popup').on('click', function() {
			var $popupName = $(this).data('popup-link');
			var $popup = $('.popup-overlay[data-popup-window="' + $popupName + '"]').addClass('popup-overlay--active');
			if($popupName == 'result-popup') {
				var personDetails = $(this).find('.result-details').html();
				$popup.find('.popup-placeholder').html(personDetails);
			}
			$popup.fadeIn(400);
			disableScroll();
			if($(this).hasClass('js-video-popup')) {
				var video = $('.popup-overlay--active .js-video-plyr')[0];
				plyr.setup(video, {"autoplay": true});
			}
		});

		// close popup on clicking "X button"
		$('.js-close-popup').on('click', function() {
			$(this).closest('.popup-overlay').removeClass('popup-overlay--active').fadeOut(300, function() {
				enableScroll();
			});
		});
		// close popup on "outside" click
		$('.popup-overlay').on('click', function(e) {
			if(e.target === this) {
				$(this).removeClass('popup-overlay--active').fadeOut(300, function() {
					enableScroll();
				});
			}
		});
		// escape key close popup
		$(document).on('keyup', function(e) {
			if(e.keyCode === 27 && $('.popup-overlay').is(':visible')) {
				$('.popup-overlay').fadeOut(300, function() {
					enableScroll();
				});
			}
		});

		var disableScroll = function() {
			if (!isScrollDisabled) {
				topScroll = $(window).scrollTop();
				$('body').css('top', - topScroll + 'px').addClass('no-scroll');
				isScrollDisabled = true;
			}
		}
		var enableScroll = function () {
			$('body').removeAttr('style').removeClass('no-scroll');
			$(window).scrollTop(topScroll);
			isScrollDisabled = false;
			if ($('.js-video-plyr').hasClass('plyr--setup')) {
				if ($('.js-video-loop').length > 0) {
					plyr.get()[1].destroy();
				} else {
					plyr.get()[0].destroy();
				}
			}
		}
	},

	//table responsive
	tableResponsive: function () {
		$('.table').each(function() {
			var $table = $(this);
			if ($table.length) {
				$table.wrap('<div class="table-responsive"></div>');
				if($table[0].scrollWidth > $table.parent().innerWidth()) {
					$table.parent().append('');
				}
			$('.rte-content').parents('.module-canvas').addClass('no-padding');
		   }
		});
	},

	initCarousel: function() {
		$('.news-slider').slick({
			infinite: true,
			dots: true,
			arrows: false,
			slidesToShow: 2,
			slidesToScroll: 1,
			speed: 1200,
			responsive: [
				{
					breakpoint: 767,
					settings: {
						slidesToShow: 1,
						slidesToScroll: 1
					}
				}
			]
		});
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
