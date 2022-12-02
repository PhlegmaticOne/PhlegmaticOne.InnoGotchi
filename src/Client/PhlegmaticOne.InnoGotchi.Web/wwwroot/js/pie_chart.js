
function donutChart(data, targetElement) {
    Morris.Donut({
        element: targetElement,
        labelColor: "#ffffff",
        data: data,
        resize: true,
        redraw: true
    });
}

function barChart(data, labels, colors, targetElement) {
    
    config = {
        data: data,
        xkey: 'x',
        ykeys: ['a', 'b'],
        labels: labels,
        fillOpacity: 0.6,
        hideHover: false,
        behaveLikeLine: true,
        resize: true,
        pointStrokeColors: ['white'],
        barColors: colors,
        element: targetElement
    };

    Morris.Bar(config);
}