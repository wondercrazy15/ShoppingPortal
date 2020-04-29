function commonAjaxCall(url, type, data, successFunc, failureFunc, errorFunc) {
    if (successFunc !== undefined || successFunc !== "") {
        if (failureFunc == undefined || failureFunc == "") {
            failureFunc = mainFailureFunc;
        }
        if (errorFunc == undefined || errorFunc == "") {
            errorFunc = mainErrorFunc;
        }
        $.ajax({
            type: type,
            url: url,
            contentType: "application/json; charset=utf-8",
            data: (data != null?JSON.stringify(data):null) ,
            dataType: "json",
            success: successFunc,
            failure: failureFunc,
            error: errorFunc
        });
    }
}

function delete_confirmation(title, text, type, Callback) {
    var confirmationValue = false;
    swal({
        title: title,
        text: text,
        type: type,// "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, Delete it!",
        cancelButtonText: "No, Cancel!",
        closeOnConfirm: false,
        closeOnCancel: false
    }, function (isConfirm) {
        if (isConfirm) {
            Callback(isConfirm)
            return isConfirm;
        }
        else {
            //swal("Cancelled", "Operation", "error");
            swal.close();
        }
    });
}

function clearFormControll(formId) {
    var form = $("#" + formId);
    var allFormInput = $(form).find(".data-input");
    $.each(allFormInput, function (val,input) {
        $(input).val("");             
    });
}
function formValidation(formId) {
    var formValidate = true;
    var form = $("#" + formId);
    var allFormInput = $(form).find(".data-input");
    $.each(allFormInput, function (val, input) {
        if ($(input).val().trim() == "" && $(input).hasClass("required")) {
            formValidate = false;
            if ($(input).next().hasClass("error-span")==false) { 
                $(input).after("<span class='text-danger error-span'>Field must be Required.</span>")
            }
           // dynamicKeupEvent(input)
        }
    });
    return formValidate;
}


function updateModalContent(modalId, headerTitile, buttonText, onclickEvent) {
    
    var modal = $(modalId);
    modal.find("#modalHeaderTitle").text(headerTitile);
    modal.find("#modalSubmitButton").text(buttonText);

    modal.find("#modalSubmitButton").attr("onclick", onclickEvent)
    $(".error-span").remove();
}

