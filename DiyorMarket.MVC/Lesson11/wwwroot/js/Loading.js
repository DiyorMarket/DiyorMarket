document.onreadystatechange = function () {
    if (document.readyState === "complete") {
        // Page is fully loaded, hide the loader
        document.querySelector('.loader').style.display = 'none';
    }
};