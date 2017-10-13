let focusNote = null;

$(document).on('pagebeforeshow ', '#main', function () {   // see: https://stackoverflow.com/questions/14468659/jquery-mobile-document-ready-vs-page-events

    const template_note = $(".templates .note");
    const el_Display = $("#notes_display");

    let notesList;
    apiGetNotes((err, result) => {
        if (!err) {
            notesList = result;
            updateNotes()
        } else {
            console.error(err);
        }
    });

    function generateNoteFromTemplate(note) {
        const newNote = template_note.clone();

        newNote.find('.note_title').html(note.Subject.charAt(0).toUpperCase() + note.Subject.slice(1));
        newNote.find('.note_priority').html(note.Priority);
        newNote.find('.note_details').html(note.Details);
        newNote.find('.delete-button').click(() => {
            deleteNote(note)
        });
        newNote.find('.edit-button').click(() => {
            editNote(note)
        });

        return newNote
    }

    function updateNotes() {
        el_Display.empty();
        notesList.forEach((note) => {
            el_Display.append($(generateNoteFromTemplate(note)).collapsible());
        });
        el_Display.listview("refresh");
    }

    function editNote(note) {
        focusNote = note;
        $.mobile.changePage($("#add"))
    }

    function deleteNote(note) {
        apiDeleteNote(note, (err) => {
            if (!err) {
                notesList.splice(notesList.indexOf(note), 1);
                updateNotes();
            }
        })
    }

});

$(document).on('pagebeforeshow ', '#add', function () {   // see: https://stackoverflow.com/questions/14468659/jquery-mobile-document-ready-vs-page-events

    // Copy the form I made from the old index over here.
    // If this is loaded and focusNote is not null, fill the form with the note data
    // The difference between add and edit is that note.Id will not be null
    // On submit, make sure to $.mobile.changePage($("#main"))
    // use apiAddNote(note, callback) and apiEditNote(note, callback) this can be found in api.js

});





