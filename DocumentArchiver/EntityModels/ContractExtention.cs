using DocumentArchiver.Helper;

namespace DocumentArchiver.EntityModels
{
    public partial class Contract
    {
        public string CreateTimeString
        {
            get
            {
                return CreateTime.ToString(Const.StandardDate);
            }
        }
    }
}
