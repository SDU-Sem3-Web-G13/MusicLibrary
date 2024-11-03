
namespace Models
{
    internal class UserCredentialsModel
    {
        public byte[] MailHash { get; private set; }
        public byte[] PassHash { get; private set; }

        internal UserCredentialsModel(byte[] mailHash, byte[] passHash)
        {
            this.MailHash = mailHash;
            this.PassHash = passHash;
        }
    }
}
