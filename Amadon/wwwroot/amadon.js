function highlightString(html, expression) {
    if (!expression) {
        return textToHighlight;
    }
    const regex = new RegExp(`(${expression})`, 'gi');
    return html.replace(regex, '<span class="highlight">$1</span>');
}


function unhighlightString(html) {
    const regex = /<span class="highlight">(.*?)<\/span>/gi;
    return html.replace(regex, '$1');
    return html;
}


function highlightAllExpression(expression) {
    try {
        unHighlightAllExpression();
        var tableHtml = $("#tableText").html();
        $("#tableText").html(highlightString(tableHtml, expression))
    } catch (err) {
        var errorMessage = "An error occurred: " + err.message;
        alert(errorMessage);
    }
}

function unHighlightAllExpression() {
    try {
        var tableHtml = $("#tableText").html();
        $("#tableText").html(unhighlightString(tableHtml))
    } catch (err) {
        var errorMessage = "An error occurred: " + err.message;
        alert(errorMessage);
    }
}
