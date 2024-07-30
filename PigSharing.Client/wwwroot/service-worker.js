self.addEventListener('install', event => {
    console.log('Service Worker installing.');
    // You can add pre-caching logic here
});

self.addEventListener('activate', event => {
    console.log('Service Worker activating.');
    // You can add cleanup logic here
});

self.addEventListener('fetch', function(event) {
    event.respondWith(
        caches.match(event.request).then(function(response) {
            return response || fetch(event.request).then(function(networkResponse) {
                return caches.open('my-cache').then(function(cache) {
                    cache.put(event.request, networkResponse.clone());
                    return networkResponse;
                });
            });
        }).catch(function() {
            return new Response('Network request failed and no cache available');
        })
    );
});