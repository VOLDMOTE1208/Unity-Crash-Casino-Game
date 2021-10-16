importScripts('https://www.gstatic.com/firebasejs/8.3.2/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.3.2/firebase-messaging.js');


  // Initialize Firebase
  var config = {
    apiKey: "AIzaSyCUR6fLmd9gXaE4rJmMV8xpARJh9uUEXo0",
    authDomain: "crash-backend-6e22c.firebaseapp.com",
    projectId: "crash-backend-6e22c",
    storageBucket: "crash-backend-6e22c.appspot.com",
    messagingSenderId: "343617536795",
    appId: "1:343617536795:web:c13a4a131cf66e434cc9be",
    measurementId: "G-1TLFX3JMK1"
  };

  firebase.initializeApp(config);
  const messaging = firebase.messaging();
  messaging.onBackgroundMessage((payload) => {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
      body: 'Background Message body.',
      icon: '/firebase-logo.png'
    };
  
    self.registration.showNotification(notificationTitle,
      notificationOptions);
  });