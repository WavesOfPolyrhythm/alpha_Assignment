document.addEventListener('DOMContentLoaded', () => {

    //Cookie-Consent

    if (!getCookie("cookieConsent"))
        showCookieModal()

})

function showCookieModal() {
    const modal = document.getElementById('cookieModal')
    if (modal) modal.style.display = "flex"

    const consentValue = getCookie('cookieConsent')
    if (!consentValue) return

    try {
        const consent = JSON.parse(consentValue)
        document.getElementById("cookiePreferences").checked = consent.preferences
        document.getElementById("cookieStatistics").checked = consent.statistics
        document.getElementById("cookieMarketing").checked = consent.marketing
    }
    catch (error) {
        console.error('unable to handle cookie consent values', error)
    }
}

function hideCookieModal() {
    const modal = document.getElementById('cookieModal')
    if (modal) modal.style.display = "none"
}

function getCookie(name) {
    const nameEQ = name + "="
    const cookies = document.cookie.split(';')
    for (let cookie of cookies) {
        cookie = cookie.trim()
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
    return null
}

function setCookie(name, value, days) {
    let expires = ""
    if (days) {
        const date = new Date()
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000)
        expires = "; expires=" + date.toUTCString()
    }

    const encodeValue = encodeURIComponent(value || "")
    document.cookie = `${name}=${encodeValue}${expires}; path=/; SameSite=Lax`
}

async function acceptAll() {
    const consent = {
        essential: true,
        preferences: true,
        statistics: true,
        marketing: true
    }

    setCookie("cookieConsent", JSON.stringify(consent), 365)
    await handleConsent(consent)
    hideCookieModal()
}

async function acceptSelected() {
    const form = document.getElementById("cookieConsentForm")
    const formData = new FormData(form);

    const consent = {
        essential: true,
        preferences: formData.get("preferences") === "on",
        statistics: formData.get("statistics") === "on",
        marketing: formData.get("marketing") === "on",
    }

    setCookie("cookieConsent", JSON.stringify(consent), 365)
    await handleConsent(consent)
    hideCookieModal()
}

async function handleConsent(consent) {

    try {
        const res = await fetch('/cookies/setcookies', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(consent)
        })

        if (!res.ok) {
            console.error('Unable to set cookie consent', await res.text())
        }
    }
    catch (error) {
        console.error("Error:: ", error)
    }
}