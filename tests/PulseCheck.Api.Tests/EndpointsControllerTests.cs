using System.Net;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PulseCheck.Api.Controllers;
using PulseCheck.Api.Data;
using PulseCheck.Api.Models;
using PulseCheck.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace PulseCheck.Api.Tests;

public class EndpointsControllerTests 
{
    private AppDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void Get_ReturnsAllEndpoints()
    {
         var db =  CreateInMemoryDb();
         var endpoint = new MonitoredEndpoint
           {
                Id = 1,
                Name = "Test",
                Url = "https://fake-url.com"
            };
         var endpoint1 = new MonitoredEndpoint
            {
                Id = 2,
                Name = "Test1",
                Url = "https://fake1-url.com"
            };
        db.MonitoredEndpoints.Add(endpoint);
        db.MonitoredEndpoints.Add(endpoint1);
        db.SaveChanges();

        var controller = new EndpointsController(db);
        var result = controller.Get();

        var okResult = result as OkObjectResult;
        var endpoints = okResult.Value as List<MonitoredEndpoint>;
        Assert.Equal(2, endpoints.Count);

    }

      [Fact]
    public void GetById_WhenIdExists_ReturnsEndpoint()
    {
         var db =  CreateInMemoryDb();
         var endpoint = new MonitoredEndpoint
           {
                Id = 1,
                Name = "Google",
                Url = "https://google.com"
            };
             db.MonitoredEndpoints.Add(endpoint);
        db.SaveChanges();

        var controller = new EndpointsController(db);
        var result = controller.GetById(1);
         var okResult = result as OkObjectResult;
        var endpoints = okResult.Value as MonitoredEndpoint;
        Assert.Equal("Google", endpoint.Name);
        Assert.Equal("https://google.com", endpoint.Url);
        Assert.Equal(1, endpoint.Id);
    }

     [Fact]
    public void GetById_WhenIdDoesNotExist_ReturnsNotFound()
    {
        var db =  CreateInMemoryDb();
        var controller = new EndpointsController(db);
        var result = controller.GetById(999);
        Assert.IsType<NotFoundResult>(result);
    }
      [Fact]
    public void Delete_WhenIdExists_RemovesEndpoint()
    {
        var db =  CreateInMemoryDb();
         var endpoint = new MonitoredEndpoint
           {
                Id = 1,
                Name = "Google",
                Url = "https://google.com"
            };
        db.MonitoredEndpoints.Add(endpoint);
        db.SaveChanges();
        var controller = new EndpointsController(db);
        var result = controller.Delete(1);
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(0, db.MonitoredEndpoints.Count());
    }

    [Fact]
    public void Create_AddsEndpointToDatabase()
    {
        var db =  CreateInMemoryDb();
         var endpoint = new MonitoredEndpoint
           {
                Id = 1,
                Name = "Google",
                Url = "https://google.com"
            };
        
        var controller = new EndpointsController(db);
        var result = controller.Create(endpoint);
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(1, db.MonitoredEndpoints.Count());
    }
}