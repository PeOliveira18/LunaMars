namespace LunaMars.API.Exceptions
{
    public class AlertaNaoEncontradoException : Exception
    {
        public AlertaNaoEncontradoException(string mensagem) : base(mensagem) { }
    }
}
