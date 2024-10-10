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