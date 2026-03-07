(function () {
    $(function () {
        $('#tsagree').on('change', function (e) {
            var isSel = $('#tsagree').is('checked');
            if (!isSel) {
                blockAlert('請勾選課程須知');
            }
        });
    });
})();