function reloadCaptcha() {
    document.getElementById("captchaImage").src = "/Account/GenerateCaptcha?" + new Date().getTime();
}