function openChat(chatSectionId) {
    const chatSectionToDisplay = document.getElementById(chatSectionId);
    const otherChatSections = document.querySelectorAll('.chat-section');
    const welcomeSection = document.getElementById('no-chat');

    otherChatSections.forEach(section => {
        section.style.display = 'none';
    });

    welcomeSection.style.display = 'none';
    chatSectionToDisplay.style.display = 'block';
}

document.getElementById("search-input").addEventListener("input", function () {
    const searchValue = this.value.toLowerCase();
    const contacts = document.querySelectorAll(".contact");

    contacts.forEach(contact => {
        const nameDiv = contact.querySelector(".name");
        const nameText = nameDiv.textContent.toLowerCase();

        // Show or hide the contact based on search match
        if (nameText.includes(searchValue)) {
            contact.style.display = "";
        } else {
            contact.style.display = "none";
        }
    });
});
