namespace LunaMars.API.Exceptions
{
    public class SetorNaoEncontradoException : Exception
    {
        public SetorNaoEncontradoException(string mensagem) : base(mensagem) { }
    }
}
