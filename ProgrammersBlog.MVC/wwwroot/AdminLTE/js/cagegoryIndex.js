$(document).ready(function () {
    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: { id: "btnAdd", },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {

                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: "GET",
                        url: "/Admin/Category/GetAllCategories/",
                        contentType: "application/json",
                        beforeSend: function () {
                            $("#categoriesTable").hide();
                            $(".spinner-border").show();
                        },
                        success: function (data) {
                            console.log(data);
                            const categoryListDto = jQuery.parseJSON(data);
                            if (categoryListDto.ResultStatus == 0) {
                                let tableBody = "";
                                $.each(categoryListDto.Categories.$values,
                                    function (index, category) {
                                        tableBody += `<tr name="${category.Id}">
                                                        <td>${category.Id}</td>
                                                        <td>${category.Name}</td>
                                                        <td>${category.Description}</td>
                                                        <td>${category.IsActive ? "Evet" :"Hayır"}</td >
                                                        <td>${category.IsDeleted ? "Evet" :"Hayır"}</td>
                                                        <td>${category.Note}</td>
                                                        <td>${convertToShortDate(category.CreatedDate)}()</td>
                                                        <td>${category.CretedByName}</td>
                                                        <td>${convertToShortDate(category.ModifiedDate)}</td>
                                                        <td>${category.ModifiedByName}</td>
                                                        <td><div class="btn-group" role="group">
                                                            <button class="btn btn-primary btn-sm btn-update" data-id="${category.Id}"><span class="fas fa-edit"></span> Düzenle</button>
                                                            <button class="btn btn-danger btn-sm btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span> Sil</button>
                                                        </div></td>
                                                    </tr>`
                                    });
                                $("#categoriesTable > tbody").replaceWith(tableBody);
                                $(".spinner-border").hide();
                                $("#categoriesTable").fadeIn(1400);
                            }
                            else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $(".spinner-border").hide();
                            $("#categoriesTable").fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                }
            }
        ],
        language: {
            "emptyTable": "Tabloda herhangi bir veri mevcut değil",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "infoEmpty": "Kayıt yok",
            "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "infoThousands": ".",
            "lengthMenu": "Sayfada _MENU_ kayıt göster",
            "loadingRecords": "Yükleniyor...",
            "processing": "İşleniyor...",
            "search": "Ara:",
            "zeroRecords": "Eşleşen kayıt bulunamadı",
            "paginate": {
                "first": "İlk",
                "last": "Son",
                "next": "Sonraki",
                "previous": "Önceki"
            },
            "aria": {
                "sortAscending": ": artan sütun sıralamasını aktifleştir",
                "sortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "1": "1 kayıt seçildi",
                    "0": "-"
                },
                "0": "-",
                "1": "%d satır seçildi",
                "2": "-",
                "_": "%d satır seçildi",
                "cells": {
                    "1": "1 hücre seçildi",
                    "_": "%d hücre seçildi"
                },
                "columns": {
                    "1": "1 sütun seçildi",
                    "_": "%d sütun seçildi"
                }
            },
            "autoFill": {
                "cancel": "İptal",
                "fill": "Bütün hücreleri <i>%d<i> ile doldur<\/i><\/i>",
                "fillHorizontal": "Hücreleri yatay olarak doldur",
                "fillVertical": "Hücreleri dikey olarak doldur",
                "info": "-"
            },
            "buttons": {
                "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                "colvis": "Sütun görünürlüğü",
                "colvisRestore": "Görünürlüğü eski haline getir",
                "copy": "Koyala",
                "copyKeys": "Tablodaki sisteminize kopyalamak için CTRL veya u2318 + C tuşlarına basınız.",
                "copySuccess": {
                    "1": "1 satır panoya kopyalandı",
                    "_": "%ds satır panoya kopyalandı"
                },
                "copyTitle": "Panoya kopyala",
                "csv": "CSV",
                "excel": "Excel",
                "pageLength": {
                    "-1": "Bütün satırları göster",
                    "1": "-",
                    "_": "%d satır göster"
                },
                "pdf": "PDF",
                "print": "Yazdır"
            },
            "decimal": "-",
            "infoPostFix": "-",
            "searchBuilder": {
                "add": "Koşul Ekle",
                "button": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "clearAll": "Hepsini Kaldır",
                "condition": "Koşul",
                "conditions": {
                    "date": {
                        "after": "Sonra",
                        "before": "Önce",
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "moment": {
                        "after": "Sonra",
                        "before": "Önce",
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "number": {
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "gt": "Büyüktür",
                        "gte": "Büyük eşittir",
                        "lt": "Küçüktür",
                        "lte": "Küçük eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "string": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "endsWith": "İle biter",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "startsWith": "İle başlar"
                    }
                },
                "data": "Veri",
                "deleteTitle": "FilUrlreleme kuralını silin",
                "leftTitle": "Kriteri dışarı çıkart",
                "logicAnd": "ve",
                "logicOr": "veya",
                "rightTitle": "Kriteri içeri al",
                "title": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "value": "Değer"
            },
            "searchPanes": {
                "clearMessage": "Hepsini Temizle",
                "collapse": {
                    "0": "Arama Bölmesi",
                    "_": "Arama Bölmesi (%d)"
                },
                "count": "{total}",
                "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama Bölmesi yok",
                "loadMessage": "Arama Bölmeleri yükleniyor ...",
                "title": "Etkin filtreler - %d"
            },
            "searchPlaceholder": "Ara",
            "thousands": "."
        }
    });
    /* Datatable bittiği yer */
    $(function () {
        const url = "/Admin/Category/Add/";
        const placeHolderDiv = $("#modalPlaceHolder");
        $("#btnAdd").click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });
        /* Modal açma işleminin bittiği yer */
        placeHolderDiv.on('click', '#btnSave', function (event) {
            event.preventDefault(); /* modaldaki kaydet butonu eğer submit ise, submit özelliğininin yitirilmesini sağlar.*/
            const form = $("#form-category-add");
            const actionUrl = form.attr("Action");
            const datatoSend = form.serialize(); /* form içerisindeki veriyi aslında CategoryAddDtoya dönüştürmüş olduk.*/
            $.post(actionUrl, datatoSend).done(function (data) {
                const categoryAddAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $(".modal-body", categoryAddAjaxModel.CategoryAddPartial);
                placeHolderDiv.find(".modal-body").replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    const newTableRow = `
                            <tr name="${categoryAddAjaxModel.CategoryDto.Category.Id}">
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.IsActive ? "Evet" : "Hayır"}</td >
                                <td>${categoryAddAjaxModel.CategoryDto.Category.IsDeleted ? "Evet" : "Hayır"}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.CretedByName}</td>
                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                <td><div class="btn-group" role="group">
                                    <button class="btn btn-primary btn-sm btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span> Düzenle</button>
                                    <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span> Sil</button>
                                </div></td>

                            </tr>`;
                    const newTableRowObject = $(newTableRow);
                    newTableRowObject.hide();
                    $('#categoriesTable').append(newTableRowObject);
                    newTableRowObject.fadeIn(3500); // Şekilli geliyor yavaş yavaş.
                    toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Başarılı İşlem!');
                }
                else {
                    let summarytext = "";
                    $("#validation-summary > ul > li").each(function () {
                        let text = $(this).text();
                        summarytext = `*${text}\n`;
                    });
                    toastr.warning(summarytext);
                }
            });
        });
    });
    /* Yeni Kategori ekleme alanının bittiği yer */
    $(document).on("click", ".btn-delete", function (event) {
        event.preventDefault();
        const id = $(this).data("id");
        const tableRow = $(`[name="${id}"]`);
        const categoryName = tableRow.find('td:eq(1)').text(); // ikinci td de ki veriyi çekecektir.
        Swal.fire({
            title: 'Silmek istediğinize emin misiniz?',
            text: `${categoryName} kategorisi silinecektir.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum.',
            cancelButtonText: 'Hayır, silmek istemiyorum.'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    data: { categoryId: id },
                    url: "/Admin/Category/Delete/",
                    success: function (data) {
                        const categoryDto = jQuery.parseJSON(data);
                        if (categoryDto.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi',
                                `${categoryDto.Cagegory.Name} kategorisi başarıyla silindi!`,
                                'success',
                            );

                            tableRow.fadeOut(3500);
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${CategoryDto.Message}`,
                            });
                        }
                    },
                    error: function (err) {
                        console.log(err);
                        toastr.error(`${err.responseText}`, "Hata");
                    }
                });
            }
        })
    });

    /*Kategori update modal doldurma alanı*/
    $(function myfunction() {
        const url = "/Admin/Category/Update/";
        const placeHolderDiv = $("#modalPlaceHolder");
        $(document).on("click", ".btn-update", function (event) {
            event.preventDefault();
            const id = $(this).data("id");
            $.ajax({
                url: url,
                type: "GET",
                data: { categoryId: id },
                success: function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find(".modal").modal("show");
                },
                error: function () {
                    toastr.error("Bir hata oluştu");
                }
            });
        });
        /*Kategoriyi update etme alanı*/
        /* Modal açma işleminin bittiği yer */
        placeHolderDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault(); /* modaldaki kaydet butonu eğer submit ise, submit özelliğininin yitirilmesini sağlar.*/
            const form = $("#form-category-update");
            const actionUrl = form.attr("Action");
            const datatoSend = form.serialize(); /* form içerisindeki veriyi aslında CategoryAddDtoya dönüştürmüş olduk.*/
            $.post(actionUrl, datatoSend).done(function (data) {
                const categoryUpdateAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $(".modal-body", categoryUpdateAjaxModel.CategoryUpdatePartial);
                placeHolderDiv.find(".modal-body").replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    const newTableRow =
                       `<tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td>
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.Description}</td>
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.IsActive ? "Evet" : "Hayır"}</td >
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted ? "Evet" : "Hayır"}</td>
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
                        <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.CretedByName}</td>
                        <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                        <td>${categoryUpdateAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                        <td><div class="btn-group" role="group">
                            <button class="btn btn-primary btn-sm btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span> Düzenle</button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span> Sil</button>
                        </div></td>

                    </tr>`;
                    const newTableRowObject = $(newTableRow);
                    const oldRow = $(`[name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"]`);
                    newTableRowObject.hide();
                    oldRow.replaceWith(newTableRowObject);
                    newTableRowObject.fadeIn(3500);

                    toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, 'Düzeltme İşlemi Başarılı!');
                }
                else {
                    let summarytext = "";
                    $("#validation-summary > ul > li").each(function () {
                        let text = $(this).text();
                        summarytext = `*${text}\n`;
                    });
                    toastr.warning(summarytext);
                }
            });
        });

    });
});