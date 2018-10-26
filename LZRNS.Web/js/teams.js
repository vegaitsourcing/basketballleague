$(document).ready(function () {
	$('#search-form').submit(function (e) {
		e.preventDefault();
		$form = $(this).closest("form");

		$.ajax({
			url: $form.attr("action"),
			type: "POST",
			data: new FormData($form[0]),
			processData: false,
			success: function (data, result, xhr) {
				if (result === "success") {
					$("#names-list").html(data);
				}
				location.reload();
			},
		});
	});
});