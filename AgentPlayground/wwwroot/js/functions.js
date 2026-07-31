window.setFocus = (element) => {
    if (element) {
        element.focus();
    }
};

window.scrollTo = (element) => {
    if (element) {
        element.scrollIntoView();
    }
}

window.initTooltips = () => {
    // Blazor re-renders the markup, so stale instances must be disposed before creating new ones.
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(element => {
        bootstrap.Tooltip.getInstance(element)?.dispose();
        new bootstrap.Tooltip(element);
    });
};
