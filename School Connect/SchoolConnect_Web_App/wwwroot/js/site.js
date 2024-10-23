function removeSelectedFile(inputFieldId, pElementId, thisBtnId, uploadBtnId, imageId) {
    const pElement = document.getElementById(pElementId);
    const uploadBtn = document.getElementById(uploadBtnId);
    const image = document.getElementById(imageId);

    if (image !== null) {
        image.style.display = 'none';
    }
    document.getElementById(inputFieldId).value = "";
    uploadBtn.style.display = "block";
    pElement.style.display = "block";
    pElement.innerHTML = "No file selected";
    document.getElementById(thisBtnId).style.display = "none";
}