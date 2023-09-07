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

window.setupF1KeyListener = () => {
    document.body.addEventListener('keydown', (event) => {
        if (event.key === 'F1') {
            event.preventDefault(); // To prevent the browser's default F1 action
            DotNet.invokeMethodAsync('Amadon', 'HandleF1KeyPress');
        }
    });
};

//window.addDoubleClickListener = function (elementId, dotNetReference) {
//    const element = document.getElementById(elementId);
//    if (element) {
//        element.addEventListener('dblclick', (event) => {
//            dotNetReference.invokeMethodAsync('Amadon', event.target.closest('tr').rowIndex);
//        });
//    }
//}

//document.addEventListener('contextmenu', function (event) {
//    event.preventDefault();
//    var menu = document.getElementById('contextMenu');
//    menu.style.left = `${event.pageX}px`;
//    menu.style.top = `${event.pageY}px`;
//    menu.style.display = 'block';
//});

//document.addEventListener('click', function (event) {
//    var menu = document.getElementById('contextMenu');
//    menu.style.display = 'none';
//});

window.addMultipleEventListeners = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        console.log("    FOUND");
        element.addEventListener('contextmenu', function (event) {
            event.preventDefault();
            DotNet.invokeMethodAsync('Amadon', 'ShowContextMenu', event.pageX, event.pageY);
        });

        element.addEventListener('dblclick', function (event) {
            DotNet.invokeMethodAsync('Amadon', 'HandleDoubleClick', event.pageX, event.pageY);
        });
    }
};

function GetTextDivData(id)
{
    var element = document.getElementById(id);
    if (element) {
        console.log('Achou o element ' + element.id);
        console.log('innerHTML: ' + innerHTML);
    }
    else
    {
        console.log('Não Achou o elemento ' + id);
    }
}

window.getAbsolutePosition = function (element) {
    var rect = element.getBoundingClientRect();
    return { top: rect.top, left: rect.left };
};

window.getElementId = function (element) {
    return element.id;
};

window.getElementById = function (id) {
    var element = document.getElementById(id);
    return element;
};


window.getElementPropertiesById = function (id) {
    var element = document.getElementById(id);
    if (element) {
        return {
            id: element.id,
            className: element.className,
            innerHTML: element.innerHTML
            // Add more properties as needed
        };
    }
    return null;
};


window.GetInnerHtml = (elementId) => {
    let element = document.getElementById(elementId);
    if (element) {
        console.log("GetInnerHtml worked: " + element.innerHTML);
    }
    return element ? element.innerHTML : '';
}

// function to get currently selected text
function getSelectedText() {
    let txt = '';
    if (window.getSelection) {
        txt = window.getSelection().toString();
    }
    else if (window.document.getSelection) {
        txt = window.document.getSelection().toString();
    }
    else if (window.document.selection) {
        txt = window.document.selection.createRange().text;
    }
    else {
        txt = 'Error: not supported';
    }
    return txt;
}



window.GetSelectedText = (elementId) => {
    var text = getSelectedText();
    console.log("Selected text: " + text);

    //let element = document.getElementById(elementId);
    //if (element) {
    //    let selection = window.getSelection();
    //    let range = document.createRange();
    //    range.selectNodeContents(element);
    //    if (selection.rangeCount > 0) {
    //        selection.removeAllRanges();
    //    }
    //    selection.addRange(range);
    //    console.log("GetSelectedText worked" + selection.toString());
    //    return selection.toString();
    //}
    return text;
}
    

