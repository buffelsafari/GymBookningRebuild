// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


if (document.getElementById("historyCheckBoxId")!=null) {
    document.getElementById("historyCheckBoxId").addEventListener("click", onHistoryCheckBox);
}


    let actionEvent = document.getElementById("actionEventId").value;


let  b = document.getElementsByClassName("booking");
for (let i = 0; i < b.length; i++) {
    b[i].addEventListener("click", onBooking);
}



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

            // re add paginator listeners
            let d = document.getElementsByClassName("pagi");
            for (let i = 0; i < d.length; i++) {
                d[i].addEventListener("click", onPaginator);
            }

            // re add booking listeners
            let b = document.getElementsByClassName("booking");
            for (let i = 0; i < b.length; i++) {
                b[i].addEventListener("click", onBooking);
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

            console.log("checkbox from java:" + event.target.checked);
            document.getElementById("gymClassesTableId").innerHTML = result;

            // re add paginator listeners
            let d = document.getElementsByClassName("pagi");
            for (let i = 0; i < d.length; i++) {
                d[i].addEventListener("click", onPaginator);
            }

            // re add booking listeners
            let b = document.getElementsByClassName("booking");
            for (let i = 0; i < b.length; i++) {
                b[i].addEventListener("click", onBooking);
            }

        }

    
    });

}

function onBooking(event)
{
    $.ajax({
        type: "GET",
        url: "/GymClasses/OnBooking",
        data: { id: event.target.value },
        cache: false,
        success: function (result) {


            console.log(event.target.value);

            let dest = document.getElementById("destroyOnBookingId").value;
            console.log(dest)
            if (dest==true) {
                document.getElementById("gymClass " + event.target.value).innerHTML = "";
            }
            else {
                document.getElementById("gymClass " + event.target.value).innerHTML = result;
            }
            // re add booking listeners
            let b = document.getElementsByClassName("booking");
            for (let i = 0; i < b.length; i++) {
                b[i].addEventListener("click", onBooking);
            }

        }

    }
    );

}