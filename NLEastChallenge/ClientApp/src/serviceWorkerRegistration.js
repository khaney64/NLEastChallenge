// The CRA build could register a service worker. The app no longer ships one,
// so drop any worker a browser still has installed from an earlier deploy.
export function unregister() {
  if ('serviceWorker' in navigator) {
    navigator.serviceWorker.getRegistrations()
      .then((registrations) => {
        registrations.forEach((registration) => registration.unregister());
      })
      .catch((error) => {
        console.error(error.message);
      });
  }
}
