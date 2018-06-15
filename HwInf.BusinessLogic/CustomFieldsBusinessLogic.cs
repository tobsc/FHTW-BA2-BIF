using System.Collections.Generic;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{
    public class CustomFieldsBusinessLogic : ICustomFieldsBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public CustomFieldsBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }

        public IEnumerable<FieldGroup> GetFieldGroups()
        {
            return _dal.FieldGroups;
        }

        public FieldGroup GetFieldGroup(string groupSlug)
        {
            return _dal.FieldGroups.SingleOrDefault(i => i.Slug.Equals(groupSlug));
        }

        public IEnumerable<Field> GetFields()
        {
            return _dal.Fields;
        }

        public Field GetField(string slug)
        {
            return _dal.Fields.SingleOrDefault(i => i.Slug.Equals(slug));
        }

        public bool FieldGroupExists(string slug)
        {
            return _dal.FieldGroups.Any(i => i.Slug.Equals(slug));
        }

        public FieldGroup CreateFieldGroup()
        {
            if (!_principal.IsAllowed) throw new SecurityException();

            return _dal.CreateFieldGroup();
        }

        public Field CreateField()
        {
            if (!_principal.IsAllowed) throw new SecurityException();

            return _dal.CreateField();
        }

        
        public void UpdateFieldGroup(FieldGroup obj)
        {
            if (!_principal.IsAllowed) return;

            _dal.UpdateObject(obj);
        }


        public void DeleteField(Field field)
        {
            if (!_principal.IsAllowed) return;
            _dal.DeleteField(field);
        }

        public void DeleteFieldGroup(FieldGroup obj)
        {
            if (!_principal.IsAllowed) return;

            var fg = _dal.FieldGroups.ToList();
            if (!fg.Any(i => i.Slug.Equals(obj.Slug)))
            {
                obj.Fields.ToList().ForEach(i => _dal.DeleteField(i));
                _dal.DeleteFieldGroup(obj);

            }
            else
            {
                UpdateFieldGroup(obj);
                obj.IsActive = false;
            }
        }
    }
}