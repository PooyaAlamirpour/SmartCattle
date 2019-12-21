function InitialWeather(Status)
{
    document.getElementById("idWheather").innerHTML = "Status:" + Status;
    switch (Status) {

        case "Thunderstorms":
            document.getElementById("idWheather").innerHTML =
                '<div class="stormy rainy animated pulse infinite">'
                + '<div class="shadow" style="margin-right:-10px" >'
                + '</div>'
                + '</div>'
                + '<div class="sub-wheather">'
                + '<div class="thunder" style="margin-right:20px">'
                + '</div>'
                + '<div class="rain">'
                + '<div class="droplet droplet1" style="margin-right:60px"></div>'
                + '<div class="droplet droplet2" style="margin-right:50px"></div>'
                + '<div class="droplet droplet3" style="margin-right:40px"></div>'
                + '</div>'
                + '</div>';
            break;

        case "Light Rain":
            document.getElementById("idWheather").innerHTML =
                '<div class="stormy rainy animated pulse infinite">'
                + '<div class="shadow" style="margin-right:-10px" >'
                + '</div>'
                + '</div>'
                + '<div class="sub-wheather">'
                + '<div class="rain">'
                + '<div class="droplet droplet1" style="margin-right:60px"></div>'
                + '<div class="droplet droplet2" style="margin-right:50px"></div>'
                + '<div class="droplet droplet3" style="margin-right:40px"></div>'
                + '</div>'
                + '</div>';
            break;

        case "Showers":
            document.getElementById("idWheather").innerHTML =
                '<div class="stormy rainy animated pulse infinite">'
                + '<div class="shadow" style="margin-right:-10px" >'
                + '</div>'
                + '</div>'
                + '<div class="sub-wheather">'
                + '<div class="rain">'
                + '<div class="droplet droplet1" style="margin-right:60px"></div>'
                + '<div class="droplet droplet2" style="margin-right:50px"></div>'
                + '<div class="droplet droplet3" style="margin-right:40px"></div>'
                + '</div>'
                + '</div>';
            break;

        case "Partly Cloudy":
            document.getElementById("idWheather").innerHTML =
                '<div class="cloudy animated pulse infinite">'
                + '</div>';
            break;

        case "Few Showers":
            document.getElementById("idWheather").innerHTML =
                '<div class="cloudy animated pulse infinite">'
                + '</div>';
            break;

        case "Mostly Cloudy":
            document.getElementById("idWheather").innerHTML =
                '<div class="cloudy animated pulse infinite">'
                + '</div>';
            break;

        case "Cloudy":
            document.getElementById("idWheather").innerHTML =
                '<div class="tornado">'
                + '<div class="cloudy animated pulse infinite">'
                + '</div>'
                + '<div class="wind wind1"></div>'
                + '<div class="wind wind2"></div>'
                + '<div class="wind wind3"></div>'
                + '<div class="wind wind4"></div>'
                + '</div>';
            break;

        case "Mostly Sunny":
            document.getElementById("idWheather").innerHTML =
                '<div class="suny">'
                + '<div class="sun animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;

        case "Mostly Clear":
            document.getElementById("idWheather").innerHTML =
                '<div class="suny">'
                + '<div class="sun animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;

        case "Clear":
            document.getElementById("idWheather").innerHTML =
                '<div class="suny">'
                + '<div class="sun animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;

        case "Mostly Sunny/Wind":
            document.getElementById("idWheather").innerHTML =
                '<div class="suny">'
                + '<div class="sun animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;

        case "Scattered Thunderstorms":
            document.getElementById("idWheather").innerHTML =
                '<div class="stormy rainy animated pulse infinite">'
                + '<div class="shadow" style="margin-right:-10px" >'
                + '</div>'
                + '</div>'
                + '<div class="sub-wheather">'
                + '<div class="thunder" style="margin-right:20px">'
                + '</div>'
                + '</div>';
            break;

        case "Partly Cloudy/Wind":
            document.getElementById("idWheather").innerHTML =
                '<div class="sun animated pulse infinite">'
                + '</div>'
                + '<div class="cloudy animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;

        case "Mostly Cloudy/Wind":
            document.getElementById("idWheather").innerHTML =
                '<div class="sun animated pulse infinite">'
                + '</div>'
                + '<div class="cloudy animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;

        case "Few Showers/Wind":
            document.getElementById("idWheather").innerHTML =
                '<div class="sun animated pulse infinite">'
                + '</div>'
                + '<div class="cloudy animated pulse infinite">'
                + '</div>'
                + '</div>';
            break;
    }
}