namespace Bitfresh_Core
{
    public interface IStatus
    {
        string STATUS { get; set; }

        string getStatusByOrder(string uid);

        void setStatusByOrder(string uid, string status);

        void removeStatusByOrder(string uid);
    }
}