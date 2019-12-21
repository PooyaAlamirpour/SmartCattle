var c_3;
var ctx_3;
var speedGradient_3;
var rpmGradient_3;

function Initial_BarnSpeed_3() {
    let dev_3 = false;

    SetHumidity_3 = Humidity_3;
    SetTemp_3 = Tempreture_3;

    c_3 = document.getElementById("canvas_3");
    c_3.width = 500;
    c_3.height = 500;
    ctx_3 = c_3.getContext("2d");
    ctx_3.scale(1, 1);
    speedGradient_3 = ctx_3.createLinearGradient(0, 500, 0, 0);
    speedGradient_3.addColorStop(0, '#00b8fe');
    speedGradient_3.addColorStop(1, '#41dcf4');

    rpmGradient_3 = ctx_3.createLinearGradient(0, 500, 0, 0);
    rpmGradient_3.addColorStop(0, '#f7b733');
    rpmGradient_3.addColorStop(1, '#fc4a1a');

    setSpeed_3();
}

function speedNeedle_3(rotation_3) {
    ctx_3.lineWidth = 2;

    ctx_3.save();
    ctx_3.translate(250, 250);
    ctx_3.rotate(rotation_3);
    ctx_3.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_3.restore();

    rotation_3 += Math.PI / 180;
}

function rpmNeedle_3(rotation_3) {
    ctx_3.lineWidth = 2;

    ctx_3.save();
    ctx_3.translate(250, 250);
    ctx_3.rotate(rotation_3);
    ctx_3.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_3.restore();

    rotation_3 += Math.PI / 180;
}

function drawMiniNeedle_3(rotation_3, width, speed) {
    ctx_3.lineWidth = width;

    ctx_3.save();
    ctx_3.translate(250, 250);
    ctx_3.rotate(rotation_3);
    ctx_3.strokeStyle = "#333";
    ctx_3.fillStyle = "#333";
    ctx_3.strokeRect(-20 / 2 + 220, -1 / 2, 20, 1);
    ctx_3.restore();

    let x = (250 + 180 * Math.cos(rotation_3));
    let y = (250 + 180 * Math.sin(rotation_3));

    ctx_3.font = "700 23px Open Sans";
    ctx_3.fillText(speed, x, y);

    rotation_3 += Math.PI / 180;
}

function calculateSpeedAngle_3(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian <= 1.45 ? radian : 1.45;
}

function calculateRPMAngel_3(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian >= -0.46153862656807704 ? radian : -0.46153862656807704;
}

function drawSpeedo_3(speed, gear, rpm, topSpeed) {
    if (speed == undefined) {
        return false;
    } else {
        speed = Math.floor(speed);
        rpm = rpm * 10;
    }

    ctx_3.clearRect(0, 0, 500, 500);

    ctx_3.beginPath();
    ctx_3.fillStyle = 'rgba(0, 0, 0, .9)';
    ctx_3.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_3.fill();
    ctx_3.save()
    ctx_3.restore();
    ctx_3.fillStyle = "#FFF";
    ctx_3.stroke();

    ctx_3.beginPath();
    ctx_3.strokeStyle = "#333";
    ctx_3.lineWidth = 10;
    ctx_3.arc(250, 250, 100, 0, 2 * Math.PI);
    ctx_3.stroke();

    ctx_3.beginPath();
    ctx_3.lineWidth = 1;
    ctx_3.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_3.stroke();

    ctx_3.font = "700 65px Open Sans";
    ctx_3.textAlign = "center";
    temperature = rpm * 10 + 20;
    animated_humidity = (speed * 5) / 8;
    THI = temperature - (0.55 - (0.55 * animated_humidity / 100)) * (temperature - 58);
    ctx_3.fillText(THI.toFixed(2), 250, 272);

    MaxTemp_3 = 100;
    ctx_3.fillStyle = "#FFF";
    for (var i = 0; i <= Math.ceil(MaxTemp_3 / 20) * 20; i += 10) {
        drawMiniNeedle_3(calculateSpeedAngle_3(i / MaxTemp_3, 83.07888, 34.3775) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i : '');

        if (i <= 100) {
            drawMiniNeedle_3(calculateSpeedAngle_3(i / 47, 0, 22.9183) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i - 20 : '');
        }
    }

    ctx_3.beginPath();
    ctx_3.strokeStyle = "#41dcf4";
    ctx_3.lineWidth = 25;
    ctx_3.shadowBlur = 20;
    ctx_3.shadowColor = "#00c6ff";

    ctx_3.strokeStyle = speedGradient_3;
    ctx_3.arc(250, 250, 228, .6 * Math.PI, calculateSpeedAngle_3(speed / topSpeed, 83.07888, 34.3775) * Math.PI);
    ctx_3.stroke();
    ctx_3.beginPath();
    ctx_3.lineWidth = 25;
    ctx_3.strokeStyle = rpmGradient_3;
    ctx_3.shadowBlur = 20;
    ctx_3.shadowColor = "#f7b733";

    ctx_3.arc(250, 250, 228, .4 * Math.PI, calculateRPMAngel_3(rpm / 4.7, 0, 22.9183) * Math.PI, true);
    ctx_3.stroke();
    ctx_3.shadowBlur = 0;

    ctx_3.strokeStyle = '#41dcf4';
    speedNeedle_3(calculateSpeedAngle_3(speed / topSpeed, 83.07888, 34.3775) * Math.PI);

    ctx_3.strokeStyle = rpmGradient_3;
    rpmNeedle_3(calculateRPMAngel_3(rpm / 4.7, 0, 22.9183) * Math.PI);

    ctx_3.strokeStyle = "#000";
}

function setSpeed_3() {
    let speedM = 0;
    let gear = 0;
    let rpm = 0;
    setInterval(function () {
        if (speedM > 160) {
            speedM = 0;
            rpm = 0;
        }
        CurrentHumidity = (SetHumidity_3 * 8) / 5;
        speedM += 1;
        if (speedM > CurrentHumidity) {
            speedM = CurrentHumidity;
        }
        if (rpm < 1) {
            rpm += .005;
        }

        CurrentRpm = ((SetTemp_1 + 20) * 0.01) + 0.2;
        if (rpm > CurrentRpm) {
            rpm = CurrentRpm;
        }
        drawSpeedo_3(speedM, gear, rpm, 160);

    }, 80);

}
