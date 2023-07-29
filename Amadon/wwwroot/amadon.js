function jumpToAnchor(anchorLink) {
    const cellElement = document.getElementById(anchorLink);
    if (cellElement) {
        try {
            cellElement.scrollIntoView();
        } catch (error) {   
            // Handle routing error
            //alert("An error occurred during routing: " + error.message);
        }
    }
 }

 window.registerClickEvent = function(dotNetObject) {
    document.querySelector('a').addEventListener('click', function(event) {
        event.preventDefault();
        dotNetObject.invokeMethodAsync('OnLinkClicked');
    });
};

