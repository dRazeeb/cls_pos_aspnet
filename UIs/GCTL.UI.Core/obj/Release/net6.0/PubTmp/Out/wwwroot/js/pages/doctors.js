﻿(function ($) {
    $.doctors = function (options) {
        // Default options
        var settings = $.extend({
            baseUrl: "/",
            formSelector: "#doctor-form",
            formContainer: ".js-doctor-form-container",
            gridSelector: "#doctor-grid",
            gridContainer: ".js-doctor-grid-container",
            editSelector: ".js-doctor-edit",
            saveSelector: ".js-doctor-save",
            selectAllSelector: "#doctor-check-all",
            deleteSelector: ".js-doctor-delete-confirm",
            deleteModal: "#doctor-delete-modal",
            finalDeleteSelector: ".js-doctor-delete",
            clearSelector: ".js-doctor-clear",
            topSelector: ".js-go",
            decimalSelector: ".js-doctor-decimalplaces",
            maxDecimalPlace: 5,
            showNagativeFormat: false,
            availabilitySelector: ".js-doctor-check-availability",
            haseFile: false,
            quickAddSelector: ".js-quick-add",
            quickAddModal: "#quickAddModal",
            load: function () {

            }
        }, options);


        var gridUrl = settings.baseUrl + "/grid";
        var saveUrl = settings.baseUrl + "/setup";
        var deleteUrl = settings.baseUrl + "/Delete";
        var selectedItems = [];
        $(() => {
            //settings.load(settings.baseUrl, settings.gridSelector);
            loadDoctors(settings.baseUrl, settings.gridSelector);
            initialize();
            $("body").on("click", `${settings.editSelector},${settings.clearSelector}`, function (e) {
                e.stopPropagation();
                e.preventDefault();
                e.stopImmediatePropagation();

                let url = saveUrl + "/" + $(this).data("id") ?? "";
                loadForm(url);

                $("html, body").animate({ scrollTop: 0 }, 500);
            });

            // Save
            $("body").on("click", settings.saveSelector, function () {
                var $valid = $(settings.formSelector).valid();
                if (!$valid) {
                    return false;
                }

                var data;
                if (settings.haseFile)
                    data = new FormData($(settings.formSelector)[0]);
                else
                    data = $(settings.formSelector).serialize();

                var url = $(settings.formSelector).attr("action");

                var options = {
                    url: url,
                    method: "POST",
                    data: data,
                    success: function (response) {
                        if (response.isSuccess) {
                            loadForm(saveUrl)
                                .then((data) => {
                                    loadDoctors(settings.baseUrl, settings.gridSelector);
                                    /*settings.load(settings.baseUrl, settings.gridSelector);*/
                                })
                                .catch((error) => {
                                    console.log(error)
                                })

                            toastr.success(response.success, 'Success');
                        }
                        else {
                            toastr.error(response.message, 'Error');
                            console.log(response);
                        }
                    }
                }
                if (settings.haseFile) {
                    options.processData = false;
                    options.contentType = false;
                }
                $.ajax(options);
            });

            $("body").on("click", settings.selectAllSelector, function () {
                $(".checkBox").prop('checked',
                    $(this).prop('checked'));
            });


            $("body").on("click", settings.deleteSelector, function (e) {
                e.preventDefault();
                $('input:checkbox.checkBox').each(function () {
                    if ($(this).prop('checked')) {
                        if (!selectedItems.includes($(this).val())) {
                            selectedItems.push($(this).val());
                        }
                    }
                });

                if (selectedItems.length > 0) {
                    $(settings.deleteModal).modal("show");
                } else {
                    toastr.info("Please select at least one item.", 'Warning');
                }
            });


            $("body").on('show.bs.modal', settings.deleteModal, function (event) {
                //event.preventDefault();
                // Get button that triggered the modal
                var source = $(event.relatedTarget);
                var id = source.data("id");

                // Extract value from data-* attributes
                var title = source.data("title");
                title = "Are you sure want to delete these items?";
                var modal = $(this);
                $(modal).find('.title').html(title);

                $("body").on("click", settings.finalDeleteSelector, function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    e.stopImmediatePropagation();


                    // Delete
                    $.ajax({
                        url: deleteUrl + "/" + selectedItems,
                        method: "POST",
                        success: function (response) {
                            console.log(response);
                            $(modal).modal("hide");
                            if (response.isSuccess) {
                                toastr.success(response.message, 'Success');
                                selectedItems = [];
                                loadDoctors(settings.baseUrl, settings.gridSelector);
                                loadForm(saveUrl);
                            }
                            else {
                                selectedItems = [];
                                toastr.error(response.message, 'Error');
                                console.log(response);
                            }
                        }
                    });
                });

            }).on('hide.bs.modal', function () {
                $("body").off("click", settings.finalDeleteSelector);
            });

            $("body").on("click", settings.topSelector, function (e) {
                e.preventDefault();
                $("html, body").animate({ scrollTop: 0 }, 500);
            });


            let loadUrl,
                target,
                reloadUrl,
                title,
                lastCode;
            // Quick add
            $("body").on("click", settings.quickAddSelector, function (e) {
                e.stopPropagation();
                e.preventDefault();
                e.stopImmediatePropagation();

                loadUrl = $(this).data("url");
                target = $(this).data("target");
                reloadUrl = $(this).data("reload-url");
                title = $(this).data("title");

                $(settings.quickAddModal + " .modal-title").html(title);
                $(settings.quickAddModal + " .modal-body").empty();

                $(settings.quickAddModal + " .modal-body").load(loadUrl, function () {
                    $(settings.quickAddModal).modal("show");
                    $("#header").hide();
                    $(settings.quickAddModal + " .modal-body #header").hide()

                    $("#left_menu").hide();
                    $(settings.quickAddModal + " .modal-body #left_menu").hide()

                    $("#main-content").toggleClass("collapse-main");
                    $(settings.quickAddModal + " .modal-body #main-content").toggleClass("collapse-main")

                    $("body").removeClass("sidebar-mini");
                })
            });

            $("body").on("click", ".js-modal-dismiss", function () {
                $("body").removeClass("sidebar-mini").addClass("sidebar-mini");
                $(settings.quickAddModal + " .modal-body #header").show()

                $("#left_menu").show();
                $(settings.quickAddModal + " .modal-body #left_menu").show()

                $("#main-content").toggleClass("collapse-main");
                $(settings.quickAddModal + " .modal-body #main-content").toggleClass("collapse-main")


                lastCode = $(settings.quickAddModal + " #lastCode").val();

                $(settings.quickAddModal + " .modal-body").empty();
                $(settings.quickAddModal).modal("hide");


                $(target).empty("");
                $(target).append($('<option>', {
                    value: '',
                    text: `--Select ${title}--`
                }));
                $.ajax({
                    url: reloadUrl,
                    method: "GET",
                    success: function (response) {
                        console.log(response);
                        $.each(response, function (i, item) {
                            $(target).append($('<option>', {
                                value: item.code,
                                text: item.name
                            }));
                        });
                        console.log(lastCode);
                        $(target).val(lastCode);
                    }
                });
            });

            $("body").on("keyup", settings.availabilitySelector, function () {
                var self = $(this);
                let code = $(".js-code").val();
                let name = self.val();

                // check
                $.ajax({
                    url: settings.baseUrl + "/CheckAvailability",
                    method: "POST",
                    data: { code: code, name: name },
                    success: function (response) {
                        console.log(response);
                        if (response.isSuccess) {
                            toastr.error(response.message);
                        }
                    }
                });
            });

            $("body").on("change", ".doctortype, .department, .speciality, .qualification", function () {
                loadDoctors(settings.baseUrl, settings.gridSelector);
            });

            $("body").on("click", "#addAppointment", function () {
                if ($("#AppointmentDays").val() == '') {
                    Swal.fire("Error", "Please Select Appointment Days!", "error");
                    $("#AppointmentDays").focus();
                    return;
                }
                if ($("#VisitingTimeFrom").val() == '') {
                    Swal.fire("Error", "Please Enter Visiting Time From", "error");
                    $("#VisitingTimeFrom").focus();
                    return;
                }

                if ($("#VisitingTimeTo").val() == '') {
                    Swal.fire("Error", "Please Enter Visiting Time To", "error");
                    $("#VisitingTimeTo").focus();
                    return;
                }      

                let appDays = $("#AppointmentDays").val(),
                    vStart = $("#VisitingTimeFrom").val(),
                    vEnd = $("#VisitingTimeTo").val(),
                    counter = $("#appointmentTable tbody tr").length;

                let item =
                    `<tr>
                        <td>
                            ${appDays}
                            <input type="hidden" id="Appointments_${counter}__AppointmentDays" name="Appointments[${counter}].AppointmentDays" value="${appDays}" />
                        </td>
                        <td>
                            ${vStart}
                            <input type="hidden" id="Appointments_${counter}__VisitingTimeFrom" name="Appointments[${counter}].VisitingTimeFrom" value="${vStart}" />
                        </td>
                        <td>
                            ${vEnd}
                            <input type="hidden" id="Appointments_${counter}__VisitingTimeTo" name="Appointments[${counter}].VisitingTimeTo" value="${vEnd}" />
                        </td>
                       <td><button type='button' class='btn btn-sm btn-danger js-remove-appointment'><i class='fa fa-times'></i></button></td>
                    </tr>`;

                $("#appointmentTable tbody").append(item);
                $("#AppointmentDays").val("0");
                $("#AppointmentDays").select2();
                $("#VisitingTimeFrom").val("");
                $("#VisitingTimeTo").val("");
                counter++;
            });

            $("body").on("click", ".js-remove-appointment", function (e) {
                e.preventDefault();
                remove($(this));
            })

            $("body").on("click", ".js-file-chooser", function (e) {
                e.preventDefault();
                var target = $(this).data("target");
                $(target).trigger("click");
            })

            $("body").on("change", ".js-file", function (e) {                
                e.preventDefault();
                var target = $(this).data("target");
                showImagePreview($(this), target);
            })

            $("body").on("click", ".js-clear-file", function (e) {
                e.preventDefault();
                var file = $(this).data("file");
                var tag = $(this).data("tag");
                clearImage(file, tag);
            })
        });

        function remove(selector) {
            $(selector).closest('tr').remove();
        }

        function showImagePreview(input, target) {
            //var target = $(input).data("target");
            if (input[0].files && input[0].files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(target).prop('src', e.target.result);
                };
                reader.readAsDataURL(input[0].files[0]);
            }
        }

        function clearImage(file, tag) {
            console.log(file);
            console.log(tag);
            $(file).removeAttr("src");
            $(tag).val(true);
        }

        //function clearSignature() {
        //    $("#signaturePreview").removeAttr("src");
        //    $("#IsClearSignature").val(true);
        //}

        function loadDoctors(baseUrl, gridSelector) {
            var data = {
                doctorTypeCode: $(".doctortype").val(),
                departmentCode: $(".department").val(),
                specialityCode: $(".speciality").val(),
                qualificationCode: $(".qualification").val()
            };

            var dataTable = $(gridSelector).DataTable({
                ajax: {
                    url: baseUrl + "/grid",
                    type: "GET",
                    datatype: "json",
                    data: data
                },

                columnDefs: [
                    { targets: [0], orderable: false }
                ],
                columns: [
                    {
                        "data": "doctorCode", "className": "text-center", width: "20px", "render": function (data) {
                            return `<input type="checkbox" class="checkBox" value="${data}" />`;
                        }
                    },
                    {
                        "data": "doctorCode", "className": "text-center", width: "10px",
                        render: function (data) {
                            return `<a href='${baseUrl}/Setup/${data}'>${data}</a>`;
                        }
                    },
                    { "data": "doctorName", "width": "200px" },
                    { "data": "banglaDoctorName", "width": "200px" },
                    
                    { "data": "departmentName", "className": "text-center", width: "120px" },
                    { "data": "specialist", "className": "text-center", width: "120px" },
                    { "data": "qualification", "className": "text-center", width: "120px" },
                    { "data": "appointmentDays", "className": "text-left", width: "380px" },
                    { "data": "visitingFee", width: "30px" },
                    { "data": "phone", "className": "text-center", width: "50px" },
                    { "data": "email", "className": "text-center", width: "50px" },
                    {
                        "data": "photoUrl", "className": "text-center", width: "120px",
                        render: function (data) {
                            if (data)
                                return `<img id="photoPreview" class="img-fluid img-thumbnail img-roundedr" src="${getBaseUrl()}/Uploads/Images/Doctors/${data}">`;
                            else
                                return '';
                        }
                    },
                    {
                        "data": "activityStatus", "className": "text-center", width: "120px",
                        render: function (data) {
                            if (data == "Active") {
                                return 'Yes';
                               // return `<span class='btn btn-sm btn-success' title="${data}"><i class='fa fa-check-circle'></i></span>`;
                            } else if (data = "Inactive") {
                                return 'No';
                               // return `<span class='btn btn-sm btn-danger' title="${data}"><i class='fa fa-times-circle'></i></span>`;
                            }

                            return 'No';
                        }
                    },
                    {
                        "data": "doctorCode", "render": function (data, type, row) {
                            return `<div class='action-buttons'>
                                        <a class='btn btn-warning btn-circle btn-sm' title="Edit ${row.doctorName}" href='${baseUrl}/Setup/${data}'><i class='fas fa-pencil-alt'></i></a>     
                                        <button type="button" class="btn btn-danger btn-circle btn-sm js-doctor-delete-confirm"
                                                data-target="#deleteModalx"
                                                data-id="${data}"
                                                title="Delete ${row.doctorName}"
                                                data-title="Are you sure want to delete ${row.doctorName}?">
                                                    <i class="fas fa-trash fa-sm"></i>
                                        </button>`;
                        },
                        "orderable": false,
                        "searchable": false,
                        width: "100px"
                    }
                ],
                lengthChange: true,
                pageLength: 10,
                order: [],
                sScrollY: "100%",
                scrollX: true,
                sScrollX: "100%",
                bDestroy: true
            });
        }

        function loadForm(url) {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: url,
                    type: 'GET',
                    success: function (data) {
                        $(settings.formContainer).empty();
                        $(settings.formContainer).html(data);
                        $.validator.unobtrusive.parse($(settings.formSelector));

                        initialize();
                        resolve(data)
                    },
                    error: function (error) {
                        reject(error)
                    },
                })
            })
        }

        function initialize() {
            $('.selectpicker').select2({
                language: {
                    noResults: function () {
                        //return 'Not found <a class="add_new_item" href="javascript:void(0)">Add New</a>';
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                }                
            });

            $('.datepicker').datetimepicker({
                format: 'DD/MM/YYYY',
                /*showTodayButton: true,*/
                // Your Icons
                // as Bootstrap 4 is not using Glyphicons anymore
                icons: {
                    time: 'fas fa-clock',
                    date: 'fas fa-calendar',
                    up: 'fas fa-chevron-up',
                    down: 'fas fa-chevron-down',
                    previous: 'fas fa-chevron-left',
                    next: 'fas fa-chevron-right',
                    today: 'fas fa-check',
                    clear: 'fas fa-trash',
                    close: 'fas fa-times'
                }
            });

            $('.timepicker').datetimepicker({
                format: 'hh:mm A',
                /*showTodayButton: true,*/
                // Your Icons
                // as Bootstrap 4 is not using Glyphicons anymore
                icons: {
                    time: 'fas fa-clock',
                    date: 'fas fa-calendar',
                    up: 'fas fa-chevron-up',
                    down: 'fas fa-chevron-down',
                    previous: 'fas fa-chevron-left',
                    next: 'fas fa-chevron-right',
                    today: 'fas fa-check',
                    clear: 'fas fa-trash',
                    close: 'fas fa-times'
                }
            });

            $('.datetimepicker').datetimepicker({
                format: 'DD/MM/YYYY hh:mm A',
                /*showTodayButton: true,*/
                // Your Icons
                // as Bootstrap 4 is not using Glyphicons anymore
                icons: {
                    time: 'fas fa-clock',
                    date: 'fas fa-calendar',
                    up: 'fas fa-chevron-up',
                    down: 'fas fa-chevron-down',
                    previous: 'fas fa-chevron-left',
                    next: 'fas fa-chevron-right',
                    today: 'fas fa-check',
                    clear: 'fas fa-trash',
                    close: 'fas fa-times'
                }
            });
        }
    }

}(jQuery));

