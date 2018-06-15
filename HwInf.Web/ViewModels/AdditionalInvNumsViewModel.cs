namespace HwInf.Web.ViewModels
{
    public class AdditionalInvNumViewModel
    {
        public string InvNum { get; set; }

        public AdditionalInvNumViewModel()
        {
            
        }

        public AdditionalInvNumViewModel(string invNum)
        {
            InvNum = invNum;
        }
    }
}