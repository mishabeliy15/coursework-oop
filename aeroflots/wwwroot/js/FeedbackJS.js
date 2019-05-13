const uri = "api/feedbacks";
let commited = false;

function CreateFeedback(obj) {
    return `<div class="card">
        <div class="card-header">
            [${new Date(obj.date).toLocaleString()}] ${obj.name}
        </div>
        <div class="card-body">
            <p class="card-text">${obj.comment}</p>
        </div>
    </div>`;
}

function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            console.log(data);
            const content = $("#content");
            $(content).empty();
            data = data.reverse();
            for (let i of data)
                $(content).append(CreateFeedback(i));
        }
    });
}

function addFeedback() {
    if (commited) return;
    const item = {
        Name: $("#fieldName")[0].value,
        Comment: $("#fieldComment")[0].value
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri,
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            commited = true;
            $("#fieldName")[0].disabled = true;
            $("#fieldComment")[0].disabled = true;
            $("#btnsubmit")[0].disabled = true;
            getData();
        }
    });
}

getData();