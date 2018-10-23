//function called when ajax call is finished for leaguesSeason edit/add forms & add game player form
function onPostSuccess(data) {
	editAction(data.responseJSON ? data.responseJSON : data);
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

//gets form data from "form" in my case jquery array of inputs
function getFormData($form) {
	var unindexed_array = $form.serializeArray();
	var indexed_array = {};

	$.map(unindexed_array, function (n, i) {
		indexed_array[n['name']] = n['value'];
	});

	return indexed_array;
}

// sets base64 string of input file for passed image
function setBase64ToImage(file, $imageEl) {
	var reader = new FileReader();
	var result;

	reader.readAsDataURL(file);
	reader.onload = function () {
		$imageEl.attr("src", reader.result);
	};

	return result;
}

//get dropdown menus for seasons and rounds
function getSeasonSelector(dataset) {
	$.get(controller.seasonSelectorAction, dataset, function (data, status) {
		if (status === "success") {
			$("#control-row").html(data);
		}
	}, "html");
}

$(document).ready(function () {
	//round management functions
	$activeSeason = $(document).find('.nav-link.season.active');

	if ($activeSeason.length > 0) {
		getSeasonSelector($activeSeason[0].dataset);
	}

	$(document).on('change', '#selected-league', function () {
		var dataset = {
			leagueSeasonId: $(this).val()
		}
		$('#selected-round').remove();
		$('.open-add-modal').remove();

		$.get(controller.roundSelectorAction, dataset, function (data, status) {
			if (status === "success") {
				$("#control-row").append(data);
			}
		}, "html");
	});

	$(document).on('change', '#selected-round', function () {
		var dataset = {
			roundId: $(this).val()
		}

		if ($(this).val() === "") {
			$('.open-add-modal').remove();
		}
		else {
			$.get(controller.gamesTableAction, dataset, function (data, status) {
				if (status === "success") {
					if ($('.open-add-modal').length === 0) {
						$("#control-row").append("<button class=\"open-add-modal btn btn-primary\">Dodaj novu utakmicu</button>");
						$(".open-add-modal")[0].dataset.leagueseasonid = $("#selected-league").val()
						$(".open-add-modal")[0].dataset.roundid = $("#selected-round").val();
					}
					else {
						$(".open-add-modal")[0].dataset.roundid = $("#selected-round").val();
					}
					$("#table-games").html(data);
					initDataTable();
				}
			}, "html");
		}
	});

	$(document).on('click', '.nav-link.season', function () {
		$navLink = $(this);

		$('.nav-link').removeClass("active");
		$navLink.addClass("active");
		getSeasonSelector($navLink[0].dataset);
	})


	//function for submitting ajax form with input type file, because ajax...
	$(document).on('click', '#image-submit', function (e) {
		e.preventDefault();
		$form = $(this).closest('form');

		if ($form.valid()) {
			$.ajax({
				url: $form.attr("action"),
				type: 'POST',
				data: new FormData($form[0]),
				processData: false,
				contentType: false,
				success: function () {
					location.reload();
				},
			});
		}
	});

	$(document).on('click', '#back', function () {
		location.reload();
	})

	// 3 standard generic operations, add modal, edit modal and delete button
	$(document).on('click', ".open-add-modal", function () {
		$.get(controller.addAction, this.dataset, function (data, status) {
			if (status === "success") {
				$('#modal').html(data);
				$('#modal').modal();
				$.validator.unobtrusive.parse($("#modal"));
			}
		}, "html");
	});

	$(document).on('dblclick', ".open-edit-modal", function () {
		editAction(this.dataset);
	});

	$(document).on('click', '.btn-delete', function (e) {
		e.preventDefault();

		$.get(controller.deleteAction, this.dataset,
			function (data, status) {
				if (data.status === "success") {
					location.reload();
					return;
				}
				if (confirm(data.message)) {
					location.reload();
				}
				else {
					location.reload();
				}
			});
	});

	//action for updating player stats
	$(document).on('click', '.btn-player-stats', function (e) {
		e.preventDefault();
		$inputs = $(this).closest('.player-stats').find('input');

		$.ajax({
			url: controller.updatePlayerStatsAction,
			type: 'POST',
			data: JSON.stringify(getFormData($inputs)),
			processData: false,
			contentType: 'application/json',
			success: function (data, status) {
				if (status === "success") {
					editAction(data);
					$('#modal').modal('hide');
				}
			},
		});
	})
	// 2 leagueSeason events, for delete and for add/edit
	$(document).on('click', '.btn-delete-leagueSeason', function () {
		$.get(controller.deleteLeagueSeasonAction, this.dataset,
			function (data, status) {
				if (status === "success") {
					editAction(data);
					$('#modal').modal('hide');
				}
			}, "json");
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
		}
		else {
			$.get(controller.addLeagueSeasonAction, this.dataset, function (data, status) {
				if (status === "success") {
					$('#modal').html(data);
					$('#modal').modal();
					$.validator.unobtrusive.parse($("#modal"));
				}
			}, "html");
		}
	})


	// game player event, for delete and for add/edit
	$(document).on('click', '.game-player-item', function (e) {
		e.preventDefault();
		$hrefElement = $(this);

		if ($hrefElement.hasClass("active")) {
			$.get(controller.deletePlayerStatsAction, this.dataset,
				function (data, status) {
					if (status === "success") {
						editAction(data);
						$('#modal').modal('hide');
					}
				}, "json");
		}
		else {
			$.get(controller.addPlayerStatsAction, this.dataset, function (data, status) {
				if (status === "success") {
					$('#modal').html(data);
					$('#modal').modal();
					$.validator.unobtrusive.parse($("#modal"));
				}
			}, "html");
		}
	})

	// players in team event
	$(document).on('click', '.player-item', function (e) {
		e.preventDefault();
		$hrefElement = $(this);
		var dataset = this.dataset;
		if ($hrefElement.hasClass("team-member")) {
			$.ajax({
				url: controller.deleteTeamMemberAction,
				type: 'POST',
				data: JSON.stringify(dataset),
				processData: false,
				contentType: 'application/json',
				success: function (result, status, xhr) {
					editAction(dataset);
				},
			});
		}
		else {
			$.ajax({
				url: controller.addPlayerToTeamAction,
				type: 'POST',
				data: JSON.stringify(dataset),
				processData: false,
				contentType: 'application/json',
				success: function (result, status, xhr) {
					editAction(dataset);
				},
			});
		}
	});

	$(document).on('change', 'input[name=ImageFile]', function () {
		var files = this.files;
		if (files.length > 0) {
			setBase64ToImage(files[0], $("#imageFrame"));
		}
	});
});