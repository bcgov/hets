/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class NoteUnitTest 
    { 
		
		private readonly NoteController _Note;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public NoteUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
            NoteService _service = new NoteService(dbAppContext.Object);
            _Note = new NoteController (_service);
		}
			
		[Fact]
		/// <summary>
        /// Unit test for NotesBulkPost
        /// </summary>
		public void TestNotesBulkPost()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for NotesGet
        /// </summary>
		public void TestNotesGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for NotesIdDeletePost
        /// </summary>
		public void TestNotesIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for NotesIdGet
        /// </summary>
		public void TestNotesIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for NotesIdPut
        /// </summary>
		public void TestNotesIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for NotesPost
        /// </summary>
		public void TestNotesPost()
		{
			Assert.True(true);
		}		        
    }
}
