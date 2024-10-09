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