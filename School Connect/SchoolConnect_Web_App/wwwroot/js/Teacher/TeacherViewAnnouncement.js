let announcements = document.querySelectorAll('.details');

announcements.forEach(announcement => {
    announcement.addEventListener('click', function () {
        this.parentElement.querySelector('a').click();
    });
});

