
$(document).ready(function () {
    $("[data-fancybox]").fancybox();

    $("#navigation li a").click(function (e) {
        //e.preventDefault();

        var targetElement = $(this).attr("href");
        var targetPosition = $(targetElement).offset().top;
        $("html, body").animate({ scrollTop: targetPosition - 140 }, "slow");

    });

    var typed = new Typed(".typed", {
        strings: ["your trusted partner moving to the cloud", "a managed services provider", "with an acumen in agile application development", "founded in 1995", "Transforming your business for a digital age!"],
        typeSpeed: 50,
        loop: false,
        startDelay: 1000,
        smartBackspace: true,
        showCursor: false
    });

});