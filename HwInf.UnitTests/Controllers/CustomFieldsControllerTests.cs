using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HwInf.Web.Controllers;
using HwInf.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

// GetGroups, GetGroupsOfDeviceType returned wrong type.
// GetGroupsOfDeviceTypes added null check.
// Error in FieldGroup creation null exception
// Put FieldGroup didnt have return value.
// Null Point Exception

namespace HwInf.UnitTests.Controllers
{
    [TestFixture]
    public class CustomFieldsControllerTests : ControllerTests
    {
        private readonly CustomFieldsController ctr;

        
        public CustomFieldsControllerTests()
        {
            var log = new Mock<ILogger<CustomFieldsController>>();
            ctr = new CustomFieldsController(Bl, log.Object) {ControllerContext = _controllerContext};
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_field_groups()
        {
            var res = (ctr.GetGroups() as OkObjectResult)?.Value as List<FieldGroupViewModel>;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_field_groups_of_device_type()
        {
            var res = (ctr.GetGroupsOfDeviceType("pc") as OkObjectResult)?.Value as List<FieldGroupViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Any(i => i.Slug.Contains("anschluesse")));
        }

        [Test]
        public void ctr_should_create_field_group()
        {
            var vmdl = ControllerHelper.GetvalidFieldGroupViewModel();
            Assert.NotNull(vmdl);
            var res = (ctr.PostGroup(vmdl) as OkObjectResult)?.Value as FieldGroupViewModel;
            Assert.NotNull(res);
            var obj = Bl.GetFieldGroup("test-1");
            Assert.True(obj.Slug.Equals(vmdl.Slug));
        }


        [Test]
        public void ctr_should_create_field_and_add_to_field_group()
        {
            var vmdl = ControllerHelper.GetValidFieldViewModel();
            Assert.NotNull(vmdl);
            var res = (ctr.PostField("anschluesse", vmdl) as OkObjectResult)?.Value as FieldViewModel;
            Assert.NotNull(res);
            var obj = Bl.GetFieldGroup("anschluesse");
            Assert.True(obj.Fields.Any(i => i.Slug.Equals(res.Slug)));
        }

        [Test]
        public void ctr_should_update_fields_of_field_group()
        {
            var obj = Bl.GetFieldGroup("prozessoren");
            Assert.NotNull(obj);
            var vmdl = new FieldGroupViewModel(obj);
            Assert.NotNull(vmdl);
            vmdl.Fields.SingleOrDefault(i => "intel-i5".Equals(i.Slug)).Name = "Intel Core i5";
            var res = (ctr.PutFieldGroups(vmdl.Slug, vmdl) as OkObjectResult)?.Value as FieldGroupViewModel;
            Assert.NotNull(res);
            var obj2 = Bl.GetFieldGroup("prozessoren");
            Assert.True(obj2.Fields.Any(i => i.Name.Equals("Intel Core i5")));
        }

        [Test]
        public void ctr_should_return_only_used_fields_from_field_group()
        {
            var a = ctr.GetFieldGroupsUsedFields();
            var res = (ctr.GetFieldGroupsUsedFields() as OkObjectResult)?.Value as List<FieldGroupViewModel>;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_only_used_fields_from_field_group_by_type()
        {
            var res = (ctr.GetFieldGroupsUsedFieldsType("pc") as OkObjectResult)?.Value as List<FieldGroupViewModel>;
            Assert.NotNull(res);
        }
    }
}