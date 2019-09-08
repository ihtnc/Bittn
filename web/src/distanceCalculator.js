const toRadians = (value) => value * Math.PI / 180;

const calculate = (src, dst) => {
    // based on https://www.movable-type.co.uk/scripts/latlong.html
    var R = 6371e3; // metres
    var φ1 = toRadians(src.lat);
    var φ2 = toRadians(dst.lat);
    var Δφ = toRadians((dst.lat - src.lat));
    var Δλ = toRadians((dst.lng - src.lng));

    var a = Math.sin(Δφ/2) * Math.sin(Δφ/2) +
            Math.cos(φ1) * Math.cos(φ2) *
            Math.sin(Δλ/2) * Math.sin(Δλ/2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));

    return R * c;
};

export default {
    calculate
};