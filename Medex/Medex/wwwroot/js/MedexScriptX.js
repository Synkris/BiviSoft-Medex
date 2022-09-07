
//function AddingDepartment() {
//    debugger;
//    var department = {};
//    department.Name = $("#Department").val();

//    var departmentInSerializedMode = JSON.stringify(department);
//     // Posting The Department
//        $.ajax({
//            type: 'POST',
//            URL: '/Department/AddNewDepartment',
//            data:
//            {
//                department: departmentInSerializedMode
//            },
//            success: function (result) {
//                debugger;
//                if (!result.isError) {
//                    var redirectUrl = "/Account/Register";
//                    newSuccessAlert(result.msg, redirectUrl)
//                }
//                else {
//                    errorAlert(result.msg);
//                    location.url.reload("AddDepartment");
//                }
//            },
//            error: function (ex) {
//                errorAlert(ex);
//            }

//        });

//}

function addingNewDepartment()
{
    debugger;
    var department = {};
    department.Name = $("#departmentNameId").val();
    department.Active = true;
    var departmentInSerializedMode = JSON.stringify(department);
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/Department/AddNewDepartment', // we are calling json method
        data:
        {
            department: departmentInSerializedMode
        },
        success: function (result) {
            if (result.isError) {
                debugger;
                var retrunToThisUrl = location.url.reload("AddDepartment");
                newErrorAlert(result.msg, retrunToThisUrl);
            }
            else {
                debugger;
                var redirectUrl = "/Department/AddNewDepartment";
                newSuccessAlert(result.msg, redirectUrl)
            }
        },
        error: function (ex) {
            errorAlert(ex);
        }
    })
}