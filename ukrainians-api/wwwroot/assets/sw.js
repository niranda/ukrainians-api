'use strict';

self.addEventListener('push', function (event) {
    console.log('[Service Worker] Push Received.');
    var data = event.data.json();

    const title = data.Title;
            const options = {
                body: data.Message,
                icon: 'assets/push.png',
                badge: 'assets/push.png',
                data: data.Url
            };

            const promiseChain = self.registration.showNotification(title, options);

    event.waitUntil(promiseChain);
});

self.addEventListener('notificationclick', function (event) {
    const urlToOpen = new URL(event.notification.data, self.location.origin).href;

    event.notification.close();

    event.waitUntil(clients.openWindow(urlToOpen));
});