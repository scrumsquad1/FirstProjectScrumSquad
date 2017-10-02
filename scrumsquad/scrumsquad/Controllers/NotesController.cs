using scrumsquad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace scrumsquad.Controllers
{
    public class NotesController : ApiController
    {
        Notes[] notes = new Notes[]
         {
           new Notes { Id = 1, Priority = 3, Subject = "Wake up", Details = "Set alarm of 7:00 am and get out of bed."},
           new Notes { Id = 2, Priority = 2, Subject = "Eat breakfast", Details = "Eat a healthy breakfast."},
           new Notes { Id = 3, Priority = 5, Subject = "Go to work", Details = "Get to work before 9:00 am."}

         };

        public IEnumerable<Notes> GetAllNotes()
        {
            return notes;
        }

        public IHttpActionResult GetNote(int id)
        {
            var note = notes.FirstOrDefault((p) => p.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }
    }
}