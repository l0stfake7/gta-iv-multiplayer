var font = API.createFont('Segoe UI', 34.0);
var font2 = API.createFont('Segoe UI', 84.0);
var draw2 = API.createTextDraw(1500, 850, 'Health: ', font, 0xFFFFFF, 255);
var draw = API.createTextDraw(200, 875, '•', font2, 0x8080FF, 255);

var linintX1 = API.createInterpolator('easeinout', 1500, 1580, 2000);
var linintX2 = API.createInterpolator('easeinout', 1580, 1500, 2000);

function movex1() {
    draw.interpolateX(linintX1);
}
function movex2() {
    draw.interpolateX(linintX2);
    draw2.text = 'Health: ' + Client.getPlayerPed().Health;
}

linintX1.OnInterpolationFinished(movex2);
linintX2.OnInterpolationFinished(movex1);

movex1();