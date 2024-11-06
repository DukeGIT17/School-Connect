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

async function sendMessage(inputId, chatBoxId, senderId, receiverId, schoolId, teacherId, parentId) {
    const messageContent = document.getElementById(inputId).value;

    const response = await fetch('/Chat/SendMessage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Message: messageContent,
            TimeSent: new Date(),
            SenderId: senderId,
            ReceiverId: receiverId,
            SchoolId: schoolId,
            TeacherId: teacherId,
            ParentId: parentId,
            Teacher: null,
            Parent: null,
            School: null,
        })
    });

    if (response.ok) {
        const result = await response.json();
        document.getElementById(inputId).value = '';

        const chatBox = document.getElementById(chatBoxId);
        const newMessage = document.createElement('div');
        newMessage.textContent = '[${result.TimeSent}] ${result.Message]';
        chatBox.appendChild(newMessage);
    } else {
        const error = response.text();
        console.error("Failed to send message", error);
    }
}