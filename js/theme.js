(function () {
    var nav = document.getElementById('nav');
    var mainContent = document.getElementById('main-content');
    var menuButton = document.getElementById('menu-button');
    menuButton.onclick = function() {
        nav.classList.toggle('open');
        mainContent.classList.toggle('nav-open');
    }
})();