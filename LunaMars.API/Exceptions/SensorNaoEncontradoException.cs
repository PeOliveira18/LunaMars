namespace LunaMars.API.Exceptions
{
    public class SensorNaoEncontradoException : Exception
    {
        public SensorNaoEncontradoException(string mensagem) : base(mensagem) { }
    }
}
