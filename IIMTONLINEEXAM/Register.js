// Init AOS
AOS.init({
    duration: 1000,
    once: true
});

// Init particles.js
particlesJS.load('particles-js', 'particles.json', function () {
    console.log('Particles.js loaded on Register Page');
});
