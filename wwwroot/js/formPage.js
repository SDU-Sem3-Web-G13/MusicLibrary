const listContainer = document.getElementById("list-container");

function addTrack() {
    if (listContainer.childElementCount === 10) {
        alert("List can have max 10 elements");
    }
    else {
        let li = document.createElement("li");
        let input = document.createElement('input');
        input.type = 'text';

        li.appendChild(input);
        listContainer.appendChild(li);
    }
}

function deleteTrack() {
    if (listContainer.childElementCount === 1) {
        alert("Has to be at least one track");
    }
    else {
        const li = listContainer.querySelector('li:last-child');
        li.remove();
    }
}