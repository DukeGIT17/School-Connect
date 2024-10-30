let principalContainer = document.getElementById("principal-div");
let teacherContainer = document.getElementById("teacher-div");
let parentContainer = document.getElementById("parent-div");
let learnerContainer = document.getElementById("learner-div");

document.getElementById("principal").addEventListener("click", function () {
    principalContainer.style.display = 'block';
    teacherContainer.style.display = 'none';
    parentContainer.style.display = 'none';
    learnerContainer.style.display = 'none';
});

document.getElementById("teacher").addEventListener("click", function () {
    teacherContainer.style.display = 'block';
    principalContainer.style.display = 'none';
    parentContainer.style.display = 'none';
    learnerContainer.style.display = 'none';
});

document.getElementById("parent").addEventListener("click", function () {
    parentContainer.style.display = 'block';
    teacherContainer.style.display = 'none';
    principalContainer.style.display = 'none';
    learnerContainer.style.display = 'none';
});

document.getElementById("learner").addEventListener("click", function () {
    learnerContainer.style.display = 'block';
    teacherContainer.style.display = 'none';
    parentContainer.style.display = 'none';
    principalContainer.style.display = 'none';
});

let subjectSelect = document.getElementById("subjects");
let subjectField = document.getElementById("subjects-field");

function updateSubjectsField() {
    const selectedOptions = Array.from(subjectSelect.selectedOptions).map(option => option.value);
    subjectField.value += selectedOptions;
    subjectField.value += ", ";
}

subjectSelect.addEventListener('change', updateSubjectsField);

let principalFileInputField = document.getElementById("uploadBox-Principal");
let principalUploadButton = document.getElementById("uploadImageBtn-Principal");
let principalFileNameDisplayField = document.getElementById("fileNameDisplay-Principal");
let removeFileBtnPrincipal = document.getElementById("remove-file-Principal");
let previewImage = document.getElementById("PrincipalPreviewImage");

principalUploadButton.addEventListener("click", function () {
    principalFileInputField.click();
});

principalFileInputField.addEventListener("change", function () {
    removeFileBtnPrincipal.style.display = "block";

    const file = this.files[0];

    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            previewImage.setAttribute('src', e.target.result);
            previewImage.style.display = 'block';
        };

        reader.readAsDataURL(file);
    }

    principalUploadButton.style.display = 'none';
    principalFileNameDisplayField.style.display = 'none';
})

let teacherFileInputField = document.getElementById("uploadBox-Teacher");
let teacherUploadButton = document.getElementById("uploadImageBtn-Teacher");
let teacherFileNameDisplayField = document.getElementById("fileNameDisplay-Teacher");
let removeFileBtnTeacher = document.getElementById("remove-file-Teacher");

teacherUploadButton.addEventListener("click", function () {
    teacherFileInputField.click();
});

teacherFileInputField.addEventListener("change", function () {
    teacherFileNameDisplayField.innerHTML = teacherFileInputField.files[0] ? teacherFileInputField.files[0].name : "No file selected";
    removeFileBtnTeacher.style.display = "block";
});

let parentFileInputField = document.getElementById("uploadBox-Parent");
let parentUploadButton = document.getElementById("uploadImageBtn-Parent");
let parentFileNameDisplayField = document.getElementById("fileNameDisplay-Parent");
let removeFileBtnParent = document.getElementById("remove-file-Parent");

parentUploadButton.addEventListener("click", function () {
    parentFileInputField.click();
});

parentFileInputField.addEventListener("change", function () {
    parentFileNameDisplayField.innerHTML = parentFileInputField.files[0] ? parentFileInputField.files[0].name : "No file selected";
    removeFileBtnParent.style.display = "block";
});

let learnerFileInputField = document.getElementById("uploadBox-Learner");
let learnerUploadButton = document.getElementById("uploadImageBtn-Learner");
let learnerFileNameDisplayField = document.getElementById("fileNameDisplay-Learner");
let removeFileBtnLearner = document.getElementById("remove-file-Learner");

learnerUploadButton.addEventListener("click", function () {
    learnerFileInputField.click();
});

learnerFileInputField.addEventListener("change", function () {
    learnerFileNameDisplayField.innerHTML = learnerFileInputField.files[0] ? learnerFileInputField.files[0].name : "No file selected";
    removeFileBtnLearner.style.display = "block";
});

let learnerParentFileInputField = document.getElementById("uploadBox-LearnerParent");
let learnerParentUploadButton = document.getElementById("uploadImageBtn-LearnerParent");
let learnerParentFileNameDisplayField = document.getElementById("fileNameDisplay-LearnerParent");
let removeFileBtnLearnerParent = document.getElementById("remove-file-LearnerParent");

learnerParentUploadButton.addEventListener("click", function () {
    learnerParentFileInputField.click();
});

learnerParentFileInputField.addEventListener("change", function () {
    learnerParentFileNameDisplayField.innerHTML = learnerParentFileInputField.files[0] ? learnerParentFileInputField.files[0].name : "No file selected";
    removeFileBtnLearnerParent.style.display = "block";
});


let bulkLearnerFileInputField = document.getElementById("uploadBox-bulk-learner");
let bulkLearnerUploadButton = document.getElementById("uploadImageBtn-bulk-learner");
let bulkLearnerFileNameDisplayField = document.getElementById("fileNameDisplay-bulk-learner");
let removeFileBtnBulkLearner = document.getElementById("remove-file-bulk-learner");

bulkLearnerUploadButton.addEventListener("click", function () {
    bulkLearnerFileInputField.click();
});

bulkLearnerFileInputField.addEventListener("change", function () {
    bulkLearnerFileNameDisplayField.innerHTML = bulkLearnerFileInputField.files[0] ? bulkLearnerFileInputField.files[0].name : "No file selected";
    removeFileBtnBulkLearner.style.display = "block";
});

let bulkTeacherFileInputField = document.getElementById("uploadBox-bulk-teacher");
let bulkTeacherUploadButton = document.getElementById("uploadImageBtn-bulk-teacher");
let bulkTeacherFileNameDisplayField = document.getElementById("fileNameDisplay-bulk-teacher");
let removeFileBtnBulkTeacher = document.getElementById("remove-file-bulk-teacher");

bulkTeacherUploadButton.addEventListener("click", function () {
    bulkTeacherFileInputField.click();
});

bulkTeacherFileInputField.addEventListener("change", function () {
    bulkTeacherFileNameDisplayField.innerHTML = bulkTeacherFileInputField.files[0] ? bulkTeacherFileInputField.files[0].name : "No file selected";
    removeFileBtnBulkTeacher.style.display = "block";
});

let bulkParentFileInputField = document.getElementById("uploadBox-bulk-parent");
let bulkParentUploadButton = document.getElementById("uploadImageBtn-bulk-parent");
let bulkParentFileNameDisplayField = document.getElementById("fileNameDisplay-bulk-parent");
let removeFileBtnBulkParent = document.getElementById("remove-file-bulk-parent");

bulkParentUploadButton.addEventListener("click", function () {
    bulkParentFileInputField.click();
});

bulkParentFileInputField.addEventListener("change", function () {
    bulkParentFileNameDisplayField.innerHTML = bulkParentFileInputField.files[0] ? bulkParentFileInputField.files[0].name : "No file selected";
    removeFileBtnBulkParent.style.display = "block";
});