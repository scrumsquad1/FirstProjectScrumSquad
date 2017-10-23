class NoteList {

    constructor() {
        this.onChange = null;
    }

    fetchNotesFromServer(callback) {
        apiGetNotes((err, result) => {
            if (!err) {
                this.list = result;                 
                this.onChange(this.list)           
            }
            if (callback)
                callback(err, result);
        });
    }

    addNoteToList(note, callback) {
        apiAddNote(note, (err, result) => {
            if (!err) {
                this.list.push(result);               
                this.onChange(this.list);
            } else {
                console.error(err);
            }
            if (callback)
                callback(err, result);
        });
    }

    editNoteInList(note, callback) {
        if (this.checkHasList()) {
            apiEditNote(note, (err, result) => {
                this.list.forEach((n, index) => {
                    if (n.Id === note.Id) {
                        this.list[index] = note;
                        this.onChange(this.list)
                    }
                });
                if (callback)
                    callback(err, result);
            })
        }
    }

    deleteNoteFromList(note, callback) {
        if (this.checkHasList()) {
            apiDeleteNote(note, (err, result) => {
                this.list.forEach((n) => {
                    //n.subject === note.subject
                    if (n.Id === note.Id) {
                        this.list.splice(this.list.indexOf(note), 1);
                        this.onChange(this.list)
                    }
                });
                if (callback)
                    callback(err, result)
            });
        }
    }

    hasList() {
        return this.list;
    }

    setOnChange(onChange) {
        this.onChange = onChange;
    }

    checkHasList() {
        if (this.list) {
            return true;
        } else {
            console.error("NoteList is null");
            return false;
        }
    }

}