const maps = new Map();

const DEFAULT_MARKER_ZOOM = 10;

export function initialize(elementId, latitude, longitude, zoom) {
    // Create map for a given ID only once
    if (maps.has(elementId)) {
        return;
    }

    const map = L.map(elementId).setView([latitude, longitude], zoom);

    // Attribution required for tile use. For more information: https://operations.osmfoundation.org/policies/tiles/
    L.tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
        maxZoom: 19,
        attribution:
            '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    maps.set(elementId, {
        map: map,
        selectedMarker: null
    });
}

export function setSelectedAnimal(elementId, latitude, longitude, title, popupHtml) {
    // Get this map
    const state = maps.get(elementId);

    if (!state) {
        throw new Error(`No Leaflet map exists for '${elementId}'.`);
    }

    if (!Number.isFinite(latitude) || !Number.isFinite(longitude)) {
        clearSelectedAnimal(elementId);
        return;
    }

    if (state.selectedMarker) {
        // Removing layer clears the marker
        state.map.removeLayer(state.selectedMarker);
        state.selectedMarker = null;
    }

    state.selectedMarker = L.marker([latitude, longitude])
        .addTo(state.map)
        .bindPopup(popupHtml || title || "Selected animal");

    state.selectedMarker.openPopup();

    state.map.setView([latitude, longitude], DEFAULT_MARKER_ZOOM, {
        animate: true
    });
}

export function clearSelectedAnimal(elementId) {
    const state = maps.get(elementId);

    if (!state || !state.selectedMarker) {
        return;
    }

    // Removing layer clears the marker
    state.map.removeLayer(state.selectedMarker);
    state.selectedMarker = null;
}

// Updates the map if the container size changes
export function invalidateSize(elementId) {
    const state = maps.get(elementId);

    if (state) {
        state.map.invalidateSize();
    }
}

export function dispose(elementId) {
    const state = maps.get(elementId);

    if (!state) {
        return;
    }

    state.map.remove();
    maps.delete(elementId);
}