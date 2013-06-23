$('.articleButton').click(function () {
    var toSelect = $(this).next('.articleStyle');

    toSelect.addClass('selected');
    $('.articleStyle').not(toSelect).removeClass('selected');
});

$('.needsConfirmation').submit(function () {
    return confirm("Are you sure you would like to delete this (cannot be reversed)? ");
});