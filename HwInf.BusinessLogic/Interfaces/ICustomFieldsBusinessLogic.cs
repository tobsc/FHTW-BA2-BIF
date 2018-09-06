using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface ICustomFieldsBusinessLogic
    {
        IEnumerable<FieldGroup> GetFieldGroups();
        FieldGroup GetFieldGroup(string groupSlug);
        IEnumerable<Field> GetFields();
        Field GetField(string slug);
        bool FieldGroupExists(string slug);
        FieldGroup CreateFieldGroup();
        Field CreateField();
        void UpdateFieldGroup(FieldGroup obj);
        void DeleteField(Field field);
        void DeleteFieldGroup(FieldGroup obj);
    }
}