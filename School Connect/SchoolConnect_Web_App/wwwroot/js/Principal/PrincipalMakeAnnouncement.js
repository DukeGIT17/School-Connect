
const recipients = document.getElementById("recipients");
const recipientsDisplay = document.getElementById("recipientsDisplay");

function addGroup(option) {
    const selectedGroupDiv = document.createElement('div');
    const removeGroupDiv = document.createElement('div');
    const removeIcon = document.createElement('i');

    recipientsDisplay.appendChild(selectedGroupDiv);
    selectedGroupDiv.className = "selected-groups";
    selectedGroupDiv.textContent = option.value;

    selectedGroupDiv.appendChild(removeGroupDiv);
    removeGroupDiv.className = "remove-group";

    removeGroupDiv.appendChild(removeIcon);
    removeIcon.classList.add("fa-regular");
    removeIcon.classList.add("fa-circle-xmark");
    removeIcon.classList.add("removeGroupBtn");
}


recipients.addEventListener("change", () => {
    const selectedOptions = Array.from(recipients.selectedOptions)
    const selectedRecipients = Array.from(recipientsDisplay.children);
    let exists = false;

    selectedOptions.forEach(option => {
        if (selectedRecipients.length === 0) {
            addGroup(option);
        } else {
            for (let rec of selectedRecipients) {
                if (rec.textContent === option.value) {
                    exists = true;
                    break;
                }
            }

            if (!exists) {
                addGroup(option);
            }
        }
    });
});


recipientsDisplay.addEventListener("click", (event) => {
    if (event.target.tagName === 'I') {
        const clickedIcon = event.target;
        parentDiv = clickedIcon.parentElement;
        parentDiv.parentElement.remove();
    }
});