//Start Loead Category Block
function updateCategoryTable() {
    commonAjaxCall("/Categories/GetAllCategories", "POST", null, GetListCategorySuccessFun, FailureFunc, ErrorFunc)
}
var GetListCategorySuccessFun = function (resp) {
    if (resp.result) {

        //1 is root category
        //Make sure table have root category
        //Do not delete root category
        var list = resp.list.filter(function (obj) {
            return obj.ID != 1;
        });
        var datatableCategory = $("#CategoryDatatable").DataTable();
        datatableCategory.destroy();
        $("#CategoryDatatable").DataTable({
            data: list,
            fixedColumns: true,
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
                    $(obj).attr('onclick', 'updateStatus(this)');
                })
                //allSwitch.remove();
            },
            columns: [
                {
                    data: null,
                    className: "center",
                    sortable: false,
                    render: function (data, type, full, meta) {
                        return (meta.row + 1)
                    }
                },
                { data: 'CategoryName' },
                {
                    data: 'Description',
                    width: "55%"
                },
                {
                    data: null,
                    className: "text-center",
                    sortable: false,
                    render: function (data, type, full, meta) {
                        if (data.IsActive)
                            return "<lable><input type='checkbox' checked='' id='" + data.ID + "' class='js-switch'   data-switchery='" + data.IsActive + "' style='display: none;'></lable>"
                        else
                            return "<lable><input type='checkbox' id='" + data.ID + "' class='js-switch'   data-switchery='" + data.IsActive + "' style='display: none;'></lable>"
                    }
                },
                {
                    width: "12%",
                    data: null,
                    className: "text-center",
                    sortable: false,
                    render: function (data, type, full, meta) {
                        return '<div><i class="fa fa-pencil-square-o edit" onclick="getCategory(' + data.ID + ')" style="font-size: 24px;margin-right:14px;cursor: pointer;" aria-hidden="true"></i>' +
                            '<i class="fa fa-trash" onclick="deleteCategory(' + data.ID + ')" style="font-size: 24px;cursor: pointer;" aria-hidden="true"></i></div>'
                    }
                }
            ]
        });

    }
}
//End

//Start Search Category Block
function searchCategory() {
    var searchName = $("#inputSearchCategory").val();
    if (searchName == "") {
        searchName = null
    }
    var obj = { searchtext: searchName };
    commonAjaxCall("/Categories/SearchCategory", "POST", obj, SearchCategorySuucessFun, FailureFunc, ErrorFunc)

}
var SearchCategorySuucessFun = function (resp) {
    if (resp.result) {
        var json_obj = resp.list;
        parentCategoryList = json_obj;
        $("#inputSearchCategory").val("");
        $("#ParentCategoryDiv").html("");
        if (json_obj.length >= 2) {
            for (var i = 1; i < json_obj.length; i++) {
                ArrengeCategoriesForSearch(0, json_obj[i], GetChildList(parseInt(json_obj[i].ID)));
            }

        }
        else {
            $("#ParentCategoryDiv").html("<span class='badge badge-info'>No Record Found</span>");
            //$("#searchCatBlock").css()
        }
    }
    else {
        swal({
            title: "Fail!",
            text: "Something went wrong will searching category",
            button: "Ok",
        });
    }

}
//End

//Start Add Category Block
function AddCategory() {

    if (formValidation("FormAddCategoryId")) {
        let isactive = false;
        if ($("#IsActive").is(":checked")) {
            isactive = true;
        }
        entity = {};
        entity.CategoryName = $("#CategoryName").val();
        entity.Description = $("#Description").val();
        entity.ParentCategoryID = ($("#ParentCategoryID").val() == "" ? null : $("#ParentCategoryID").val());
        entity.IsActive = isactive;
        commonAjaxCall("/Categories/AddCategory", "POST", entity, AddCategorySuccessFun, FailureFunc, ErrorFunc)
    }
}
var AddCategorySuccessFun = function (resp) {
    if (resp.result) {
        $("#CategoryModal").modal("hide");
        clearFormControll("FormAddCategoryId");
        swal({
            title: "Success!",
            text: "Category has been Added.",
            icon: "success",
            button: "Ok",
        });
        updateCategoryTable();
    }
    else {
        $("#CategoryModal").modal("hide");
        swal({
            title: "Fail!",
            text: "Something went wrong will adding category",
            button: "Ok",
        });
    }
}
//End

//Start Update Category Block
function UpdateCategory() {

    if (formValidation("FormAddCategoryId")) {
        let isactive = false;
        if ($("#IsActive").is(":checked")) {
            isactive = true;
        }
        entity = {};
        entity.ID = $("#CategoryID").val();
        entity.CategoryName = $("#CategoryName").val();
        entity.Description = $("#Description").val();
        entity.ParentCategoryID = ($("#ParentCategoryID").val() == "" ? null : $("#ParentCategoryID").val());
        entity.IsActive = isactive;
        commonAjaxCall("/Categories/UpdateCategory", "POST", entity, UpdateCategorySuccessFun, FailureFunc, ErrorFunc)
    }
}
var UpdateCategorySuccessFun = function (resp) {
    if (resp.result) {
        $("#CategoryModal").modal("hide");
        clearFormControll("FormAddCategoryId");
        swal({
            title: "Success!",
            text: "Category has been Updated.",
            icon: "success",
            button: "Ok",
        });
        updateCategoryTable();
    }
    else {
        swal({
            title: "Fail!",
            text: "Something went wrong will updating category",
            button: "Ok",
        });
    }
}
function getCategory(catId) {
    var data = { "cateId": catId };
    commonAjaxCall("/Categories/GetCategoryById", "POST", data, GetCategorySuccessFun, FailureFunc, ErrorFunc)
}
var GetCategorySuccessFun = function (resp) {
    if (resp.result) {
        clearFormControll("FormAddCategoryId");
        var headerTitile = "Update Category";
        var buttonText = "Update";
        var onclickEvent = "UpdateCategory()";
        updateModalContent("#CategoryModal", headerTitile, buttonText, onclickEvent);
        var value = resp.obj;

        $("#CategoryName").val(value.CategoryName);

        if (value.ParentCategoryName.toLowerCase() == "root") {
            $("#ParentCategoryName").val("");
        }
        else {
            $("#ParentCategoryName").val(value.ParentCategoryName);
        }
        $("#Description").val(value.Description);
        $("#IsActive").prop("checked", value.IsActive);

        $("#CategoryID").val(value.ID);
        if (value.ParentCategoryID != null) {
            $("#ParentCategoryID").val(value.ParentCategoryID);
        }
        else {
            $("#ParentCategoryID").val("");
        }

        $("#CategoryModal").modal("show");

    }
    else {
        swal({
            title: "Fail!",
            text: "Something went wrong will adding category",
            button: "Ok",
        });
    }

}
//End

//Start Update Parent Category Block
function selectParentCategory(obj) {
    var id = $(obj).attr('id').split('_')[1];
    var catName = $(obj).attr('data-name');
    $("#ParentCategoryName").val(catName);
    $("#ParentCategoryID").val(id)
    $("#SelectParentCategoryModal").modal('hide');

}
function removeParentCategory() {
    $("#ParentCategoryName").val("");
    $("#ParentCategoryID").val(null);

}
function changeIcon(obj) {

    if ($(obj).children().hasClass('fa-chevron-circle-down')) {
        $(obj).children().removeClass('fa-chevron-circle-down')
        $(obj).children().addClass('fa-chevron-circle-right');
    }
    else {
        $(obj).children().removeClass('fa-chevron-circle-right')
        $(obj).children().addClass('fa-chevron-circle-down');
    }
}
function updateStatus(obj) {
    var status = $($(obj).parent().children()[0]).is(":checked")
    var id = $($(obj).parent().children()[0]).attr('id');

    var entity = {
        id: parseInt(id),
        status: status
    }
    commonAjaxCall("/Categories/UpdateStatus", "POST", entity, updateStatusSuccessFun, FailureFunc, ErrorFunc)

}
var updateStatusSuccessFun = function (resp) {
    if (resp.result) {
        swal({
            title: "Success!",
            text: "Status has been Updated.",
            icon: "success",
            button: "Ok",
        });
    }
    else {
        swal({
            title: "Fail!",
            text: "Something went wrong will updating a category status",
            button: "Ok",
        });
    }
}
//End

//Start Select Parent Category Block
//Function call Parent Category Input click
function loadParentCategoryDiv() {
    commonAjaxCall("/Categories/GetAllCategories", "POST", null, LoadParentCategorySuucessFun, FailureFunc, ErrorFunc)
}
var LoadParentCategorySuucessFun = function (resp) {

    var _Html = "";
    if (resp.result) {
        $("#inputSearchCategory").val("");
        $("#ParentCategoryDiv").html("");
        var json_obj = resp.list;
        parentCategoryList = json_obj;
        if (json_obj.length > 1) {

            ArrengeCategories(0, parseInt(json_obj[0].ID), GetChildList(parseInt(json_obj[0].ID)));

            $("#SelectParentCategoryModal").modal('show');
        } else {
            $("span#statusmsgcategory").show();
            $("span#statusmsgcategory").html("No more category ");
            setTimeout(function () {
                $("span#statusmsgcategory").fadeOut();
                $("span#statusmsgcategory").html("");
            }, 2000);
        }

    }
    //else {
    //    $(".categorylistbody").html("<span class='error-msg txt-center'>No search result found.</span>");
    //    $("#categorydialogheader").hide();
    //    $("#categorydialogimgstatus").hide();
    //}
}
//End

//Start Search Parent Category Block
function GetChildList(CatId) {
    try {
        viewData = [];
        var cnt = 0;
        for (var c in parentCategoryList) {
            if (parseInt(parentCategoryList[c].ParentCategoryID) == CatId) {
                viewData.push(parentCategoryList[c]);
            }
        }
        return viewData;
    } catch (e) {
    }
}
function ArrengeCategories(lvl, catid, ChildList) {
    try {
        var cname, detect = false;
        if (ChildList.length == 0) {
            var obj = $("." + subcategorydiv);
            $(obj).next().css("margin-left", "20px");
            $(obj).remove();

        }

        for (var j in ChildList) {
            if (parseInt(ChildList[j].ParentCategoryID) == catid) {
                var startdiv = "";
                if ($(".parentdiv_" + ChildList[j].ParentCategoryID).length == 0) {
                    var parnetstartdiv = "<div class='parentdiv_" + ChildList[j].ParentCategoryID + " parentcategorydiv '>";
                    $("#ParentCategoryDiv").append(parnetstartdiv);
                }
                startdiv += "<div class='div_" + ChildList[j].ParentCategoryID + " categorycontainer parentdiv_" + ChildList[j].ID + " collapse show'><div class='row_" + ChildList[j].ParentCategoryID + "' >";
                startdiv += "<span><div style='padding-top:5px;padding-bottom:5px;display:inline-block;padding-left:" + (20 * ChildList[j].Depth) + "px;'><span class='collapse_" + ChildList[j].ID + "'><a style='padding:3px;' onclick='changeIcon(this)'   href='.div_" + ChildList[j].ID + "' data-toggle='collapse'><i class='fa fa-chevron-circle-down'></i></a></span><a id='a_" + ChildList[j].ID + "' onclick='selectParentCategory(this)' title='Select Category' data-name='" + ChildList[j].CategoryName + "' class='subcategorylink'>";
                $(".parentdiv_" + ChildList[j].ParentCategoryID).append(startdiv + ChildList[j].CategoryName + "</a></div>");
                subcategorydiv = "collapse_" + ChildList[j].ID;
                subtxtdiv = "spantxt_" + ChildList[j].ID;
                ArrengeCategories(lvl, ChildList[j].ID, GetChildList(ChildList[j].ID));
                var enddiv = "</span></div></div>";
                $(".parentdiv_" + ChildList[j].ParentCategoryID).append(enddiv);
            }
        }
    } catch (e) {
    }
}
function ArrengeCategoriesForSearch(ind, cat, ChildList) {
    try {
        var cname, detect = false;
        var startdiv = "";
        var parentCatId = (cat.ParentCategoryID == null ? ind : cat.ParentCategoryID);
        if ($(".parentdiv_" + parentCatId).length == 0) {
            var parnetstartdiv = "<div class='parentdiv_" + parentCatId + " parentcategorydiv '>";
            $("#ParentCategoryDiv").append(parnetstartdiv);
        }
        startdiv += "<div class='div_" + parentCatId + " categorycontainer parentdiv_" + cat.ID + " collapse show'><div class='row_" + parentCatId + "' >";
        startdiv += "<span><div style='padding-top:5px;padding-bottom:5px;display:inline-block;padding-left:" + (20 * cat.Depth) + "px;'><span class='collapse_" + cat.ID + "'><a style='padding:3px;' onclick='changeIcon(this)'   href='.div_" + cat.ID + "' data-toggle='collapse'><i class='fa fa-chevron-circle-down'></i></a></span><a id='a_" + cat.ID + "' onclick='selectParentCategory(this)' title='Select Category' data-name='" + cat.CategoryName + "' class='subcategorylink'>";
        $(".parentdiv_" + parentCatId).append(startdiv + cat.CategoryName + "</a></div>");
        subcategorydiv = "collapse_" + cat.ID;
        subtxtdiv = "spantxt_" + cat.ID;
        var enddiv = "</span></div></div>";
        $(".parentdiv_" + parentCatId).append(enddiv);
        if ($(".parentdiv_" + parentCatId).before().hasClass('parentcategorydiv') && $($(".parentdiv_" + parentCatId).find('span').children()[0]).css('padding-left') != "20px") {
            $($(".parentdiv_" + parentCatId).find('span').children()[0]).css('padding-left', "17.5px")
        }
        if (ChildList.length == 0) {
            var obj = $("." + subcategorydiv);
            $(obj).next().css("margin-left", "20px");
            $(obj).remove();
        }
        //  }
        //}
    } catch (e) {
    }
}
//End

//Start Error Handling Block
var FailureFunc = function (error) {

}
var ErrorFunc = function (error) {

}
//End

//Start Delete Category Block
function deleteCategory(catId) {

    delete_confirmation(
        "Are you sure?",
        "Are you sure you want to delete this category?",
        "warning",
        function (data) {
            var data = { "cateId": catId };
            commonAjaxCall("/Categories/Delete", "POST", data, DeleteCategorySuccessFun, FailureFunc, ErrorFunc)
            //$.ajax({
            //    type: "POST",
            //    url: '/Categories/Delete',
            //    //contentType: "application/json; charset=utf-8",
            //    data: { "cateId": catId },
            //    dataType: "json",
            //    success: function (response) {

            //        //$.ajax({
            //        //    method: 'GET',
            //        //    url: '/User/RefreshDiv2',
            //        //    // data: { }, // Pass value that you want to controller.
            //        //    dataType: "json",
            //        //    success: function (data) {
            //        //        
            //        //        // $("#ib2").val(data); //Implement partial refreshing.
            //        //    }
            //        //});

            //        swal("Poof! Category has been deleted!", {
            //            icon: "success"
            //        });
            //        window.location.href = "/User/Index"
            //    },
            //    failure: function (response) {
            //        alert(response.responseText);
            //    },
            //    error: function (response) {
            //        alert(response.responseText);
            //    }
            //});
        });
}
var DeleteCategorySuccessFun = function (resp) {
    if (resp.result) {
        updateCategoryTable();
        swal({
            title: "Success!",
            text: "Category has been Deleted.",
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
//End

//Start Mpdal Block
function openCategoryModel() {
    clearFormControll("FormAddCategoryId");
    var headerTitile = "Add Category";
    var buttonText = "Submit";
    var onclickEvent = "AddCategory()";
    updateModalContent("#CategoryModal", headerTitile, buttonText, onclickEvent);
    $("#CategoryModal").modal("show");
}
//End

//Start Other Block
$(".required").keyup(function () {
    if ($(this).next().hasClass("error-span")) {
        $(this).next().remove();
    }
});
//End