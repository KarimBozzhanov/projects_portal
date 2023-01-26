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

function imageEdit(editImageInput) {
    var reader = new FileReader();
    var image = editImageInput.files[0];
    var imageChange = document.getElementById("imageChange");
    reader.readAsDataURL(image);
    reader.onload = function (p) {
        imageChange.src = p.target.result;
    }
}

function imageName(imageInput) {
    var reader = new FileReader();
    var image = imageInput.files[0];
    var imageBlock = document.getElementById("imageBlock");
    var imageBtn = document.getElementById("imageBtn");
    imageBtn.style.display = "none"
    imageBlock.style.display = "block";
    reader.readAsDataURL(image);
    reader.onload = function (e) {
        document.querySelector("#imageSrc").src = e.target.result;
    }
}

function studentButton() {
    var studentBtn = document.getElementById("student_btn");
    var teacherBtn = document.getElementById("teacher_btn");
    var groupInput = document.getElementById("groupInput")
    groupInput.type = "text";
    studentBtn.style.zIndex = "99";
    teacherBtn.style.zIndex = "98";
}

function teacherButton() {
    var teacherBtn = document.getElementById("teacher_btn");
    var studentBtn = document.getElementById("student_btn");
    var groupInput = document.getElementById("groupInput")
    groupInput.type = "hidden";
    teacherBtn.style.zIndex = "99";
    studentBtn.style.zIndex = "98";
}