using Bogus;
using Template.Aws.Lambda.Domain.Entities;
using Template.Aws.Lambda.Domain.ValuesObjects;

namespace Template.Aws.Lambda.Utils.Tests.Domain
{
    public class ClientBuilder
    {
        private ClientBuilder() { }

        public static Client Build() =>
            Factory().Generate();

        public static ICollection<Client> Build(int? qtd = null)
        {
            var rnd = new Random();

            if(qtd is null)
                qtd = rnd.Next(1,5);

            return Factory().Generate(qtd.Value);
        }

        private static Faker<Client> Factory()
        {
            var rnd = new Random();
            
            var ddd = new char[2];
            ddd[0] = (char)rnd.Next(1,9);
            ddd[1] = (char)rnd.Next(1, 9);

            return new Faker<Client>("pt_BR")
                .RuleFor(o => o.Name, f => f.Name.FullName())
                .RuleFor(o => o.Cpf, f => new Cpf("123.456.456-45"))
                .RuleFor(o => o.Endereco, f => new Endereco())
                .RuleFor(o => o.Phone, f => new Phone(ddd, f.Phone.PhoneNumber()));
        }
    }
}
