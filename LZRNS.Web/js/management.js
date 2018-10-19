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

$(document).ready(function () {
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
		$.get(controller.addAction, function (data, status) {
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
				else{
					location.reload();
				}
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