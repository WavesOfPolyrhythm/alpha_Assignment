document.addEventListener('DOMContentLoaded', () => {

    // open modal
    //Some code made by CHAT GPT to solve the issue where validation remained visible when switching modals.
    // by introducing "currentModal" and closing + clearing all other modals before opening the selected one.
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const currentModal = document.querySelector(modalTarget)

            const allModals = document.querySelectorAll('._modal')
            allModals.forEach(modal => {
                if (modal !== currentModal) {
                    modal.style.display = 'none'

                    modal.querySelectorAll('form').forEach(form => {
                        form.reset()

                        const validationMessages = form.querySelectorAll('.field-validation-error')
                        validationMessages.forEach(message => {
                            message.textContent = ''
                        })

                        const errorInputs = form.querySelectorAll('.input-validation-error')
                        errorInputs.forEach(input => {
                            input.classList.remove('input-validation-error')
                        })
                    })
                }
            })

            if (currentModal)
                currentModal.style.display = 'flex';
        })
    })

    //Handle submit form
    const forms = document.querySelectorAll('._form-modal')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            form.querySelectorAll('[data-val="true"]').forEach(input => {
                input.classList.remove('input-validation-error')
            })

            form.querySelectorAll('[data-valmsg-for]').forEach(span => {
                span.innerText = ''
                span.classList.remove('field-validation-error')
            })

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('._modal')
                    if (modal) {
                        modal.style.display = 'none';
                        window.location.reload()
                    }
                }
                else if (res.status == 400) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            let input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            let span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n')
                                span.classList.add('field-validation-error')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('error submitting the form')
            }
        })
    })

    // close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('._modal')
            if (modal) {
                modal.style.display = 'none'

                modal.querySelectorAll('form').forEach(form => {
                    form.reset()

                    const validationMessages = form.querySelectorAll('.field-validation-error')
                    validationMessages.forEach(message => {
                        message.textContent = ''
                    })

                    const errorInputs = form.querySelectorAll('.input-validation-error')
                    errorInputs.forEach(input => {
                        input.classList.remove('input-validation-error')
                    })
                    
                })
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
