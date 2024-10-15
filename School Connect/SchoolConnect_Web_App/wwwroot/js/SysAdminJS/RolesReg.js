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


let subjectSelectLearner = document.getElementById("subjects-learner");
let subjectFieldLearner = document.getElementById("subjects-field-learner");

function updateSubjectsFieldLearner() {
    const selectedOptions = Array.from(subjectSelectLearner.selectedOptions).map(option => option.value);
    subjectFieldLearner.value += selectedOptions;
    subjectFieldLearner.value += ", ";
}

subjectSelectLearner.addEventListener('change', updateSubjectsFieldLearner);


function populateLearnerIdNo() {
    let idnoField = document.getElementById("idNum-learner");
    let learnerIdNoInputField = document.getElementsByClassName("learner-idno");

    learnerIdNoInputField.forEach(function (field) {
        field.value = idnoField.value;
    })
}

document.getElementById("idNum-learner").addEventListener('focusout', populateLearnerIdNo);


let principalFileInputField = document.getElementById("uploadBox-Principal");
let principalUploadButton = document.getElementById("uploadImageBtn-Principal");
let principalFileNameDisplayField = document.getElementById("fileNameDisplay-Principal");
let removeFileBtnPrincipal = document.getElementById("remove-file-Principal");

principalUploadButton.addEventListener("click", function () {
    principalFileInputField.click();
});

principalFileInputField.addEventListener("change", function () {
    principalFileNameDisplayField.innerHTML = principalFileInputField.files[0] ? principalFileInputField.files[0].name : "No file selected";
    removeFileBtnPrincipal.style.display = "block";
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
    parentFileNameDisplayField.innerHtml = parentFileInputField.files[0] ? parentFileInputField.files[0].name : "No file selected";
    removeFileBtnParent.style.display = "block";
});

let learnerFileInputField = document.getElementById("uploadBox-Learner");
let learnerUploadButton = document.getElementById("uploadImageBtn-Learner");
let learnerFileNameDisplayField = document.getElementById("fileNameDisplay-Learner");
let removeFileBtnLearner = document.getElementById("remove-file-Learner");

learnerUploadButton.addEventListener("click", function () {
    learnerFileInputField.click();
    removeFileBtnLearner.style.display = "block";
});

learnerFileInputField.addEventListener("change", function () {
    learnerFileNameDisplayField.innerHtml = learnerFileInputField.files[0] ? learnerFileInputField.files[0].name : "No file selected";
});

let learnerParentFileInputField = document.getElementById("uploadBox-LearnerParent");
let learnerParentUploadButton = document.getElementById("uploadImageBtn-LearnerParent");
let learnerParentFileNameDisplayField = document.getElementById("fileNameDisplay-LearnerParent");
let removeFileBtnLearnerParent = document.getElementById("remove-file-LearnerParent");

learnerParentUploadButton.addEventListener("click", function () {
    learnerParentFileInputField.click();
});

learnerParentFileInputField.addEventListener("change", function () {
    learnerParentFileNameDisplayField.innerHtml = learnerParentFileInputField.files[0] ? learnerParentFileInputField.files[0].name : "No file selected";
    removeFileBtnLearnerParent.style.display = "block";
});