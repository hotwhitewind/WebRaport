// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('#date').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        opens: 'center',
        drops: 'up',
        autoUpdateInput: true,
        locale: {
            format: 'DD.MM.YYYY'
        }
    });
});

$(function () {
    //Optional: turn the chache off
    $.ajaxSetup({ cache: false });
    $('a[data-toggle="ajax-modal"]').click(function (e) {
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

$(function () {
    //Optional: turn the chache off
    $.ajaxSetup({ cache: false });
    $('button[data-toggle="ajax-modal"]').click(function (e) {
            $('#dialogDiv').modal({
                backdrop: 'static',
                keyboard: true
            }, 'show');
            submitDlg(this);
        });
        return false;    
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

function fileSelected(sender, e) {
    var fileManager = sender;
    var fileName = e.items[0].name;
    $('#PathToRaportFile').val(fileName);
}