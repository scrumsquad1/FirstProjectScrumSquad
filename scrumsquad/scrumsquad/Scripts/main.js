let focusNote = null;
let notesList;

$(document).on('pagebeforeshow ', '#main', function () {   // see: https://stackoverflow.com/questions/14468659/jquery-mobile-document-ready-vs-page-events
    focusNote = null;
    const template_note = $(".templates .note");
    const el_Display = $("#notes_display");

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
        console.log('edit note called');
        console.log(focusNote);
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
    let pressCount = 0;
    let in_Button_Add_Note = $("#in_button_add_note");
    let in_Text_Subject = $("#in_text_subject");
    let in_Text_Details = $("#in_text_details");
    let in_Number_Priority = $("#in_number_priority");   

    if (focusNote != null) {         
        in_Text_Subject.val(focusNote.Subject);
        in_Text_Details.val(focusNote.Details);
        in_Number_Priority.val(focusNote.Priority);              
    }
    else {       
        in_Text_Subject.val(null);
        in_Text_Details.val(null);
        in_Number_Priority.val(0);
    }

    in_Button_Add_Note.click(function () {
        if (pressCount == 0 && focusNote != null) {
            pressCount = 1;
            apiEditNote({
                Id: focusNote.Id,
                Subject: in_Text_Subject.val(),
                Details: in_Text_Details.val(),
                Priority: in_Number_Priority.val()
            }, (err, result) => {
                if (!err) {                    
                    console.log('successfully edited note');                    
                    console.log(result);
                    in_Text_Subject.val(null);
                    in_Text_Details.val(null);
                    in_Number_Priority.val(0); 
                    $.mobile.changePage($("#main"));                   
                    
                } else {
                    console.error(err);
                }
                });
                     
        }
        else if (pressCount == 0 && focusNote == null){
            pressCount = 1;
            apiAddNote({
                Subject: in_Text_Subject.val(),
                Details: in_Text_Details.val(),
                Priority: in_Number_Priority.val()
            }, (err, result) => {
                if (!err) {                    
                    // console.log(result);
                    in_Text_Subject.val(null);
                    in_Text_Details.val(null);
                    in_Number_Priority.val(0);     
                    $.mobile.changePage($("#main"));                                     
                } else {
                    console.error(err);
                }
                });                     
        }
    });
});





