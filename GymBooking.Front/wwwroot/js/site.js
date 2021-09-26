// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


if (document.getElementById("historyCheckBoxId")!=null) {
    document.getElementById("historyCheckBoxId").addEventListener("click", onHistoryCheckBox);
}


    let actionEvent = document.getElementById("actionEventId").value;






let d = document.getElementsByClassName("pagi");
for (let i = 0; i < d.length; i++) {
    d[i].addEventListener("click", onPaginator);
}

function onPaginator(event) {

    let viewHistory = document.getElementById("viewHistoryId").value;
    console.log("vHistory:" + viewHistory);

    $.ajax({
        type: "GET",
        url: "/GymClasses/" + actionEvent,
        data: { buttonValue: event.target.value, viewHistory: viewHistory },
        cache: false,
        success: function (result) {

            console.log(event.target.value);
            document.getElementById("gymClassesTableId").innerHTML = result;

            // re add listeners
            let d = document.getElementsByClassName("pagi");
            for (let i = 0; i < d.length; i++) {
                d[i].addEventListener("click", onPaginator);
            }

        }
    });

}








function onHistoryCheckBox(event) {

    

    $.ajax({
        type: "GET",
        url: "/GymClasses/" + actionEvent,
        data: { buttonValue: "1", viewHistory: event.target.checked },
        cache: false,
        success: function (result) {

            console.log("checkbox from java:"+event.target.checked);
            document.getElementById("gymClassesTableId").innerHTML = result;

            // re add listeners
            let d = document.getElementsByClassName("pagi");
            for (let i = 0; i < d.length; i++) {
                d[i].addEventListener("click", onPaginator);
            }

            }

        }
    );

}