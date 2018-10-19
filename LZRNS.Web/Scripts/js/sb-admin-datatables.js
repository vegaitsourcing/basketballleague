var initDataTable = function () {
	$('#dataTable').DataTable({
		"language": {
			"sProcessing": "Procesiranje u toku...",
			"sLengthMenu": "Prika&#382;i _MENU_ elemenata",
			"sZeroRecords": "Nije prona&#273;en nijedan rezultat",
			"sInfo": "Prikazano _START_ do _END_ od ukupno _TOTAL_ elemenata",
			"sInfoEmpty": "Prikazano 0 do 0 od ukupno 0 elemenata",
			"sInfoFiltered": "(filtrirano od ukupno _MAX_ elemenata)",
			"sInfoPostFix": "",
			"sSearch": "Pretraga:",
			"sUrl": "",
			"oPaginate": {
				"sFirst": "Po&#269;etna",
				"sPrevious": "Prethodna",
				"sNext": "Slede&#263;a",
				"sLast": "Poslednja"
			}
		}
	});
}

// Call the dataTables jQuery plugin
$(document).ready(function() {
	initDataTable();
});
