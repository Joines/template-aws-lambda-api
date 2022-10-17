namespace Template.Aws.Lambda.Domain.ValuesObjects
{
    public struct Phone
    {
        public char[] Ddd { get; private set; }
        public string Numero { get; private set; }

        public Phone(char[] _ddd, string _numero)
        {
            Ddd = _ddd;
            Numero = _numero;
        }
    }
}
