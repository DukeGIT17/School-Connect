const fileInputField = document.getElementById("uploadBox");
const uploadButton = document.getElementById("uploadImageBtn");
const fileNameDisplayField = document.getElementById("fileNameDisplay");
const removeFileBtn = document.getElementById("remove-file");

uploadButton.addEventListener("click", function () {
    fileInputField.click();
    removeFileBtn.style.display = "block";
});

fileInputField.addEventListener("change", function () {
    fileNameDisplayField.innerHTML = fileInputField.files[0] ? fileInputField.files[0].name : "No file selected";
});