$(document).ready(function () {
    const activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $(`#adminTabs a[href="${activeTab}"]`).tab('show');
    }

    $('#adminTabs a').on('click', function (e) {
        e.preventDefault();
        $(this).tab('show'); 
        localStorage.setItem('activeTab', $(this).attr('href')); 
    });
});


function showDeleteModal(albumId) {
    var modal = document.getElementById('deleteModal');
    var confirmButton = document.getElementById('confirmDeleteButton');
    var cancelButton = document.getElementById('cancelDeleteButton');
    confirmButton.onclick = function() {
        deleteAlbum(albumId);
    };
    cancelButton.onclick = function() {
        $(modal).modal('hide');
    };

    $(modal).modal('show');
}

function showDeleteUserModal(userId) {
    var modal = document.getElementById('deleteModal');
    var confirmButton = document.getElementById('confirmDeleteButton');
    var cancelButton = document.getElementById('cancelDeleteButton');
    modal.querySelector('.modal-body').innerHTML = '<p>Are you sure you want to delete this user and all albums associated to them?</p>';
    confirmButton.onclick = function() {
        deleteUser(userId);
    };
    cancelButton.onclick = function() {
        $(modal).modal('hide');
    };

    $(modal).modal('show');
}

function deleteAlbum(albumId) {
    fetch(`?handler=DeleteAlbum&albumId=${albumId}`).then(response => {
        if (response.ok) {
            location.reload();
        } else {
            alert('Failed to delete album.');
        }
    });
}

function deleteUser(userId) {
    fetch(`?handler=DeleteUser&userId=${userId}`).then(response => {
        if (response.ok) {
            location.reload();
        } else {
            alert('Failed to delete user: ' + response.message);
        }
    });
}