namespace Template.Aws.Lambda.Domain.ValuesObjects
{
    public struct Estado
    {
        public char[] Abreviado { get; private set;  }
        public string Extenso { get; private set;  }

        public Estado(char[] _Abreviado,
            string _Extenso)
        {
            Abreviado = _Abreviado;
            Extenso = _Extenso;
        }
    }
}