document.addEventListener('DOMContentLoaded', () => {

    //Some code made by CHAT GPT in open modal, to solve the issue where validation remained visible when switching modals.
    // Introducing "currentModal" and closing + clearing all other modals before opening the selected one.

    // open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const currentModal = document.querySelector(modalTarget)
            const editId = button.getAttribute('data-id');

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

            //Made by ChatGPT
            // Check if the modal exists
            // Then find the hidden input field named "Id" inside the modal.
            // If both the input and the button's data-id are found, set the input value to the data-id.
            // This makes sure the correct project ID is included when the form is submitted
            // Show the modal with "flex"
            if (currentModal) {
                const idInput = currentModal.querySelector('input[name="Id"]');
                if (idInput && editId) {
                    idInput.value = editId;
                }

                if (currentModal)
                    currentModal.style.display = 'flex';
            }
        })
    })

    document.querySelectorAll('.image-preview-container').forEach(container => {
        const fileInput = container.parentElement.querySelector('input[type="file"]');
        const imagePreview = container.querySelector('#image-preview');
        const imagePreviewIcon = container.querySelector('#image-preview-icon');
        const imagePreviewIconContainer = container.querySelector('#image-preview-icon-container');

        container.addEventListener('click', () => {
            fileInput.click();
        });

        fileInput.addEventListener('change', (e) => {
            const file = e.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    imagePreview.src = e.target.result;
                    imagePreview.classList.remove('hide');
                    imagePreview.classList.add('selected');
                    imagePreviewIconContainer.classList.add('selected');
                    imagePreviewIcon.className = 'fa-solid fa-pen-to-square';
                };
                reader.readAsDataURL(file);
            }
        });
    });


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

                    const imagePreview = modal.querySelector('#image-preview');
                    const imagePreviewIcon = modal.querySelector('#image-preview-icon');
                    const imagePreviewIconContainer = modal.querySelector('#image-preview-icon-container');

                    if (imagePreview) {
                        imagePreview.src = '#';
                        imagePreview.classList.add('hide');
                        imagePreview.classList.remove('selected');
                    }
                    if (imagePreviewIcon) {
                        imagePreviewIcon.className = 'fa-duotone fa-solid fa-camera';
                    }
                    if (imagePreviewIconContainer) {
                        imagePreviewIconContainer.classList.remove('selected');
                    }
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

    //Sort Status
    //Some code made by CHAT GPT
    // This code handles the tab filtering for project cards.
    // When a user clicks a tab (All, Started, Completed), it highlights the active tab
    // and shows only the project cards that match the selected status.
    // If "All" is selected, all project cards are displayed.

    const tabLinks = document.querySelectorAll('.tab-link');
    const projectCards = document.querySelectorAll('.project-card');

    tabLinks.forEach(tab => {
        tab.addEventListener('click', function (e) {
            e.preventDefault();

            tabLinks.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            const filter = tab.getAttribute('data-filter').toLowerCase();

            projectCards.forEach(card => {
                const status = card.getAttribute('data-status').toLowerCase();

                if (filter === 'all') {
                    card.style.display = 'flex';
                }
                else if (filter === status) {
                    card.style.display = 'flex';
                }
                else {
                    card.style.display = 'none';
                }
            });
        });
    });


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