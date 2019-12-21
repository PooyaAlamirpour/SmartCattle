var c_1;
var ctx_1;
var speedGradient_1;
var rpmGradient_1;

function Initial_BarnSpeed_1()
{
    let dev_1 = false;

    SetHumidity_1 = Humidity_1;
    SetTemp_1 = Tempreture_1;

    c_1 = document.getElementById("canvas_1");
    c_1.width = 500;
    c_1.height = 500;
    ctx_1 = c_1.getContext("2d");
    ctx_1.scale(1, 1);
    speedGradient_1 = ctx_1.createLinearGradient(0, 500, 0, 0);
    speedGradient_1.addColorStop(0, '#00b8fe');
    speedGradient_1.addColorStop(1, '#41dcf4');

    rpmGradient_1 = ctx_1.createLinearGradient(0, 500, 0, 0);
    rpmGradient_1.addColorStop(0, '#f7b733');
    rpmGradient_1.addColorStop(1, '#fc4a1a');

    setSpeed_1();
}

function speedNeedle_1(rotation_1) {
    ctx_1.lineWidth = 2;

    ctx_1.save();
    ctx_1.translate(250, 250);
    ctx_1.rotate(rotation_1);
    ctx_1.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_1.restore();

    rotation_1 += Math.PI / 180;
}

function rpmNeedle_1(rotation_1) {
    ctx_1.lineWidth = 2;

    ctx_1.save();
    ctx_1.translate(250, 250);
    ctx_1.rotate(rotation_1);
    ctx_1.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_1.restore();

    rotation_1 += Math.PI / 180;
}

function drawMiniNeedle_1(rotation_1, width, speed) {
    ctx_1.lineWidth = width;

    ctx_1.save();
    ctx_1.translate(250, 250);
    ctx_1.rotate(rotation_1);
    ctx_1.strokeStyle = "#333";
    ctx_1.fillStyle = "#333";
    ctx_1.strokeRect(-20 / 2 + 220, -1 / 2, 20, 1);
    ctx_1.restore();

    let x = (250 + 180 * Math.cos(rotation_1));
    let y = (250 + 180 * Math.sin(rotation_1));

    ctx_1.font = "700 23px Open Sans";
    ctx_1.fillText(speed, x, y);

    rotation_1 += Math.PI / 180;
}

function calculateSpeedAngle_1(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian <= 1.45 ? radian : 1.45;
}

function calculateRPMAngel_1(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian >= -0.46153862656807704 ? radian : -0.46153862656807704;
}

function drawSpeedo_1(speed, gear, rpm, topSpeed) {
    if (speed == undefined) {
        return false;
    } else {
        speed = Math.floor(speed);
        rpm = rpm * 10;
    }

    ctx_1.clearRect(0, 0, 500, 500);

    ctx_1.beginPath();
    ctx_1.fillStyle = 'rgba(0, 0, 0, .9)';
    ctx_1.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_1.fill();
    ctx_1.save()
    ctx_1.restore();
    ctx_1.fillStyle = "#FFF";
    ctx_1.stroke();

    ctx_1.beginPath();
    ctx_1.strokeStyle = "#333";
    ctx_1.lineWidth = 10;
    ctx_1.arc(250, 250, 100, 0, 2 * Math.PI);
    ctx_1.stroke();

    ctx_1.beginPath();
    ctx_1.lineWidth = 1;
    ctx_1.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_1.stroke();

    ctx_1.font = "700 65px Open Sans";
    ctx_1.textAlign = "center";
    temperature = rpm * 10 + 20;
    animated_humidity = (speed * 5) / 8;
    THI = temperature - (0.55 - (0.55 * animated_humidity / 100)) * (temperature - 58);
    ctx_1.fillText(THI.toFixed(2), 250, 272);

    MaxTemp_1 = 100;
    ctx_1.fillStyle = "#FFF";
    for (var i = 0; i <= Math.ceil(MaxTemp_1 / 20) * 20; i += 10) {
        drawMiniNeedle_1(calculateSpeedAngle_1(i / MaxTemp_1, 83.07888, 34.3775) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i : '');

        if (i <= 100) {
            drawMiniNeedle_1(calculateSpeedAngle_1(i / 47, 0, 22.9183) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i - 20 : '');
        }
    }

    ctx_1.beginPath();
    ctx_1.strokeStyle = "#41dcf4";
    ctx_1.lineWidth = 25;
    ctx_1.shadowBlur = 20;
    ctx_1.shadowColor = "#00c6ff";

    ctx_1.strokeStyle = speedGradient_1;
    ctx_1.arc(250, 250, 228, .6 * Math.PI, calculateSpeedAngle_1(speed / topSpeed, 83.07888, 34.3775) * Math.PI);
    ctx_1.stroke();
    ctx_1.beginPath();
    ctx_1.lineWidth = 25;
    ctx_1.strokeStyle = rpmGradient_1;
    ctx_1.shadowBlur = 20;
    ctx_1.shadowColor = "#f7b733";

    ctx_1.arc(250, 250, 228, .4 * Math.PI, calculateRPMAngel_1(rpm / 4.7, 0, 22.9183) * Math.PI, true);
    ctx_1.stroke();
    ctx_1.shadowBlur = 0;

    ctx_1.strokeStyle = '#41dcf4';
    speedNeedle_1(calculateSpeedAngle_1(speed / topSpeed, 83.07888, 34.3775) * Math.PI);

    ctx_1.strokeStyle = rpmGradient_1;
    rpmNeedle_1(calculateRPMAngel_1(rpm / 4.7, 0, 22.9183) * Math.PI);

    ctx_1.strokeStyle = "#000";
}

function setSpeed_1() {
    let speedM = 0;
    let gear = 0;
    let rpm = 0;
    setInterval(function () {
        if (speedM > 160) {
            speedM = 0;
            rpm = 0;
        }
        CurrentHumidity = ((SetHumidity_1 * 8) / 5);
        speedM += 1;
        if (speedM > CurrentHumidity)
        {
            speedM = CurrentHumidity;
        }
        if (rpm < 1) {
            rpm += .005;
        }

        CurrentRpm = ((SetTemp_1 + 20) * 0.01) + 0.2;
        if (rpm > CurrentRpm)
        {
            rpm = CurrentRpm;
        }
        drawSpeedo_1(speedM, gear, rpm, 160);

    }, 80);

}
