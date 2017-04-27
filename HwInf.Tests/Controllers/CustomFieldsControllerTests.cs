using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using HwInf.Controllers;
using HwInf.ViewModels;
using NUnit.Framework;
using NUnit.Framework.Internal;

// GetGroups, GetGroupsOfDeviceType returned wrong type.
// GetGroupsOfDeviceTypes added null check.
// Error in FieldGroup creation null exception
// Put FieldGroup didnt have return value.
// Null Point Exception

namespace HwInf.Tests.Controllers
{
    [TestFixture]
    public class CustomFieldsControllerTests : ControllerTests
    {
        private readonly CustomFieldsController ctr;

        
        public CustomFieldsControllerTests()
        {
            ctr = new CustomFieldsController(_dal);
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_field_groups()
        {
            var res = ctr.GetGroups() as OkNegotiatedContentResult<List<FieldGroupViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }

        [Test]
        public void ctr_should_return_field_groups_of_device_type()
        {
            var res = ctr.GetGroupsOfDeviceType("pc") as OkNegotiatedContentResult<List<FieldGroupViewModel>>;
            Assert.NotNull(res);
            Assert.True(res.Content.Any(i => i.Slug.Contains("anschluesse")));
        }

        [Test]
        public void ctr_should_create_field_group()
        {
            var vmdl = ControllerHelper.GetvalidFieldGroupViewModel();
            vmdl.Slug = "test-1";
            Assert.NotNull(vmdl);
            var res = ctr.PostGroup(vmdl) as OkNegotiatedContentResult<FieldGroupViewModel>;
            Assert.NotNull(res);
            var obj = _bl.GetFieldGroups("test-1");
            Assert.True(obj.Slug.Equals(vmdl.Slug));
        }


        [Test]
        public void ctr_should_create_field_and_add_to_field_group()
        {
            var vmdl = ControllerHelper.GetValidFieldViewModel();
            Assert.NotNull(vmdl);
            var res = ctr.PostField("anschluesse", vmdl) as OkNegotiatedContentResult<FieldViewModel>;
            Assert.NotNull(res);
            var obj = _bl.GetFieldGroups("anschluesse");
            Assert.True(obj.Fields.Any(i => i.Slug.Equals(res.Content.Slug)));
        }

        [Test]
        public void ctr_should_update_fields_of_field_group()
        {
            var obj = _bl.GetFieldGroups("prozessoren");
            Assert.NotNull(obj);
            var vmdl = new FieldGroupViewModel(obj);
            Assert.NotNull(vmdl);
            vmdl.Fields.SingleOrDefault(i => i.Slug.Equals("intel-i5")).Name = "Intel Core i5";
            var res = ctr.PutFieldGroups(vmdl.Slug, vmdl) as OkNegotiatedContentResult<FieldGroupViewModel>;
            Assert.NotNull(res);
            var obj2 = _bl.GetFieldGroups("prozessoren-1");
            Assert.True(obj2.Fields.Any(i => i.Name.Equals("Intel Core i5")));
        }

        [Test]
        public void ctr_should_return_only_used_fields_from_field_group()
        {
            var res = ctr.GetFieldGroupsUsedFields() as OkNegotiatedContentResult<List<FieldGroupViewModel>>;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_only_used_fields_from_field_group_by_type()
        {
            var res = ctr.GetFieldGroupsUsedFieldsType("pc") as OkNegotiatedContentResult<List<FieldGroupViewModel>>;
            Assert.NotNull(res);
        }
    }
}