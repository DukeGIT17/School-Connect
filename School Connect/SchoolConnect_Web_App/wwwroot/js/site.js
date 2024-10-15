function removeSelectedFile(inputFieldId, pElementId, thisBtnId) {
    document.getElementById(inputFieldId).value = "";
    document.getElementById(pElementId).innerHTML = "No file selected";
    document.getElementById(thisBtnId).style.display = "none";
}