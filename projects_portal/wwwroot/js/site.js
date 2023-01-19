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

function imageName(imageInput) {
    var image = imageInput.files;
    var imageText = document.getElementById("imageText");
    for (var i = 0; image.length; i++) {
        if (image[i].name.length > 40) {
            imageText.style.paddingTop = "0%";
        }
        else {
            imageText.style.paddingTop = "50%";
        }
        imageText.innerHTML = image[i].name;
    }
}