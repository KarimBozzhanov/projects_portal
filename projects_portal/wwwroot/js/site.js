function limitStr(str, n, symb) {
    if (!n && !symb) return str;
    symb = symb || '...';
    return str.substr(0, n - symb.length) + symb;
}

function presentationName(presentationInput) {
    var presentation = presentationInput.files;
    var presentationText = document.getElementById("presentationText");
    for (var i = 0; i < presentation.length; i++) {
        presentationText.innerHTML = limitStr(presentation[i].name, 17);
    }
}

function apkName(apkInput) {
    var apk = apkInput.files;
    var apkText = document.getElementById("apkText");
    for (var i = 0; i < apk.length; i++) {

        apkText.innerHTML = limitStr(apk[i].name, 15);
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

function addEmail() {
    var emailInput = document.getElementById("emailInput");
    var emailButton = document.getElementById("emailButton");
    var saveUser = document.getElementById("saveUser");
    var cancelBtn = document.getElementById("cancelBtn");
    cancelBtn.style.display = "block"
    saveUser.style.display = "block";
    emailButton.style.display = "none";
    emailInput.setAttribute('type', 'text');
}

function cancelChangeUser() {
    var emailInput = document.getElementById("emailInput");
    var emailButton = document.getElementById("emailButton");
    var saveUser = document.getElementById("saveUser");
    var cancelBtn = document.getElementById("cancelBtn");
    cancelBtn.style.display = "none"
    saveUser.style.display = "none";
    emailButton.style.display = "block";
    emailInput.setAttribute('type', 'hidden');
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
    imageBtn.style.display = "none";
    imageBlock.style.display = "block";
    reader.readAsDataURL(image);
    reader.onload = function (e) {
        document.querySelector("#imageSrc").src = e.target.result;
    }
}

function studentButton() {
    var studentBtn = document.getElementById("student_btn");
    var teacherBtn = document.getElementById("teacher_btn");
    var groupInput = document.getElementById("groupInput");
    var curatorInput = document.getElementById("curatorInput");
    var codeInput = document.getElementById("codeInput");
    groupInput.type = "text";
    groupInput.required = "true";
    curatorInput.type = "text";
    codeInput.type = "hidden";
    curatorInput.required = "true";
    codeInput.required = "false";
    studentBtn.style.zIndex = "99";
    teacherBtn.style.zIndex = "98";
}

function teacherButton() {
    var teacherBtn = document.getElementById("teacher_btn");
    var studentBtn = document.getElementById("student_btn");
    var groupInput = document.getElementById("groupInput");
    var curatorInput = document.getElementById("curatorInput");
    var codeInput = document.getElementById("codeInput");
    groupInput.type = "hidden";
    groupInput.required = "false";
    curatorInput.type = "hidden";
    codeInput.type = "password";
    codeInput.required = "true";
    curatorInput.required = "false";
    teacherBtn.style.zIndex = "99";
    studentBtn.style.zIndex = "98";
}

function projectButton() {
    var projectBtn = document.getElementById("project_btn");
    var graduateBtn = document.getElementById("graduate_btn");
    projectBtn.style.zIndex = "99";
    graduateBtn.style.zIndex = "98";
}

function graduateButton() {
    var graduateBtn = document.getElementById("graduate_btn");
    var projectBtn = document.getElementById("project_btn");
    graduateBtn.style.zIndex = "99";
    projectBtn.style.zIndex = "98";
}