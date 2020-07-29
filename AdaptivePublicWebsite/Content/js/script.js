
$(document).ready(function () {
    $("[data-fancybox]").fancybox();

    $("#navigation li a").click(function (e) {
        //e.preventDefault();

        var targetElement = $(this).attr("href");
        var targetPosition = $(targetElement).offset().top;
        $("html, body").animate({ scrollTop: targetPosition - 140 }, "slow");

    });

    var typed = new Typed(".typed", {
        strings: ["grab a coffee or something nice", "start your day with a quick checkup", "not an app, just an old-fashioned website", "scripture and grace, a prayer or two", "Just a reminder - Jesus loves you!"],
        typeSpeed: 50,
        loop: false,
        startDelay: 1000,
        smartBackspace: true,
        showCursor: false
    });

});