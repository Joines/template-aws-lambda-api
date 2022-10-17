
using Template.Aws.Lambda.Domain.Entities.bases;
using Template.Aws.Lambda.Domain.ValuesObjects;

namespace Template.Aws.Lambda.Domain.Entities
{
    public class Client: EntityBase
    {
        public string? Name { get; set; }
        public Cpf Cpf { get; set; }
        public Phone Phone { get; set; }
        public Endereco Endereco { get; set; }
    }
}
