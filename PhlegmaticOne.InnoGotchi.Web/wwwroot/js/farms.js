$(() => {

    window.fetch("/Farms/MyPreviewPartial")
        .then(response => response.text())
        .then(data => {
            document.getElementById("preview_statistics").innerHTML = data;
        });

    window.fetch("/Farms/MyDetailedPartial")
        .then(response => response.text())
        .then(data => {
            document.getElementById("detailed_statistics").innerHTML = data;
        });
})