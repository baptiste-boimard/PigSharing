// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
// self.addEventListener('fetch', () => { });

// Utilisez importScripts uniquement si disponible
if (typeof importScripts === 'function') {
    importScripts('https://example.com/path/to/your/script.js');

    self.addEventListener('install', event => {
        console.log('Service Worker: Installed');
        // Logic for installation
    });

    self.addEventListener('activate', event => {
        console.log('Service Worker: Activated');
        // Logic for activation
    });

    self.addEventListener('fetch', event => {
        console.log('Service Worker: Fetch event for', event.request.url);
        event.respondWith(fetch(event.request));
    });
} else {
    console.error('importScripts is not a function in this context.');
}