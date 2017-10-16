const NOTES_URI = "/api/notes";

function generateDefaultApiResponse(callback) {
    return {
        success: function (result) {
            callback(null, result)
        },
        error: function (err) {
            callback(err)
        }
    }
}

const defaultApiReponse = {};

function apiGetNotes(callback) {
    $.ajax({
        url: NOTES_URI,
        type: 'GET',
        contentType: "application/json",
        ...generateDefaultApiResponse(callback)
    });
}

function apiAddNote(note, callback) {
    $.ajax({
        url: NOTES_URI,
        type: 'POST',
        data: JSON.stringify(note),
        contentType: "application/json",
        ...generateDefaultApiResponse(callback)
    });
}

function apiEditNote(note, callback) {
    $.ajax({
        url: NOTES_URI,
        type: 'POST',
        data: JSON.stringify(note),
        contentType: "application/json",
        ...generateDefaultApiResponse(callback)
    });
}

function apiDeleteNote(note, callback) {
    $.ajax({
        url: NOTES_URI + '/' + note.Id,
        type: 'DELETE',
        ...generateDefaultApiResponse(callback)
    });
}