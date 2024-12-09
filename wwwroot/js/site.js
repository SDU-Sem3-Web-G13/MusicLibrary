// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Logout() {
    $.ajax({
        url: '/Index?handler=Logout',
        type: 'GET',
        async: false,
        success: function(response) {
            if (response.success) {
                window.location.href = '/Login';
            } else {
                console.error('Failed to logout');
            }
        },
        error: function() {
            console.error('Error in AJAX request');
        }
    });
}