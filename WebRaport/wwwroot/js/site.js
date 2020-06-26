// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#simpleDlg").dialog({ autoOpen: false });
    //Prepare jtable plugin
    $('#FieldsTable').jtable({
        title: 'Поля для подстановки в шаблон рапорта',
        paging: true, //Enables paging
        pageSize: 10, //Actually this is not needed since default value is 10.
        sorting: true, //Enables sorting
        defaultSorting: 'Name ASC', //Optional. Default sorting on first load.
        actions: {
            listAction: '/CreateRaport/FieldsList',
            createAction: '/CreateRaport/AddField',
            //updateAction: '/PagingAndSorting.aspx/UpdateStudent',
            deleteAction: '/CreateRaport/DeleteField'
        },
        fields: {
            FieldId: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            FieldTitle: {
                title: 'Название поля',
                create: true
            },
            FieldType: {
                title: 'Тип поля',
                width: '12%',
                create: true,
                options: '/CreateRaport/GetFieldTypeOptions',
            },
            FromInfoTableName: {
                title: 'Название таблицы',
                create: true,
                dependsOn: 'FieldType',
                options: function (data) {
                    if (data.source == 'list') {
                        return '/CreateRaport/GetTablesOptions?fieldType=0';
                    }
                    return '/CreateRaport/GetTablesOptions?fieldType=' + data.dependedValues.FieldType;
                }
            },
            FromInfoColumnName: {
                title: 'Название колонки',
                create: true,
                dependsOn: 'FromInfoTableName',
                options: function (data) {
                    if (data.source == 'list') {
                        //Return url of all cities for optimization. 
                        //This method is called for each row on the table and jTable caches options based on this url.
                        return '/CreateRaport/GetTableColumnsOptions?fromInfoTableName=""';
                    }

                    //This code runs when user opens edit/create form or changes country combobox on an edit/create form.
                    //data.source == 'edit' || data.source == 'create'
                    return '/CreateRaport/GetTableColumnsOptions?fromInfoTableName=' + data.dependedValues.FromInfoTableName;
                }
            },
            FieldDescription: {
                title: 'Описание поля',
                create: true
            },
            FieldDirectValue: {
                title: 'Прямая замена',
                create: true
            },
            FieldCalculateType: {
                title: 'Вычисляемое поле',
                create: true,
                options: '/CreateRaport/GetCalculatedFieldTypeOptions'
            }
        },
        messages: {
            loadingMessage: 'Загрузка полей...',
            noDataAvailable: 'Нет созданных полей!',
            addNewRecord: 'Добавить новое поле',
            editRecord: 'Редактировать поле',
            areYouSure: 'Вы уверены?',
            deleteConfirmation: 'Это поле будет удалено. Вы уверены?',
            save: 'Сохранить',
            saving: 'Сохранение',
            cancel: 'Отменить',
            deleteText: 'Удалить',
            deleting: 'Удаление',
            error: 'Ошибка',
            close: 'Закрыть',
        }
    });

    //Load student list from server
    $('#FieldsTable').jtable('load');
});

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
    $('a[data-toggle="ajax-modal-simple"]').click(function (e) {
        $("#simpleDlg").dialog("open");
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

function fileSelected(sender, e) {
    var fileManager = sender;
    var fileName = e.items[0].name;
    $('#PathToRaportFile').val(fileName);
}