using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public abstract class ControllerTestBase<TEntity, TController, TService> 
        : TestBase<TEntity> 
        where TController : Controller
        where TService : IService<TEntity>
        where TEntity: Entity

    {
        protected TController controller;
        protected TService service;

        public ControllerTestBase(ITestOutputHelper output)
            : base(output)
        {
            controller = Resolve<TController>();
            service = Resolve<TService>();
        }

        public int GetPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling(((decimal)totalCount / pageSize)); 
        }
    }
}
