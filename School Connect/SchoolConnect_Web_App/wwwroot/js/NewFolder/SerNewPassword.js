const passwordField = document.getElementById('passwordField');
const newPasswordField = document.getElementById('newPasswordField');
const confirmPasswordField = document.getElementById('confirmPasswordField');

const togglePasswordButton = document.getElementById('togglePassword');
const toggleNewPasswordButton = document.getElementById('toggleNew');
const toggleConfirmPasswordButton = document.getElementById('toggleConfirm');

const passwordEyeIcon = document.getElementById('password-eyeIcon');
const newEyeIcon = document.getElementById('new-eyeIcon');
const confirmEyeIcon = document.getElementById('confirm-eyeIcon');

togglePasswordButton.addEventListener('click', () => {
    passwordField.type = passwordField.type === 'password' ? 'text' : 'password';

    if (passwordEyeIcon.classList.contains('fa-eye-slash')) {
        passwordEyeIcon.classList.replace('fa-eye-slash', 'fa-eye');
    } else {
        passwordEyeIcon.classList.replace('fa-eye', 'fa-eye-slash');
    }
});

toggleNewPasswordButton.addEventListener('click', () => {
    newPasswordField.type = newPasswordField.type === 'password' ? 'text' : 'password';

    if (newEyeIcon.classList.contains('fa-eye-slash')) {
        newEyeIcon.classList.replace('fa-eye-slash', 'fa-eye');
    } else {
        newEyeIcon.classList.replace('fa-eye', 'fa-eye-slash');
    }
});

toggleConfirmPasswordButton.addEventListener('click', () => {
    confirmPasswordField.type = confirmPasswordField.type === 'password' ? 'text' : 'password';

    if (confirmEyeIcon.classList.contains('fa-eye-slash')) {
        confirmEyeIcon.classList.replace('fa-eye-slash', 'fa-eye');
    } else {
        confirmEyeIcon.classList.replace('fa-eye', 'fa-eye-slash');
    }
});

