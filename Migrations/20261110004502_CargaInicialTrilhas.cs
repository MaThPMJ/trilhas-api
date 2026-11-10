using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrilhasApi.Migrations
{
    /// <inheritdoc />
    public partial class CargaInicialTrilhas : Migration
    {
        private const string TrilhaJuros = "Fugindo dos Juros";
        private const string TrilhaConsumo = "O Desafio do Supermercado";
        private const string TrilhaPoupanca = "Guardando um Pouquinho";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            InserirTrilha(migrationBuilder, TrilhaJuros, "JUROS",
                "Entenda como os juros do cartão e do parcelamento fazem a dívida crescer e como escapar deles.");

            InserirTrilha(migrationBuilder, TrilhaConsumo, "CONSUMO",
                "Aprenda a comparar preços e a planejar as compras para o dinheiro render mais no mercado.");

            InserirTrilha(migrationBuilder, TrilhaPoupanca, "POUPANCA",
                "Descubra como montar uma reserva de emergência começando com valores pequenos.");

            InserirDesafio(migrationBuilder, TrilhaJuros,
                "O que acontece quando você paga só o valor mínimo da fatura do cartão?",
                "A dívida some no mês seguinte",
                "O resto da fatura vira dívida com juros altos",
                "O banco dá um desconto na próxima fatura",
                "O limite do cartão aumenta sozinho",
                "B", 10);

            InserirDesafio(migrationBuilder, TrilhaJuros,
                "Sua fatura veio R$ 300 e você só tem R$ 200 agora. O que costuma sair mais barato?",
                "Pagar só o mínimo e deixar o resto para depois",
                "Não pagar nada e esperar o banco ligar",
                "Pagar o que der e negociar o restante em parcelas com juros menores",
                "Fazer outro cartão para pagar o primeiro",
                "C", 20);

            InserirDesafio(migrationBuilder, TrilhaJuros,
                "Uma TV custa R$ 1.000 à vista ou 10 parcelas de R$ 130. Quanto você paga a mais no parcelado?",
                "Nada, o preço é o mesmo",
                "R$ 30",
                "R$ 130",
                "R$ 300",
                "D", 20);

            InserirDesafio(migrationBuilder, TrilhaJuros,
                "Qual é a melhor atitude para não cair no rotativo do cartão?",
                "Gastar no cartão só o que cabe no orçamento do mês",
                "Usar todo o limite, porque ele faz parte da renda",
                "Pagar sempre o mínimo para sobrar dinheiro",
                "Ter vários cartões para dividir os gastos",
                "A", 30);

            InserirDesafio(migrationBuilder, TrilhaConsumo,
                "Para que serve fazer uma lista de compras antes de ir ao mercado?",
                "Para comprar só o que precisa e não gastar por impulso",
                "Para demorar mais tempo no mercado",
                "Para comprar tudo o que estiver em promoção",
                "Não serve para nada, o importante é levar o cartão",
                "A", 10);

            InserirDesafio(migrationBuilder, TrilhaConsumo,
                "O pacote de 1 kg de arroz custa R$ 6 e o de 5 kg custa R$ 25. Qual sai mais barato por quilo?",
                "O de 1 kg, que custa R$ 6 o quilo",
                "O de 5 kg, que sai por R$ 5 o quilo",
                "Os dois custam o mesmo por quilo",
                "Não dá para comparar pacotes de tamanhos diferentes",
                "B", 20);

            InserirDesafio(migrationBuilder, TrilhaConsumo,
                "O sabão em pó de 800 g custa R$ 8 e o de 1 kg custa R$ 9. Como saber qual vale mais a pena?",
                "Escolher sempre o de menor preço na etiqueta",
                "Escolher sempre a embalagem maior",
                "Escolher o de embalagem mais bonita",
                "Dividir o preço pelo peso e comparar o valor por quilo",
                "D", 20);

            InserirDesafio(migrationBuilder, TrilhaConsumo,
                "Uma promoção diz \"leve 3 e pague 2\" em um produto que você não usa. O que fazer?",
                "Levar, porque promoção não se perde",
                "Levar e guardar para um dia usar",
                "Não levar: gastar com o que não precisa não é economia",
                "Levar no cartão para pagar depois",
                "C", 30);

            InserirDesafio(migrationBuilder, TrilhaPoupanca,
                "O que é uma reserva de emergência?",
                "Dinheiro para comprar presentes no fim do ano",
                "O limite do cartão de crédito",
                "Um empréstimo que o banco deixa separado",
                "Um dinheiro guardado para imprevistos, como remédio ou conserto",
                "D", 10);

            InserirDesafio(migrationBuilder, TrilhaPoupanca,
                "Se você guardar R$ 5 por semana, quanto terá depois de 10 semanas?",
                "R$ 15",
                "R$ 50",
                "R$ 100",
                "R$ 500",
                "B", 20);

            InserirDesafio(migrationBuilder, TrilhaPoupanca,
                "Qual é o melhor momento para separar o dinheiro da reserva?",
                "Logo que o dinheiro entra, antes dos outros gastos",
                "No fim do mês, se sobrar alguma coisa",
                "Só quando aparecer uma emergência",
                "Só quando ganhar um aumento",
                "A", 20);

            InserirDesafio(migrationBuilder, TrilhaPoupanca,
                "A geladeira quebrou e o conserto custa R$ 150. Você tem R$ 200 na reserva. O que é melhor?",
                "Pegar um empréstimo e não mexer na reserva",
                "Deixar a geladeira quebrada",
                "Usar a reserva para o conserto e depois voltar a guardar aos poucos",
                "Passar no cartão e pagar só o mínimo da fatura",
                "C", 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var titulos = $"{Texto(TrilhaJuros)}, {Texto(TrilhaConsumo)}, {Texto(TrilhaPoupanca)}";

            migrationBuilder.Sql(
                $"DELETE FROM DESAFIO WHERE TRILHA_ID IN (SELECT ID FROM TRILHA WHERE TITULO IN ({titulos}))");

            migrationBuilder.Sql(
                $"DELETE FROM TRILHA WHERE TITULO IN ({titulos})");
        }

        // O ID não é informado: a coluna é identity, e fixar valores faria o próximo INSERT da API colidir com a sequência.
        private static void InserirTrilha(MigrationBuilder migrationBuilder, string titulo, string tema, string descricao)
        {
            migrationBuilder.Sql(
                "INSERT INTO TRILHA (TITULO, DESCRICAO, TEMA, NIVEL, DT_CRIACAO) " +
                $"VALUES ({Texto(titulo)}, {Texto(descricao)}, {Texto(tema)}, 1, SYSTIMESTAMP)");
        }

        private static void InserirDesafio(
            MigrationBuilder migrationBuilder,
            string tituloTrilha,
            string pergunta,
            string alternativaA,
            string alternativaB,
            string alternativaC,
            string alternativaD,
            string respostaCorreta,
            int pontos)
        {
            migrationBuilder.Sql(
                "INSERT INTO DESAFIO (TRILHA_ID, PERGUNTA, ALTERNATIVA_A, ALTERNATIVA_B, ALTERNATIVA_C, ALTERNATIVA_D, RESPOSTA_CORRETA, PONTOS) " +
                $"VALUES ((SELECT ID FROM TRILHA WHERE TITULO = {Texto(tituloTrilha)}), {Texto(pergunta)}, " +
                $"{Texto(alternativaA)}, {Texto(alternativaB)}, {Texto(alternativaC)}, {Texto(alternativaD)}, " +
                $"{Texto(respostaCorreta)}, {pontos})");
        }

        private static string Texto(string valor) => "'" + valor.Replace("'", "''") + "'";
    }
}
