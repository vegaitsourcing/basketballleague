var contactForm = contactForm || {
    init: function () {
        this.listeners();
    },
    listeners: function () {
        $(document).on('click', '.form-control', function (e) {
            e.preventDefault();
            var form = $(this).closest('form');
            $(form).submit();
        })
    },
    showResult: function () {
        $("#form-outer").hide();
        $("#form-result").show();
    }
};
contactForm.init();