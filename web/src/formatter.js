const toDuration = (minutes) => {
    const hours = (minutes / 60);
    const days = (hours / 24);
    const months = (days / 30);

    if(months >= 1) {
        return `${months.toFixed(2)} mo(s)`;
    }

    if(days >= 1) {
        return `${days.toFixed(2)} day(s)`;
    }

    if(hours >= 1) {
        return `${hours.toFixed(2)} hr(s)`;
    }

    return `${minutes.toFixed(2)} min(s)`;
};

const toDistance = (meters) => {
    const km = (meters / 1000);

    if(km >= 1) {
        return `${km.toFixed(2)} km`;
    }

    return `${meters.toFixed(2)} m`;
};

const toIsoDate = (date) => {
    const value = new Date(date);
    return value.toISOString().substring(0, 10);
};

export default {
    toDuration,
    toDistance,
    toIsoDate
};