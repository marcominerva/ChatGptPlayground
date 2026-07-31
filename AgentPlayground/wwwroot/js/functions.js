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

window.resetFileInput = (elementId) => {
    document.getElementById(elementId).value = '';
};

function getLocalTime(utcDateTime) {
    return new Date(utcDateTime).toLocaleString();
}

window.initTooltips = () => {
    // Blazor re-renders the markup, so stale instances must be disposed before creating new ones.
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(element => {
        bootstrap.Tooltip.getInstance(element)?.dispose();
        new bootstrap.Tooltip(element);
    });
};

window.themeManager = {
    // Returns the theme currently applied to the document, either forced by the user or inherited from the system.
    getCurrentTheme: () => document.documentElement.getAttribute('data-bs-theme') ?? 'light',

    // Stores the theme chosen by the user and applies it to the document.
    setTheme: (theme) => {
        localStorage.setItem('theme', theme);
        document.documentElement.setAttribute('data-bs-theme', theme);
    },

    // The document attribute is already kept up to date by the bootstrap script in the page head:
    // this only notifies the UI so that the switch reflects the theme applied by the system.
    watchSystemTheme: (dotNetReference) => {
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
            if (localStorage.getItem('theme')) {
                return;
            }

            dotNetReference.invokeMethodAsync('OnSystemThemeChanged', event.matches ? 'dark' : 'light');
        });
    }
};
