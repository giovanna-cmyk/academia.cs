using System;

class Plano
{
    public double Preco { get; private set; }
    public int Duracao { get; private set; }
    public double Desconto { get; private set; }

    public Plano(double preco, int duracao, double desconto)
    {
        if (preco < 0 || duracao <= 0 || desconto < 0 || desconto > 100)
            throw new ArgumentException("Dados inválidos!");

        Preco = preco;
        Duracao = duracao;
        Desconto = desconto;
    }

    public virtual double CalcularPreco()
    {
        return Preco * Duracao * (1 - Desconto / 100);
    }
}

class PlanoMensal : Plano
{
    public PlanoMensal() : base(100, 1, 0) { }
}

class PlanoTrimestral : Plano
{
    public PlanoTrimestral() : base(100, 3, 10) { }
}

class PlanoAnual : Plano
{
    public PlanoAnual() : base(100, 12, 20) { }
}

class AlunoAcademia
{
    public string Nome { get; private set; }
    public Plano PlanoAtual { get; private set; }
    public DateTime Termino { get; private set; }
    public bool Ativa { get; private set; }

    public AlunoAcademia(string nome)
    {
        Nome = nome;
    }

    public void Matricular(Plano plano)
    {
        if (plano == null || Ativa)
        {
            Console.WriteLine("Não foi possível matricular.");
            return;
        }

        PlanoAtual = plano;
        Termino = DateTime.Today.AddMonths(plano.Duracao);
        Ativa = true;

        Console.WriteLine("Matrícula realizada!");
        Console.WriteLine("Valor: R$ " +
            plano.CalcularPreco().ToString("F2"));
        Console.WriteLine("Término: " +
            Termino.ToString("dd/MM/yyyy"));
    }

    public void Consultar()
    {
        Console.WriteLine("Aluno: " + Nome);
        Console.WriteLine("Plano: " +
            (PlanoAtual == null ? "Nenhum" :
            PlanoAtual.GetType().Name));
        Console.WriteLine("Matrícula ativa: " +
            (Ativa && DateTime.Today < Termino));
    }

    public void Renovar()
    {
        if (!Ativa || PlanoAtual == null ||
            DateTime.Today >= Termino)
        {
            Console.WriteLine("Não é possível renovar.");
            return;
        }

        Termino = Termino.AddMonths(PlanoAtual.Duracao);
        Console.WriteLine("Matrícula renovada!");
    }

    public void Cancelar()
    {
        if (!Ativa)
        {
            Console.WriteLine("Matrícula já está inativa.");
            return;
        }

        Ativa = false;
        Console.WriteLine("Matrícula cancelada!");
    }

    public void VerificarAcesso()
    {
        if (Ativa && DateTime.Today < Termino)
            Console.WriteLine("Acesso liberado!");
        else
            Console.WriteLine("Acesso bloqueado!");
    }
}

class Program
{
    static void Main()
    {
        Plano mensal = new PlanoMensal();
        Plano trimestral = new PlanoTrimestral();
        Plano anual = new PlanoAnual();

        Console.WriteLine("ACADEMIA");
        Console.WriteLine("1 - Mensal");
        Console.WriteLine("2 - Trimestral");
        Console.WriteLine("3 - Anual");
        Console.Write("Escolha um plano: ");

        if (!int.TryParse(Console.ReadLine(), out int opcao))
        {
            Console.WriteLine("Opção inválida!");
            return;
        }

        Plano escolhido;

        if (opcao == 1)
            escolhido = mensal;
        else if (opcao == 2)
            escolhido = trimestral;
        else if (opcao == 3)
            escolhido = anual;
        else
        {
            Console.WriteLine("Opção inválida!");
            return;
        }

        Console.Write("Nome do aluno: ");
        string nome = Console.ReadLine() ?? "";

        AlunoAcademia aluno = new AlunoAcademia(nome);
        aluno.Matricular(escolhido);

        int opcaoMenu = 0;

        while (opcaoMenu != 5)
        {
            Console.WriteLine("MENU");
            Console.WriteLine("1 - Consultar matrícula");
            Console.WriteLine("2 - Verificar acesso");
            Console.WriteLine("3 - Renovar matrícula");
            Console.WriteLine("4 - Cancelar matrícula");
            Console.WriteLine("5 - Sair");
            Console.Write("Escolha: ");

            if (!int.TryParse(Console.ReadLine(), out opcaoMenu))
            {
                Console.WriteLine("Opção inválida!");
                continue;
            }

            switch (opcaoMenu)
            {
                case 1:
                    aluno.Consultar();
                    break;
                case 2:
                    aluno.VerificarAcesso();
                    break;
                case 3:
                    aluno.Renovar();
                    break;
                case 4:
                    aluno.Cancelar();
                    break;
                case 5:
                    Console.WriteLine("Programa encerrado!");
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
}

