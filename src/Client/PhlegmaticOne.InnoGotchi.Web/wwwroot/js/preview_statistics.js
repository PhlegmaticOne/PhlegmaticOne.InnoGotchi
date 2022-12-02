$(() => {
    window.fetch("/Overview/MyPreviewPartial")
        .then(response => response.text())
        .then(data => {
            document.getElementById("preview_statistics").innerHTML = data;
        });
})