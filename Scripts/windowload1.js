/* windowload1 */
$(window).load(function () {
    $('.flexslider').flexslider({
        animation: "slide",
        animationLoop: false,
        itemWidth: 185,
        itemMargin: 5,
        pausePlay: true,
        start: function (slider) {
            $('body').removeClass('loading');
        }
    });
});