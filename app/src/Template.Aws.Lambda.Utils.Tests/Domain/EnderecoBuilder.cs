using Bogus;
using Template.Aws.Lambda.Domain.ValuesObjects;

namespace Template.Aws.Lambda.Utils.Tests.Domain
{
    public class EnderecoBuilder
    {
        private EnderecoBuilder() { }

        private static Faker<Endereco> Factory()
        {

            return new Faker<Endereco>("pt_BR")
                .CustomInstantiator(e => new Endereco());
                //.RuleFor(o => o.Logradouro, f => f.Address.FullAddress())
                //.RuleFor(o => o.Tipo, f => f.PickRandom<TipoLogradouro>())
                //.RuleFor(o => o.Numero, f => f.Random.Short())
                //.RuleFor(o => o.Bairro, f => f.Address.City())
                //.RuleFor(o => o.Cidade, f => f.Address.City())
                //.RuleFor(o => o.Estado, f => f.PickRandom<Estado>());
        }

    }
}
