$(document).ready(function () {
	$(".open-add-modal").on('click', function () {
		$.get(controller.addAction, function (data, status) {
			if (status === "success") {
				$('#modal').html(data);
				$('#modal').modal();
				$.validator.unobtrusive.parse($(".modal"));
			}
		}, "html");
	});

	$(".open-edit-modal").on('dblclick', function () {
		$.get(controller.editAction,
			{
				leagueId: this.dataset.id
			},
			function (data, status) {
			if (status === "success") {
				$('#modal').html(data);
				$('#modal').modal();
				$.validator.unobtrusive.parse($("#modal"));
			}
		}, "html");
	});

	$(document).on('click', '.btn-delete', function () {
		$.get(controller.deleteAction,
			{
				leagueId: this.dataset.id
			},
			function (data, status) {
				if (status === "success") {
					location.reload();
				}
			}, "html");
	});
});