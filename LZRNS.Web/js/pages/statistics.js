'use strict';

let statistics = {
	init: function () {
		$('[all-seasons]').on('click', function () {
			const $this = $(this);
			const cssClass = "active";

			$this.addClass(cssClass).siblings().removeClass(cssClass);
		});

		$('[active-season]').on('click', function () {
			const seasonParam = $('[seasons] option:first').data('query');

			window.location.search = seasonParam;
		});

		$('.statistic-page').on('click', '[category]', function () {
			const $this = $(this);
			const categoryParam = $this.data('query');
			const seasonParam = $('[seasons] option:selected').data('query');

			window.location.search = seasonParam + "&" + categoryParam;
		});

		$('.newListSelected').on('click', '.newList li', function () {
			const seasonParam = $('[seasons] option:eq(' + $(this).index() + ')').data('query');

			if (window.location.search.indexOf(seasonParam) > -1) return;

			window.location.search = seasonParam;
		});
		
		$('.statistic-page').on('click', '[data-table-popup-link]', function () {
			const $this = $(this);
			const popupName = $this.data('table-popup-link');
			const $popup = $('.popup-overlay[data-popup-window="' + popupName + '"]').addClass('popup-overlay--active');
			const tableType = $this.data('table-type');
			const $tableDetails = $this.find('[data-result-type=' + tableType + ']');

			if ($tableDetails.length === 0) {
				return;
			}

			$popup.find('.popup-placeholder').html($tableDetails.html());
			$popup.fadeIn(400);
		});
	}
};

module.exports = statistics;
