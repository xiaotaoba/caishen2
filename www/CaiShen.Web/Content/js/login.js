
jQuery(document).ready(function () {
    $('.login-form input[type="text"], .login-form input[type="password"], .login-form textarea').on('focus', function () {
        $(this).parent().removeClass('has-error');
    });

    $('.login-form').on('submit', function (e) {

        $(this).find('input[type="text"], input[type="password"], textarea').each(function () {
            if ($(this).val() == "") {
                e.preventDefault();
                $(this).parent().addClass('has-error');
            }
            else {
                $(this).parent().removeClass('input-error');
            }
        });

    });


});
