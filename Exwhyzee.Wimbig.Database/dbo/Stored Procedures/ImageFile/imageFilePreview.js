window.onload = function () {
    var fileUpload = document.getElementById("gallery-photo-add");
    fileUpload.onchange = function () {
        if (typeof (FileReader) != "undefined") {
            var gallery = document.getElementById("gallery");
            gallery.innerHTML = "";
            
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;

            for (var i = 0; i < fileUpload.files.length; i++) {
                var file = fileUpload.files[i];
                if (regex.test(file.name.toLowerCase())) {
                    var reader = new FileReader();
                    reader.onload = function (e) {

                        var img = document.createElement("IMG");
                        var textbox = document.createElement('input');
                        textbox.type = 'text';
                        textbox.name = 'tag_line[]';
                        textbox.placeholder = 'Enter image tag line';
                        img.height = "100";
                        img.width = "100";
                        img.src = e.target.result;
                        gallery.appendChild(img);
                        gallery.appendChild(textbox);
                    }
                    reader.readAsDataURL(file);                   
                } else {
                    alert(file.name + " is not a valid image file.");
                    gallery.innerHTML = "";
                    return false;
                }
            }
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    }
};