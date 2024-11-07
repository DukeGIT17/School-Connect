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

async function sendMessage(inputId, subjectId, chatBoxId, senderIdentificate, receiverIdentificate, schoolId, teacherId, parentId) {
    const messageContent = document.getElementById(inputId).value;
    const subject = document.getElementById(subjectId).value;

    const response = await fetch('/Chat/SendMessage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Message: messageContent,
            Subject: subject,
            TimeSent: new Date(),
            SchoolId: schoolId,
            TeacherId: teacherId,
            ParentId: parentId,
            SenderIdentificate: senderIdentificate,
            receiverIdentificate: receiverIdentificate,
            Teacher: null,
            Parent: null,
            School: null,
        })
    });

    if (response.ok) {
        const result = await response.json();

        let timeStamp = new Date(result.timeSent);
        const hours = timeStamp.getHours().toString().padStart(2, '0');
        const minutes = timeStamp.getMinutes().toString().padStart(2, '0');

        document.getElementById(inputId).value = '';

        const chatBox = document.getElementById(chatBoxId);
        const newMessage = document.createElement('div');
        const messageDetails = document.createElement('div');
        const icon = document.createElement('i');

        newMessage.classList = 'message sent';
        newMessage.textContent = result.message;

        messageDetails.className = 'message-details'
        messageDetails.textContent = result.subject + ' ' + hours + ':' + minutes + ' ';

        icon.classList = 'fa-solid fa-check';

        messageDetails.appendChild(icon);
        newMessage.appendChild(messageDetails);
        chatBox.appendChild(newMessage);
    } else {
        const error = await response.text();
        console.error("Failed to send message", error);
    }
}

const options = document.querySelectorAll('.options-menu');

function toggleOptions(optionsId) {
    const optionsMenu = document.getElementById(optionsId);
    optionsMenu.style.display = optionsMenu.style.display === 'block' ? 'none' : 'block';
}

options.forEach(option => {
    option.addEventListener('mouseleave', function () {
        setTimeout(() => {
            option.style.display = 'none';
        }, 200); 
    });
});

function selectSubject(subject, subFieldId, optionsId) {
    const subField = document.getElementById(subFieldId);
    subField.value = subject;
    const child = subField.parentElement.firstChild;
    child.textContent = 'Subject: ' + subject;
    toggleOptions(optionsId);
}

const textInputs = document.querySelectorAll('.chatInput');
textInputs.forEach(input => {
    input.addEventListener('input', function () {
        this.style.height = 'auto';
        this.style.height = Math.min(this.scrollHeight, 70) + 'px';
    });
});