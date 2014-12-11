var font = API.createFont('Impact', 24.0);
var draw = API.createTextDraw(200, 300, 'Chuj', font, 0xFFFFFF, 255);

var linintX1 = API.createInterpolator('easeinout', 1150, 300, 1000);
var linintX2 = API.createInterpolator('easeinout', 300, 1150, 2000);

function movex1() {
    draw.interpolateX(linintX1);
}
function movex2() {
    draw.interpolateX(linintX2);
}

linintX1.OnInterpolationFinished(movex2);
linintX2.OnInterpolationFinished(movex1);

movex1();