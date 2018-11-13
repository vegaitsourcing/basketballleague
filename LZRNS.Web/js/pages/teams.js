'use strict';

let teams = {
	init: function () {
		$(document).on("click",
			"#active-teams",
			function () {
				const $this = $(this);
				if ($this.hasClass("active")) {
					$this.removeClass("active");
					$("#all-teams").addClass("active");
					$("#seasonYearStart").val("");
					$("#teams-form").submit();
				} else {
					$this.addClass("active");
					$("#all-teams").removeClass("active");
					$("#seasonYearStart").val(this.dataset.seasonyearstart);
					$("#teams-form").submit();
				}
			});

		$(document).on("click",
			"#all-teams",
			function () {
				const $this = $(this);
				if ($this.hasClass("active")) {
					$this.removeClass("active");
					$("#active-teams").addClass("active");
					$("#seasonYearStart").val(this.dataset.seasonyearstart);
					$("#teams-form").submit();
				} else {
					$this.addClass("active");
					$("#active-teams").removeClass("active");
					$("#seasonYearStart").val("");
					$("#teams-form").submit();
				}
			});
	}
};

module.exports = teams;
