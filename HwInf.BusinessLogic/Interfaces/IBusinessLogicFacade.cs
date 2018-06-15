namespace HwInf.BusinessLogic.Interfaces
{
    public interface IBusinessLogicFacade: IBusinessLogic, IDeviceBusinessLogic, ICustomFieldsBusinessLogic, IAccessoryBusinessLogic, IUserBusinessLogic, IOrderBusinessLogic, ISettingBusinessLogic, IDamageBusinessLogic
    {
        void SaveChanges();
    }
}