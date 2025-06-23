namespace Entidades
{
    public class ResetPasswordToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Identificacion { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }

}
