namespace Template.Aws.Lambda.Domain.ValuesObjects
{
    public struct Endereco
    {
        public string Logradouro { get; private set; }
        public short Numero { get; private set; }
        public TipoLogradouro Tipo { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public Estado Estado { get; private set; }

        public Endereco(string logradouro,
            short numero, TipoLogradouro tipo,
            string bairro, string cidade, Estado estado)
        {
            Logradouro = logradouro;
            Numero = numero;
            Tipo = tipo;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
        }
    }
}
