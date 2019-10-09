'use strict';

$(document).on("click", "#active-teams", function () {
	const $this = $(this);

	if (!$this.hasClass("active")) {
		$this.addClass("active");
		$("#all-teams").removeClass("active");
		$("#seasonYearStart").val(this.dataset.seasonyearstart);
		$("#teams-form").submit();
	}
});

$(document).on("click", "#all-teams", function () {
	const $this = $(this);

	if (!$this.hasClass("active")) {
		$this.addClass("active");
		$("#active-teams").removeClass("active");
		$("#seasonYearStart").val("");
		$("#teams-form").submit();
	}
});