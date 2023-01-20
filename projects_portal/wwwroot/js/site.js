function presentationName(presentationInput) {
    var presentation = presentationInput.files;
    var presentationText = document.getElementById("presentationText");
    for (var i = 0; i < presentation.length; i++) {
        presentationText.innerHTML = presentation[i].name;
    }
}

function apkName(apkInput) {
    var apk = apkInput.files;
    var apkText = document.getElementById("apkText");
    for (var i = 0; i < apk.length; i++) {
        apkText.innerHTML = apk[i].name;
    }
}

function addGit() {
    var gitInput = document.getElementById("gitInput");
    var gitButton = document.getElementById("gitButton");
    var gitClass = document.getElementById("gitClass");
    gitButton.style.display = "none";
    gitClass.className = "form-group"
    gitInput.setAttribute('type', 'text');
}

function addSite() {
    var siteInput = document.getElementById("siteInput");
    var siteButton = document.getElementById("siteButton");
    siteButton.style.display = "none";
    siteInput.setAttribute('type', 'text');
}

function imageEdit(imageInput) {
    var imageBlock = document.getElementById("imageBlock");
    var imageButton = document.getElementById("imageButton");
    var imageText = document.getElementById("imageText");
    var image = imageInput.files;
    imageBlock.style.display = "none";
    imageButton.style.display = "block";
    for (var i = 0; image.length; i++) {
        if (image[i].name.length > 30) {
            imageText.style.paddingTop = "0%";
        }
        else {
            imageText.style.paddingTop = "50%";
        }
        imageText.innerHTML = image[i].name;
    }
}