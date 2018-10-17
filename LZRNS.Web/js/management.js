//function called when ajax call is finished for leaguesSeason edit/add forms
function leagueSeasonSuccess(data) {
	editAction(data.responseJSON);
	$('#modal').modal('hide');
}

//editAction which is recycled to refresh view after update on leaguesSeason
function editAction(dataset) {
	$.get(controller.editAction, dataset,
		function (data, status) {
			if (status === "success") {
				$('.main-content').html(data);
				$.validator.unobtrusive.parse($(".main-content"));
			}
		}, "html");
}

$(document).ready(function () {
	$(document).on('click', '#back', function () {
		location.reload();
	})

	// 3 standard generic operations, add modal, edit modal and delete button
	$(".open-add-modal").on('click', function () {
		$.get(controller.addAction, function (data, status) {
			if (status === "success") {
				$('#modal').html(data);
				$('#modal').modal();
				$.validator.unobtrusive.parse($("#modal"));
			}
		}, "html");
	});

	$(".open-edit-modal").on('dblclick', function () {
		editAction(this.dataset);
	});

	$(document).on('click', '.btn-delete', function () {
		$.get(controller.deleteAction, this.dataset,
			function (data, status) {
				if (data.status === "success") {
					location.reload();
					return;
				}
				confirm(data.message);
			});
	});

	// 2 leagueSeason events, for delete and for add/edit
	$(document).on('click', '.btn-delete-leagueSeason', function () {
		$.get(controller.deleteLeagueSeasonAction, this.dataset,
			function (data, status) {
				if (status === "success") {
					editAction(data);
					$('#modal').modal('hide');
				}
			}, "html");
	});

	$(document).on('click', '.league-item', function (e) {
		e.preventDefault();
		$hrefElement = $(this);

		if ($hrefElement.hasClass("active")) {
			$.get(controller.editLeagueSeasonAction, this.dataset, function (data, status) {
				if (status === "success") {
					$('#modal').html(data);
					$('#modal').modal();
					$.validator.unobtrusive.parse($("#modal"));
				}
			}, "html");
			$hrefElement.addClass("in-progress");
		}
		else {
			$.get(controller.addLeagueSeasonAction, this.dataset, function (data, status) {
				if (status === "success") {
					$('#modal').html(data);
					$('#modal').modal();
					$.validator.unobtrusive.parse($("#modal"));
				}
			}, "html");
			$hrefElement.addClass("in-progress");
		}
	})
});