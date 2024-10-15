const passwordField = document.getElementById('passwordField');
const togglePasswordButton = document.getElementById('togglePassword');
const eyeIcon = document.getElementById('eyeIcon');

// Toggle password visibility
togglePasswordButton.addEventListener('click', () => {
    // Toggle the type attribute between 'password' and 'text'
    const type = passwordField.type === 'password' ? 'text' : 'password';
    passwordField.type = type;

    // Toggle the eye icon between 'fa-eye' and 'fa-eye-slash'
    eyeIcon.classList.toggle('fa-eye-slash');
});

