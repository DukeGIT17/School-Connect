
const recipients = document.getElementById("recipients");
const recipientsDisplay = document.getElementById("recipientsDisplay");
function addGroup() {
    const selectedOption = recipients.value;
    const selectedGroupDiv = document.createElement('div');
    const removeGroupDiv = document.createElement('div');
    const removeIcon = document.createElement('i');

    recipientsDisplay.appendChild(selectedGroupDiv);
    selectedGroupDiv.className = "selected-groups";
    selectedGroupDiv.textContent = selectedOption;

    selectedGroupDiv.appendChild(removeGroupDiv);
    removeGroupDiv.className = "remove-group";

    removeGroupDiv.appendChild(removeIcon);
    removeIcon.classList.add("fa-regular");
    removeIcon.classList.add("fa-circle-xmark");
    removeIcon.classList.add("removeGroupBtn");
}


recipients.addEventListener("change", () => {
    const selectedRecipients = Array.from(recipientsDisplay.children);
    let exists = false;

    selectedRecipients.forEach(function (rec) {
        if (rec.textContent === recipients.value) {
            exists = true;
        }
    });

    if (!exists) {
        addGroup();
    }
});


recipientsDisplay.addEventListener("click", (event) => {
    if (event.target.tagName === 'I') {
        const clickedIcon = event.target;
        parentDiv = clickedIcon.parentElement;
        parentDiv.parentElement.remove();
    }
});