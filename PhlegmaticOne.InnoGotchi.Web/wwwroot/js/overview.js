$(() => {

    window.fetch("/Overview/MyPreviewPartial")
        .then(response => response.text())
        .then(data => {
            document.getElementById("preview_statistics").innerHTML = data;
        });

    window.fetch("/Overview/MyDetailedPartial")
        .then(response => response.text())
        .then(data => {
            document.getElementById("detailed_statistics").innerHTML = data;
        });
})