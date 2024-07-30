// Ceci est la liste des fichiers à mettre en cache
self.assetsManifest = {
    "assets": [
        {
            "hash": "sha256-....",
            "url": "_framework/blazor.webassembly.js"
        },
        // Ajoutez ici tous les fichiers à mettre en cache
    ],
    "version": "v1.0"
};

const CACHE_NAME = `blazor-cache-${self.assetsManifest.version}`;
const ASSETS_TO_CACHE = self.assetsManifest.assets.map(asset => asset.url);

self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME).then(cache => {
            return cache.addAll(ASSETS_TO_CACHE);
        })
    );
});

self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames
                    .filter(cacheName => cacheName !== CACHE_NAME)
                    .map(cacheName => caches.delete(cacheName))
            );
        })
    );
});

self.addEventListener('fetch', event => {
    if (event.request.method === 'GET') {
        event.respondWith(
            caches.match(event.request).then(cachedResponse => {
                return cachedResponse || fetch(event.request).then(response => {
                    return caches.open(CACHE_NAME).then(cache => {
                        cache.put(event.request, response.clone());
                        return response;
                    });
                });
            })
        );
    }
});
