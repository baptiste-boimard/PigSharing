self.addEventListener('install', event => {
    console.log('Service Worker installing.');
    // You can add pre-caching logic here
});

self.addEventListener('activate', event => {
    console.log('Service Worker activating.');
    // You can add cleanup logic here
});

self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request).then(response => {
            return response || fetch(event.request);
        })
    );
});