var c_4;
var ctx_4;
var speedGradient_4;
var rpmGradient_4;

function Initial_BarnSpeed_4() {
    let dev_4 = false;

    SetHumidity_4 = Humidity_4;
    SetTemp_4 = Tempreture_4;

    c_4 = document.getElementById("canvas_4");
    c_4.width = 500;
    c_4.height = 500;
    ctx_4 = c_4.getContext("2d");
    ctx_4.scale(1, 1);
    speedGradient_4 = ctx_4.createLinearGradient(0, 500, 0, 0);
    speedGradient_4.addColorStop(0, '#00b8fe');
    speedGradient_4.addColorStop(1, '#41dcf4');

    rpmGradient_4 = ctx_4.createLinearGradient(0, 500, 0, 0);
    rpmGradient_4.addColorStop(0, '#f7b733');
    rpmGradient_4.addColorStop(1, '#fc4a1a');

    setSpeed_4();
}

function speedNeedle_4(rotation_4) {
    ctx_4.lineWidth = 2;

    ctx_4.save();
    ctx_4.translate(250, 250);
    ctx_4.rotate(rotation_4);
    ctx_4.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_4.restore();

    rotation_4 += Math.PI / 180;
}

function rpmNeedle_4(rotation_4) {
    ctx_4.lineWidth = 2;

    ctx_4.save();
    ctx_4.translate(250, 250);
    ctx_4.rotate(rotation_4);
    ctx_4.strokeRect(-130 / 2 + 170, -1 / 2, 135, 1);
    ctx_4.restore();

    rotation_4 += Math.PI / 180;
}

function drawMiniNeedle_4(rotation_4, width, speed) {
    ctx_4.lineWidth = width;

    ctx_4.save();
    ctx_4.translate(250, 250);
    ctx_4.rotate(rotation_4);
    ctx_4.strokeStyle = "#333";
    ctx_4.fillStyle = "#333";
    ctx_4.strokeRect(-20 / 2 + 220, -1 / 2, 20, 1);
    ctx_4.restore();

    let x = (250 + 180 * Math.cos(rotation_4));
    let y = (250 + 180 * Math.sin(rotation_4));

    ctx_4.font = "700 23px Open Sans";
    ctx_4.fillText(speed, x, y);

    rotation_4 += Math.PI / 180;
}

function calculateSpeedAngle_4(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian <= 1.45 ? radian : 1.45;
}

function calculateRPMAngel_4(x, a, b) {
    let degree = (a - b) * (x) + b;
    let radian = (degree * Math.PI) / 180;
    return radian >= -0.46153862656807704 ? radian : -0.46153862656807704;
}

function drawSpeedo_4(speed, gear, rpm, topSpeed) {
    if (speed == undefined) {
        return false;
    } else {
        speed = Math.floor(speed);
        rpm = rpm * 10;
    }

    ctx_4.clearRect(0, 0, 500, 500);

    ctx_4.beginPath();
    ctx_4.fillStyle = 'rgba(0, 0, 0, .9)';
    ctx_4.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_4.fill();
    ctx_4.save()
    ctx_4.restore();
    ctx_4.fillStyle = "#FFF";
    ctx_4.stroke();

    ctx_4.beginPath();
    ctx_4.strokeStyle = "#333";
    ctx_4.lineWidth = 10;
    ctx_4.arc(250, 250, 100, 0, 2 * Math.PI);
    ctx_4.stroke();

    ctx_4.beginPath();
    ctx_4.lineWidth = 1;
    ctx_4.arc(250, 250, 240, 0, 2 * Math.PI);
    ctx_4.stroke();

    ctx_4.font = "700 65px Open Sans";
    ctx_4.textAlign = "center";
    temperature = rpm * 10 + 20;
    animated_humidity = (speed * 5) / 8;
    THI = temperature - (0.55 - (0.55 * animated_humidity / 100)) * (temperature - 58);
    ctx_4.fillText(THI.toFixed(2), 250, 272);

    MaxTemp_4 = 100;
    ctx_4.fillStyle = "#FFF";
    for (var i = 0; i <= Math.ceil(MaxTemp_4 / 20) * 20; i += 10) {
        drawMiniNeedle_4(calculateSpeedAngle_4(i / MaxTemp_4, 83.07888, 34.3775) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i : '');

        if (i <= 100) {
            drawMiniNeedle_4(calculateSpeedAngle_4(i / 47, 0, 22.9183) * Math.PI, i % 10 == 0 ? 3 : 1, i % 10 == 0 ? i - 20 : '');
        }
    }

    ctx_4.beginPath();
    ctx_4.strokeStyle = "#41dcf4";
    ctx_4.lineWidth = 25;
    ctx_4.shadowBlur = 20;
    ctx_4.shadowColor = "#00c6ff";

    ctx_4.strokeStyle = speedGradient_4;
    ctx_4.arc(250, 250, 228, .6 * Math.PI, calculateSpeedAngle_4(speed / topSpeed, 83.07888, 34.3775) * Math.PI);
    ctx_4.stroke();
    ctx_4.beginPath();
    ctx_4.lineWidth = 25;
    ctx_4.strokeStyle = rpmGradient_4;
    ctx_4.shadowBlur = 20;
    ctx_4.shadowColor = "#f7b733";

    ctx_4.arc(250, 250, 228, .4 * Math.PI, calculateRPMAngel_4(rpm / 4.7, 0, 22.9183) * Math.PI, true);
    ctx_4.stroke();
    ctx_4.shadowBlur = 0;

    ctx_4.strokeStyle = '#41dcf4';
    speedNeedle_4(calculateSpeedAngle_4(speed / topSpeed, 83.07888, 34.3775) * Math.PI);

    ctx_4.strokeStyle = rpmGradient_4;
    rpmNeedle_4(calculateRPMAngel_4(rpm / 4.7, 0, 22.9183) * Math.PI);

    ctx_4.strokeStyle = "#000";
}

function setSpeed_4() {
    let speedM = 0;
    let gear = 0;
    let rpm = 0;
    setInterval(function () {
        if (speedM > 160) {
            speedM = 0;
            rpm = 0;
        }
        CurrentHumidity = (SetHumidity_4 * 8) / 5;
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
        drawSpeedo_4(speedM, gear, rpm, 160);

    }, 80);

}
