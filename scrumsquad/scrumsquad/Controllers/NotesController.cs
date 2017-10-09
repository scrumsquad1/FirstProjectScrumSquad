using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using scrumsquad.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace scrumsquad.Controllers
{
    public class NotesController : ApiController
    {
        MongoDatabase mongoDatabase;

        Note[] notes = new Note[]
         {
           new Note { Priority = 3, Subject = "Wake up", Details = "Set alarm of 7:00 am and get out of bed."},
           new Note { Priority = 2, Subject = "Eat breakfast", Details = "Eat a healthy breakfast."},
           new Note { Priority = 5, Subject = "Go to work", Details = "Get to work before 9:00 am."}
         };

        private MongoDatabase RetreiveMongohqDb()
        {
            MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient(myMongoURL);
            MongoServer server = mongoClient.GetServer();
            return mongoClient.GetServer().GetDatabase("notedb");
        }

        //public IEnumerable<Note> GetAllNotes()
        //{
        //    mongoDatabase = RetreiveMongohqDb();

        //    List<Note> noteList = GetNoteList();
        //    // noteList.Sort(); // comment this out until you implement the IComparable<Note>
        //    // interface definition to your Note class,
        //    return noteList;  // ASP API will convert a List of Note objects to json
        //}

        [HttpGet]
        public IHttpActionResult GetNote(string id)  // make sure its string
        {
            mongoDatabase = RetreiveMongohqDb();

            List<Note> noteList = GetNoteList();

            var note = noteList.FirstOrDefault((p) => p.Subject == id);

            if (note == null)
                return NotFound();

            return Ok(note);
        }

        public List<Note> GetNoteList()
        {
            mongoDatabase = RetreiveMongohqDb();

            List<Note> noteList = new List<Note>();

            try
            {
                var mongoList = mongoDatabase.GetCollection("Notes").FindAll().AsEnumerable();
                noteList = (from nextNote in mongoList
                            select new Note
                            {   
                                Id = nextNote["_id"].AsString,
                                Subject = nextNote["Subject"].AsString,
                                Details = nextNote["Details"].AsString,
                                Priority = nextNote["Priority"].AsInt32,
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            noteList.Sort(); 
            return noteList;

        }

        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            bool found = true;
            string subject = id;
            try
            {
                mongoDatabase = RetreiveMongohqDb();
                var mongoCollection = mongoDatabase.GetCollection("Notes");
                var query = Query.EQ("_id", id);
                WriteConcernResult results = mongoCollection.Remove(query);

                if (results.DocumentsAffected < 1)
                {
                    found = false;
                }

            }
            catch (Exception ex)
            {
                found = false;
            }
            if (!found)
            {
                HttpResponseMessage badResponse = new HttpResponseMessage();
                badResponse.StatusCode = HttpStatusCode.BadRequest;
                return badResponse;
            }
            else
            {
                HttpResponseMessage goodResponse = new HttpResponseMessage();
                goodResponse.StatusCode = HttpStatusCode.OK;
                return goodResponse;
            }
 
        }

        [HttpPost]
        public Note Save(Note newNote)
        {
            
            mongoDatabase = RetreiveMongohqDb();
            var noteList = mongoDatabase.GetCollection("Notes");
            WriteConcernResult result;

            bool hasError = false;
            if (string.IsNullOrEmpty(newNote.Id))
            {
                newNote.Id = ObjectId.GenerateNewId().ToString();
                result = noteList.Insert<Note>(newNote);
                hasError = result.HasLastErrorMessage;
            }
            else
            {
                IMongoQuery query = Query.EQ("_id", newNote.Id);
                IMongoUpdate update = Update
                    .Set("Subject", newNote.Subject)
                    .Set("Details", newNote.Details)
                    .Set("Priority", newNote.Priority);
                result = noteList.Update(query, update);
                hasError = result.HasLastErrorMessage;
            }
            if (!hasError)
            {
                return newNote;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            } 
        }

    }

}
