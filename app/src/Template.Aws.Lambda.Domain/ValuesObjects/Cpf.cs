namespace Template.Aws.Lambda.Domain.ValuesObjects
{
    public struct Cpf
    {
        public string Numero { get; private set; }

        public Cpf(string _numero)
        {
            Numero = _numero;
        }
    }
}
