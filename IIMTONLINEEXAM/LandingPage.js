AOS.init();

// Scroll to top
const scrollTopBtn = document.getElementById("scrollTopBtn");
window.onscroll = function () {
    scrollTopBtn.style.display = (document.documentElement.scrollTop > 300) ? "block" : "none";
};
scrollTopBtn.onclick = function () {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

// Particle JS config
particlesJS("particles-js", {
    "particles": {
        "number": { "value": 80 },
        "color": { "value": "#ffd700" },
        "shape": { "type": "circle" },
        "opacity": { "value": 0.5 },
        "size": { "value": 3 },
        "line_linked": { "enable": true, "color": "#ffd700", "opacity": 0.4 },
        "move": { "enable": true, "speed": 1.2 }
    },
    "interactivity": {
        "events": {
            "onhover": { "enable": true, "mode": "repulse" }
        }
    },
    "retina_detect": true
});
