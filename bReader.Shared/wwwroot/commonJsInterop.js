// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}
export function showAlert(message) {
    alert(message);
}
export function getCurrentTitle() {
    return document.title;
}
export function setTitle(text) {
    document.title = text + "bReader - The Blazor Reader";
    var t_element = document.getElementById("top-title");
    t_element.innerHTML = text;
}