let focusNote = null;
let noteList;

noteList = new NoteList();

$(document).on('pagebeforeshow ', '#main', onCreateMain);
$(document).on('pagebeforeshow ', '#add', onCreateAdd);
$(document).on('pagebeforeshow ', '#details', onCreateDetails);

function onCreateMain() {

    $('.page_title').html("Main");

    focusNote = null;

    const template_note = $(".templates .note");
    const el_Display = $("#notes_display");

    noteList.setOnChange((list) => {
        el_Display.empty();
        list.forEach((note) => {
            el_Display.append($(generateNoteFromTemplate(note)).collapsible());
        });
        el_Display.listview("refresh");
    });

    if (!noteList.hasList()) {
        noteList.fetchNotesFromServer()
    }

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
        newNote.find('#detailsButton').click(() => {
            detailedNote(note)
        });

        return newNote
    }

    function editNote(note) {
        focusNote = note;
        $.mobile.changePage($("#add"))
    }

    function detailedNote(note) {
        focusNote = note;
        $.mobile.changePage($("#details"))
    }

    function deleteNote(note) {
        noteList.deleteNoteFromList(note)
    }
}

function onCreateAdd() {

    $('.page_title').html("Add");

    const in_Button_Add_Note = $("#in_button_add_note");
    const in_Text_Subject = $("#in_text_subject");
    const in_Text_Details = $("#in_text_details");
    const in_Number_Priority = $("#in_number_priority");

    function initBinds() {
        in_Button_Add_Note.click(function () {

            const note = {
                Subject: in_Text_Subject.val(),
                Details: in_Text_Details.val(),
                Priority: in_Number_Priority.val()
            };

            if (focusNote !== null) {
                noteList.editNoteInList({Id: focusNote.Id, ...note}, finishOrErrorResponse);
            } else {
                noteList.addNoteToList(note, finishOrErrorResponse);
            }

        });
    }

    function destroyBinds() {
        in_Button_Add_Note.off();
    }

    const finishOrErrorResponse = (err) => {
        if (!err) {
            focusNote = null;
            setEditFields(null);
            destroyBinds();
            $.mobile.changePage($("#main"));
        } else {
            console.error(err);
        }
    };

    function setEditFields(note) {
        if (note) {
            in_Text_Subject.val(note.Subject);
            in_Text_Details.val(note.Details);
            in_Number_Priority.val(note.Priority);
        } else {
            in_Text_Subject.val(null);
            in_Text_Details.val(null);
            in_Number_Priority.val(0);
        }
    }

    setEditFields(focusNote);
    initBinds();

}

function onCreateDetails() {

    $('.page_title').html("Details");

    const displayNote = $("#showData");
    const noteTitle = $("#noteTitle");
    const titleText = "" + focusNote.Subject;
    const noteText = "<p>Priority: " + focusNote.Priority + "<br /> Subject: " + focusNote.Subject + "<br /> Details: " + focusNote.Details + "</p>";
    noteTitle.text(titleText);
    displayNote.html(noteText);

}





