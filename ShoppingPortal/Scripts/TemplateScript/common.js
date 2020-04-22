function delete_confirmation(Callback) {
    var confirmationValue = false;
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    },function (isConfirm) {
            if (isConfirm) {
                Callback(isConfirm)
                return isConfirm;
            }
            else {
                swal("Cancelled", "Your imaginary file is safe :)", "error");
            }
        });
}

