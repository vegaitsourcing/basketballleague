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
	}
};

module.exports = statistics;
