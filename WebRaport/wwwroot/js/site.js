// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    //Optional: turn the chache off
    $.ajaxSetup({ cache: false });
    $('a[data-toggle="ajax-modal"]').click(function (e) {
   // $('.linkDelete').click(function () {
        $('#dialogContent').load(this.href, function () {
            $('#dialogDiv').modal({
                backdrop: 'static',
                keyboard: true
            }, 'show');
            submitDlg(this);
        });
        return false;
    });
});

function submitDlg(dialog) {
    $('button[data-save="modal"]').click(function (e) {
        var form = $(this).parents('.modal').find('form');
        var action = form.attr('action');
        var dataSend = form.serialize();
        $.post(action, dataSend).done(function (data) {
            $('#dialogDiv').modal('hide');
        })
    });
}