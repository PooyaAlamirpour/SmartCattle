var c_2;
var ctx_2;
var speedGradient_2;
var rpmGradient_2;

function Initial_BarnSpeed_2() {
    let dev_2 = false;

    SetHumidity_2 = Humidity_2;
    SetTemp_2 = Tempreture_2;

    c_2 = document.getElementById("canvas_2");
    c_2.width = 500;
    c_2.height = 500;
    ctx_2 = c_2.getContext("2d");
    ctx_2.scale(1, 1);
    speedGradient_2 = ctx_2.createLinearGradient(0, 500, 0, 0);
    speedGradient_2.addColorStop(0, '#00b8fe');
    speedGradient_2.addColorStop(1, '#41dcf4');

    rpmGradient_2 = ctx_2.createLinearGradient(0, 500, 0, 0);
    rpmGradient_2.addColorStop(0, '#f7b733');
    rpmGradient_2.addColorStop(1, '#fc4a1a');

    setSpeed_2();
}

function speedNeedle_2(rotation_2) {
    ctx_2.lineWidth = 2;

    ctx_2.save();
    ctx_2.translate(250, 250);
    ctx_2.rotate(rotation_2);
    ctx_2.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_2.restore();

    rotation_2 += Math.PI / 180;
}

function rpmNeedle_2(rotation_2) {
    ctx_2.lineWidth = 2;

    ctx_2.save();
    ctx_2.translate(250, 250);
    ctx_2.rotate(rotation_2);
    ctx_2.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_2.restore();

    rotation_2 += Math.PI / 180;
}

function drawMiniNeedle_2(rotation_2, width, speed) {
    ctx_2.lineWidth = width;

    ctx_2.save();
    ctx_2.translate(250, 250);
    ctx_2.rotate(rotation_2);
    ctx_2.strokeStyle = "#333";
    ctx_2.fillStyle = "#333";
    ctx_2.strokeRect(-20 / 2 + 220, -1 / 2, 20, 1);
    ctx_2.restore();

    let x = (250 + 180 * Math.cos(rotation_2));
    let y = (250 + 180 * Math.sin(rotation_2));

    ctx_2.font = "700 23px Open Sans";
    ctx_2.fillText(speed, x, y);

    rotation_2 += Math.PI / 180;
}

function calculateSpeedAngle_2(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian <= 1.45 ? radian : 1.45;
}

function calculateRPMAngel_2(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian >= -0.46153862656807704 ? radian : -0.46153862656807704;
}

function drawSpeedo_2(speed, gear, rpm, topSpeed) {
    if (speed == undefined) {
        return false;
    } else {
        speed = Math.floor(speed);
        rpm = rpm * 10;
    }

    ctx_2.clearRect(0, 0, 500, 500);

    ctx_2.beginPath();
    ctx_2.fillStyle = 'rgba(0, 0, 0, .9)';
    ctx_2.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_2.fill();
    ctx_2.save()
    ctx_2.restore();
    ctx_2.fillStyle = "#FFF";
    ctx_2.stroke();

    ctx_2.beginPath();
    ctx_2.strokeStyle = "#333";
    ctx_2.lineWidth = 10;
    ctx_2.arc(250, 250, 100, 0, 2 * Math.PI);
    ctx_2.stroke();

    ctx_2.beginPath();
    ctx_2.lineWidth = 1;
    ctx_2.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_2.stroke();

    ctx_2.font = "700 65px Open Sans";
    ctx_2.textAlign = "center";
    temperature = rpm * 10 + 20;
    animated_humidity = (speed * 5) / 8;
    THI = temperature - (0.55 - (0.55 * animated_humidity / 100)) * (temperature - 58);
    ctx_2.fillText(THI.toFixed(2), 250, 272);

    MaxTemp_2 = 100;
    ctx_2.fillStyle = "#FFF";
    for (var i = 0; i <= Math.ceil(MaxTemp_2 / 20) * 20; i += 10) {
        drawMiniNeedle_2(calculateSpeedAngle_2(i / MaxTemp_2, 83.07888, 34.3775) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i : '');

        if (i <= 100) {
            drawMiniNeedle_2(calculateSpeedAngle_2(i / 47, 0, 22.9183) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i - 20 : '');
        }
    }

    ctx_2.beginPath();
    ctx_2.strokeStyle = "#41dcf4";
    ctx_2.lineWidth = 25;
    ctx_2.shadowBlur = 20;
    ctx_2.shadowColor = "#00c6ff";

    ctx_2.strokeStyle = speedGradient_2;
    ctx_2.arc(250, 250, 228, .6 * Math.PI, calculateSpeedAngle_2(speed / topSpeed, 83.07888, 34.3775) * Math.PI);
    ctx_2.stroke();
    ctx_2.beginPath();
    ctx_2.lineWidth = 25;
    ctx_2.strokeStyle = rpmGradient_2;
    ctx_2.shadowBlur = 20;
    ctx_2.shadowColor = "#f7b733";

    ctx_2.arc(250, 250, 228, .4 * Math.PI, calculateRPMAngel_2(rpm / 4.7, 0, 22.9183) * Math.PI, true);
    ctx_2.stroke();
    ctx_2.shadowBlur = 0;

    ctx_2.strokeStyle = '#41dcf4';
    speedNeedle_2(calculateSpeedAngle_2(speed / topSpeed, 83.07888, 34.3775) * Math.PI);

    ctx_2.strokeStyle = rpmGradient_2;
    rpmNeedle_2(calculateRPMAngel_2(rpm / 4.7, 0, 22.9183) * Math.PI);

    ctx_2.strokeStyle = "#000";
}

function setSpeed_2() {
    let speedM = 0;
    let gear = 0;
    let rpm = 0;
    setInterval(function () {
        if (speedM > 160) {
            speedM = 0;
            rpm = 0;
        }
        CurrentHumidity = (SetHumidity_2 * 8) / 5;
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
        drawSpeedo_2(speedM, gear, rpm, 160);

    }, 80);

}
