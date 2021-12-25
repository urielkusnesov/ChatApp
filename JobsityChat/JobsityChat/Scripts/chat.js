$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.jobsityHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');
    };
    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            chat.server.send($('#username').val(), $('#message').val());

            //handle message in backend
            $.ajax({
                type: "POST",
                url: "/Home/HandlePost/",
                data: { name: $('#username').val(), message: $('#message').val() },
            });

            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
    });
});

// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
