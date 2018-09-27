'use strict';

let functions = require('./functions');

let pages = {
	index: require('./pages/index'),
	home: require('./pages/home'),
	content: require('./pages/content')
};

let app = {
	init: function () {
		let pageSelector = 'data-template-name';
		if (document.querySelector('[' + pageSelector + ']') !== null) {
			let pageName = document.querySelector('[' + pageSelector + ']').getAttribute(pageSelector);
			if (pageName && pages[pageName] && pages[pageName].init) {
				pages[pageName].init();
			}
		}
		// call your functions here
		functions.initSlider();
		functions.mobileNav();
		functions.popup();
		functions.tableResponsive();
		functions.initCarousel();
		functions.stationFinder();
		functions.tabsMobile();
		functions.tabsClick();
		functions.tabsContent();
		// functions.objectFit();
		if(('ontouchstart' in window || navigator.msMaxTouchPoints > 0) && window.matchMedia('screen and (max-width: 1024px)').matches) {
			$('html').addClass('touch');
		} else {
			$('html').addClass('no-touch');
		}
	},
	winLoad: function () {
		// call functions that are needed for window load
		console.log('window loaded');
	},
	winResize: function () {
		// call functions that are needed on window resize
		console.log('window resized');
	}
};

$(function() {
	app.init();
});

$(window).on('load', function() {
	app.winLoad();
});

$(window).on('resize', function() {
	app.winResize();
});
