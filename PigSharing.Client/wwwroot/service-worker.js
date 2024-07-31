// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('install', function(event) {
    // console.log('Service Worker installing.');
    // Perform install steps
});

self.addEventListener('activate', function(event) {
    console.log('Service Worker activating.');
    // Perform activate steps
});

self.addEventListener('fetch', function(event) {
    // console.log('Fetching:', event.request.url);
    event.respondWith(
        fetch(event.request).catch(function() {
            return new Response('Service Worker fetch failed.');
        })
    );
});
