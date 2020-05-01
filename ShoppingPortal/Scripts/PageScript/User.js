$(document).ready(function () {
    loadUserData();
});



function loadUserData() {
    Get('GET', '/User/GetUser',
        function (data) {
            $('#example').DataTable({
                destroy: true,
                "aaData": data.data,
                fnDrawCallback: function (oSettings) {
                    //Add Switch
                    if ($(".js-switch")[0]) {
                        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
                        $.each(elems, function (key, obj) {
                            if ($(obj).next().hasClass("switchery-default")) {
                                $(obj).next().remove();
                            }

                        });
                        elems.forEach(function (html) {
                            var switchery = new Switchery(html, {
                                color: '#26B99A'
                            });
                        });
                    }
                    var allSwitch = $(".switchery-default");
                    $.each(allSwitch, function (key, obj) {
                        $(obj).attr('onclick', 'UpdateUserStatus(this)');
                    })
                    //allSwitch.remove();
                },
                "aoColumns": [
                    { 'data': 'FirstName' },
                    {
                        data: null,
                        className: "text-center",
                        sortable: false,
                        render: function (data, type, row) {

                            if (row.IsActive)
                                return "<lable><input type='checkbox' checked='' id='" + row.userId + "' class='js-switch'   data-switchery='" + row.IsActive + "' style='display: none;'></lable>"
                            else
                                return "<lable><input type='checkbox' id='" + row.userId + "' class='js-switch'   data-switchery='" + row.IsActive + "' style='display: none;'></lable>"
                        }
                    },
                    {
                        data: null,
                        className: "text-center",
                        sortable: false,
                        render: function (data, type, row) {
                            return "<div><i class='fa fa-pencil-square-o edit' onclick=loadUserForm('" + row.userId + "') style='font-size: 24px;margin-right:14px;cursor: pointer;' aria-hidden='true'></i>" +
                                "<i class='fa fa-trash' onclick=deleteUser('" + row.userId + "') style='font-size: 24px;margin-right:14px;cursor: pointer;' aria-hidden='true'></i>" +
                                "<i class='fa fa-eye' onclick=userDetail('" + row.userId + "') style='font-size: 24px;cursor: pointer;' aria-hidden='true'></i></div>";
                        }
                    }
                ]
            });
        },
        function (data) {
            swal({
                title: "Fail!",
                text: "Something went wrong while fetching data!!!",
                button: "Ok",
            });
        },
        function (data) {
            swal({
                title: "Fail!",
                text: data.statusText,
                button: "Ok",
            });

        });
}


function loadUserForm(userId) {
    var modelTitle = userId !== null ? "Update customer" : "Add new customer";
    var data = { "userId": userId };
    $("#modelTitle").html(modelTitle);

    POST('POST', '/User/LoadUserForm', data, "html",
        function (data) {
            $(".jsUsermodelBody").html(data);

            $("#usermodel").modal("show");
        },
        function (data) {
            swal({
                title: "Fail!",
                text: "Something went wrong while fetching data!!!",
                button: "Ok",
            });
        },
        function (data) {
            swal({
                title: "Fail!",
                text: data.statusText,
                button: "Ok",
            });
        });
}

function addOrUpdateUser() {

    $('form').removeData("validator");
    $('form').removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse('form');

    if ($("#ModalForm").valid()) {
        var data = $("#ModalForm").serializeArray();
        var Msg = data[0].value !== "" ? "Updated" : "Inserted";

        POST('POST', '/User/AddOrUpdate', data, "json",
            function (data) {
                $("#usermodel").modal("hide");
                swal(Msg, "Record has been " + Msg + " ", 'success');
                loadUserData();
            },
            function (data) {
                swal({
                    title: "Fail!",
                    text: "Something went wrong while fetching data!!!",
                    button: "Ok",
                });
            },
            function (data) {
                swal({
                    title: "Fail!",
                    text: data.statusText,
                    button: "Ok",
                });
            });
    }

}

function openImportFileModel() {
    $("#fileImportUrl").val("/User/ImportFile");
    $("#fileImportModelTitle").html("Import Customers By EXCEL,CSV");
    $("#fileUploadModel").modal("show");
}

function UpdateUserStatus(obj) {
    var status = $($(obj).parent().children()[0]).is(":checked")
    var id = $($(obj).parent().children()[0]).attr('id');

    var data = {
        userId: id,
        status: status
    }

    POST('POST', '/User/UpdateStatus', data, "json",
        function (data) {
            swal("Success!", "Status Update Successfully", 'success');
            loadUserData();
        },
        function (data) {
            swal({
                title: "Fail!",
                text: "Something went wrong while fetching data!!!",
                button: "Ok",
            });
        },
        function (data) {
            swal({
                title: "Fail!",
                text: data.statusText,
                button: "Ok",
            });
        });

}

function deleteUser(userId) {

    delete_confirmation(
        "Are you sure?",
        "Are you sure you want to delete this category?",
        "warning",
        function (data) {
            var data = { "userId": userId };
            commonAjaxCall('/User/Delete', "POST", data, DeleteCategorySuccessFun, FailureFunc, ErrorFunc)
        });
}
var DeleteCategorySuccessFun = function (resp) {
    if (resp.Status) {
        loadUserData();
        swal({
            title: "Success!",
            text: "User has been Deleted.",
            icon: "success",
            button: "Ok",
        });
    }
    else {
        swal({
            title: "Fail!",
            text: "Something went wrong will deleting category",
            button: "Ok",
        });
    }
}

//Start Error Handling Block
var FailureFunc = function (error) {

}
var ErrorFunc = function (error) {

}
//End

function userDetail(userId) {
    var modelTitle = userId !== null ? "Update customer" : "Add new customer";
    var data = { "userId": userId };
    $("#modelTitle").html(modelTitle);

    POST('POST', '/User/LoadUserDetail', data, "html",
        function (data) {
            $('.info-model-body').html(data);
            $("#userInfoModel").modal("show");
        },
        function (data) {
            swal({
                title: "Fail!",
                text: "Something went wrong while fetching data!!!",
                button: "Ok",
            });
        },
        function (data) {
            swal({
                title: "Fail!",
                text: data.statusText,
                button: "Ok",
            });
        });
}
