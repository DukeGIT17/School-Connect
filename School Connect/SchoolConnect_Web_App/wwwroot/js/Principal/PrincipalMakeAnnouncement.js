const recipients = document.getElementById("recipients");
const recipientsDisplay = document.getElementById("recipientsDisplay");

function addGroup(option) {
    // Check if the option is already selected
    option.selected = true; // Ensure it's selected in the original <select>

    const selectedGroupDiv = document.createElement('div');
    const removeGroupDiv = document.createElement('div');
    const removeIcon = document.createElement('i');

    recipientsDisplay.appendChild(selectedGroupDiv);
    selectedGroupDiv.className = "selected-groups";
    selectedGroupDiv.textContent = option.value;

    selectedGroupDiv.appendChild(removeGroupDiv);
    removeGroupDiv.className = "remove-group";

    removeGroupDiv.appendChild(removeIcon);
    removeIcon.classList.add("fa-regular", "fa-circle-xmark", "removeGroupBtn");
}


recipients.addEventListener("change", () => {
    const selectedOptions = Array.from(recipients.selectedOptions);
    const selectedRecipients = Array.from(recipientsDisplay.children);

    selectedOptions.forEach(option => {
        // Check if the option is already displayed
        const exists = selectedRecipients.some(rec => rec.textContent === option.value);
        if (!exists) {
            addGroup(option);
        }
    });
});


recipientsDisplay.addEventListener("click", (event) => {
    if (event.target.tagName === 'I') {
        const clickedIcon = event.target;
        const parentDiv = clickedIcon.parentElement;
        const selectedValue = parentDiv.parentElement.textContent;

        // Find and deselect the option in the original <select>
        Array.from(recipients.options).forEach(option => {
            if (option.value === selectedValue) {
                option.selected = false;
            }
        });

        // Remove the displayed group
        parentDiv.parentElement.remove();
    }
});
