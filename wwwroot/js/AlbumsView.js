document.addEventListener('keydown', function(event) {
    if (event.key === 'Enter') {
        var modal = document.querySelector('.modal.show');
        if (modal) {
            var confirmButton = modal.querySelector('.btn-primary');
            if (confirmButton) {
                event.preventDefault();
                confirmButton.click();
            }
        }
    }
});

function validateAlbumForm() {
    var isValid = true;

    var albumName = document.getElementById('albumName').value.trim();
    var albumReleaseDate = document.getElementById('albumReleaseDate').value.trim();
    var albumArtist = document.getElementById('albumArtist').value.trim();
    var albumDescription = document.getElementById('albumDescription').value.trim();
    var albumCoverImage = document.getElementById('albumCoverImage').files[0];
    

    document.getElementById('albumNameError').innerText = '';
    document.getElementById('albumReleaseDateError').innerText = '';
    document.getElementById('albumArtistError').innerText = '';
    document.getElementById('albumTypeError').innerText = '';
    document.getElementById('albumDescriptionError').innerText = '';
    document.getElementById('albumTracksError').innerText = '';
    document.getElementById('albumCoverImageError').innerText = '';

    if (!albumName) {
        document.getElementById('albumNameError').innerText = 'Album Name is required.';
        isValid = false;
    }

    if (!albumReleaseDate) {
        document.getElementById('albumReleaseDateError').innerText = 'Release Date is required.';
        isValid = false;
    }

    if (!albumArtist) {
        document.getElementById('albumArtistError').innerText = 'Artist is required.';
        isValid = false;
    }

    if(!albumDescription) {
        document.getElementById('albumDescriptionError').innerText = 'Description is required.';
        isValid = false;
    }

    if (albumCoverImage && albumCoverImage.size > 4 * 1024 * 1024) { // 4 MB in bytes
        document.getElementById('albumCoverImageError').innerText = 'File size exceeds 4 MB.';
        isValid = false;
    }


    return isValid;
}


function showAlbumDetails(albumId) {
    var album = getAlbum(albumId);
    document.getElementById('album-name').innerText = album.albumName;
    document.getElementById('details-releaseDate').innerText = new Date(album.releaseDate).toLocaleDateString();
    document.getElementById('details-albumType').innerText = album.albumType;
    document.getElementById('details-description').innerText = album.description;
    document.getElementById('details-artist').innerText = album.artist;
    var closeDetailsButton = document.getElementById('closeDetailsButton');

    const tracksList = document.getElementById('details-tracksList');
    tracksList.innerHTML = '';

    album.tracks.forEach(track => {
        const li = document.createElement('li');
        li.innerText = track;
        tracksList.appendChild(li);
    });

    closeDetailsButton.onclick = function() {
        $('#detailModal').modal('hide');
    };

    var img = document.getElementById('details-coverImage');
    img.src = `data:image/png;base64,${album.coverImage}`; 
    img.style.display = 'block';

    document.getElementById('detailModal').setAttribute('data-album-id', album.id);

    $('#detailModal').modal('show');
}

function displayImage(input) {
    img = document.getElementById('coverImagePreview');
    img.src = '#';
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function(e) {
            var img = document.getElementById('coverImagePreview');
            img.src = e.target.result;
            img.style.display = 'block';
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function showDeleteModal() {
    var albumId = document.getElementById('detailModal').getAttribute('data-album-id');
    var modal = document.getElementById('deleteModal');
    var confirmButton = document.getElementById('confirmDeleteButton');
    var cancelButton = document.getElementById('cancelDeleteButton');
    confirmButton.onclick = function() {
        deleteAlbum(albumId);
    };
    cancelButton.onclick = function() {
        $(modal).modal('hide');
        $(document.getElementById('detailModal')).modal('show');
    };

    $(document.getElementById('detailModal')).modal('hide');
    $(modal).modal('show');
}

function showCreateModal() {
    var modal = document.getElementById('createModal');
    var confirmButton = document.getElementById('confirmCreateButton');
    var cancelButton = document.getElementById('cancelCreateButton');
    var inputFields = modal.querySelectorAll('input, textarea'); 
    var tracksContainer = document.getElementById('tracksContainer'); 

    clearCreateModal(inputFields, tracksContainer);

    confirmButton.addEventListener('click', function(event) {
        event.preventDefault();
        createAlbum();
    });
    cancelButton.onclick = function() {
        $(modal).modal('hide');
    };

    $(modal).modal('show');
}

function showEditModal() {
    var albumId = document.getElementById('detailModal').getAttribute('data-album-id');
    var modal = document.getElementById('createModal');
    var modalTitle = document.getElementById('createModalTitle');
    var confirmButton = document.getElementById('confirmCreateButton');
    var cancelButton = document.getElementById('cancelCreateButton');
    var inputFields = modal.querySelectorAll('input, textarea'); 
    var albumTypeSelect = modal.querySelector('select#albumType');
    var tracksContainer = document.getElementById('tracksContainer');
    var img = document.getElementById('coverImagePreview');

    clearCreateModal(inputFields, tracksContainer); 
    var album = getAlbum(albumId);
    

    confirmButton.onclick = function() {
        editAlbum(albumId)
    };
    cancelButton.onclick = function() {
        $(modal).modal('hide');
        $(document.getElementById('detailModal')).modal('show');
    };

    modalTitle.innerHTML = 'Edit Album';
    confirmButton.innerHTML = 'Save';

    img.src = `data:image/png;base64,${album.coverImage}`; 
    img.style.display = 'block';

    inputFields.forEach(function(input) {
        switch (input.id) {
            case 'albumName':
                input.value = album.albumName;
                break;
            case 'albumReleaseDate':
                var releaseDate = new Date(album.releaseDate);
                var formattedDate = releaseDate.getFullYear() + '-' + 
                                    ('0' + (releaseDate.getMonth() + 1)).slice(-2) + '-' + 
                                    ('0' + releaseDate.getDate()).slice(-2);
                input.value = formattedDate;
                break;
            case 'albumArtist':
                input.value = album.artist;
                break;
            case 'albumDescription':
                input.value = album.description;
                break;
        }
    });

    Array.from(albumTypeSelect.options).forEach(function(option) {
        if (option.value === album.albumType) {
            option.selected = true;
        }
    });

    album.tracks.forEach(function(track) {
        addNewTrackInput(track);
    });

    $(document.getElementById('detailModal')).modal('hide');
    $(modal).modal('show');
}

function clearCreateModal(inputFields, tracksContainer) {
    inputFields.forEach(function(input) {
        input.value = '';
    });

    while (tracksContainer.firstChild) {
        tracksContainer.removeChild(tracksContainer.firstChild);
    }
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

function getAlbum(albumId) {
    $.ajax({
        url: '/AlbumsView?handler=GetAlbum',
        type: 'GET',
        data: { albumId: albumId },
        async: false,
        success: function(response) {
            if (response.success) {
                album = response.album;
            } else {
                console.error('Failed to retrieve album');
            }
        },
        error: function() {
            console.error('Error in AJAX request');
        }
    });

    return album;
}

function createAlbum() {
    if (!validateAlbumForm()) {
        return;
    }

    var albumName = document.getElementById('albumName').value;
    var albumReleaseDate = document.getElementById('albumReleaseDate').value;
    var albumArtist = document.getElementById('albumArtist').value;
    var albumType = document.getElementById('albumType').value;
    var albumDescription = document.getElementById('albumDescription').value;
    var albumTracks = Array.from(document.getElementsByName('albumTracks[]')).map(input => input.value);
    var albumCoverImage = document.getElementById('albumCoverImage').files[0];
    var tracksParam = albumTracks.map(track => `tracks=${encodeURIComponent(track)}`).join('&');

    if (albumCoverImage && albumCoverImage.size > 4 * 1024 * 1024) { // 4 MB in bytes
        alert('File size exceeds 4 MB.');
        return;
    }

    fetch('?handler=ClearInputAlbum');

    var formData = new FormData();
    formData.append('InputAlbum.AlbumName', albumName);
    formData.append('InputAlbum.ReleaseDate', albumReleaseDate);
    formData.append('InputAlbum.Artist', albumArtist);
    formData.append('InputAlbum.AlbumType', albumType);
    formData.append('InputAlbum.Description', albumDescription);
    formData.append('InputAlbum.CoverImage', albumCoverImage);
    albumTracks.forEach((track, index) => {
        formData.append(`InputAlbum.Tracks[${index}]`, track);
    });

    fetch('?handler=AddAlbum', {
        method: 'POST',
        headers: {
            "XSRF-TOKEN": document.querySelector("[name='__RequestVerificationToken']").value
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        } else {
            alert('Failed to create album.');
        }
    });
}

function editAlbum(albumId) {
    if (!validateAlbumForm()) {
        return;
    }

    var albumName = document.getElementById('albumName').value;
    var albumReleaseDate = document.getElementById('albumReleaseDate').value;
    var albumArtist = document.getElementById('albumArtist').value;
    var albumType = document.getElementById('albumType').value;
    var albumDescription = document.getElementById('albumDescription').value;
    var albumCoverImage = document.getElementById('albumCoverImage').files[0];
    var albumTracks = Array.from(document.getElementsByName('albumTracks[]')).map(input => input.value);
    var tracksParam = albumTracks.map(track => `tracks=${encodeURIComponent(track)}`).join('&');

    fetch('?handler=ClearInputAlbum');

    var formData = new FormData();
    formData.append('InputAlbum.AlbumId', albumId);
    formData.append('InputAlbum.AlbumName', albumName);
    formData.append('InputAlbum.ReleaseDate', albumReleaseDate);
    formData.append('InputAlbum.Artist', albumArtist);
    formData.append('InputAlbum.AlbumType', albumType);
    formData.append('InputAlbum.Description', albumDescription);
    formData.append('InputAlbum.CoverImage', albumCoverImage);
    albumTracks.forEach((track, index) => {
        formData.append(`InputAlbum.Tracks[${index}]`, track);
    });

    
    if (albumCoverImage && albumCoverImage.size > 4 * 1024 * 1024) { // 4 MB in bytes
        alert('File size exceeds 4 MB.');
        return;
    }


    fetch('?handler=EditAlbum', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        } else {
            alert('Failed to edit album.');
        }
    });

    return true;
}

function addNewTrackInput(trackName = '') {
    var newTrackInput = document.createElement('div');
    newTrackInput.className = 'track-item'; 

    var inputField = document.createElement('input');
    inputField.type = 'text';
    inputField.name = 'albumTracks[]';
    inputField.className = 'form-control track-input';
    inputField.placeholder = 'Track Name';
    inputField.value = trackName;
    newTrackInput.appendChild(inputField);

    var deleteButton = document.createElement('button');
    deleteButton.innerHTML = 'Delete';
    deleteButton.className = 'btn btn-danger btn-sm delete-track-button'; 
    deleteButton.innerHTML = '<i class="fas fa-trash-alt"></i>';
    deleteButton.onclick = function() {
        newTrackInput.remove();
    };
    newTrackInput.appendChild(deleteButton);

    document.getElementById('tracksContainer').appendChild(newTrackInput);
}