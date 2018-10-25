function Redirect(result) {
    if (result.url) {
        window.location.href = result.url;
    }
}

function HideNotify() {
    setTimeout(function () {
        $('#notify').hide();
    }, 3000);
}

function SetNotify(type, title, text) {
    let notifyTitle = $('#notifyTitle');
    let notifyText = $('#notifyText');
    let closeBtn = $('.notification-dismiss');

    $('#notifyType').removeClass();
    $('#notifyType').addClass(type);
    notifyTitle.html(title);
    notifyText.html(text);

    SetStyles(type, notifyTitle, notifyText, closeBtn);
}

function SetStyles(type, title, text, btn) {
    if (type.includes('success')) {
        title.css('color', 'rgb(94, 164, 0)');
        text.css('color', 'rgb(75, 88, 58)');
        btn.css('background-color', 'rgb(176, 202, 146)');
    } else if (type.includes('error')) {
        title.css('color', 'rgb(236, 61, 61)');
        text.css('color', 'rgb(65, 47, 47)');
        btn.css('background-color', 'rgb(228, 190, 190)');
    }
}

$('.notification-dismiss').click(function () {
    $('#notify').hide();
});