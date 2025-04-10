document.addEventListener('DOMContentLoaded', () => {

    // open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]') //i html "data-modal="true"
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            if (modal)
                modal.style.display = 'flex';
        })
    })

    // close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')//i html "data-close="true"
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('._modal') //letar efter en knapp som är närmast klassen .modal
            if (modal) {
                modal.style.display = 'none'

                //clear formdata här senare
            }
        })
    })



    // open and close dots-popup
    const dotsIcons = document.querySelectorAll('[data-popup="true"]');
    dotsIcons.forEach(icon => {
        icon.addEventListener('click', (e) => {
            e.stopPropagation();
            document.querySelectorAll('.dots-popup').forEach(p => p.style.display = 'none');

            const projectCard = icon.closest('.project-card');
            const popup = projectCard.querySelector('.dots-popup');
            if (popup)
                popup.style.display = 'flex';

            //close popup
            document.addEventListener('click', () => {
                document.querySelectorAll('.dots-popup').forEach(p => p.style.display = 'none');
            })
        })
    })

    //open and close settings
    const gearIcon = document.querySelectorAll('[data-settings="true"]');
    gearIcon.forEach(icon => {
        icon.addEventListener('click', (e) => {
            e.stopPropagation();

            const settings = icon.closest('.settings-wrapper');
            const popUp = settings.querySelector('.settings-popup')
            if (popUp)
                popUp.style.display = "flex";

            //close settings
            document.addEventListener('click', () => {
                document.querySelectorAll('.settings-popup').forEach(p => p.style.display = 'none')
            })
        })
    })
})
