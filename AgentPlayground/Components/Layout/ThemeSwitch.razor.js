// Returns the theme currently applied to the document, either forced by the user or inherited from the system.
export function getCurrentTheme() {
    return document.documentElement.getAttribute("data-bs-theme") ?? "light";
}

// Stores the theme chosen by the user and applies it to the document.
export function setTheme(theme) {
    localStorage.setItem("theme", theme);
    document.documentElement.setAttribute("data-bs-theme", theme);
}

// The document attribute is already kept up to date by the bootstrap script in the page head:
// this only notifies the UI so that the switch reflects the theme applied by the system.
export function watchSystemTheme(dotNetReference) {
    window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", event => {
        if (localStorage.getItem("theme")) {
            return;
        }

        dotNetReference.invokeMethodAsync("OnSystemThemeChanged", event.matches ? "dark" : "light");
    });
}
