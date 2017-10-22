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
using System.Net.Mime;
using System.Web.Http;


namespace scrumsquad.Controllers
{
    public class NotesController : ApiController
    {
        //string collectionName = "Notes";  // production
        string collectionName = "NotesTest";  // testing

        bool testing = false;
        List<Note> noteList = new List<Note>();
        // add default controller for normal opperation
        public NotesController()
        {
            testing = false;
        }

        // add controller that lets you pass in a fake db for testing
        public NotesController(List<Note> FakeDataList)
        {
            noteList = FakeDataList;
            testing = true;
        }




        MongoDatabase mongoDatabase;

        public IEnumerable<Note> GetAllNotes()

        {
            if (!testing)  // if not testing, read data from real db
            {
                mongoDatabase = RetreiveMongohqDb();


                try
                {
                    var mongoList = mongoDatabase.GetCollection(collectionName).FindAll().AsEnumerable();
                    noteList = (from note in mongoList
                                select new Note
                                {
                                    Id = note["_id"].AsString,
                                    Subject = note["Subject"].AsString,
                                    Details = note["Details"].AsString,
                                    Priority = note["Priority"].AsInt32

                                }).ToList();
                }
                catch (Exception)
                {
                    throw new ApplicationException("failed to get data from Mongo");
                }
            }

            noteList.Sort();
            return noteList;
        }



        public IHttpActionResult GetNote(string id)
        {
            if (!testing)
            {
                mongoDatabase = RetreiveMongohqDb();


                try
                {
                    var mongoList = mongoDatabase.GetCollection(collectionName).FindAll().AsEnumerable();
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

                    throw;
                }
            }
            var note = noteList.FirstOrDefault((p) => p.Subject == id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }


        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            bool found = true;
            string subject = id;
            try
            {
                mongoDatabase = RetreiveMongohqDb();
                var mongoCollection = mongoDatabase.GetCollection(collectionName);
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
            var noteList = mongoDatabase.GetCollection(collectionName);
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

        private MongoDatabase RetreiveMongohqDb()
        {
            // to make unit test work, had to not use the ConfigurationManager
            string connectionString = "mongodb://scrumuser:scrumpass@ds036577.mlab.com:36577/notedb";
            MongoUrl myMongoURL = new MongoUrl(connectionString);
            //MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient(myMongoURL);
            MongoServer server = mongoClient.GetServer();
            return mongoClient.GetServer().GetDatabase("notedb");
        }
    }
}
