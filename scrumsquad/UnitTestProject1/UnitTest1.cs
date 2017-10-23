using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web.Http;  // had to grab the .dll by browsing in "Add reference" to scrumsquad

using scrumsquad.Controllers;
using scrumsquad.Models;
using System.Web.Http.Results;


namespace UnitTestProject1
{
    [TestClass]
    public class TestNotesController
    {
        List<Note> noteList = new List<Note>();

        // method used to generate fake List of valid data
        private List<Note> GenerateFakeDataList()
        {
            List<Note> workingList = new List<Note>();
            for (int i = 0; i < 3; i++)
            {
                Note nextNote = new Note();

                nextNote.Id = i.ToString();
                nextNote.Subject = "Test" + i.ToString();
                nextNote.Details = "Test" + i.ToString() + " Details";
                nextNote.Priority = i;
                workingList.Add(nextNote);
            }
            return workingList;
        }       

        //=======================================================================
        // test first API   GetAllNotes()
        [TestMethod]
        // first test local logic, using fake data
        public void GetAllFakeNotes_ShouldReturnAllNotes()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use 1 of 2 constructors

            var result = controller.GetAllNotes() as List<Note>;
            Assert.AreEqual(testNotes.Count, result.Count);
        }

        [TestMethod]
        // now test against test data in mongo
        public void GetAllMongoNotes_ShouldReturnAllNotes()
        {
            // need to modify Controller to point to NotesTest
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(); // use the other constructor

            var result = controller.GetAllNotes() as List<Note>;
            Assert.AreEqual(testNotes.Count, result.Count);
        }




        //=======================================================================
        // test 2nd API   GetNote(string id)
        [TestMethod]
        // first test local logic, using fake data
        public void GetFakeNote_ShouldReturnParticularNote()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use 1 of 2 constructors

            IHttpActionResult result = controller.GetNote("Test2");
            var contentResult = result as OkNegotiatedContentResult<Note>;

            Assert.AreEqual(testNotes[2].Subject, contentResult.Content.Subject);

        }

        [TestMethod]
        // now test against test data in mongo
        public void GetMongoNote_ShouldReturnParticularNote()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(); // use other constructors

            IHttpActionResult result = controller.GetNote("Test2");
            var contentResult = result as OkNegotiatedContentResult<Note>; ;

            Assert.AreEqual(testNotes[2].Subject, contentResult.Content.Subject);

        }

        [TestMethod]
        // first test local logic, using fake data
        public void GetFakeNote_ShouldReturnNotFound()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use 1 of 2 constructors

            IHttpActionResult result = controller.GetNote("Test5");
            var contentResult = result as NotFoundResult;

            Assert.AreEqual(result, contentResult);

        }
        [TestMethod]
        // now test against test data in mongo
        public void GetMongoNote_ShouldReturnNotFound()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(); // use other constructors

            IHttpActionResult result = controller.GetNote("Test5");
            var contentResult = result as NotFoundResult;

            Assert.AreEqual(result, contentResult);           
        }
        [TestMethod]
        public void GetFakeNote_DeleteReturnsOk()        
        {
                // Arrange
                List<Note> testNotes = GenerateFakeDataList();
                var controller = new NotesController(testNotes);

            // test to delete 59ed42eb4cc50b1be00fada8, should pass one time 
            // but i need to manually retrive the id from mongo or by logging
            // and then pass them in here
           
            HttpResponseMessage result = controller.Delete("addIdHere");
                var returnsOK = false;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    returnsOK = true;
                };

                // Assert
                Assert.IsTrue(returnsOK);
         }

        [TestMethod]
        public void GetMongoNote_VerifySavedNote()
        {           
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController();
            Note note = new Note();
            note.Subject = "Test8";
            note.Details = "Test8 Details";
            note.Priority = 8; 
   
            Note result = controller.Save(note);
            IHttpActionResult verifyNote = controller.GetNote("Test8");
            var contentResult = verifyNote as OkNegotiatedContentResult<Note>;
          
            controller.Delete(result.Id);
            // Assert
            Assert.AreEqual(result.Subject, contentResult.Content.Subject);
        }

        [TestMethod]
        public void GetFakeNote_VerifySavedNote()
        {           
            List<Note> testNotes = GenerateFakeDataList();
            Note note = new Note();
            note.Subject = "Test8";
            note.Details = "Test8 Details";
            note.Priority = 8;

            testNotes.Add(note);

            var controller = new NotesController(testNotes);
            
            Note result = controller.Save(note);
            IHttpActionResult verifyNote = controller.GetNote("Test8");
            var contentResult = verifyNote as OkNegotiatedContentResult<Note>;

            testNotes.Remove(note);
            controller.Delete(result.Id);
            // Assert
            Assert.AreEqual(result.Subject, contentResult.Content.Subject);
        }
    }
}
