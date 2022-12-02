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
            pieChart();
        });


    function pieChart() {
        const alivePetsCount = document.getElementById("alive-pets-count");
        const deadPetsCount = document.getElementById("dead-pets-count");

        Morris.Donut({
            element: "donut-chart",
            labelColor: "#ffffff",
            data: [
                { label: "Alive pets count", value: +alivePetsCount.innerHTML, color: "#30c970" },
                { label: "Dead pets count", value: +deadPetsCount.innerHTML, color: "#FF1744" }
            ],
            resize: true,
            redraw: true
        });
    }
})