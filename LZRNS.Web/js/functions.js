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
		var $body = $('body');

		$('.header .open-menu').on('click', function() {
			var $this = $(this);
			$this.toggleClass('active');
			$body.toggleClass('no-scroll');
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
			} else if($popupName == 'gallery-popup') {
				var $content = $(this).parents('.gallery-wrap').find('.gallery-content').html();
				var dataId = $(this).find('img').data('slide');

				$popup.find('.image-content').html($content);
				$('.image-slider').slick('slickGoTo', dataId);
			}
			$popup.fadeIn(400);
			disableScroll();
			if($(this).hasClass('js-video-popup')) {
				var video = $('.popup-overlay--active .js-video-plyr')[0];
				plyr.setup(video, {"autoplay": true});
			}
		});

		$('.statistic-page').on('click', '[data-table-popup-link]', function () {
			const $this = $(this);
			const $popupName = $this.data('table-popup-link');
			const $popup = $('.popup-overlay[data-popup-window="' + $popupName + '"]').addClass('popup-overlay--active');
			const $tableType = $this.data('table-type');
			const $tableDetails = $this.find('[data-result-type=' + $tableType + ']');

			if ($tableDetails.length === 0) {
				return;
			}

			$popup.find('.popup-placeholder').html($tableDetails.html());
			$popup.fadeIn(400);

			if (!isScrollDisabled) {
				disableScroll();
			} else {
				enableScroll();
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

		// close popup on "x" btn
		$('.close-btn').on('click', function() {
			$('.popup-overlay').removeClass('popup-overlay--active').fadeOut(300, function() {
				enableScroll();
			});
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

	imageSlider: function() {
		$('.image-slider').slick({
			infinite: true,
			prevArrow: '<button class="slick-prev" aria-label="Previous" type="button"><span class="dot-left"></span></button>',
			nextArrow: '<button class="slick-next" aria-label="Next" type="button"><span class="dot-right"></span></button>',
			responsive: [
				{
					breakpoint: 992,
					settings: {
						arrows: false
					}
				}
			]
		});
	},

	stationFinder: function() {
		$('.letters-button').on('click', function() {
			var wrap = $(this).closest('.letters-nav');
			wrap.toggleClass('open');
		});

		$('.letters-nav li').on('click', function() {
			var $this = $(this);

			if ($this.hasClass("active")) {
				$this.removeClass("active");
				$this.siblings(".all").addClass("active");

				$this.children("a").attr("href", "/igrači");

				return;
			}

			var val = $this.text();
			var button = $this.closest('ul').prev('.letters-button');
			var wrap = $this.closest('.letters-nav');
			var items = $this.siblings('li');

			items.removeClass('active');
			$this.addClass('active');
			button.attr('data-placeholder', val);
			wrap.removeClass('open');
		});
	},


		//tab for mobile
		tabsMobile: function () {
			$('.tabs-container .tab-heading').click(function () {
				if (!$(this).hasClass('selected')) {
					$(this).parent().find('.tab-heading').removeClass('selected');
					$(this).addClass('selected');
					$(this).parent().find('.tab-content').stop().slideUp('fast');
					var activeTab = $(this).parent().find('.' + $(this).data('tab'));
					activeTab.stop().slideDown(500);
					$(this).parent().find('ul.tabs li').removeClass('selected');
					$(this).parent().find('ul.tabs li[data-tab="' + $(this).data('tab') + '"]').addClass('selected');
				} else {
					$(this).removeClass('selected');
					var activeTab = $(this).parent().find('.' + $(this).data('tab'));
					activeTab.stop().slideUp(500);
					$(this).parent().find('ul.tabs li').removeClass('selected');
				}
			});
		},

		//Tabs On Click
		tabsClick: function () {
			$('ul.tabs li').click(function (e) {
				e.preventDefault ? e.preventDefault() : e.returnValue = false;


				var  $this = $(this);
				var row = $this.parents('.js-tabs-container');
				row.find('.tab-content').hide();
				row.find('li.selected').removeClass('selected');
				$this.addClass('selected');
				var activeTab = row.find('.'+ $this.data('tab'));
				activeTab.show();
			});
		},

		//tabs content
		tabsContent: function () {
			// $('ul.tabs li').click(function() {
			// 	var $grandpa = $(this).parent().parent();
			// 	$grandpa.find('ul.tabs li').removeClass('selected');
			// 	$(this).addClass('selected');
			// 	$grandpa.find('.tab-content').hide();
			// 	var activeTab = $grandpa.find('.' + $(this).data('tab'));
			// 	activeTab.show();
			// 	$grandpa.find('.tab-heading').removeClass('selected');
			// 	$grandpa.find('.tab-heading[data-tab="' + $(this).data('tab') + '"]').addClass('selected');
			// });

			//tabs
			$('.tab-content').hide();
			if($(window).width() < 768) {
				$('.tabs-container .tab-heading:first').addClass('selected').show();
			}
			$('.js-tabs-container').each(function(e){
				$(this).find('.tab-content:first').show();
			});
		},

		//select dropdown
		selectDropdown: function () {
			$('.select-dropdown .selected-item').on('click', function () {
				if (!$(this).hasClass('open')) {
					$('.select-dropdown .selected-item').removeClass('open');
					$('.select-dropdown ul').slideUp().attr('aria-hidden', 'true');
					$('.select-dropdown .select-overlay').hide().attr('aria-hidden', 'true');
					$(this).addClass('open');
					$(this).siblings('ul').slideDown().removeAttr('aria-hidden');
					$(this).siblings('.select-overlay').show().removeAttr('aria-hidden');
				} else {
					$(this).removeClass('open');
					$(this).siblings('ul').slideUp().attr('aria-hidden', 'true');
					$(this).siblings('.select-overlay').hide().attr('aria-hidden', 'true');
				}
			});

			$('.select-dropdown .select-overlay').on('click', function () {
				$(this).hide().attr('aria-hidden', 'true');
				$(this).siblings('.selected-item').removeClass('open');
				$(this).siblings('ul').slideUp().attr('aria-hidden', 'true');
			});

			$('.select-dropdown ul li').on('click', function () {
				$(this).parent().siblings('.selected-item').text($(this).text()).removeClass('open');
				$(this).parent().slideUp().attr('aria-hidden', 'true');
				$(this).parent().siblings('.select-overlay').hide().attr('aria-hidden', 'true');
			});
		},

		//custom select
		customSelect: function() {
			$('select').sSelect();
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
	},

	//show season content
	showSeasonContent: function() {
		var activeSeason = $('.years li.active')[0].dataset.season;

		$('.years-container').filter(function(i) {
			return $('.years-container')[i].dataset.season === activeSeason;
		}).show();
	},

	//event for click on history season
	initOnClickSeason: function(){
		var self = this;
		$(document).on('click',
		'.years li a',
		function(e) {
			e.preventDefault();
			$('.years li').removeClass('active');
			$('.years-container').hide();
			$(this).parent().addClass('active');

			self.showSeasonContent();
		});
	},

	//init first element as active on history pageX
	initHistorySeason: function(){
		$('.years li').first().addClass('active');
		this.showSeasonContent();
	}
};
